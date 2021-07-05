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
    public partial class EMVClessConfigureHandler
    { 
        private async Task<EMVClessConfigureCompletion.PayloadData> HandleEMVClessConfigure(IEMVClessConfigureEvents events, EMVClessConfigureCommand eMVClessConfigure, CancellationToken cancel)
        {
            if (CardReader.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless)
            {
                return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                   $"This device is not an intelligent contactless CardReader. {CardReader.CardReaderCapabilities.Type}");
            }

            // Data check
            if ((eMVClessConfigure.Payload.AidData is null || eMVClessConfigure.Payload.AidData.Count == 0) &&
                eMVClessConfigure.Payload.TerminalData is null &&
                (eMVClessConfigure.Payload.KeyData is null || eMVClessConfigure.Payload.KeyData.Count == 0))
            {
                return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   "No terminal configuration data supplied.");
            }

            List<AIDInfo> AIDs = new(); 
            foreach (EMVClessConfigureCommand.PayloadData.AidDataClass AID in eMVClessConfigure.Payload.AidData)
            {
                if (string.IsNullOrEmpty(AID.Aid))
                {
                    return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       "No AID is supplied.");
                }
                if (AID.PartialSelection is null)
                {
                    return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       "No PartialSelection is supplied.");
                }
                if (AID.TransactionType is null)
                {
                    return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       "No TransactionType is supplied.");
                }
                AIDs.Add(new AIDInfo(new List<byte>(Convert.FromBase64String(AID.Aid)), 
                         (bool)AID.PartialSelection, 
                         (int)AID.TransactionType, 
                         string.IsNullOrEmpty(AID.KernelIdentifier) ? null : new List<byte>(Convert.FromBase64String(AID.KernelIdentifier)),
                         string.IsNullOrEmpty(AID.ConfigData) ? null : new List<byte>(Convert.FromBase64String(AID.ConfigData))));
            }

            List<PublicKeyInfo> PublicKeys = new();
            foreach (EMVClessConfigureCommand.PayloadData.KeyDataClass PKs in eMVClessConfigure.Payload.KeyData)
            {
                if (string.IsNullOrEmpty(PKs.Rid))
                {
                    return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       "No RID is supplied.");
                }
                if (PKs.CaPublicKey is null)
                {
                    return new EMVClessConfigureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       "No CA Public Key is supplied.");
                }

                // MISSING PKs.CaPublicKey structure
                PublicKeys.Add(new PublicKeyInfo(new List<byte>(Convert.FromBase64String(PKs.Rid)),
                                                 null));
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessConfigureAsync()");
            var result = await Device.EMVContactlessConfigureAsync(new EMVContactlessConfigureRequest(string.IsNullOrEmpty(eMVClessConfigure.Payload.TerminalData) ? null : new List<byte>(Convert.FromBase64String(eMVClessConfigure.Payload.TerminalData)), 
                                                                   AIDs, 
                                                                   PublicKeys),
                                                                   cancel);

            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessConfigureAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new EMVClessConfigureCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
        }
    }
}
