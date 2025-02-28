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
using XFS4IoT.MixedMedia.Commands;
using XFS4IoT.MixedMedia.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.MixedMedia
{
    public partial class SetModeHandler
    {
        private async Task<CommandResult<SetModeCompletion.PayloadData>> HandleSetMode(ISetModeEvents events, SetModeCommand setMode, CancellationToken cancel)
        {
            if (setMode.Payload is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No payload specified.");
            }

            Common.MixedMedia.ModeTypeEnum modes = XFS4IoTFramework.Common.MixedMedia.ModeTypeEnum.None;

            if (setMode.Payload.Modes is not null)
            {
                if (setMode.Payload.Modes.CashAccept is not null && (bool)setMode.Payload.Modes.CashAccept)
                {
                    if (!Common.MixedMediaCapabilities.Modes.HasFlag(XFS4IoTFramework.Common.MixedMedia.ModeTypeEnum.Cash))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Specified mode is not supported by the device. {Common.MixedMediaCapabilities.Modes}");
                    }
                    modes |= XFS4IoTFramework.Common.MixedMedia.ModeTypeEnum.Cash;
                }

                if (setMode.Payload.Modes.CashAccept is not null && (bool)setMode.Payload.Modes.CashAccept)
                {
                    if (!Common.MixedMediaCapabilities.Modes.HasFlag(XFS4IoTFramework.Common.MixedMedia.ModeTypeEnum.Check))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Specified mode is not supported by the device. {Common.MixedMediaCapabilities.Modes}");
                    }
                    modes |= XFS4IoTFramework.Common.MixedMedia.ModeTypeEnum.Check;
                }
            }

            Logger.Log(Constants.DeviceClass, "MixedMediaDev.SetModeAsync()");

            var result = await Device.SetModeAsync(
                request: new(modes),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"MixedMediaDev.SetModeAsync() -> {result.CompletionCode}");

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

    }
}
