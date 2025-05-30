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
using XFS4IoT.German.Commands;
using XFS4IoT.German.Completions;

namespace XFS4IoTFramework.German
{
    public partial class SecureMsgSendHandler
    {
        private async Task<CommandResult<SecureMsgSendCompletion.PayloadData>> HandleSecureMsgSend(ISecureMsgSendEvents events, SecureMsgSendCommand secureMsgSend, CancellationToken cancel)
        {
            if (secureMsgSend.Payload is null)
            {
                throw new InvalidDataException($"No payload is set.");
            }

            if ((secureMsgSend.Payload.Msg is null ||
                 secureMsgSend.Payload.Msg.Count == 0) &&
                secureMsgSend.Payload.Protocol != SecureMsgSendCommand.PayloadData.ProtocolEnum.HsmLdi)
            {
                throw new InvalidDataException($"The message data must be set if the property is other than HSMLDI.");
            }

            Logger.Log(Constants.DeviceClass, "GermanDev.SecureMsgSend()");
            var result = await Device.SecureMsgSendAsync(
                new(
                    Protocol: secureMsgSend.Payload.Protocol switch
                    {
                        SecureMsgSendCommand.PayloadData.ProtocolEnum.RawData => SecureMsgSendRequest.ProtocolEnum.RawData,
                        SecureMsgSendCommand.PayloadData.ProtocolEnum.HsmLdi => SecureMsgSendRequest.ProtocolEnum.HSMLDI,
                        SecureMsgSendCommand.PayloadData.ProtocolEnum.PinCmp => SecureMsgSendRequest.ProtocolEnum.PinCmp,
                        SecureMsgSendCommand.PayloadData.ProtocolEnum.GenAs => SecureMsgSendRequest.ProtocolEnum.GenAs,
                        SecureMsgSendCommand.PayloadData.ProtocolEnum.IsoPs => SecureMsgSendRequest.ProtocolEnum.ISOPS,
                        _ => throw new InvalidDataException($"Invalid InitMode value. {secureMsgSend.Payload.Protocol}")
                    },
                    Message: secureMsgSend.Payload.Msg),
                cancel);
            Logger.Log(Constants.DeviceClass, $"GermanDev.SecureMsgSend() -> {result.CompletionCode}");

            return new(
                Payload: result.ErrorCode is null && (result.Message is null || result.Message?.Count == 0) ? null :
                new(
                    ErrorCode: result.ErrorCode switch
                    {
                        SecureMsgSendResponse.ErrorCodeEnum.ProtocolInvalid => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.ProtocolInvalid,
                        SecureMsgSendResponse.ErrorCodeEnum.KeyNotFound => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                        SecureMsgSendResponse.ErrorCodeEnum.FormatInvalid => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.FormatInvalid,
                        SecureMsgSendResponse.ErrorCodeEnum.HSMStateInvalid => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.HsmStateInvalid,
                        SecureMsgSendResponse.ErrorCodeEnum.ContentInvalid => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.ContentInvalid,
                        SecureMsgSendResponse.ErrorCodeEnum.AccessDenied => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                        SecureMsgSendResponse.ErrorCodeEnum.NoPin => SecureMsgSendCompletion.PayloadData.ErrorCodeEnum.NoPin,
                        _ => null,
                    },
                    Msg: result.Message),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
