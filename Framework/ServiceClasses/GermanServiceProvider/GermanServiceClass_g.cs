/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * GermanServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.German;

namespace XFS4IoTServer
{
    public partial class GermanServiceClass : IGermanServiceClass
    {

        public async Task HSMTDataChangedEvent(XFS4IoT.German.Events.HSMTDataChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.German.Events.HSMTDataChangedEvent(Payload));

        public async Task OPTRequiredEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.German.Events.OPTRequiredEvent());

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.German.Commands.GetHSMTDataCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.German.GetHSMTDataHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetHSMTDataHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.German.Commands.HSMInitCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.German.HSMInitHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, HSMInitHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.German.Commands.SecureMsgReceiveCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.German.SecureMsgReceiveHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SecureMsgReceiveHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.German.Commands.SecureMsgSendCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.German.SecureMsgSendHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SecureMsgSendHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.German.Commands.SetHSMTDataCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.German.SetHSMTDataHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetHSMTDataHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "German.GetHSMTData", typeof(XFS4IoT.German.Commands.GetHSMTDataCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "German.HSMInit", typeof(XFS4IoT.German.Commands.HSMInitCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "German.SecureMsgReceive", typeof(XFS4IoT.German.Commands.SecureMsgReceiveCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "German.SecureMsgSend", typeof(XFS4IoT.German.Commands.SecureMsgSendCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "German.SetHSMTData", typeof(XFS4IoT.German.Commands.SetHSMTDataCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "German.GetHSMTData", typeof(XFS4IoT.German.Completions.GetHSMTDataCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "German.HSMInit", typeof(XFS4IoT.German.Completions.HSMInitCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "German.SecureMsgReceive", typeof(XFS4IoT.German.Completions.SecureMsgReceiveCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "German.SecureMsgSend", typeof(XFS4IoT.German.Completions.SecureMsgSendCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "German.SetHSMTData", typeof(XFS4IoT.German.Completions.SetHSMTDataCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "German.HSMTDataChangedEvent", typeof(XFS4IoT.German.Events.HSMTDataChangedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "German.OPTRequiredEvent", typeof(XFS4IoT.German.Events.OPTRequiredEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IGermanDevice Device { get => ServiceProvider.Device.IsA<IGermanDevice>(); }
    }
}
