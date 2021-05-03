/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;

namespace Server
{
    class Server
    {
        static async Task Main(/*string[] args*/)
        {
            ConsoleLogger Logger = new();
            try
            {
                Logger.Log($"Running ServiceProvider Server");

                var Publisher = new ServicePublisher(Logger);
                var EndpointDetails = Publisher.EndpointDetails;

                var simCardReaderDevice = new KAL.XFS4IoTSP.CardReader.Sample.CardReaderSample(Logger);
                var cardReaderService = new CardReaderServiceProvider(EndpointDetails,
                                                                   ServiceName: "SimCardReader",
                                                                   simCardReaderDevice,
                                                                   Logger);
                simCardReaderDevice.SetServiceProvider = cardReaderService;
                Publisher.Add(cardReaderService);

                // TODO: adding other services
              
                await Publisher.RunAsync();
            }
            catch(Exception e) when (e.InnerException != null)
            {
                Logger.Warning($"Unhandled exception {e.InnerException.Message}");
            }
            catch(Exception e)
            {
                Logger.Warning($"Unhandled exception {e.Message}");
            }

        }

        /// <summary>
        /// Sample logger. This should be replaced with a more robust implementation. 
        /// </summary>
        private class ConsoleLogger : ILogger
        {
            public void Warning(string Message) => Warning("SvrHost", Message);
            public void Log(string Message) => Log("SvrHost", Message);

            public void Trace(string SubSystem, string Operation, string Message) => Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff} ({(DateTime.Now - Start).TotalSeconds:000.000}): {Message}");
           
            public void Warning(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

            public void Log(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

            public void TraceSensitive(string SubSystem, string Operation, string Message) => Trace(SubSystem, Operation, Message);

            public void WarningSensitive(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

            public void LogSensitive(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

            private readonly DateTime Start = DateTime.Now;
        }
    }
}
