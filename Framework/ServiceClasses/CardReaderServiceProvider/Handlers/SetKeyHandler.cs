/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

namespace XFS4IoTFramework.CardReader
{
    public partial class SetKeyHandler
    {
        private async Task<SetKeyCompletion.PayloadData> HandleSetKey(ISetKeyEvents events, SetKeyCommand setKey, CancellationToken cancel)
        {
            if (CardReader.CardReaderCapabilities.SecurityType != CardReaderCapabilitiesClass.SecurityTypeEnum.Cim86)
            {
                return new SetKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                        $"This device doesn't support CIM86 module. {CardReader.CardReaderCapabilities.SecurityType}");
            }

            if (string.IsNullOrEmpty(setKey.Payload.KeyValue))
            {
                return new SetKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                        "No key data supplied.",
                                                        SetKeyCompletion.PayloadData.ErrorCodeEnum.InvalidKey);
            }

            List<byte> keyValue = new(Convert.FromBase64String(setKey.Payload.KeyValue));

            Logger.Log(Constants.DeviceClass, "CardReaderDev.SetCIM86KeyAsync()");
            var result = await Device.SetCIM86KeyAsync(new SetCIM86KeyRequest(keyValue),
                                                       cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.SetCIM86KeyAsync() -> {result.CompletionCode}");

            return new SetKeyCompletion.PayloadData(result.CompletionCode,
                                                    result.ErrorDescription,
                                                    result.ErrorCode);
        }

    }
}
