/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessIssuerUpdate_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.EMVClessIssuerUpdate")]
    public sealed class EMVClessIssuerUpdateCompletion : Completion<EMVClessIssuerUpdateCompletion.PayloadData>
    {
        public EMVClessIssuerUpdateCompletion(string RequestId, EMVClessIssuerUpdateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                NoMedia,
                InvalidMedia,
                TransactionNotInitiated,
            }

            /// <summary>
            ///Contains the BER-TLV formatted data read from the chip. This will be omitted if no data has been returned.
            /// </summary>
            public class ChipClass
            {
                public enum TxOutcomeEnum
                {
                    MultipleCards,
                    Approve,
                    Decline,
                    OnlineRequest,
                    OnlineRequestCompletionRequired,
                    TryAgain,
                    TryAnotherInterface,
                    EndApplication,
                    ConfirmationRequired,
                }
                [DataMember(Name = "txOutcome")] 
                public TxOutcomeEnum? TxOutcome { get; private set; }
                public enum CardholderActionEnum
                {
                    None,
                    Retap,
                    HoldCard,
                }
                [DataMember(Name = "cardholderAction")] 
                public CardholderActionEnum? CardholderAction { get; private set; }
                [DataMember(Name = "dataRead")] 
                public string DataRead { get; private set; }
                
                /// <summary>
                ///The Entry Point Outcome specified in EMVCo Specifications for Contactless Payment Systems (Book A and B). This can be omitted for contactless chip card readers that do not follow EMVCo Entry Point Specifications.
                /// </summary>
                public class ClessOutcomeClass 
                {
                    public enum CvmEnum
                    {
                        OnlinePIN,
                        ConfirmationCodeVerified,
                        Sign,
                        NoCVM,
                        NoCVMPreference,
                    }
                    [DataMember(Name = "cvm")] 
                    public CvmEnum? Cvm { get; private set; }
                    public enum AlternateInterfaceEnum
                    {
                        Contact,
                        MagneticStripe,
                    }
                    [DataMember(Name = "alternateInterface")] 
                    public AlternateInterfaceEnum? AlternateInterface { get; private set; }
                    [DataMember(Name = "receipt")] 
                    public bool? Receipt { get; private set; }
                    
                    /// <summary>
                    ///The user interface details required to be displayed to the cardholder after processing the outcome of a contactless transaction. If no user interface details are required, this will be omitted. Please refer to EMVCo Contactless Specifications for Payment Systems Book A, Section 6.2 for details of the data within this object.
                    /// </summary>
                    public class UiOutcomeClass 
                    {
                        [DataMember(Name = "messageId")] 
                        public int? MessageId { get; private set; }
                        public enum StatusEnum
                        {
                            NotReady,
                            Idle,
                            ReadyToRead,
                            Processing,
                            CardReadOk,
                            ProcessingError,
                        }
                        [DataMember(Name = "status")] 
                        public StatusEnum? Status { get; private set; }
                        [DataMember(Name = "holdTime")] 
                        public int? HoldTime { get; private set; }
                        public enum ValueQualifierEnum
                        {
                            Amount,
                            Balance,
                            NotApplicable,
                        }
                        [DataMember(Name = "valueQualifier")] 
                        public ValueQualifierEnum? ValueQualifier { get; private set; }
                        [DataMember(Name = "value")] 
                        public string Value { get; private set; }
                        [DataMember(Name = "currencyCode")] 
                        public string CurrencyCode { get; private set; }
                        [DataMember(Name = "languagePreferenceData")] 
                        public string LanguagePreferenceData { get; private set; }

                        public UiOutcomeClass (int? MessageId, StatusEnum? Status, int? HoldTime, ValueQualifierEnum? ValueQualifier, string Value, string CurrencyCode, string LanguagePreferenceData)
                        {
                            this.MessageId = MessageId;
                            this.Status = Status;
                            this.HoldTime = HoldTime;
                            this.ValueQualifier = ValueQualifier;
                            this.Value = Value;
                            this.CurrencyCode = CurrencyCode;
                            this.LanguagePreferenceData = LanguagePreferenceData;
                        }
                    }
                    [DataMember(Name = "uiOutcome")] 
                    public UiOutcomeClass UiOutcome { get; private set; }
                    
                    /// <summary>
                    ///The user interface details required to be displayed to the cardholder when a transaction needs to be completed with a re-tap. If no user interface details are required, this will be omitted.
                    /// </summary>
                    public class UiRestartClass 
                    {
                        [DataMember(Name = "messageId")] 
                        public int? MessageId { get; private set; }
                        public enum StatusEnum
                        {
                            NotReady,
                            Idle,
                            ReadyToRead,
                            Processing,
                            CardReadOk,
                            ProcessingError,
                        }
                        [DataMember(Name = "status")] 
                        public StatusEnum? Status { get; private set; }
                        [DataMember(Name = "holdTime")] 
                        public int? HoldTime { get; private set; }
                        public enum ValueQualifierEnum
                        {
                            Amount,
                            Balance,
                            NotApplicable,
                        }
                        [DataMember(Name = "valueQualifier")] 
                        public ValueQualifierEnum? ValueQualifier { get; private set; }
                        [DataMember(Name = "value")] 
                        public string Value { get; private set; }
                        [DataMember(Name = "currencyCode")] 
                        public string CurrencyCode { get; private set; }
                        [DataMember(Name = "languagePreferenceData")] 
                        public string LanguagePreferenceData { get; private set; }

                        public UiRestartClass (int? MessageId, StatusEnum? Status, int? HoldTime, ValueQualifierEnum? ValueQualifier, string Value, string CurrencyCode, string LanguagePreferenceData)
                        {
                            this.MessageId = MessageId;
                            this.Status = Status;
                            this.HoldTime = HoldTime;
                            this.ValueQualifier = ValueQualifier;
                            this.Value = Value;
                            this.CurrencyCode = CurrencyCode;
                            this.LanguagePreferenceData = LanguagePreferenceData;
                        }
                    }
                    [DataMember(Name = "uiRestart")] 
                    public UiRestartClass UiRestart { get; private set; }
                    [DataMember(Name = "fieldOffHoldTime")] 
                    public int? FieldOffHoldTime { get; private set; }
                    [DataMember(Name = "cardRemovalTimeout")] 
                    public int? CardRemovalTimeout { get; private set; }
                    [DataMember(Name = "discretionaryData")] 
                    public string DiscretionaryData { get; private set; }

                    public ClessOutcomeClass (CvmEnum? Cvm, AlternateInterfaceEnum? AlternateInterface, bool? Receipt, UiOutcomeClass UiOutcome, UiRestartClass UiRestart, int? FieldOffHoldTime, int? CardRemovalTimeout, string DiscretionaryData)
                    {
                        this.Cvm = Cvm;
                        this.AlternateInterface = AlternateInterface;
                        this.Receipt = Receipt;
                        this.UiOutcome = UiOutcome;
                        this.UiRestart = UiRestart;
                        this.FieldOffHoldTime = FieldOffHoldTime;
                        this.CardRemovalTimeout = CardRemovalTimeout;
                        this.DiscretionaryData = DiscretionaryData;
                    }
                }
                [DataMember(Name = "clessOutcome")] 
                public ClessOutcomeClass ClessOutcome { get; private set; }

                public ChipClass (TxOutcomeEnum? TxOutcome, CardholderActionEnum? CardholderAction, string DataRead, ClessOutcomeClass ClessOutcome)
                {
                    this.TxOutcome = TxOutcome;
                    this.CardholderAction = CardholderAction;
                    this.DataRead = DataRead;
                    this.ClessOutcome = ClessOutcome;
                }


            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, ChipClass Chip = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(EMVClessIssuerUpdateCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.Chip = Chip;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**noMedia**
            ////The card was removed before completion of the read action.**invalidMedia**
            ////No track or chip found or card tapped cannot be used with this command (e.g. contactless storage cards or a different card than what was used to complete the [CardReader.EMVClessPerformTransaction](#command-CardReader.EMVClessPerformTransaction) command).**transactionNotInitiated**
            ////This command was issued before calling the CardReader.EMVClessPerformTransaction command.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///Contains the BER-TLV formatted data read from the chip. This will be omitted if no data has been returned.
            /// </summary>
            [DataMember(Name = "chip")] 
            public ChipClass Chip { get; private set; }

        }
    }
}
