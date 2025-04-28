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
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.German
{
    public partial class SecureMsgReceiveHandler
    {
        private async Task<CommandResult<SecureMsgReceiveCompletion.PayloadData>> HandleSecureMsgReceive(ISecureMsgReceiveEvents events, SecureMsgReceiveCommand secureMsgReceive, CancellationToken cancel)
        {
            if (secureMsgReceive.Payload is null)
            {
                throw new InvalidDataException($"No payload is set.");
            }

            Logger.Log(Constants.DeviceClass, "GermanDev.SecureMsgReceive()");
            var result = await Device.SecureMsgReceiveAsync(
                new(
                    Protocol: secureMsgReceive.Payload.Protocol switch
                    {
                        SecureMsgReceiveCommand.PayloadData.ProtocolEnum.RawData => SecureMsgReceiveRequest.ProtocolEnum.RawData,
                        SecureMsgReceiveCommand.PayloadData.ProtocolEnum.GenAs => SecureMsgReceiveRequest.ProtocolEnum.GenAs,
                        SecureMsgReceiveCommand.PayloadData.ProtocolEnum.IsoPs => SecureMsgReceiveRequest.ProtocolEnum.ISOPS,
                        _ => throw new InvalidDataException($"Invalid InitMode value. {secureMsgReceive.Payload.Protocol}")
                    },
                    Message: secureMsgReceive.Payload.Msg),
                cancel);
            Logger.Log(Constants.DeviceClass, $"GermanDev.SecureMsgReceive() -> {result.CompletionCode}");

            return new(
                Payload: result.ErrorCode is null ? null :
                new(ErrorCode: result.ErrorCode switch
                {
                    SecureMsgReceiveResponse.ErrorCodeEnum.ProtocolInvalid => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.ProtocolInvalid,
                    SecureMsgReceiveResponse.ErrorCodeEnum.AccessDenied => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                    SecureMsgReceiveResponse.ErrorCodeEnum.MACInvalid => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.MacInvalid,
                    SecureMsgReceiveResponse.ErrorCodeEnum.FormatInvalid => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.FormatInvalid,
                    SecureMsgReceiveResponse.ErrorCodeEnum.HSMStateInvalid => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.HsmStateInvalid,
                    SecureMsgReceiveResponse.ErrorCodeEnum.KeyNotFound => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                    SecureMsgReceiveResponse.ErrorCodeEnum.ContentInvalid => SecureMsgReceiveCompletion.PayloadData.ErrorCodeEnum.ContentInvalid,
                    _ => throw new InternalErrorException($"Unexpected error code. {result.ErrorCode}"),
                }),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
