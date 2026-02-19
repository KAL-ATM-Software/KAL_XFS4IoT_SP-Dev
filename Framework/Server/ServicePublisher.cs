/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.ServicePublisher.Commands;
using XFS4IoT.ServicePublisher.Completions;

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
        /// <param name="ServiceConfiguration">To get service configuration</param>
        /// <param name="JsonSchemaValidator">3rd party JSON schema validator can be used</param>
        public ServicePublisher(ILogger Logger, 
                                IServiceConfiguration ServiceConfiguration,
                                IJsonSchemaValidator JsonSchemaValidator = null)
            : base([XFSConstants.ServiceClass.Publisher ], Logger)
        {
            Logger.IsNotNull($"Invalid parameter received in the {nameof(ServicePublisher)} constructor. {nameof(Logger)}");

            if (ServiceConfiguration is null)
            {
                Logger.Log(Constants.Framework, $"No configuration object is set and use default value. {Configurations.ServerAddressUri}");
            }

            this.JsonSchemaValidator = JsonSchemaValidator;

            // Register command handler for ServicePublisher service.
            AddHandler(this, typeof(XFS4IoT.ServicePublisher.Commands.GetServicesCommand), (connection, dispatcher, logger) => new GetServiceHandler(connection, dispatcher, logger), true);
            // Register service publisher messages explicitly.
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "ServicePublisher.GetServices", typeof(GetServicesCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "ServicePublisher.GetServices", typeof(GetServicesCommand));
            
            // Set service publisher specific command name and version to the dispacher.
            Type type = typeof(XFS4IoT.ServicePublisher.Commands.GetServicesCommand);
            CommandAttribute commandAttrib = Attribute.GetCustomAttribute(type, typeof(CommandAttribute)) as CommandAttribute;
            commandAttrib.IsNotNull($"Internal command object XFS4IoT.ServicePublisher.Commands.GetServicesCommand has no Command attribute.");
            
            XFS4VersionAttribute versionAttrib = Attribute.GetCustomAttribute(type, typeof(XFS4VersionAttribute)) as XFS4VersionAttribute;
            versionAttrib.IsNotNull($"Internal command object XFS4IoT.ServicePublisher.Commands.GetServicesCommand has no XFS4Version attribute.");

            SetMessagesSupported(new()
            {
                { commandAttrib.Name, new(MessageTypeInfo.MessageTypeEnum.Command, [versionAttrib.Version]) }
            });

            // Service URI is configuration parameter
            string serverAddressUri = ServiceConfiguration?.Get(Configurations.ServerAddressUri);
            if (string.IsNullOrEmpty(serverAddressUri))
            {
                Logger.Log(Constants.Framework, $"No configuration value '{serverAddressUri}' exists and use default value. {Configurations.ServerAddressUri}");
                serverAddressUri = Configurations.Default.ServerAddressUri;
            }
            else
            {
                bool result = Regex.IsMatch(serverAddressUri, "^https?://[-_.!~*'()a-z0-9%]+$", RegexOptions.IgnoreCase);
                result.IsTrue($"Invalid service URI is configured. URI must be with out port number. i.e. http(s)://Terminal321.ATMNetwork.corporatenet and no ");
            }

            List<int> portRanges = XFSConstants.PortRanges.ToList();
            string specificPort = ServiceConfiguration?.Get(Configurations.ServerPort);
            if (!string.IsNullOrEmpty(specificPort))
            {
                if (int.TryParse(specificPort, out int port))
                {
                    if (portRanges.Contains(port))
                    {
                        portRanges.Remove(port);
                        portRanges.Insert(0, port);
                    }
                    else
                    {
                        Logger.Warning(Constants.Framework, $"Invalid configuration for '{Configurations.ServerPort}'. Valid port number is 80, 443 or 5846-5856");
                    }
                }
            }

            if (serverAddressUri.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // Remove port 80 for secure channel.
                if (portRanges.Contains(80))
                {
                    portRanges.Remove(80);
                }
            }

            foreach (int port in portRanges)
            {
                try
                {
                    // From the spec, valid URI are like: 
                    // wss://Terminal321.ATMNetwork.corporatenet:443/xfs4iot/v1.0 
                    // wss://192.168.21.43:5848/xfs4iot/v1.0/CardReader1 
                    // We're going to open a HTTP connection first, then upgrade to WSS, hence http://

                    Uri = new Uri($"{serverAddressUri}:{port}/xfs4iot/v1.0/");

                    string serverAddressWUri = Regex.Replace(serverAddressUri, "^http", "ws", RegexOptions.IgnoreCase);
                    WSUri = new Uri($"{serverAddressWUri}:{port}/xfs4iot/v1.0/");
           
                    EndpointDetails = new EndpointDetails(serverAddressUri, serverAddressWUri, port);

                    Logger.Log(Constants.Component, $"Attempting to bind to {Uri.OriginalString}");

                    EndPoint = new EndPoint(Uri,
                                            CommandDecoder: CommandDecoder,
                                            CommandDispatcher: this,
                                            ServiceProvider: this,
                                            Logger);

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

        public async override Task RunAsync(CancellationSource cancellationSource)
        {
            try
            {
                if (JsonSchemaValidator is not null)
                {
                    await JsonSchemaValidator.LoadSchemaAsync();
                }
                EndPoint.SetJsonSchemaValidator(JsonSchemaValidator);

                var thisTask = Task.WhenAll(EndPoint.RunAsync(cancellationSource.Token), base.RunAsync(cancellationSource));
                var otherTasks = from service in _Services
                                 select service.RunAsync(cancellationSource);

                var allTasks = Enumerable.Append(otherTasks, thisTask);

                await Task.WhenAll(allTasks);
            }
            catch (Exception ex)
            {
                Logger.Warning(Constants.Component, $"Caught an exception thrown in {nameof(ServicePublisher)}: {ex.Message}");
                // It's a critical error that it cannot recover from and terminate this process.
                Environment.FailFast($"Caught an exception thrown in {nameof(ServicePublisher)}.", ex);
            }
        }

        public void Dispose() => EndPoint.Dispose();

        public void Add(IServiceProvider Service)
        {
            _Services.Add(Service);
            Service.SetJsonSchemaValidator(JsonSchemaValidator);
        }

        public Task BroadcastEvent(object payload)
        {
            throw Contracts.Fail<Exception>($"No broadcast events defined for the service publisher. Do not call {nameof(BroadcastEvent)} on this class.");
        }

        public Task BroadcastEvent(IEnumerable<IConnection> connections, object payload)
        {
            throw Contracts.Fail<Exception>($"No broadcast events defined for the service publisher. Do not call {nameof(BroadcastEvent)} on this class.");
        }

        public IEnumerable<IServiceProvider> Services { get => _Services; } 
        private readonly List<IServiceProvider> _Services = []; 

        private readonly XFS4IoTServer.EndPoint EndPoint;

        // Autopopulate the CommandDecoder with reflection based on the Command attribute added
        // to the relevant classes. 
        private readonly MessageDecoder CommandDecoder = new MessageDecoder();

        // JSON schema validator
        private IJsonSchemaValidator JsonSchemaValidator;

        public void SetJsonSchemaValidator(IJsonSchemaValidator JsonSchemaValidator)
        {
            this.JsonSchemaValidator = JsonSchemaValidator;
        }

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

        /// <summary>
        /// Set supported commands and events to the dispatcher.
        /// </summary>
        /// <param name="MessagesSupported"></param>
        public void SetMessagesSupported(Dictionary<string, MessageTypeInfo> MessagesSupported)
        {
            base.MessagesSupported = MessagesSupported;
        }
        public Dictionary<string, MessageTypeInfo> GetMessagesSupported() => base.MessagesSupported;
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