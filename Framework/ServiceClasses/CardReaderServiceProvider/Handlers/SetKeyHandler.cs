/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT;

namespace XFS4IoTFramework.CardReader
{
    public partial class SetKeyHandler
    {
        private async Task<CommandResult<SetKeyCompletion.PayloadData>> HandleSetKey(ISetKeyEvents events, SetKeyCommand setKey, CancellationToken cancel)
        {
            if (Common.CardReaderCapabilities.SecurityType != CardReaderCapabilitiesClass.SecurityTypeEnum.Cim86)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"This device doesn't support CIM86 module. {Common.CardReaderCapabilities.SecurityType}");
            }

            if (setKey.Payload.KeyValue is null ||
                setKey.Payload.KeyValue.Count == 0)
            {
                return new(
                    new(SetKeyCompletion.PayloadData.ErrorCodeEnum.InvalidKey),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    "No key data supplied.");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.SetCIM86KeyAsync()");
            var result = await Device.SetCIM86KeyAsync(new SetCIM86KeyRequest(setKey.Payload.KeyValue),
                                                       cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.SetCIM86KeyAsync() -> {result.CompletionCode}");

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }

    }
}
