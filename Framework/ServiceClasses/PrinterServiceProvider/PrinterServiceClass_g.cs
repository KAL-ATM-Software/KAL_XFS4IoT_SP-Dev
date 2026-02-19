/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrinterServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Printer;

namespace XFS4IoTServer
{
    public partial class PrinterServiceClass : IPrinterServiceClass
    {

        public async Task MediaTakenEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.MediaTakenEvent());

        public async Task MediaInsertedUnsolicitedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.MediaInsertedUnsolicitedEvent());

        public async Task MediaPresentedUnsolicitedEvent(XFS4IoT.Printer.Events.MediaPresentedUnsolicitedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.MediaPresentedUnsolicitedEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.MediaDetectedEvent(Payload));

        public async Task MediaAutoRetractedEvent(XFS4IoT.Printer.Events.MediaAutoRetractedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.MediaAutoRetractedEvent(Payload));

        public async Task PaperThresholdEvent(XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.PaperThresholdEvent(Payload));

        public async Task TonerThresholdEvent(XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.TonerThresholdEvent(Payload));

        public async Task LampThresholdEvent(XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.LampThresholdEvent(Payload));

        public async Task InkThresholdEvent(XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Printer.Events.InkThresholdEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.ClearBufferCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.ClearBufferHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ClearBufferHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.ControlMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.ControlMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ControlMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.ControlPassbookCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.ControlPassbookHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ControlPassbookHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.DispensePaperCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.DispensePaperHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DispensePaperHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.GetFormListCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.GetFormListHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetFormListHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.GetMediaListCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.GetMediaListHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetMediaListHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.GetQueryFieldCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.GetQueryFieldHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetQueryFieldHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.GetQueryFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.GetQueryFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetQueryFormHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.GetQueryMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.GetQueryMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetQueryMediaHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.MediaExtentsCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.MediaExtentsHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MediaExtentsHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.PrintFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.PrintFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PrintFormHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.PrintNativeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.PrintNativeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PrintNativeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.PrintRawCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.PrintRawHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PrintRawHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.ReadFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.ReadFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadFormHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.ReadImageCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.ReadImageHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadImageHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.RetractMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.RetractMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, RetractMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.SetBlackMarkModeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.SetBlackMarkModeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetBlackMarkModeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.SetFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.SetFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetFormHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.SetMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.SetMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Printer.Commands.SupplyReplenishCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Printer.SupplyReplenishHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SupplyReplenishHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.ClearBuffer", typeof(XFS4IoT.Printer.Commands.ClearBufferCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.ControlMedia", typeof(XFS4IoT.Printer.Commands.ControlMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.ControlPassbook", typeof(XFS4IoT.Printer.Commands.ControlPassbookCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.DispensePaper", typeof(XFS4IoT.Printer.Commands.DispensePaperCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.GetFormList", typeof(XFS4IoT.Printer.Commands.GetFormListCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.GetMediaList", typeof(XFS4IoT.Printer.Commands.GetMediaListCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.GetQueryField", typeof(XFS4IoT.Printer.Commands.GetQueryFieldCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.GetQueryForm", typeof(XFS4IoT.Printer.Commands.GetQueryFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.GetQueryMedia", typeof(XFS4IoT.Printer.Commands.GetQueryMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.MediaExtents", typeof(XFS4IoT.Printer.Commands.MediaExtentsCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.PrintForm", typeof(XFS4IoT.Printer.Commands.PrintFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.PrintNative", typeof(XFS4IoT.Printer.Commands.PrintNativeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.PrintRaw", typeof(XFS4IoT.Printer.Commands.PrintRawCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.ReadForm", typeof(XFS4IoT.Printer.Commands.ReadFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.ReadImage", typeof(XFS4IoT.Printer.Commands.ReadImageCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.Reset", typeof(XFS4IoT.Printer.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.RetractMedia", typeof(XFS4IoT.Printer.Commands.RetractMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.SetBlackMarkMode", typeof(XFS4IoT.Printer.Commands.SetBlackMarkModeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.SetForm", typeof(XFS4IoT.Printer.Commands.SetFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.SetMedia", typeof(XFS4IoT.Printer.Commands.SetMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Printer.SupplyReplenish", typeof(XFS4IoT.Printer.Commands.SupplyReplenishCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.ClearBuffer", typeof(XFS4IoT.Printer.Completions.ClearBufferCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.ControlMedia", typeof(XFS4IoT.Printer.Completions.ControlMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.ControlPassbook", typeof(XFS4IoT.Printer.Completions.ControlPassbookCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.DispensePaper", typeof(XFS4IoT.Printer.Completions.DispensePaperCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.GetFormList", typeof(XFS4IoT.Printer.Completions.GetFormListCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.GetMediaList", typeof(XFS4IoT.Printer.Completions.GetMediaListCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.GetQueryField", typeof(XFS4IoT.Printer.Completions.GetQueryFieldCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.GetQueryForm", typeof(XFS4IoT.Printer.Completions.GetQueryFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.GetQueryMedia", typeof(XFS4IoT.Printer.Completions.GetQueryMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.MediaExtents", typeof(XFS4IoT.Printer.Completions.MediaExtentsCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.PrintForm", typeof(XFS4IoT.Printer.Completions.PrintFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.PrintNative", typeof(XFS4IoT.Printer.Completions.PrintNativeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.PrintRaw", typeof(XFS4IoT.Printer.Completions.PrintRawCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.ReadForm", typeof(XFS4IoT.Printer.Completions.ReadFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.ReadImage", typeof(XFS4IoT.Printer.Completions.ReadImageCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.Reset", typeof(XFS4IoT.Printer.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.RetractMedia", typeof(XFS4IoT.Printer.Completions.RetractMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.SetBlackMarkMode", typeof(XFS4IoT.Printer.Completions.SetBlackMarkModeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.SetForm", typeof(XFS4IoT.Printer.Completions.SetFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.SetMedia", typeof(XFS4IoT.Printer.Completions.SetMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Printer.SupplyReplenish", typeof(XFS4IoT.Printer.Completions.SupplyReplenishCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.FieldErrorEvent", typeof(XFS4IoT.Printer.Events.FieldErrorEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.FieldWarningEvent", typeof(XFS4IoT.Printer.Events.FieldWarningEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.InkThresholdEvent", typeof(XFS4IoT.Printer.Events.InkThresholdEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.LampThresholdEvent", typeof(XFS4IoT.Printer.Events.LampThresholdEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaAutoRetractedEvent", typeof(XFS4IoT.Printer.Events.MediaAutoRetractedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaDetectedEvent", typeof(XFS4IoT.Printer.Events.MediaDetectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaInsertedEvent", typeof(XFS4IoT.Printer.Events.MediaInsertedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaInsertedUnsolicitedEvent", typeof(XFS4IoT.Printer.Events.MediaInsertedUnsolicitedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaPresentedEvent", typeof(XFS4IoT.Printer.Events.MediaPresentedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaPresentedUnsolicitedEvent", typeof(XFS4IoT.Printer.Events.MediaPresentedUnsolicitedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaRejectedEvent", typeof(XFS4IoT.Printer.Events.MediaRejectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.MediaTakenEvent", typeof(XFS4IoT.Printer.Events.MediaTakenEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.NoMediaEvent", typeof(XFS4IoT.Printer.Events.NoMediaEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.PaperThresholdEvent", typeof(XFS4IoT.Printer.Events.PaperThresholdEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Printer.TonerThresholdEvent", typeof(XFS4IoT.Printer.Events.TonerThresholdEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IPrinterDevice Device { get => ServiceProvider.Device.IsA<IPrinterDevice>(); }
    }
}
