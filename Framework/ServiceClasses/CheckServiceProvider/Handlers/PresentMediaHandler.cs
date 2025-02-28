/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Check
{
    public partial class PresentMediaHandler
    {
        private async Task<CommandResult<PresentMediaCompletion.PayloadData>> HandlePresentMedia(IPresentMediaEvents events, PresentMediaCommand presentMedia, CancellationToken cancel)
        {
            PresentMediaRequest.PositionEnum position = PresentMediaRequest.PositionEnum.All;

            if (presentMedia.Payload?.Source is not null)
            {
                if (presentMedia.Payload?.Source.Position == XFS4IoT.Check.PresentMediaPositionEnum.Input &&
                    !Common.CheckScannerCapabilities.Positions.ContainsKey(CheckScannerCapabilitiesClass.PositionEnum.Input))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported source. Check Positions capability reported. {presentMedia.Payload?.Source.Position}");
                }
                if (presentMedia.Payload?.Source.Position == XFS4IoT.Check.PresentMediaPositionEnum.Refused &&
                    !Common.CheckScannerCapabilities.Positions.ContainsKey(CheckScannerCapabilitiesClass.PositionEnum.Refused))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported source. Check Positions capability reported. {presentMedia.Payload?.Source.Position}");
                }
                if (presentMedia.Payload?.Source.Position == XFS4IoT.Check.PresentMediaPositionEnum.Rebuncher &&
                    Common.CheckScannerStatus.ReBuncher != CheckScannerStatusClass.ReBuncherEnum.NotSupported)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported source. Check ReBuncher status reported. {presentMedia.Payload?.Source.Position}");
                }
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.PresentMediaAsync()");

            var result = await Device.PresentMediaAsync(
                events: new(events),
                request: new(position),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.PresentMediaAsync() -> {result.CompletionCode}");

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
