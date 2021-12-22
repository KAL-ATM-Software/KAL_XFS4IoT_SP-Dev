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
using XFS4IoT.CardReader;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CardReader
{
    public partial class EMVClessIssuerUpdateHandler
    {
        private async Task<EMVClessIssuerUpdateCompletion.PayloadData> HandleEMVClessIssuerUpdate(IEMVClessIssuerUpdateEvents events, EMVClessIssuerUpdateCommand eMVClessIssuerUpdate, CancellationToken cancel)
        {
            if (CardReader.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless)
            {
                return new EMVClessIssuerUpdateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                      $"This device is not an intelligent contactless CardReader. {CardReader.CardReaderCapabilities.Type}");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessIssuerUpdateAsync()");
            var result = await Device.EMVContactlessIssuerUpdateAsync(new EMVClessCommandEvents(events),
                                                                      new EMVContactlessIssuerUpdateRequest(eMVClessIssuerUpdate.Payload.Data, eMVClessIssuerUpdate.Payload.Timeout),
                                                                      cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessIssuerUpdateAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success &&
                result.TransactionResult is not null)
            {
                // Build transaction output data
                EMVClessIssuerUpdateEMVClessTxOutputDataClass chip = new((EMVClessIssuerUpdateEMVClessTxOutputDataClass.TxOutcomeEnum)result.TransactionResult.TransactionOutcome,
                                                                         result.TransactionResult.DataRead,
                                                                         new EMVClessIssuerUpdateEMVClessTxOutputDataClass.ClessOutcomeClass((EMVClessIssuerUpdateEMVClessTxOutputDataClass.ClessOutcomeClass.CvmEnum)result.TransactionResult.ClessOutcome.Cvm,
                                                                                                                                             (EMVClessIssuerUpdateEMVClessTxOutputDataClass.ClessOutcomeClass.AlternateInterfaceEnum)result.TransactionResult.ClessOutcome.AlternateInterface,
                                                                                                                                             result.TransactionResult.ClessOutcome.Receipt,
                                                                                                                                             new EMVClessUIClass(result.TransactionResult.ClessOutcome.UiOutcome.MessageId,
                                                                                                                                                                 (EMVClessUIClass.StatusEnum)result.TransactionResult.ClessOutcome.UiOutcome.Status,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiOutcome.HoldTime,
                                                                                                                                                                 (EMVClessUIClass.ValueQualifierEnum)result.TransactionResult.ClessOutcome.UiOutcome.ValueQualifier,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiOutcome.Value,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiOutcome.CurrencyCode,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiOutcome.LanguagePreferenceData),
                                                                                                                                             new EMVClessUIClass(result.TransactionResult.ClessOutcome.UiRestart.MessageId,
                                                                                                                                                                 (EMVClessUIClass.StatusEnum)result.TransactionResult.ClessOutcome.UiRestart.Status,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiRestart.HoldTime,
                                                                                                                                                                 (EMVClessUIClass.ValueQualifierEnum)result.TransactionResult.ClessOutcome.UiRestart.ValueQualifier,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiRestart.Value,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiRestart.CurrencyCode,
                                                                                                                                                                 result.TransactionResult.ClessOutcome.UiRestart.LanguagePreferenceData),
                                                                                                                                             result.TransactionResult.ClessOutcome.FieldOffHoldTime,
                                                                                                                                             result.TransactionResult.ClessOutcome.CardRemovalTimeout,
                                                                                                                                             result.TransactionResult.ClessOutcome.DiscretionaryData));
                return new EMVClessIssuerUpdateCompletion.PayloadData(result.CompletionCode, 
                                                                      result.ErrorDescription,
                                                                      result.ErrorCode,
                                                                      chip);
            }
            else
            {
                return new EMVClessIssuerUpdateCompletion.PayloadData(result.CompletionCode,
                                                                      result.ErrorDescription,
                                                                      result.ErrorCode);
            }
        }
    }
}
