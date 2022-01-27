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
    public partial class ChipIOHandler
    {
        private async Task<ChipIOCompletion.PayloadData> HandleChipIO(IChipIOEvents events, ChipIOCommand chipIO, CancellationToken cancel)
        {
            if (chipIO.Payload.ChipData is null ||
                chipIO.Payload.ChipData.Count == 0)
            {
                return new ChipIOCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                        "No chip IO data supplied.",
                                                        ChipIOCompletion.PayloadData.ErrorCodeEnum.InvalidData);
            }

            ChipIORequest.ChipProtocolEnum? chipProtocol = chipIO.Payload.ChipProtocol switch
            {
                ChipIOCommand.PayloadData.ChipProtocolEnum.ChipT0 => ChipIORequest.ChipProtocolEnum.ChipT0,
                ChipIOCommand.PayloadData.ChipProtocolEnum.ChipT1 => ChipIORequest.ChipProtocolEnum.ChipT1,
                ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeAPart3 => ChipIORequest.ChipProtocolEnum.ChipTypeAPart3,
                ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeAPart4 => ChipIORequest.ChipProtocolEnum.ChipTypeAPart4,
                ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeB => ChipIORequest.ChipProtocolEnum.ChipTypeB,
                ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeNFC => ChipIORequest.ChipProtocolEnum.ChipTypeNFC,
                _ => ChipIORequest.ChipProtocolEnum.ChipProtocolNotRequired,
            };

            if (chipProtocol is null)
            {
                return new ChipIOCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                        $"No chip protocol supplied.",
                                                        ChipIOCompletion.PayloadData.ErrorCodeEnum.InvalidData);
            }

            // check capability for the protocol
            if (chipProtocol == ChipIORequest.ChipProtocolEnum.ChipT0 &&
                !Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.T0) ||
                chipProtocol == ChipIORequest.ChipProtocolEnum.ChipT1 &&
                !Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.T1) ||
                chipProtocol == ChipIORequest.ChipProtocolEnum.ChipTypeAPart3 &&
                !Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart3) ||
                chipProtocol == ChipIORequest.ChipProtocolEnum.ChipTypeAPart4 &&
                !Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart4) ||
                chipProtocol == ChipIORequest.ChipProtocolEnum.ChipTypeB &&
                !Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeB) ||
                chipProtocol == ChipIORequest.ChipProtocolEnum.ChipTypeNFC &&
                !Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeNFC))
            {
                return new ChipIOCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                        $"Unsupported chip protocol supplied. {chipIO.Payload.ChipProtocol}",
                                                        ChipIOCompletion.PayloadData.ErrorCodeEnum.InvalidData);
            }
            
            Logger.Log(Constants.DeviceClass, "CardReaderDev.ChipIOAsync()");
            var result = await Device.ChipIOAsync(new ChipIORequest((ChipIORequest.ChipProtocolEnum)chipProtocol, chipIO.Payload.ChipData),
                                                  cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ChipIOAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ChipIOCompletion.PayloadData(result.CompletionCode,
                                                    result.ErrorDescription,
                                                    result.ErrorCode,
                                                    chipIO.Payload.ChipProtocol switch
                                                    {
                                                        ChipIOCommand.PayloadData.ChipProtocolEnum.ChipT0 => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipT0,
                                                        ChipIOCommand.PayloadData.ChipProtocolEnum.ChipT1 => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipT1,
                                                        ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeAPart3 => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipTypeAPart3,
                                                        ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeAPart4 => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipTypeAPart4,
                                                        ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeB => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipTypeB,
                                                        ChipIOCommand.PayloadData.ChipProtocolEnum.ChipTypeNFC => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipTypeNFC,
                                                        _ => ChipIOCompletion.PayloadData.ChipProtocolEnum.ChipProtocolNotRequired,
                                                    },
                                                    result.ChipData);
        }

    }
}
