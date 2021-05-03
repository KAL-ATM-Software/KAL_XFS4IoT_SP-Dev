/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using XFS4IoT;
using System.Linq;
using System.Threading;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoT.CardReader.Events;
using System.Text.RegularExpressions;

namespace TestClient
{
    class TestClient
    {
        static async Task Main(string[] args)
        {
            await new TestClient().Run(args);
        }
        private async Task Run(string[] args)
        {
            try
            {
                Logger.LogLine("Running test XFS4IoT application.");

                bool ShowJSON = false;
                foreach (var (name, value) in from p in args
                                              let m = paramRegex.Match(p)
                                              select (m.Groups["name"].Value, m.Groups["value"].Value))
                {
                    switch (name.ToLower())
                    {
                        case "address"  : Address = value; break;
                        case "showjson" :
                            if (string.IsNullOrEmpty(value))
                                ShowJSON = true;
                            else if (bool.TryParse(value, out bool bValue))
                                ShowJSON = bValue;
                            else 
                                throw new Exception($"Invalid value {value}");
                            break;
                        default: throw new Exception($"unknown parameter{name}");
                    }
                }

                Logger.WriteJSON = ShowJSON; 

                Logger.LogLine("Doing service discovery.");
                await DoServiceDiscovery();

                Logger.LogLine("Connecting to the card reader");
                await OpenCardReader();

                Logger.LogLine("Get card reader status");
                await GetCardReaderStatus();

                Logger.LogLine("Doing accept card");
                await DoAcceptCard();

                Logger.LogLine($"Done");

                // Start listening for unsolicited messages from the server. 
                while (true)
                {
                    Logger.LogMessage(await cardReader.ReceiveMessageAsync());
                }
            }
            catch (WebSocketException e)
            {
                Logger.LogError($"Connection error {e.Message}");
                System.Diagnostics.Debugger.Break();
            }
            catch (Exception e)
            {
                Logger.LogError($"Unhandled exception: {e.Message}");
                System.Diagnostics.Debugger.Break();
            }
        }

        private static readonly Regex paramRegex = new("^[/-](?<name>.*?)(?:[=:](?<value>.*))?$");

        /// <summary>
        /// Messages that we expect to receive so that we can decode them. 
        /// </summary>
        private readonly MessageDecoder ResponseDecoder = new(MessageDecoder.AutoPopulateType.Response);

        private readonly ConsoleLogger Logger = new();
        private string Address { get; set; } = "localhost";

        private Uri CardReaderUri { get; set; }
        //public Uri PinPadUri { get; private set; }
        //public Uri PrinterUri { get; private set; }


        private async Task DoServiceDiscovery()
        {
            const int port = 5846;
            var Discovery = new XFS4IoTClient.ClientConnection(
                    EndPoint: new Uri($"ws://{Address}:{port}/xfs4iot/v1.0")
                    );

            try
            {
                await Discovery.ConnectAsync();
            }
            catch (Exception e)
            {
                Logger.LogLine($"Caught exception ... {e}");
                throw;
            }

            Logger.LogLine($"{nameof(GetServiceCommand)}", ConsoleColor.Blue);

            await Discovery.SendCommandAsync(new GetServiceCommand(Guid.NewGuid().ToString(), new GetServiceCommand.PayloadData(60000)));
            Logger.LogLine($"Waiting for response...");

            var message = await Discovery.ReceiveMessageAsync();
            Logger.LogMessage(message);
            if (message is GetServiceCompletion response) 
            {
                EndPointDetails(response.Payload);
                return;
            }
        }

        private void EndPointDetails(GetServiceCompletion.PayloadData endpointDetails)
        {
            Logger.LogLine($"Got endpoint details {endpointDetails}");
            Logger.LogLine($"Services:\n{string.Join("\n", from ep in endpointDetails.Services select ep.ServiceUri)}");

            CardReaderUri = FindServiceUri(endpointDetails, XFSConstants.ServiceClass.CardReader);
        }

        private static Uri FindServiceUri(GetServiceCompletion.PayloadData endpointDetails, XFSConstants.ServiceClass ServiceClass)
        {
            var service =
                (from ep in endpointDetails.Services
                 where ep.ServiceUri.Contains(ServiceClass.ToString())
                 select ep
                 ).FirstOrDefault()
                 ?.ServiceUri;

            return !string.IsNullOrEmpty(service) ? new Uri(service) : throw new Exception($"Failed to find a device {ServiceClass} endpoint");
        }

        private XFS4IoTClient.ClientConnection cardReader;
        private async Task OpenCardReader()
        {
            // Create the connection object. This doesn't start anything...  
            cardReader = new XFS4IoTClient.ClientConnection(
                    EndPoint: CardReaderUri ?? throw new NullReferenceException()
                    );

            // Open the actual network connection, with a timeout. 
            var cancel = new CancellationTokenSource();
            cancel.CancelAfter(10_000);
            await cardReader.ConnectAsync(cancel.Token);
        }

        private async Task GetCardReaderStatus()
        {
            Logger.LogLine($"{nameof(StatusCommand)}", ConsoleColor.Blue);

            // Create a new command and send it to the device
            var command = new StatusCommand(Guid.NewGuid().ToString(), new StatusCommand.PayloadData(Timeout: 1_000));
            await cardReader.SendCommandAsync(command);

            // Wait for a response from the device. 
            Logger.LogLine("Waiting for response... ");

            await GetCompletionAsync(typeof(StatusCompletion));
        }


        private async Task DoAcceptCard()
        {
            Logger.LogLine($"{nameof(ReadRawDataCommand)}", ConsoleColor.Blue);

            //MessageBox((IntPtr)0, "Send CardReader ReadRawData command to read chip card", "XFS4IoT Test Client", 0);
            // Create a new command and send it to the device
            var command = new ReadRawDataCommand(Guid.NewGuid().ToString(),
                                                 new ReadRawDataCommand.PayloadData(
                                                        60_000,
                                                        Track1: true,
                                                        Track2: true,
                                                        Track3: true,
                                                        Chip: true,
                                                        Security: false,
                                                        FluxInactive: false,
                                                        Watermark: false,
                                                        MemoryChip: false,
                                                        Track1Front: false,
                                                        FrontImage: false,
                                                        BackImage: false,
                                                        Track1JIS: false,
                                                        Track3JIS: false,
                                                        Ddi: false));
            await cardReader.SendCommandAsync(command);

            // Wait for a response from the device. 
            Logger.LogLine("Waiting for response... ");

            await GetCompletionAsync(typeof(ReadRawDataCompletion));
        }

        private async Task GetCompletionAsync( Type CompletionType )
        {
            while (true)
            {
                var Message = await cardReader.ReceiveMessageAsync();
                Logger.LogMessage(Message);
                if (Message.GetType() == CompletionType) return;
            }
        }

        /// <summary>
        /// Log to the console, including timing details. 
        /// </summary>
        private class ConsoleLogger
        {
            public ConsoleLogger()
            {
                defaultColour = Console.ForegroundColor;
            }
            public void Log(string v, ConsoleColor? colour = null)
            {
                Console.ForegroundColor = colour ?? defaultColour; 
                Console.Write($"{DateTime.Now:hh:mm:ss.ffff} ({DateTime.Now - Start}): {v}");
                Console.ForegroundColor = defaultColour;
            }
            public void Write(string v, ConsoleColor? colour = null)
            {
                Console.ForegroundColor = colour ?? defaultColour;
                Console.Write(v);
                Console.ForegroundColor = defaultColour;
            }
            public void LogLine(string v, ConsoleColor? colour = null)
            {
                Console.ForegroundColor = colour ?? defaultColour;
                Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffff} ({DateTime.Now - Start}): {v}");
                Console.ForegroundColor = defaultColour;
            }
            public void WriteLine(string v, ConsoleColor? colour = null)
            {
                Console.ForegroundColor = colour ?? defaultColour;
                Console.WriteLine(v);
                Console.ForegroundColor = defaultColour;
            }
            public void LogError(string v)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffff} ({DateTime.Now - Start}): {v}");
                Console.ForegroundColor = defaultColour;
            }
            public void LogWarning(string v)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffff} ({DateTime.Now - Start}): {v}");
                Console.ForegroundColor = defaultColour;
            }
            public void LogMessage(object Message)
            {
                switch (Message)
                {
                    case GetServiceCompletion getServiceCompletion:
                        LogMessage(nameof(GetServiceCompletion), ConsoleColor.Green, getServiceCompletion.Serialise());
                        break;

                    case ReadRawDataCompletion readRawDataCompletion:
                        LogMessage(nameof(ReadRawDataCompletion), ConsoleColor.Green, readRawDataCompletion.Serialise());
                        break;

                    case StatusCompletion statusCompletion:
                        LogMessage(nameof(StatusCompletion), ConsoleColor.Green, statusCompletion.Serialise());
                        break;

                    case InsertCardEvent insertCardEvent:
                        LogMessage(nameof(MediaInsertedEvent),ConsoleColor.Yellow, insertCardEvent.Serialise());
                        break;

                    case MediaInsertedEvent mediaInsertedEvent:
                        LogMessage(nameof(MediaInsertedEvent),ConsoleColor.Yellow, mediaInsertedEvent.Serialise());
                        break;

                    case null:
                        LogError($"Invalid response to {nameof(GetServiceCompletion)}");
                        break;

                    case object message:
                        LogError($"Invalid type of response {message.GetType()}");
                        break;
                }
            }

            private void LogMessage(string name, ConsoleColor colour, string JSON )
            {
                Log($"{name}", colour);
                if (WriteJSON)
                    WriteLine($" : {JSON}");
                else
                    WriteLine("");
            }

            public bool WriteJSON { private get; set; } = false;

            public void Restart() => Start = DateTime.Now;

            private DateTime Start = DateTime.Now;
            private readonly ConsoleColor defaultColour;
        }
    }
}
