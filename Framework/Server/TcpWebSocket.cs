/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace XFS4IoTServer
{
    /// <summary>
    /// WebSocket server connection implemented over a raw TcpClient.
    /// Replaces HttpListener-based WebSocket which is not supported on Android.
    /// Performs the RFC 6455 HTTP upgrade handshake and frame encoding/decoding.
    /// </summary>
    internal sealed class TcpWebSocket : WebSocket
    {
        private readonly Stream _stream;
        private volatile WebSocketState _state = WebSocketState.Open;
        private WebSocketCloseStatus? _closeStatus;
        private string _closeStatusDescription;
        private WebSocketMessageType _currentMessageType;
        private byte[] _receiveRemainder;
        private int _receiveRemainderOffset;
        private bool _receiveRemainderFin;
        private readonly SemaphoreSlim _sendLock = new(1, 1);

        private const int MaxPayloadBytes = 2 * 1024 * 1024;

        private TcpWebSocket(Stream stream)
        {
            _stream = stream;
        }

        public override WebSocketCloseStatus? CloseStatus => _closeStatus;
        public override string CloseStatusDescription => _closeStatusDescription;
        public override WebSocketState State => _state;
        public override string SubProtocol => null;

        /// <summary>
        /// Performs the WebSocket HTTP upgrade handshake on an accepted TcpClient.
        /// Returns (null, null) and sends HTTP 400 if the request is not a WebSocket upgrade.
        /// The returned requestPath is the URL path from the HTTP GET line (e.g. "/xfs4iot/v1.0/CardReader/").
        /// </summary>
        internal static async Task<(TcpWebSocket ws, string requestPath)> AcceptAsync(TcpClient client, SslServerAuthenticationOptions tlsOptions = null)
        {
            Stream stream = client.GetStream();

            if (tlsOptions is not null)
            {
                var sslStream = new SslStream(stream, leaveInnerStreamOpen: false);
                await sslStream.AuthenticateAsServerAsync(tlsOptions);
                stream = sslStream;
            }

            var request = await ReadHttpHeadersAsync(stream);

            if (request is null)
                return (null, null);

            var pathMatch = Regex.Match(request, @"^GET\s+(\S+)\s+HTTP/", RegexOptions.Multiline);
            string requestPath = pathMatch.Success ? pathMatch.Groups[1].Value : "/";

            var keyMatch = Regex.Match(request, @"Sec-WebSocket-Key:\s*(.+)\r\n");
            if (!keyMatch.Success)
            {
                var bad = Encoding.UTF8.GetBytes("HTTP/1.1 400 Bad Request\r\nContent-Length: 0\r\nConnection: close\r\n\r\n");
                await stream.WriteAsync(bad, 0, bad.Length);
                return (null, null);
            }

            var accept = ComputeAcceptKey(keyMatch.Groups[1].Value.Trim());

            var protocolMatch = Regex.Match(request, @"Sec-WebSocket-Protocol:\s*(.+)\r\n");
            string protocolLine = protocolMatch.Success
                ? $"Sec-WebSocket-Protocol: {protocolMatch.Groups[1].Value.Trim()}\r\n"
                : string.Empty;

            var response = Encoding.UTF8.GetBytes(
                "HTTP/1.1 101 Switching Protocols\r\n" +
                "Upgrade: websocket\r\n" +
                "Connection: Upgrade\r\n" +
                $"Sec-WebSocket-Accept: {accept}\r\n" +
                protocolLine +
                "\r\n");
            await stream.WriteAsync(response, 0, response.Length);

            return (new TcpWebSocket(stream), requestPath);
        }

        // Reads HTTP request headers incrementally until \r\n\r\n is found.
        // A single ReadAsync call is not guaranteed to return the complete headers.
        private static async Task<string> ReadHttpHeadersAsync(Stream stream)
        {
            var buffer = new byte[8192];
            int total = 0;
            while (total < buffer.Length)
            {
                int read = await stream.ReadAsync(buffer, total, buffer.Length - total);
                if (read == 0) return null;
                total += read;
                int scanFrom = Math.Max(0, total - read - 3);
                for (int i = scanFrom; i <= total - 4; i++)
                {
                    if (buffer[i] == '\r' && buffer[i + 1] == '\n' && buffer[i + 2] == '\r' && buffer[i + 3] == '\n')
                        return Encoding.UTF8.GetString(buffer, 0, i + 4);
                }
            }
            return null;
        }

        private static string ComputeAcceptKey(string clientKey)
        {
            using var sha1 = SHA1.Create();
            return Convert.ToBase64String(sha1.ComputeHash(
                Encoding.UTF8.GetBytes(clientKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11")));
        }

        public override async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            // Drain buffered remainder from a previous frame that exceeded the caller's buffer.
            if (_receiveRemainder is not null)
            {
                int remaining = _receiveRemainder.Length - _receiveRemainderOffset;
                int copy = Math.Min(remaining, buffer.Length);
                _receiveRemainder.AsMemory(_receiveRemainderOffset, copy).CopyTo(buffer);
                _receiveRemainderOffset += copy;
                bool done = _receiveRemainderOffset >= _receiveRemainder.Length;
                if (done) { _receiveRemainder = null; _receiveRemainderOffset = 0; }
                return new ValueWebSocketReceiveResult(copy, _currentMessageType, done && _receiveRemainderFin);
            }

            while (true)
            {
                var (fin, opcode, payload) = await ReadFrameAsync(cancellationToken);

                switch (opcode)
                {
                    case 0x1:
                        _currentMessageType = WebSocketMessageType.Text;
                        goto case 0x0;
                    case 0x2:
                        _currentMessageType = WebSocketMessageType.Binary;
                        goto case 0x0;
                    case 0x0:
                        int copy = Math.Min(payload.Length, buffer.Length);
                        payload.AsMemory(0, copy).CopyTo(buffer);
                        if (copy < payload.Length)
                        {
                            _receiveRemainder = payload;
                            _receiveRemainderOffset = copy;
                            _receiveRemainderFin = fin;
                            return new ValueWebSocketReceiveResult(copy, _currentMessageType, false);
                        }
                        return new ValueWebSocketReceiveResult(copy, _currentMessageType, fin);

                    case 0x8:
                        _state = WebSocketState.CloseReceived;
                        _closeStatus = payload.Length >= 2
                            ? (WebSocketCloseStatus)((payload[0] << 8) | payload[1])
                            : WebSocketCloseStatus.NormalClosure;
                        _closeStatusDescription = payload.Length > 2
                            ? Encoding.UTF8.GetString(payload, 2, payload.Length - 2)
                            : string.Empty;
                        await SendFrameAsync(0x8, payload, true, CancellationToken.None);
                        _state = WebSocketState.Closed;
                        return new ValueWebSocketReceiveResult(0, WebSocketMessageType.Close, true);

                    case 0x9:
                        await SendFrameAsync(0xA, payload, true, cancellationToken);
                        continue;

                    default:
                        continue;
                }
            }
        }

        public override async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            var result = await ReceiveAsync(new Memory<byte>(buffer.Array, buffer.Offset, buffer.Count), cancellationToken);
            return new WebSocketReceiveResult(result.Count, result.MessageType, result.EndOfMessage, CloseStatus, CloseStatusDescription);
        }

        public override async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            int opcode = messageType == WebSocketMessageType.Binary ? 0x2 : 0x1;
            await SendFrameAsync(opcode, buffer.AsMemory(), endOfMessage, cancellationToken);
        }

        public override async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            if (_state is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                await SendCloseFrameAsync(closeStatus, statusDescription, cancellationToken);
                _state = WebSocketState.Closed;
            }
        }

        public override async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await SendCloseFrameAsync(closeStatus, statusDescription, cancellationToken);
            _state = _state == WebSocketState.CloseReceived ? WebSocketState.Closed : WebSocketState.CloseSent;
        }

        public override void Abort()
        {
            _state = WebSocketState.Aborted;
            try { _stream.Close(); } catch { }
        }

        public override void Dispose()
        {
            _sendLock.Dispose();
            _stream.Dispose();
        }

        private async Task SendCloseFrameAsync(WebSocketCloseStatus status, string description, CancellationToken ct)
        {
            var descBytes = description is null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(description);
            var payload = new byte[2 + descBytes.Length];
            payload[0] = (byte)((int)status >> 8);
            payload[1] = (byte)((int)status & 0xFF);
            descBytes.CopyTo(payload, 2);
            await SendFrameAsync(0x8, payload, true, ct);
        }

        // Builds a single combined frame (header + payload) and writes it in one call.
        private async Task SendFrameAsync(int opcode, ReadOnlyMemory<byte> payload, bool fin, CancellationToken ct)
        {
            if (_state == WebSocketState.Aborted) return;

            int length = payload.Length;
            int headerLen = length < 126 ? 2 : length < 65536 ? 4 : 10;
            var frame = new byte[headerLen + length];

            if (length < 126)
            {
                frame[1] = (byte)length;
            }
            else if (length < 65536)
            {
                frame[1] = 126;
                frame[2] = (byte)(length >> 8);
                frame[3] = (byte)length;
            }
            else
            {
                frame[1] = 127;
                for (int i = 0; i < 8; i++)
                    frame[9 - i] = (byte)((length >> (8 * i)) & 0xFF);
            }
            frame[0] = (byte)((fin ? 0x80 : 0x00) | opcode);

            if (length > 0)
                payload.CopyTo(frame.AsMemory(headerLen));

            await _sendLock.WaitAsync(ct);
            try
            {
                await _stream.WriteAsync(frame, 0, frame.Length, ct);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async Task<(bool fin, int opcode, byte[] payload)> ReadFrameAsync(CancellationToken ct)
        {
            var header = new byte[2];
            await ReadExactAsync(header, ct);

            bool fin = (header[0] & 0x80) != 0;
            int opcode = header[0] & 0x0F;
            bool masked = (header[1] & 0x80) != 0;
            long length = header[1] & 0x7F;

            if (length == 126)
            {
                var ext = new byte[2];
                await ReadExactAsync(ext, ct);
                length = (ext[0] << 8) | ext[1];
            }
            else if (length == 127)
            {
                var ext = new byte[8];
                await ReadExactAsync(ext, ct);
                length = 0;
                for (int i = 0; i < 8; i++) length = (length << 8) | ext[i];
            }

            if (length < 0 || length > MaxPayloadBytes)
                throw new IOException($"WebSocket frame payload length {length} is invalid or exceeds {MaxPayloadBytes} byte limit");

            byte[] mask = null;
            if (masked)
            {
                mask = new byte[4];
                await ReadExactAsync(mask, ct);
            }

            var payload = new byte[length];
            if (length > 0)
                await ReadExactAsync(payload, ct);

            if (masked)
                for (int i = 0; i < payload.Length; i++)
                    payload[i] ^= mask[i % 4];

            return (fin, opcode, payload);
        }

        private async Task ReadExactAsync(byte[] buffer, CancellationToken ct)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                int read = await _stream.ReadAsync(buffer, offset, buffer.Length - offset, ct);
                if (read == 0) throw new IOException("WebSocket connection closed");
                offset += read;
            }
        }
    }
}
