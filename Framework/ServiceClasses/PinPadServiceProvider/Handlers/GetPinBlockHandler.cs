/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTFramework.PinPad
{
    public partial class GetPinBlockHandler
    {
        private async Task<GetPinBlockCompletion.PayloadData> HandleGetPinBlock(IGetPinBlockEvents events, GetPinBlockCommand getPinBlock, CancellationToken cancel)
        {
            if (getPinBlock.Payload.Format is null)
            {
                return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"PIN block format is not specified.");
            }

            PINBlockRequest.PINFormatEnum format = getPinBlock.Payload.Format switch
            {
                GetPinBlockCommand.PayloadData.FormatEnum.Ansi => PINBlockRequest.PINFormatEnum.ANSI,
                GetPinBlockCommand.PayloadData.FormatEnum.Ap => PINBlockRequest.PINFormatEnum.AP,
                GetPinBlockCommand.PayloadData.FormatEnum.Banksys => PINBlockRequest.PINFormatEnum.BANKSYS,
                GetPinBlockCommand.PayloadData.FormatEnum.Diebold => PINBlockRequest.PINFormatEnum.DIEBOLD,
                GetPinBlockCommand.PayloadData.FormatEnum.Dieboldco => PINBlockRequest.PINFormatEnum.DIEBOLDCO,
                GetPinBlockCommand.PayloadData.FormatEnum.Eci2 => PINBlockRequest.PINFormatEnum.ECI2,
                GetPinBlockCommand.PayloadData.FormatEnum.Eci3 => PINBlockRequest.PINFormatEnum.ECI3,
                GetPinBlockCommand.PayloadData.FormatEnum.Emv => PINBlockRequest.PINFormatEnum.EMV,
                GetPinBlockCommand.PayloadData.FormatEnum.Ibm3624 => PINBlockRequest.PINFormatEnum.IBM3624,
                GetPinBlockCommand.PayloadData.FormatEnum.Iso0 => PINBlockRequest.PINFormatEnum.ISO0,
                GetPinBlockCommand.PayloadData.FormatEnum.Iso1 => PINBlockRequest.PINFormatEnum.ISO1,
                GetPinBlockCommand.PayloadData.FormatEnum.Iso3 => PINBlockRequest.PINFormatEnum.ISO3,
                GetPinBlockCommand.PayloadData.FormatEnum.Visa => PINBlockRequest.PINFormatEnum.VISA,
                _ => PINBlockRequest.PINFormatEnum.VISA3,
            };

            if (format == PINBlockRequest.PINFormatEnum.ANSI ||
                format == PINBlockRequest.PINFormatEnum.ISO0 ||
                format == PINBlockRequest.PINFormatEnum.DIEBOLD ||
                format == PINBlockRequest.PINFormatEnum.DIEBOLDCO)
            {
                if (string.IsNullOrEmpty(getPinBlock.Payload.CustomerData))
                {
                    return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"Customer data required for this pin format. {getPinBlock.Payload.Format}");
                }
            }

            KeyDetail key = PinPad.GetKeyDetail(getPinBlock.Payload.Key);
            if (key is null)
            {
                return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"Specified key is not loaded.",
                                                             GetPinBlockCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }

            if (key.KeyUsage != "P0")
            {
                return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"Specified key usage is not expected.{key.KeyUsage}",
                                                             GetPinBlockCompletion.PayloadData.ErrorCodeEnum.UseViolation);
            }

            if (!string.IsNullOrEmpty(getPinBlock.Payload.SecondEncKey))
            {
                KeyDetail pinblockEncKey = PinPad.GetKeyDetail(getPinBlock.Payload.SecondEncKey);
                if (pinblockEncKey is null)
                {
                    return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key encryption key is not loaded.",
                                                                 GetPinBlockCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (pinblockEncKey.KeyUsage != "D0" &&
                    pinblockEncKey.KeyUsage != "D1")
                {
                    return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key encryption key usage is not expected.{pinblockEncKey.KeyUsage}",
                                                                 GetPinBlockCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }

                if (pinblockEncKey.ModeOfUse != "E")
                {
                    return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key encryption key usage is not expected.{pinblockEncKey.ModeOfUse}",
                                                                 GetPinBlockCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }
            }
            
            if (getPinBlock.Payload.Padding is null)
            {
                return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"No padding data specified.");
            }

            if (getPinBlock.Payload.Padding > 0xf)
            {
                return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Padding data range is up to 0xf");
            }

            if (!string.IsNullOrEmpty(getPinBlock.Payload.XorData))
            {
                if ((getPinBlock.Payload.XorData.Length % 2) != 0)
                {
                    return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"XorData must be even number.");
                }

                foreach (char c in getPinBlock.Payload.XorData)
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        return new GetPinBlockCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"XorData data should be in hexstring. {getPinBlock.Payload.XorData}");
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.GetPinBlock()");

            var result = await Device.GetPinBlock(events,
                                                  new PINBlockRequest(getPinBlock.Payload.CustomerData,
                                                                      getPinBlock.Payload.XorData,
                                                                      (byte)getPinBlock.Payload.Padding,
                                                                      format,
                                                                      getPinBlock.Payload.Key,
                                                                      getPinBlock.Payload.SecondEncKey,
                                                                      getPinBlock.Payload.PinBlockAttributes.CryptoMethod switch
                                                                      { 
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Cbc => PINBlockRequest.EncryptionAlgorithmEnum.CBC,
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Cfb => PINBlockRequest.EncryptionAlgorithmEnum.CFB,
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Ctr => PINBlockRequest.EncryptionAlgorithmEnum.CTR,
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Ecb => PINBlockRequest.EncryptionAlgorithmEnum.ECB,
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Ofb => PINBlockRequest.EncryptionAlgorithmEnum.OFB,
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Xts => PINBlockRequest.EncryptionAlgorithmEnum.XTS,
                                                                          GetPinBlockCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.RsaesOaep => PINBlockRequest.EncryptionAlgorithmEnum.RSAES_OAEP,
                                                                          _ => PINBlockRequest.EncryptionAlgorithmEnum.RSAES_PKCS1_V1_5,
                                                                      }),
                                                        cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.GetPinBlock() -> {result.CompletionCode}, {result.ErrorCode}");

            return new GetPinBlockCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription,
                                                         result.ErrorCode,
                                                         result.PINBlock is not null && result.PINBlock.Count > 0 ? Convert.ToBase64String(result.PINBlock.ToArray()) : null);
        }
    }
}
