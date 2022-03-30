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
using XFS4IoT.CardReader;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CardReader
{
    public partial class EMVClessPerformTransactionHandler
    {
        private async Task<EMVClessPerformTransactionCompletion.PayloadData> HandleEMVClessPerformTransaction(IEMVClessPerformTransactionEvents events, EMVClessPerformTransactionCommand eMVClessPerformTransaction, CancellationToken cancel)
        {
            if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless)
            {
                return new EMVClessPerformTransactionCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                            $"This device is not an intelligent contactless CardReader. {Common.CardReaderCapabilities.Type}");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessPerformTransactionAsync()");
            var result = await Device.EMVContactlessPerformTransactionAsync(new EMVClessCommandEvents(events),
                                                                            new EMVContactlessPerformTransactionRequest(eMVClessPerformTransaction.Payload.Data, eMVClessPerformTransaction.Payload.Timeout),
                                                                            cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessPerformTransactionAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success &&
                result.TransactionResults is not null &&
                result.TransactionResults.Count > 0)
            {
                EMVClessPerformTransactionEMVClessTxOutputDataClass Chip = null, Track1 = null, Track2 = null, Track3 = null;

                if (result.TransactionResults.ContainsKey(EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track1) &&
                    result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track1] is not null)
                {
                    Track1 = ToOutputClass(result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track1]);
                }

                if (result.TransactionResults.ContainsKey(EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track2) &&
                    result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track2] is not null)
                {
                    Track2 = ToOutputClass(result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track2]);
                }

                if (result.TransactionResults.ContainsKey(EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track3) &&
                    result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track3] is not null)
                {
                    Track3 = ToOutputClass(result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Track3]);
                }

                if (result.TransactionResults.ContainsKey(EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Chip) &&
                    result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Chip] is not null)
                {
                    Chip = ToOutputClass(result.TransactionResults[EMVContactlessPerformTransactionResult.DataSourceTypeEnum.Chip]);
                }

                return new EMVClessPerformTransactionCompletion.PayloadData(result.CompletionCode,
                                                                            result.ErrorDescription,
                                                                            result.ErrorCode,
                                                                            Track1,
                                                                            Track2,
                                                                            Track3,
                                                                            Chip);
            }
            else
            {
                return new EMVClessPerformTransactionCompletion.PayloadData(result.CompletionCode,
                                                                            result.ErrorDescription,
                                                                            result.ErrorCode);
            }
        }

        private EMVClessPerformTransactionEMVClessTxOutputDataClass ToOutputClass(EMVContactlessTransactionDataOutput track)
            => new(
                (EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum)track.TransactionOutcome, 
                (EMVClessPerformTransactionEMVClessTxOutputDataClass.CardholderActionEnum)track.CardholderAction, 
                track.DataRead,
                new((EMVClessPerformTransactionEMVClessTxOutputDataClass.ClessOutcomeClass.CvmEnum)track.ClessOutcome.Cvm,
                    (EMVClessPerformTransactionEMVClessTxOutputDataClass.ClessOutcomeClass.AlternateInterfaceEnum)track.ClessOutcome.AlternateInterface,
                    track.ClessOutcome.Receipt,
                    new EMVClessUIClass (track.ClessOutcome.UiOutcome.MessageId,
                                         (EMVClessUIClass.StatusEnum)track.ClessOutcome.UiOutcome.Status,
                                         track.ClessOutcome.UiOutcome.HoldTime,
                                         (EMVClessUIClass.ValueQualifierEnum)track.ClessOutcome.UiOutcome.ValueQualifier,
                                         track.ClessOutcome.UiOutcome.Value,
                                         track.ClessOutcome.UiOutcome.CurrencyCode,
                                         track.ClessOutcome.UiOutcome.LanguagePreferenceData),
                    new EMVClessUIClass (track.ClessOutcome.UiRestart.MessageId,
                                         (EMVClessUIClass.StatusEnum)track.ClessOutcome.UiRestart.Status,
                                         track.ClessOutcome.UiRestart.HoldTime,
                                         (EMVClessUIClass.ValueQualifierEnum)track.ClessOutcome.UiRestart.ValueQualifier,
                                         track.ClessOutcome.UiRestart.Value,
                                         track.ClessOutcome.UiRestart.CurrencyCode,
                                         track.ClessOutcome.UiRestart.LanguagePreferenceData),
                    track.ClessOutcome.FieldOffHoldTime,
                    track.ClessOutcome.CardRemovalTimeout,
                    track.ClessOutcome.DiscretionaryData));
    }
}
