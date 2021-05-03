/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * IPrinterEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Printer
{
    public interface IPrinterUnsolicitedEvents
    {

        Task RetractBinThresholdEvent(XFS4IoT.Printer.Events.RetractBinThresholdEvent.PayloadData Payload);

        Task MediaTakenEvent();

        Task PaperThresholdEvent(XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData Payload);

        Task TonerThresholdEvent(XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData Payload);

        Task InkThresholdEvent(XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData Payload);

        Task MediaAutoRetractedEvent(XFS4IoT.Printer.Events.MediaAutoRetractedEvent.PayloadData Payload);

        Task LampThresholdEvent(XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData Payload);

        Task MediaDetectedEvent(XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData Payload);

        Task DefinitionLoadedEvent(XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData Payload);

        Task MediaInsertedUnsolicitedEvent();

        Task MediaPresentedUnsolicitedEvent(XFS4IoT.Printer.Events.MediaPresentedUnsolicitedEvent.PayloadData Payload);

        Task RetractBinStatusEvent(XFS4IoT.Printer.Events.RetractBinStatusEvent.PayloadData Payload);

    }
}
