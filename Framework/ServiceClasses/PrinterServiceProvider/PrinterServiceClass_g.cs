﻿/***********************************************************************************************\
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

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IPrinterDevice Device { get => ServiceProvider.Device.IsA<IPrinterDevice>(); }
    }
}
