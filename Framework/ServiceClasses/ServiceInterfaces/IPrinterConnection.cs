/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using XFS4IoTFramework.Common;

namespace Printer
{
    public interface IPrinterConnection : ICommonConnection
    {

        void RetractBinThresholdEvent(XFS4IoT.Printer.Events.RetractBinThresholdEvent.PayloadData Payload);

        void MediaTakenEvent();

        void PaperThresholdEvent(XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData Payload);

        void TonerThresholdEvent(XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData Payload);

        void InkThresholdEvent(XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData Payload);

        void MediaPresentedEvent(XFS4IoT.Printer.Events.MediaPresentedEvent.PayloadData Payload);

        void MediaAutoRetractedEvent(XFS4IoT.Printer.Events.MediaAutoRetractedEvent.PayloadData Payload);

        void NoMediaEvent(XFS4IoT.Printer.Events.NoMediaEvent.PayloadData Payload);

        void MediaInsertedEvent();

        void FieldErrorEvent(XFS4IoT.Printer.Events.FieldErrorEvent.PayloadData Payload);

        void FieldWarningEvent(XFS4IoT.Printer.Events.FieldWarningEvent.PayloadData Payload);

        void MediaRejectedEvent(XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData Payload);

        void LampThresholdEvent(XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData Payload);

        void MediaDetectedEvent(XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData Payload);

        void DefinitionLoadedEvent(XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData Payload);

        void MediaInsertedUnsolicitedEvent();

        void MediaPresentedUnsolicitedEvent(XFS4IoT.Printer.Events.MediaPresentedUnsolicitedEvent.PayloadData Payload);

        void RetractBinStatusEvent(XFS4IoT.Printer.Events.RetractBinStatusEvent.PayloadData Payload);

        void PowerSaveChangeEvent(XFS4IoT.Common.Events.PowerSaveChangeEvent.PayloadData Payload);

        void DevicePositionEvent(XFS4IoT.Common.Events.DevicePositionEvent.PayloadData Payload);

    }
}
