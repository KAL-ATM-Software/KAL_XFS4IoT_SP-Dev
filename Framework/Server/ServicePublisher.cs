/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;
using XFS4IoT;

namespace XFS4IoTServer
{
    /// <summary>
    /// Server publisher is responsible for the service discovery 
    /// endpoint, and managing services. 
    /// </summary>
    /// <remarks> 
    /// </remarks>
    public sealed class ServicePublisher : CommandDispatcher, IServiceProvider, IDisposable
    {
        /// <summary>
        /// A new service publisher. 
        /// </summary>
        /// <remarks>
        /// The new service publisher will automatically bind to the next available port 
        /// (as defined by XFS4IoT.) 
        /// </remarks>
        /// <param name="Logger">To use for all logging</param>
        /// <param name="CommandDispatcher">For dispatching incomming command messages</param>
        public ServicePublisher(ILogger Logger)
            : base(new[] { XFSConstants.ServiceClass.Publisher }, Logger)
        {
            Logger.IsNotNull($"Invalid parameter received in the {nameof(ServicePublisher)} constructor. {nameof(Logger)}");

            foreach (int port in XFSConstants.PortRanges)
            {
                try
                {
                    // From the spec, valid URI are like: 
                    // wss://Terminal321.ATMNetwork.corporatenet:443/xfs4iot/v1.0 
                    // wss://192.168.21.43:5848/xfs4iot/v1.0/CardReader1 
                    // We're going to open a HTTP connection first, then upgrade to WSS, hence http://

                    // Service URI is configuration parameter
                    string serverAddressUri = ConfigurationManager.AppSettings.Get(Configurations.ServerAddressUri);
                    if (string.IsNullOrEmpty(serverAddressUri))
                    {
                        serverAddressUri = Configurations.Default.ServerAddressUri;
                    }
                    else
                    {
                        bool result = Regex.IsMatch(serverAddressUri, "^https?://[-_.!~*'()a-z0-9%]+$", RegexOptions.IgnoreCase);
                        result.IsTrue($"Invalid service URI is configured. URI must be with out port number. i.e. http(s)://Terminal321.ATMNetwork.corporatenet and no ");
                    }

                    Uri = new Uri($"{serverAddressUri}:{port}/xfs4iot/v1.0/");

                    string serverAddressWUri = Regex.Replace(serverAddressUri, "^http", "ws", RegexOptions.IgnoreCase);
                    WSUri = new Uri($"{serverAddressWUri}:{port}/xfs4iot/v1.0/");
           
                    EndpointDetails = new EndpointDetails(serverAddressUri, serverAddressWUri, port);

                    Logger.Log(Constants.Component, $"Attempting to bind to {Uri}");

                    EndPoint = new EndPoint(Uri,
                                            CommandDecoder: CommandDecoder,
                                            CommandDispatcher: this,
                                            Logger);

                    _Services.Add(this);
                    return;
                }
                catch (System.Net.HttpListenerException)
                {
                    continue;
                }
            }
            // If we excape from the loop then we've run out of things to try and 
            // we've failed. Time to die. 
            Contracts.Fail($"Can't find an XFS4IoT port to listen on");
        }

        public async Task RunAsync()
        {
            var thisTask = EndPoint.RunAsync();
            var otherTasks = from service in _Services
                             where service != this
                             select service.RunAsync();

            var allTasks = Enumerable.Append(otherTasks, thisTask);

            await Task.WhenAll(allTasks);
        }

        public void Dispose() => EndPoint.Dispose();
        
        public void Add(IServiceProvider Service) => _Services.Add(Service);

        public Task BroadcastEvent(object payload)
        {
            throw Contracts.Fail<Exception>("No broadcast events defined for the service publisher. Do not call BroadcastEvent on this class.");
        }

        public IEnumerable<IServiceProvider> Services { get => _Services; } 
        private readonly List<IServiceProvider> _Services = new List<IServiceProvider>(); 

        private readonly XFS4IoTServer.EndPoint EndPoint;

        // Autopopulate the CommandDecoder with reflection based on the Command attribute added
        // to the relevant classes. 
        private readonly MessageDecoder CommandDecoder = new MessageDecoder(MessageDecoder.AutoPopulateType.Command);

        /// <summary>
        /// Details relating to endpoints on this publisher. 
        /// </summary>
        /// <remarks>
        /// This can be used to find the specific Uri for services. 
        /// </remarks>
        public EndpointDetails EndpointDetails { get; set; }

        public string Name { get; } = String.Empty;
        public Uri Uri { get; }
        public Uri WSUri { get; }
        public IDevice Device { get => Contracts.Fail<IDevice>("A device object was requested from the Publisher service, but the publisher service does not have a device class"); }
    }

    /// <summary>
    /// Constants for only Server assembly
    /// </summary>
    internal static class Constants
    {
        public const string Component = "Server";
        public const string Framework = "Framework";
    }
}