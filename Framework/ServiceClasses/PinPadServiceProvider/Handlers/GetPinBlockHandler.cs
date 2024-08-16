/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTFramework.PinPad
{
    public partial class GetPinBlockHandler
    {
        private async Task<CommandResult<GetPinBlockCompletion.PayloadData>> HandleGetPinBlock(IGetPinBlockEvents events, GetPinBlockCommand getPinBlock, CancellationToken cancel)
        {
            if (getPinBlock.Payload.Format is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"PIN block format is not specified.");
            }

            PINBlockRequest.PINFormatEnum format = getPinBlock.Payload.Format switch
            {
                GetPinBlockCommand.PayloadData.FormatEnum.Ansi => PINBlockRequest.PINFormatEnum.ANSI,
                GetPinBlockCommand.PayloadData.FormatEnum.Ap => PINBlockRequest.PINFormatEnum.AP,
                GetPinBlockCommand.PayloadData.FormatEnum.Banksys => PINBlockRequest.PINFormatEnum.BANKSYS,
                GetPinBlockCommand.PayloadData.FormatEnum.Diebold => PINBlockRequest.PINFormatEnum.DIEBOLD,
                GetPinBlockCommand.PayloadData.FormatEnum.DieboldCo => PINBlockRequest.PINFormatEnum.DIEBOLDCO,
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
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Customer data required for this pin format. {getPinBlock.Payload.Format}");
                }
            }

            KeyDetail key = KeyManagement.GetKeyDetail(getPinBlock.Payload.Key);
            if (key is null)
            {
                return new(
                    new(GetPinBlockCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key is not loaded.");
            }

            if (key.KeyUsage != "P0")
            {
                return new(
                    new(GetPinBlockCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key usage is not expected.{key.KeyUsage}");
            }

            if (!string.IsNullOrEmpty(getPinBlock.Payload.SecondEncKey))
            {
                KeyDetail pinblockEncKey = KeyManagement.GetKeyDetail(getPinBlock.Payload.SecondEncKey);
                if (pinblockEncKey is null)
                {
                    return new(
                        new(GetPinBlockCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key encryption key is not loaded.");
                }

                if (pinblockEncKey.KeyUsage != "D0" &&
                    pinblockEncKey.KeyUsage != "D1")
                {
                    return new(
                        new(GetPinBlockCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key encryption key usage is not expected.{pinblockEncKey.KeyUsage}");
                }

                if (pinblockEncKey.ModeOfUse != "E")
                {
                    return new(
                        new(GetPinBlockCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key encryption key usage is not expected.{pinblockEncKey.ModeOfUse}");
                }
            }
            
            if (getPinBlock.Payload.Padding is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No padding data specified.");
            }

            if (getPinBlock.Payload.Padding > 0xf)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Padding data range is up to 0xf");
            }

            if (!string.IsNullOrEmpty(getPinBlock.Payload.XorData))
            {
                if ((getPinBlock.Payload.XorData.Length % 2) != 0)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"XorData must be even number.");
                }

                foreach (char c in getPinBlock.Payload.XorData)
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"XorData data should be in hexstring. {getPinBlock.Payload.XorData}");
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.GetPinBlock()");

            var result = await Device.GetPinBlock(new PinPadCommandEvents(events),
                                                  new PINBlockRequest(getPinBlock.Payload.CustomerData,
                                                                      getPinBlock.Payload.XorData,
                                                                      (byte)getPinBlock.Payload.Padding,
                                                                      format,
                                                                      getPinBlock.Payload.Key,
                                                                      getPinBlock.Payload.SecondEncKey,
                                                                      getPinBlock.Payload.CryptoMethod switch
                                                                      { 
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.Cbc => PINBlockRequest.EncryptionAlgorithmEnum.CBC,
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.Cfb => PINBlockRequest.EncryptionAlgorithmEnum.CFB,
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.Ctr => PINBlockRequest.EncryptionAlgorithmEnum.CTR,
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.Ecb => PINBlockRequest.EncryptionAlgorithmEnum.ECB,
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.Ofb => PINBlockRequest.EncryptionAlgorithmEnum.OFB,
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.Xts => PINBlockRequest.EncryptionAlgorithmEnum.XTS,
                                                                          GetPinBlockCommand.PayloadData.CryptoMethodEnum.RsaesOaep => PINBlockRequest.EncryptionAlgorithmEnum.RSAES_OAEP,
                                                                          _ => PINBlockRequest.EncryptionAlgorithmEnum.RSAES_PKCS1_V1_5,
                                                                      }),
                                                        cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.GetPinBlock() -> {result.CompletionCode}, {result.ErrorCode}");

            GetPinBlockCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.PINBlock?.Count > 0)
            {
                payload = new(
                    result.ErrorCode,
                    result.PINBlock);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}
