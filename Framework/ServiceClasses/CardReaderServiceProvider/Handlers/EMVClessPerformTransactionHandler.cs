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
using XFS4IoT;

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
                                                                            new EMVContactlessPerformTransactionRequest(eMVClessPerformTransaction.Payload.Data, eMVClessPerformTransaction.Header.Timeout ?? 0),
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

                return new EMVClessPerformTransactionCompletion.PayloadData(CompletionCode: result.CompletionCode,
                                                                            ErrorDescription: result.ErrorDescription,
                                                                            ErrorCode: result.ErrorCode,
                                                                            Track1: Track1,
                                                                            Track2: Track2,
                                                                            Track3: Track3,
                                                                            Chip: Chip);
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
                TxOutcome: track.TransactionOutcome switch
                {
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.Approve => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.Approve,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.MultipleCards => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.MultipleCards,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.Decline => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.Decline,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.TryAnotherInterface => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.TryAnotherInterface,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.TryAgain => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.TryAgain,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.ConfirmationRequired => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.ConfirmationRequired,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.OnlineRequestCompletionRequired => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.OnlineRequestCompletionRequired,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.OnlineRequest => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.OnlineRequest,
                    EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.EndApplication => EMVClessPerformTransactionEMVClessTxOutputDataClass.TxOutcomeEnum.EndApplication,
                    _ => throw new InternalErrorException($"Unexpected transaction outcome specified for perform transaction command. {track.TransactionOutcome}"),
                },
                CardholderAction: track.CardholderAction switch
                {
                    EMVContactlessTransactionDataOutput.CardholderActionEnum.Retap => EMVClessPerformTransactionEMVClessTxOutputDataClass.CardholderActionEnum.Retap,
                    EMVContactlessTransactionDataOutput.CardholderActionEnum.HoldCard => EMVClessPerformTransactionEMVClessTxOutputDataClass.CardholderActionEnum.HoldCard,
                    EMVContactlessTransactionDataOutput.CardholderActionEnum.None => EMVClessPerformTransactionEMVClessTxOutputDataClass.CardholderActionEnum.None,
                    _ => throw new InternalErrorException($"Unexpected transaction cardholder action specified for perform transaction command. {track.CardholderAction}"),
                }, 
                DataRead: track.DataRead.Count == 0 ?
                          null :
                          track.DataRead,
                ClessOutcome: new(
                    Cvm: track.ClessOutcome.Cvm switch
                    {
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.ConfirmationCodeVerified => EMVClessOutcomeClass.CvmEnum.ConfirmationCodeVerified,
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.NoCVM => EMVClessOutcomeClass.CvmEnum.NoCVM,
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.OnlinePIN => EMVClessOutcomeClass.CvmEnum.OnlinePIN,
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.Sign => EMVClessOutcomeClass.CvmEnum.Sign,
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.NoCVMPreference => EMVClessOutcomeClass.CvmEnum.NoCVMPreference,
                        _ => throw new InternalErrorException($"Unexpected transaction outcome specified for CVM. {track.ClessOutcome.Cvm}"),
                    },
                    AlternateInterface: track.ClessOutcome.AlternateInterface switch
                    {
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.AlternateInterfaceEnum.Contact => EMVClessOutcomeClass.AlternateInterfaceEnum.Contact,
                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.AlternateInterfaceEnum.MagneticStripe => EMVClessOutcomeClass.AlternateInterfaceEnum.MagneticStripe,
                        _ => throw new InternalErrorException($"Unexpected transaction outcome specified for an alternate interface. {track.ClessOutcome.AlternateInterface}"),
                    },
                    Receipt: track.ClessOutcome.Receipt,
                    UiOutcome: track.ClessOutcome.UiOutcome is null ? 
                               null :
                               new EMVClessUIClass(
                                   MessageId: track.ClessOutcome.UiOutcome.MessageId,
                                   Status: track.ClessOutcome.UiOutcome.Status switch
                                   {
                                       EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Idle => EMVClessUIClass.StatusEnum.Idle,
                                       EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.NotReady => EMVClessUIClass.StatusEnum.NotReady,
                                       EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.CardReadOk => EMVClessUIClass.StatusEnum.CardReadOk,
                                       EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.ReadyToRead => EMVClessUIClass.StatusEnum.ReadyToRead,
                                       EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Processing => EMVClessUIClass.StatusEnum.Processing,
                                       _ => throw new InternalErrorException($"Unexpected transaction outcome specified for the status of UI outcome. {track.ClessOutcome.UiOutcome.Status}"),
                                   },
                                   HoldTime: track.ClessOutcome.UiOutcome.HoldTime,
                                   ValueDetails: track.ClessOutcome.UiOutcome.ValueQualifier == EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.NotApplicable ?
                                                null :
                                                new ValueDetailsClass(
                                                    Qualifier: track.ClessOutcome.UiOutcome.ValueQualifier switch
                                                    {
                                                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.Amount => ValueDetailsClass.QualifierEnum.Amount,
                                                        _ => ValueDetailsClass.QualifierEnum.Balance,
                                                    },
                                                    Value: track.ClessOutcome.UiOutcome.Value,
                                                    CurrencyCode: track.ClessOutcome.UiOutcome.CurrencyCode
                                                    ),
                                   LanguagePreferenceData: track.ClessOutcome.UiOutcome.LanguagePreferenceData),
                   UiRestart: track.ClessOutcome.UiRestart is null ? 
                              null :
                              new EMVClessUIClass(
                                  MessageId: track.ClessOutcome.UiRestart.MessageId,
                                  Status: track.ClessOutcome.UiRestart.Status switch
                                  {
                                      EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Idle => EMVClessUIClass.StatusEnum.Idle,
                                      EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.NotReady => EMVClessUIClass.StatusEnum.NotReady,
                                      EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.CardReadOk => EMVClessUIClass.StatusEnum.CardReadOk,
                                      EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.ReadyToRead => EMVClessUIClass.StatusEnum.ReadyToRead,
                                      EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Processing => EMVClessUIClass.StatusEnum.Processing,
                                      _ => throw new InternalErrorException($"Unexpected transaction outcome specified for the status of UI restart. {track.ClessOutcome.UiRestart.Status}"),
                                  },
                                  HoldTime: track.ClessOutcome.UiRestart.HoldTime,
                                  ValueDetails: track.ClessOutcome.UiRestart.ValueQualifier == EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.NotApplicable ?
                                      null :
                                  new ValueDetailsClass(
                                                    Qualifier: track.ClessOutcome.UiRestart.ValueQualifier switch
                                                    {
                                                        EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.Amount => ValueDetailsClass.QualifierEnum.Amount,
                                                        _ => ValueDetailsClass.QualifierEnum.Balance,
                                                    },
                                                    Value: track.ClessOutcome.UiRestart.Value,
                                                    CurrencyCode: track.ClessOutcome.UiRestart.CurrencyCode),
                                LanguagePreferenceData: track.ClessOutcome.UiRestart.LanguagePreferenceData),
                   FieldOffHoldTime: track.ClessOutcome.FieldOffHoldTime,
                   CardRemovalTimeout: track.ClessOutcome.CardRemovalTimeout,
                   DiscretionaryData: track.ClessOutcome.DiscretionaryData));
    }
}
