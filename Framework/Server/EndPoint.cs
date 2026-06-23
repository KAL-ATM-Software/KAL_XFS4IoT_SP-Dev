/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    /// <summary>
    /// Single-port endpoint that accepts WebSocket connections and routes them to registered
    /// services by URL path. All services (Publisher, CardReader, Printer, …) share this one
    /// TcpListener so they all live on the same port.
    /// </summary>
    public sealed class EndPoint : IDisposable
    {
        public EndPoint(Uri EndPointUri,
                        IMessageDecoder CommandDecoder,
                        ILogger Logger)
        {
            EndPointUri.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(EndPointUri)}");
            CommandDecoder.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(CommandDecoder)}");
            Logger.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(Logger)}");

            this.CommandDecoder = CommandDecoder;
            this.Logger = Logger;

            bool fallbackIPv4 = true;
            // Check first OS supports IPv6 protocol
            if (Socket.OSSupportsIPv6)
            {
                try
                {
                    TcpListener = new TcpListener(IPAddress.IPv6Any, EndPointUri.Port);
                    TcpListener.Server.DualMode = true;
                    TcpListener.Start();
                    // OK, it must be IPv6 supported and could be mapped to IPv4 successfully, so no need to fallback to IPv4 explicitly.
                    fallbackIPv4 = false;
                }
                catch (Exception)
                {
                    try 
                    { 
                        TcpListener?.Stop(); 
                    } 
                    catch { }
                }
            }
            if (fallbackIPv4)
            {
                // If IPv6 is not supported, fallback to IPv4
                TcpListener = new TcpListener(IPAddress.Any, EndPointUri.Port);
                TcpListener.Start();
            }

            Logger.Log(Constants.Component, $"New endpoint at {EndPointUri.OriginalString}");
        }

        /// <summary>
        /// Register a service (or the publisher itself) to handle connections on <paramref name="path"/>.
        /// Call this once per service before <see cref="RunAsync"/> is started.
        /// </summary>
        public void Register(string path, ICommandDispatcher dispatcher, IServiceProvider provider)
        {
            string normalized = NormalizePath(path);
            _routes[normalized] = (dispatcher, provider);
            _serviceConnections[normalized] = new List<(Task task, IConnection connection)>();
        }

        /// <summary>
        /// Returns a snapshot of active connections for the service registered at <paramref name="path"/>.
        /// Used by ServiceProvider.BroadcastEvent.
        /// </summary>
        public IEnumerable<IConnection> GetConnections(string path)
        {
            string normalized = NormalizePath(path);
            if (!_serviceConnections.TryGetValue(normalized, out var list))
            {
                return [];
            }
            lock (list)
            {
                return [.. list.Select(x => x.connection)];
            }
        }

        public IEnumerable<IConnection> Connections { get => from d in _allConnections select d.connection; }

        public async Task RunAsync(CancellationToken token)
        {
            Task<TcpClient> acceptTask = TcpListener.AcceptTcpClientAsync();
            Task cancelTask = Task.Delay(-1, token);
            while (!token.IsCancellationRequested)
            {
                Logger.Log(Constants.Component, $"Listening for new connections and on {_allConnections.Count} existing connections");

                var tasks = from c in _allConnections select c.task;
                Task completedTask = await Task.WhenAny(Enumerable.Append(Enumerable.Append(tasks, acceptTask), cancelTask));

                if (completedTask == cancelTask)
                {
                }
                else if (completedTask != acceptTask)
                {
                    var entry = _allConnections.Find(x => x.task == completedTask);
                    _allConnections.Remove(entry);
                    if (entry.path is not null && _serviceConnections.TryGetValue(entry.path, out var connList))
                    {
                        lock (connList)
                        {
                            connList.RemoveAll(x => x.task == completedTask);
                        }
                    }
                }
                else
                {
                    TcpClient tcpClient = acceptTask.Result;
                    acceptTask = TcpListener.AcceptTcpClientAsync();
                    var (ws, requestPath) = await TcpWebSocket.AcceptAsync(tcpClient, TlsOptions);

                    if (ws is not null)
                    {
                        string normalized = NormalizePath(requestPath);
                        if (_routes.TryGetValue(normalized, out var route))
                        {
                            ClientConnection clientConnection = new(ws,
                                                                    CommandDecoder,
                                                                    route.dispatcher,
                                                                    route.provider,
                                                                    Logger,
                                                                    JsonSchemaValidator);
                            var task = clientConnection.RunAsync(token);
                            _allConnections.Add((task, clientConnection, normalized));
                            if (_serviceConnections.TryGetValue(normalized, out var connList))
                            {
                                lock (connList)
                                {
                                    connList.Add((task, clientConnection));
                                }
                            }
                        }
                        else
                        {
                            Logger.Warning(Constants.Component, $"No service registered for path '{requestPath}' — closing connection");
                            ws.Abort();
                            tcpClient.Dispose();
                        }
                    }
                    else
                    {
                        tcpClient.Dispose();
                    }

                }
            }
            TcpListener.Stop();
        }

        public void SetJsonSchemaValidator(IJsonSchemaValidator JsonSchemaValidator)
        {
            this.JsonSchemaValidator = JsonSchemaValidator;
        }

        public void SetTlsOptions(SslServerAuthenticationOptions tlsOptions)
        {
            TlsOptions = tlsOptions;
        }

        public void Dispose()
        {
            TcpListener.Stop();
        }

        private static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return "/";
            return path.TrimEnd('/') + "/";
        }

        private readonly Dictionary<string, (ICommandDispatcher dispatcher, IServiceProvider provider)> _routes = new();
        private readonly Dictionary<string, List<(Task task, IConnection connection)>> _serviceConnections = new();
        private readonly List<(Task task, IConnection connection, string path)> _allConnections = new();
        private readonly TcpListener TcpListener;
        private readonly IMessageDecoder CommandDecoder;
        private readonly ILogger Logger;
        private IJsonSchemaValidator JsonSchemaValidator;
        private SslServerAuthenticationOptions TlsOptions;
    }
}
