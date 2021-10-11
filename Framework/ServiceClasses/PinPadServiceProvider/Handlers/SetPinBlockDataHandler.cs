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
    public partial class SetPinBlockDataHandler
    {
        private async Task<SetPinBlockDataCompletion.PayloadData> HandleSetPinBlockData(ISetPinBlockDataEvents events, SetPinBlockDataCommand setPinBlockData, CancellationToken cancel)
        {
            if (setPinBlockData.Payload.Format is null)
            {
                return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"PIN block format is not specified.");
            }

            PINBlockRequest.PINFormatEnum format = setPinBlockData.Payload.Format switch
            {
                SetPinBlockDataCommand.PayloadData.FormatEnum.Ansi => PINBlockRequest.PINFormatEnum.ANSI,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Ap => PINBlockRequest.PINFormatEnum.AP,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Banksys => PINBlockRequest.PINFormatEnum.BANKSYS,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Diebold => PINBlockRequest.PINFormatEnum.DIEBOLD,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Dieboldco => PINBlockRequest.PINFormatEnum.DIEBOLDCO,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Eci2 => PINBlockRequest.PINFormatEnum.ECI2,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Eci3 => PINBlockRequest.PINFormatEnum.ECI3,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Emv => PINBlockRequest.PINFormatEnum.EMV,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Ibm3624 => PINBlockRequest.PINFormatEnum.IBM3624,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Iso0 => PINBlockRequest.PINFormatEnum.ISO0,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Iso1 => PINBlockRequest.PINFormatEnum.ISO1,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Iso3 => PINBlockRequest.PINFormatEnum.ISO3,
                SetPinBlockDataCommand.PayloadData.FormatEnum.Visa => PINBlockRequest.PINFormatEnum.VISA,
                _ => PINBlockRequest.PINFormatEnum.VISA3,
            };

            if (format == PINBlockRequest.PINFormatEnum.ANSI ||
                format == PINBlockRequest.PINFormatEnum.ISO0 ||
                format == PINBlockRequest.PINFormatEnum.DIEBOLD ||
                format == PINBlockRequest.PINFormatEnum.DIEBOLDCO)
            {
                if (string.IsNullOrEmpty(setPinBlockData.Payload.CustomerData))
                {
                    return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"Customer data required for this pin format. {setPinBlockData.Payload.Format}");
                }
            }

            KeyDetail key = PinPad.GetKeyDetail(setPinBlockData.Payload.Key);
            if (key is null)
            {
                return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key is not loaded.");
            }

            if (key.KeyUsage != "P0")
            {
                return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key usage is not expected.{key.KeyUsage}");
            }

            if (!string.IsNullOrEmpty(setPinBlockData.Payload.SecondEncKey))
            {
                KeyDetail pinblockEncKey = PinPad.GetKeyDetail(setPinBlockData.Payload.SecondEncKey);
                if (pinblockEncKey is null)
                {
                    return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                     $"Specified key encryption key is not loaded.");
                }

                if (pinblockEncKey.KeyUsage != "D0" &&
                    pinblockEncKey.KeyUsage != "D1")
                {
                    return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                    $"Specified key encryption key usage is not expected.{pinblockEncKey.KeyUsage}");
                }

                if (pinblockEncKey.ModeOfUse != "E")
                {
                    return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                     $"Specified key encryption key usage is not expected.{pinblockEncKey.ModeOfUse}");
                }
            }
            
            if (setPinBlockData.Payload.Padding is null)
            {
                return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"No padding data specified.");
            }

            if (setPinBlockData.Payload.Padding > 0xf)
            {
                return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"Padding data range is up to 0xf");
            }

            if (!string.IsNullOrEmpty(setPinBlockData.Payload.XorData))
            {
                if ((setPinBlockData.Payload.XorData.Length % 2) != 0)
                {
                    return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"XorData must be even number.");
                }

                foreach (char c in setPinBlockData.Payload.XorData)
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        return new SetPinBlockDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                         $"XorData data should be in hexstring. {setPinBlockData.Payload.XorData}");
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.SetPinBlockData()");

            var result = await Device.SetPinBlockData(new PINBlockRequest(setPinBlockData.Payload.CustomerData,
                                                                          setPinBlockData.Payload.XorData,
                                                                          (byte)setPinBlockData.Payload.Padding,
                                                                          format,
                                                                          setPinBlockData.Payload.Key,
                                                                          setPinBlockData.Payload.SecondEncKey,
                                                                          setPinBlockData.Payload.PinBlockAttributes.CryptoMethod switch
                                                                          { 
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Cbc => PINBlockRequest.EncryptionAlgorithmEnum.CBC,
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Cfb => PINBlockRequest.EncryptionAlgorithmEnum.CFB,
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Ctr => PINBlockRequest.EncryptionAlgorithmEnum.CTR,
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Ecb => PINBlockRequest.EncryptionAlgorithmEnum.ECB,
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Ofb => PINBlockRequest.EncryptionAlgorithmEnum.OFB,
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.Xts => PINBlockRequest.EncryptionAlgorithmEnum.XTS,
                                                                              SetPinBlockDataCommand.PayloadData.PinBlockAttributesClass.CryptoMethodEnum.RsaesOaep => PINBlockRequest.EncryptionAlgorithmEnum.RSAES_OAEP,
                                                                              _ => PINBlockRequest.EncryptionAlgorithmEnum.RSAES_PKCS1_V1_5,
                                                                          }),
                                                       cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.SetPinBlockData() -> {result.CompletionCode}");

            return new SetPinBlockDataCompletion.PayloadData(result.CompletionCode,
                                                             result.ErrorDescription);
        }
    }
}
