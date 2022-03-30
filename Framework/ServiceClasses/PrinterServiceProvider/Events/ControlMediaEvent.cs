/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;
using XFS4IoT.Printer.Events;

namespace XFS4IoTFramework.Printer
{
    public class ControlMediaEvent
    {
        public ControlMediaEvent(IControlMediaEvents MediaEvent)
        {
            ControlMedia = MediaEvent;
        }
        public ControlMediaEvent(IPrintFormEvents MediaEvent)
        {
            PrintForm = MediaEvent;
        }

        public async Task MediaPresentedEvent(MediaPresentedEvent.PayloadData Payload)
        {
            Contracts.Assert(ControlMedia is not null || PrintForm is not null, $"Not control media event interface set.");

            if (ControlMedia is not null)
                await ControlMedia.MediaPresentedEvent(Payload);
            if (PrintForm is not null)
                await PrintForm.MediaPresentedEvent(Payload);
        }

        private IControlMediaEvents ControlMedia { get; init; } = null;
        private IPrintFormEvents PrintForm { get; init; } = null;
    }
}
