/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2026
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace XFS4IoTServerTest
{
	[TestClass]
	public class TcpWebSocketFrameEncodingTest
	{
		/// <summary>
		/// Test for a frame-length encoding in the TcpWebSocket class.
		/// </summary>
		[TestMethod]
		public async Task SendFrameAsyncEncodesExtendedLengthCorrectlyForLargePayloads()
		{
			byte[] payload = new byte[141384];
			new Random(42).NextBytes(payload);

			byte[] frame = await SendFrameAndCaptureBytes(payload);

			Assert.AreEqual(10, GetHeaderLength(frame), "Header length should be 2 base bytes + 8 extended-length bytes for payloads >= 65536 bytes.");

			long declaredLength = 0;
			for (int i = 2; i < 10; i++)
			{
				declaredLength = (declaredLength << 8) | frame[i];
			}
			Assert.AreEqual(payload.Length, declaredLength, "Declared frame length must equal the actual payload length.");

			Assert.AreEqual(10 + payload.Length, frame.Length, "Frame must contain the header plus the entire payload.");

			for (int i = 0; i < payload.Length; i++)
			{
				Assert.AreEqual(payload[i], frame[10 + i], $"Payload byte at offset {i} was not written correctly.");
			}
		}

		[TestMethod]
		public async Task SendFrameAsyncStillUses16BitLengthUnderThreshold()
		{
			byte[] payload = new byte[70000];
			new Random(7).NextBytes(payload);

			byte[] frame = await SendFrameAndCaptureBytes(payload);

			Assert.AreEqual(10, GetHeaderLength(frame));

			long declaredLength = 0;
			for (int i = 2; i < 10; i++)
			{
				declaredLength = (declaredLength << 8) | frame[i];
			}
			Assert.AreEqual(payload.Length, declaredLength);
		}

		private static int GetHeaderLength(byte[] frame)
		{
			int lengthByte = frame[1] & 0x7F;
			return lengthByte switch
			{
				127 => 10,
				126 => 4,
				_ => 2,
			};
		}

		// TcpWebSocket is internal; reach SendFrameAsync via reflection since this test project
		// has no InternalsVisibleTo relationship with the Server assembly.
		private static async Task<byte[]> SendFrameAndCaptureBytes(byte[] payload)
		{
			Type tcpWebSocketType = typeof(XFS4IoTServer.EndPoint).Assembly.GetType("XFS4IoTServer.TcpWebSocket");
			ConstructorInfo ctor = tcpWebSocketType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Stream) }, null);

			using MemoryStream stream = new();
			object tcpWebSocket = ctor.Invoke([stream]);

			MethodInfo sendFrameAsync = tcpWebSocketType.GetMethod("SendFrameAsync", BindingFlags.NonPublic | BindingFlags.Instance);
			Task sendTask = (Task)sendFrameAsync.Invoke(tcpWebSocket, [0x1, (ReadOnlyMemory<byte>)payload, true, CancellationToken.None]);
			await sendTask;

			return stream.ToArray();
		}
	}
}
