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

namespace XFS4IoTFramework.CardReader
{
    public partial class EMVClessIssuerUpdateHandler
    {
        private async Task<EMVClessIssuerUpdateCompletion.PayloadData> HandleEMVClessIssuerUpdate(IEMVClessIssuerUpdateEvents events, EMVClessIssuerUpdateCommand eMVClessIssuerUpdate, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessIssuerUpdateAsync()");
            var result = await Device.EMVContactlessIssuerUpdateAsync(events,
                                                                      new EMVContactlessIssuerUpdateRequest(string.IsNullOrEmpty(eMVClessIssuerUpdate.Payload.Data) ? null : new List<byte>(Convert.FromBase64String(eMVClessIssuerUpdate.Payload.Data)), eMVClessIssuerUpdate.Payload.Timeout),
                                                                      cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessIssuerUpdateAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success &&
                result.TransactionResult is not null)
            {
                // Build transaction output data
                EMVClessIssuerUpdateCompletion.PayloadData.ChipClass chip = new ((EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.TxOutcomeEnum)result.TransactionResult.TransactionOutcome,
                                                                                 (EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.CardholderActionEnum)result.TransactionResult.CardholderAction,
                                                                                 result.TransactionResult.DataRead.Count == 0 ? null : Convert.ToBase64String(result.TransactionResult.DataRead.ToArray()),
                                                                                 new EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass((EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.CvmEnum)result.TransactionResult.ClessOutcome.Cvm,
                                                                                                                                                            (EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.AlternateInterfaceEnum)result.TransactionResult.ClessOutcome.AlternateInterface,
                                                                                                                                                            result.TransactionResult.ClessOutcome.Receipt,
                                                                                                                                                            new EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.UiOutcomeClass(result.TransactionResult.ClessOutcome.UiOutcome.MessageId,
                                                                                                                                                                                                                                                      (EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.UiOutcomeClass.StatusEnum)result.TransactionResult.ClessOutcome.UiOutcome.Status,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiOutcome.HoldTime,
                                                                                                                                                                                                                                                      (EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.UiOutcomeClass.ValueQualifierEnum)result.TransactionResult.ClessOutcome.UiOutcome.ValueQualifier,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiOutcome.Value,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiOutcome.CurrencyCode,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiOutcome.LanguagePreferenceData),
                                                                                                                                                            new EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.UiRestartClass(result.TransactionResult.ClessOutcome.UiRestart.MessageId,
                                                                                                                                                                                                                                                      (EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.UiRestartClass.StatusEnum)result.TransactionResult.ClessOutcome.UiRestart.Status,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiRestart.HoldTime,
                                                                                                                                                                                                                                                      (EMVClessIssuerUpdateCompletion.PayloadData.ChipClass.ClessOutcomeClass.UiRestartClass.ValueQualifierEnum)result.TransactionResult.ClessOutcome.UiRestart.ValueQualifier,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiRestart.Value,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiRestart.CurrencyCode,
                                                                                                                                                                                                                                                      result.TransactionResult.ClessOutcome.UiRestart.LanguagePreferenceData),
                                                                                                                                                            result.TransactionResult.ClessOutcome.FieldOffHoldTime,
                                                                                                                                                            result.TransactionResult.ClessOutcome.CardRemovalTimeout,
                                                                                                                                                            result.TransactionResult.ClessOutcome.DiscretionaryData.Count == 0 ? null : Convert.ToBase64String(result.TransactionResult.ClessOutcome.DiscretionaryData.ToArray())));
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
