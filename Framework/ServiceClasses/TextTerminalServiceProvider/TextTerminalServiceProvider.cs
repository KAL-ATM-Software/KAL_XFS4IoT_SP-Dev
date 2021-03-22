/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.TextTerminal;

namespace XFS4IoTTextTerminal
{
    public partial class TextTerminalServiceProvider : CommandDispatcher, XFS4IoTServer.IServiceProvider
    {
        public TextTerminalServiceProvider(XFS4IoTServer.EndpointDetails EndpointDetails, ITextTerminalDevice Device, ILogger Logger)
            : base(typeof(TextTerminalServiceProvider), Logger)
        {
            EndpointDetails.IsNotNull($"The endpoint details are invalid. {nameof(EndpointDetails)}");
            Device.IsNotNull($"The device interface is an invalid. {nameof(Device)}");

            this.Device = Device;

            (Uri, WSUri) = EndpointDetails.ServiceUri(ServiceClass);

            Logger.Log(Constants.Framework, $"Listening on {Uri}");

            this.EndPoint = new EndPoint(Uri,
                                         CommandDecoder,
                                         this,
                                         Logger);
        }

        public async Task RunAsync() => await EndPoint.RunAsync();

        public string Name { get; } = Constants.DeviceName;
        private readonly XFS4IoTServer.EndPoint EndPoint;

        private MessageDecoder CommandDecoder { get; } = new MessageDecoder(MessageDecoder.AutoPopulateType.Command);
        public Uri Uri { get; }
        public Uri WSUri { get; }
        public XFSConstants.ServiceClass ServiceClass { get; } = XFSConstants.ServiceClass.TextTerminal;
        public ITextTerminalDevice Device { get; internal set; }

    }

    /// <summary>
    /// Constants for only TextTerminal framework assembly
    /// </summary>
    internal static class Constants
    {
        public const string Framework = "Framework";
        public const string DeviceName = "TextTerminal";
        public const string DeviceClass = "DevClass";
    }
}
