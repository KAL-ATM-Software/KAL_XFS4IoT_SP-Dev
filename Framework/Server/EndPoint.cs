/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    /// <summary>
    /// Endpoint that will receive and process commands from a client
    /// </summary>
    public sealed class EndPoint : IDisposable
    {
        public EndPoint(Uri EndPointUri, 
                        IMessageDecoder CommandDecoder,
                        ICommandDispatcher CommandDispatcher,
                        ILogger Logger)
        {
            EndPointUri.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(EndPointUri)}");
            CommandDecoder.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(CommandDecoder)}");
            CommandDispatcher.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(CommandDispatcher)}");
            Logger.IsNotNull($"Invalid parameter received in the {nameof(EndPoint)} constructor. {nameof(Logger)}");

            this.CommandDecoder = CommandDecoder;
            this.CommandDispatcher = CommandDispatcher;
            this.Logger = Logger;

            HttpListener = new HttpListener()
            {
                IgnoreWriteExceptions = false,
                Prefixes = { EndPointUri.ToString() },
            };

            HttpListener.Start();

            Logger.Log(Constants.Component, $"New endpoint at {EndPointUri.ToString()}");
        }

        private readonly HttpListener HttpListener;

        public async Task RunAsync()
        {
            var tasks = new List<Task>();
            Task<HttpListenerContext> listenerTask = HttpListener.GetContextAsync();
            while (true)
            {
                // Wait until client connects
                // We need to let client connections keep running, so await everything. 

                Logger.Log(Constants.Component, $"{HttpListener.Prefixes.First()} listing for new connections and on {tasks.Count} existing connections");

                // And wait for something to happen. Note that multiple things can happen at the 
                // same time. 
                Task completedTask = await Task.WhenAny(Enumerable.Append(tasks, listenerTask ));

                // If it's one of the client connection tasks ending then we just need to stop waiting for it.  
                if (completedTask != listenerTask)
                {
                    tasks.Remove(completedTask);
                }
                // If we got a new connection we need to start handling that connection, and start 
                // listening for new connections. 
                else
                {
                    // Otherwise we know it's a new connection we need to handle. 
                    HttpListenerContext context = listenerTask.Result;

                    if (context.Request.IsWebSocketRequest)
                    {
                        // Turn the HTTP connection into a websocket connection
                        var client = (await context.AcceptWebSocketAsync(null)).WebSocket;

                        // Create the client connection and run it. 
                        tasks.Add(new ClientConnection(client,
                                                    CommandDecoder,
                                                    CommandDispatcher,
                                                    Logger)
                              .RunAsync()
                              );
                    }
                    else
                    {
                        // Return empty body in case http message and return bad request as we only expect websocket
                        // The 400 Bad Request error is an HTTP status code that means that the request you sent to the website server, 
                        // often something simple like a request to load a web page, was somehow incorrect or corrupted and the server couldn't understand it.
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                    // Restart listening for new connections. 
                    listenerTask = HttpListener.GetContextAsync();
                }
            }
        }

        private readonly IMessageDecoder CommandDecoder;
        private readonly ICommandDispatcher CommandDispatcher;
        private readonly ILogger Logger;

        public void Dispose()
        {
            HttpListener.Close();
        }
    }
}
