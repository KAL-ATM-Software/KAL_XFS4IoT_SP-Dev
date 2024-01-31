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
    public partial class EMVClessIssuerUpdateHandler
    {
        private async Task<EMVClessIssuerUpdateCompletion.PayloadData> HandleEMVClessIssuerUpdate(IEMVClessIssuerUpdateEvents events, EMVClessIssuerUpdateCommand eMVClessIssuerUpdate, CancellationToken cancel)
        {
            if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless)
            {
                return new EMVClessIssuerUpdateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                      $"This device is not an intelligent contactless CardReader. {Common.CardReaderCapabilities.Type}");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessIssuerUpdateAsync()");
            var result = await Device.EMVContactlessIssuerUpdateAsync(new EMVClessCommandEvents(events),
                                                                      new EMVContactlessIssuerUpdateRequest(eMVClessIssuerUpdate.Payload.Data, eMVClessIssuerUpdate.Header.Timeout ?? 0),
                                                                      cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessIssuerUpdateAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success &&
                result.TransactionResult is not null)
            {
                // Build transaction output data
                EMVClessIssuerUpdateEMVClessTxOutputDataClass chip = 
                    new(TxOutcome: result.TransactionResult.TransactionOutcome switch
                        {
                            EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.Approve => EMVClessIssuerUpdateEMVClessTxOutputDataClass.TxOutcomeEnum.Approve,
                            EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.MultipleCards => EMVClessIssuerUpdateEMVClessTxOutputDataClass.TxOutcomeEnum.MultipleCards,
                            EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.Decline => EMVClessIssuerUpdateEMVClessTxOutputDataClass.TxOutcomeEnum.Decline,
                            EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.TryAnotherInterface => EMVClessIssuerUpdateEMVClessTxOutputDataClass.TxOutcomeEnum.TryAnotherInterface,
                            EMVContactlessTransactionDataOutput.TransactionOutcomeEnum.TryAgain => EMVClessIssuerUpdateEMVClessTxOutputDataClass.TxOutcomeEnum.TryAgain,
                            _ => throw new InternalErrorException($"Unexpected transaction outcome specified for update issuer command. {result.TransactionResult.TransactionOutcome}"),
                        },
                        DataRead: result.TransactionResult.DataRead.Count == 0 ?
                                null :
                                result.TransactionResult.DataRead,
                        ClessOutcome: new EMVClessOutcomeClass(
                            Cvm: result.TransactionResult.ClessOutcome.Cvm switch
                            {
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.ConfirmationCodeVerified => EMVClessOutcomeClass.CvmEnum.ConfirmationCodeVerified,
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.NoCVM => EMVClessOutcomeClass.CvmEnum.NoCVM,
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.OnlinePIN => EMVClessOutcomeClass.CvmEnum.OnlinePIN,
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.Sign => EMVClessOutcomeClass.CvmEnum.Sign,
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.CvmEnum.NoCVMPreference => EMVClessOutcomeClass.CvmEnum.NoCVMPreference,
                                _ => throw new InternalErrorException($"Unexpected transaction outcome specified for CVM. {result.TransactionResult.ClessOutcome.Cvm}"),
                            },
                            AlternateInterface: result.TransactionResult.ClessOutcome.AlternateInterface switch
                            {
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.AlternateInterfaceEnum.Contact => EMVClessOutcomeClass.AlternateInterfaceEnum.Contact,
                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.AlternateInterfaceEnum.MagneticStripe => EMVClessOutcomeClass.AlternateInterfaceEnum.MagneticStripe,
                                _ => throw new InternalErrorException($"Unexpected transaction outcome specified for an alternate interface. {result.TransactionResult.ClessOutcome.AlternateInterface}"),
                            },
                            Receipt: result.TransactionResult.ClessOutcome.Receipt,
                            UiOutcome: result.TransactionResult.ClessOutcome.UiOutcome is null ?
                                    null :
                                    new EMVClessUIClass(
                                        MessageId: result.TransactionResult.ClessOutcome.UiOutcome.MessageId,
                                        Status: result.TransactionResult.ClessOutcome.UiOutcome.Status switch
                                        {
                                            EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Idle => EMVClessUIClass.StatusEnum.Idle,
                                            EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.NotReady => EMVClessUIClass.StatusEnum.NotReady,
                                            EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.CardReadOk => EMVClessUIClass.StatusEnum.CardReadOk,
                                            EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.ReadyToRead => EMVClessUIClass.StatusEnum.ReadyToRead,
                                            EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Processing => EMVClessUIClass.StatusEnum.Processing,
                                            _ => throw new InternalErrorException($"Unexpected transaction outcome specified for the status of UI outcome. {result.TransactionResult.ClessOutcome.UiOutcome.Status}"),
                                        },
                                        HoldTime: result.TransactionResult.ClessOutcome.UiOutcome.HoldTime,
                                        ValueDetails: result.TransactionResult.ClessOutcome.UiOutcome.ValueQualifier == EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.NotApplicable ?
                                                    null :
                                                    new ValueDetailsClass(
                                                        Qualifier: result.TransactionResult.ClessOutcome.UiOutcome.ValueQualifier switch
                                                        {
                                                            EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.Amount => ValueDetailsClass.QualifierEnum.Amount,
                                                            _ => ValueDetailsClass.QualifierEnum.Balance,
                                                        },
                                                        Value: result.TransactionResult.ClessOutcome.UiOutcome.Value,
                                                        CurrencyCode: result.TransactionResult.ClessOutcome.UiOutcome.CurrencyCode
                                                        ),
                                        LanguagePreferenceData: result.TransactionResult.ClessOutcome.UiOutcome.LanguagePreferenceData),
                            UiRestart: result.TransactionResult.ClessOutcome.UiRestart is null ?
                                        null :
                                        new EMVClessUIClass(
                                            MessageId: result.TransactionResult.ClessOutcome.UiRestart.MessageId,
                                            Status: result.TransactionResult.ClessOutcome.UiRestart.Status switch
                                            {
                                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Idle => EMVClessUIClass.StatusEnum.Idle,
                                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.NotReady => EMVClessUIClass.StatusEnum.NotReady,
                                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.CardReadOk => EMVClessUIClass.StatusEnum.CardReadOk,
                                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.ReadyToRead => EMVClessUIClass.StatusEnum.ReadyToRead,
                                                EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.StatusEnum.Processing => EMVClessUIClass.StatusEnum.Processing,
                                                _ => throw new InternalErrorException($"Unexpected transaction outcome specified for the status of UI restart. {result.TransactionResult.ClessOutcome.UiRestart.Status}"),
                                            },
                                            HoldTime: result.TransactionResult.ClessOutcome.UiRestart.HoldTime,
                                            ValueDetails: result.TransactionResult.ClessOutcome.UiRestart.ValueQualifier == EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.NotApplicable ?
                                                        null :
                                                        new ValueDetailsClass(
                                                                Qualifier: result.TransactionResult.ClessOutcome.UiRestart.ValueQualifier switch
                                                                {
                                                                    EMVContactlessTransactionDataOutput.EMVContactlessOutcome.EMVContactlessUI.ValueQualifierEnum.Amount => ValueDetailsClass.QualifierEnum.Amount,
                                                                    _ => ValueDetailsClass.QualifierEnum.Balance,
                                                                },
                                                                Value: result.TransactionResult.ClessOutcome.UiRestart.Value,
                                                                CurrencyCode: result.TransactionResult.ClessOutcome.UiRestart.CurrencyCode),
                                            LanguagePreferenceData: result.TransactionResult.ClessOutcome.UiRestart.LanguagePreferenceData),
                            FieldOffHoldTime: result.TransactionResult.ClessOutcome.FieldOffHoldTime,
                            CardRemovalTimeout: result.TransactionResult.ClessOutcome.CardRemovalTimeout,
                            DiscretionaryData: result.TransactionResult.ClessOutcome.DiscretionaryData));

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
