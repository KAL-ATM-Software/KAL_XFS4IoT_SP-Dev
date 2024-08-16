/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
    public partial class EMVClessConfigureHandler
    { 
        private async Task<CommandResult<EMVClessConfigureCompletion.PayloadData>> HandleEMVClessConfigure(IEMVClessConfigureEvents events, EMVClessConfigureCommand eMVClessConfigure, CancellationToken cancel)
        {
            if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"This device is not an intelligent contactless CardReader. {Common.CardReaderCapabilities.Type}");
            }

            // Data check
            if (eMVClessConfigure.Payload.AidData is not { Count: > 0 } &&
                eMVClessConfigure.Payload.TerminalData is null &&
                eMVClessConfigure.Payload.KeyData is not { Count: > 0 })
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "No terminal configuration data supplied.");
            }

            List<AIDInfo> AIDs = new(); 
            foreach (EMVClessConfigureCommand.PayloadData.AidDataClass AID in eMVClessConfigure.Payload.AidData)
            {
                if (AID.Aid is not { Count: > 0 })
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No AID is supplied.");
                }
                if (AID.PartialSelection is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No PartialSelection is supplied.");
                }
                if (AID.TransactionType is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No TransactionType is supplied.");
                }
                AIDs.Add(new AIDInfo(AID.Aid, 
                                     (bool)AID.PartialSelection, 
                                     (int)AID.TransactionType, 
                                     AID.KernelIdentifier,
                                     AID.ConfigData));
            }

            List<PublicKeyInfo> PublicKeys = new();
            foreach (EMVClessConfigureCommand.PayloadData.KeyDataClass PKs in eMVClessConfigure.Payload.KeyData)
            {
                if (PKs.Rid is not { Count: > 0})
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No RID is supplied.");
                }
                if (PKs.CaPublicKey is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No CA Public Key is supplied.");
                }

                if (PKs.CaPublicKey.Index is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No index of the CA Public Key is supplied.");
                }

                if (PKs.CaPublicKey.AlgorithmIndicator is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No algorithm indicator of the CA Public Key is supplied.");
                }

                if (PKs.CaPublicKey.Exponent is not { Count: > 0 })
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No exponents of the CA Public Key is supplied.");
                }

                if (PKs.CaPublicKey.Modulus is not { Count: > 0 })
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No modules of the CA Public Key is supplied.");
                }

                if (PKs.CaPublicKey.Checksum is not { Count: > 0 })
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No checksum of the CA Public Key is supplied.");
                }

                PublicKeys.Add(new PublicKeyInfo(PKs.Rid,
                                                 new PublicKey(AlgorithmIndicator: (int)PKs.CaPublicKey.AlgorithmIndicator,
                                                               Index: (int)PKs.CaPublicKey.Index,
                                                               Exponent: PKs.CaPublicKey.Exponent,
                                                               Modulus: PKs.CaPublicKey.Modulus,
                                                               Checksum: PKs.CaPublicKey.Checksum)));
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessConfigureAsync()");
            var result = await Device.EMVContactlessConfigureAsync(new EMVContactlessConfigureRequest(eMVClessConfigure.Payload.TerminalData, 
                                                                   AIDs, 
                                                                   PublicKeys),
                                                                   cancel);

            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessConfigureAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
