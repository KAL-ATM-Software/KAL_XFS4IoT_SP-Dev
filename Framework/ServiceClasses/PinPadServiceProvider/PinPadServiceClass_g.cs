/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PinPadServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.PinPad;

namespace XFS4IoTServer
{
    public partial class PinPadServiceClass : IPinPadServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.GetPinBlockCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.GetPinBlockHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetPinBlockHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.GetQueryPCIPTSDeviceIdCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.GetQueryPCIPTSDeviceIdHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetQueryPCIPTSDeviceIdHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.LocalDESCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.LocalDESHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, LocalDESHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.LocalVisaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.LocalVisaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, LocalVisaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.MaintainPinCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.MaintainPinHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MaintainPinHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.PresentIDCCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.PresentIDCHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PresentIDCHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.PinPad.Commands.SetPinBlockDataCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.PinPad.SetPinBlockDataHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetPinBlockDataHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.GetPinBlock", typeof(XFS4IoT.PinPad.Commands.GetPinBlockCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.GetQueryPCIPTSDeviceId", typeof(XFS4IoT.PinPad.Commands.GetQueryPCIPTSDeviceIdCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.LocalDES", typeof(XFS4IoT.PinPad.Commands.LocalDESCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.LocalVisa", typeof(XFS4IoT.PinPad.Commands.LocalVisaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.MaintainPin", typeof(XFS4IoT.PinPad.Commands.MaintainPinCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.PresentIDC", typeof(XFS4IoT.PinPad.Commands.PresentIDCCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.Reset", typeof(XFS4IoT.PinPad.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "PinPad.SetPinBlockData", typeof(XFS4IoT.PinPad.Commands.SetPinBlockDataCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.GetPinBlock", typeof(XFS4IoT.PinPad.Completions.GetPinBlockCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.GetQueryPCIPTSDeviceId", typeof(XFS4IoT.PinPad.Completions.GetQueryPCIPTSDeviceIdCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.LocalDES", typeof(XFS4IoT.PinPad.Completions.LocalDESCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.LocalVisa", typeof(XFS4IoT.PinPad.Completions.LocalVisaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.MaintainPin", typeof(XFS4IoT.PinPad.Completions.MaintainPinCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.PresentIDC", typeof(XFS4IoT.PinPad.Completions.PresentIDCCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.Reset", typeof(XFS4IoT.PinPad.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "PinPad.SetPinBlockData", typeof(XFS4IoT.PinPad.Completions.SetPinBlockDataCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IPinPadDevice Device { get => ServiceProvider.Device.IsA<IPinPadDevice>(); }
    }
}
