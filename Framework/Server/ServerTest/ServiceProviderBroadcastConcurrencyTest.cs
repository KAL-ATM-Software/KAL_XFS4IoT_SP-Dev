/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTServerTest
{
	[TestClass]
	public class ServiceProviderBroadcastConcurrencyTest
	{
		[TestMethod]
		public async Task BroadcastEventToleratesConcurrentConnectionChangesAndSendFailures()
		{
			int port = GetAvailablePort();

			using XFS4IoTServer.EndPoint endPoint = new(
				new Uri($"http://localhost:{port}/xfs4iot/v1.0/"),
				new MessageDecoder(),
				new TestLogger(),
				null);

			XFS4IoTServer.ServiceProvider serviceProvider = new(
				new EndpointDetails("http://localhost", "ws://localhost", port),
				nameof(XFSConstants.ServiceClass.CardReader),
				[XFSConstants.ServiceClass.CardReader],
				new TestDevice(),
				new TestLogger(),
				null);

			// Mirrors ServicePublisher.Add(): register the service on the shared EndPoint
			// and wire ServiceProvider.BroadcastEvent to read connections from it.
			string normalizedPath = NormalizePath(serviceProvider.Uri.AbsolutePath);
			endPoint.Register(serviceProvider.Uri.AbsolutePath, serviceProvider, serviceProvider);
			SetConnectionSource(serviceProvider, () => endPoint.GetConnections(normalizedPath));

			List<(Task task, IConnection connection)> connectionList = GetServiceConnectionList(endPoint, normalizedPath);

			object payload = new();
			RecordingConnection liveConnection = new();
			RecordingConnection throwingConnection = new(throwOnSend: true);
			using CancellationTokenSource cancellation = new();

			try
			{
				lock (connectionList)
				{
					connectionList.Add((Task.CompletedTask, (IConnection)liveConnection));
					connectionList.Add((Task.CompletedTask, (IConnection)throwingConnection));
				}

				Task mutateConnections = Task.Run(() => MutateConnections(connectionList, cancellation.Token));

				for (int i = 0; i < 1000; i++)
				{
					await serviceProvider.BroadcastEvent(payload);
					await serviceProvider.BroadcastEvent(new IConnection[] { liveConnection, throwingConnection }, payload);
				}

				cancellation.Cancel();
				await mutateConnections;

				Assert.IsTrue(liveConnection.SendCount > 0);
				Assert.AreSame(payload, liveConnection.LastMessage);
			}
			finally
			{
				cancellation.Cancel();
			}
		}

		private static void MutateConnections(List<(Task task, IConnection connection)> connectionList, CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				RecordingConnection transientConnection = new();
				(Task task, IConnection connection) detail = (Task.CompletedTask, transientConnection);

				lock (connectionList)
				{
					connectionList.Add(detail);
					connectionList.Remove(detail);
				}
			}
		}

		// Matches EndPoint's private static NormalizePath, which keys _serviceConnections.
		private static string NormalizePath(string path)
		{
			if (string.IsNullOrEmpty(path)) return "/";
			return path.TrimEnd('/') + "/";
		}

		// ServiceProvider.SetConnectionSource is internal (set normally by ServicePublisher.Add());
		// invoke it via reflection since this test constructs the EndPoint directly instead.
		private static void SetConnectionSource(XFS4IoTServer.ServiceProvider serviceProvider, Func<IEnumerable<IConnection>> source)
		{
			MethodInfo method = typeof(XFS4IoTServer.ServiceProvider).GetMethod("SetConnectionSource", BindingFlags.Instance | BindingFlags.NonPublic);
			method.Invoke(serviceProvider, new object[] { source });
		}

		// EndPoint.GetConnections(path) locks and reads from this same per-path list, so mutating
		// it here exercises the real synchronization the production accept loop relies on.
		private static List<(Task task, IConnection connection)> GetServiceConnectionList(XFS4IoTServer.EndPoint endPoint, string normalizedPath)
		{
			var serviceConnections = (ConcurrentDictionary<string, List<(Task task, IConnection connection)>>)
				GetPrivateField(typeof(XFS4IoTServer.EndPoint), endPoint, "_serviceConnections");
			return serviceConnections[normalizedPath];
		}

		private static object GetPrivateField(Type type, object instance, string name)
		{
			FieldInfo typeField = type?.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
			object value = typeField?.GetValue(instance);
			return value;
		}

		private static int GetAvailablePort()
		{
			TcpListener listener = new(IPAddress.Loopback, 0);
			listener.Start();
			int port = ((IPEndPoint)listener.LocalEndpoint).Port;
			listener.Stop();
			return port;
		}

		private sealed class RecordingConnection : IConnection
		{
			public RecordingConnection(bool throwOnSend = false)
			{
				this.throwOnSend = throwOnSend;
			}

			public int SendCount { get; private set; }
			public object LastMessage { get; private set; }

			public Task SendMessageAsync(object message)
			{
				if (throwOnSend)
				{
					throw new InvalidOperationException("Connection closed.");
				}

				SendCount++;
				LastMessage = message;
				return Task.CompletedTask;
			}

			private readonly bool throwOnSend;
		}

		private sealed class TestDevice : IDevice
		{
			public Task RunAsync(CancellationToken token) => Task.CompletedTask;

			public XFS4IoTServer.IServiceProvider SetServiceProvider { get; set; }
		}

		private sealed class TestLogger : ILogger
		{
			public void Log(string SubSystem, string Message) { }

			public void LogSensitive(string SubSystem, string Message) { }

			public void Trace(string SubSystem, string Operation, string Message) { }

			public void TraceSensitive(string SubSystem, string Operation, string Message) { }

			public void Warning(string SubSystem, string Message) { }

			public void WarningSensitive(string SubSystem, string Message) { }
		}
	}
}
