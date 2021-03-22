/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessReadStatusEvent_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [Event(Name = "CardReader.EMVClessReadStatusEvent")]
    public sealed class EMVClessReadStatusEvent : Event<EMVClessReadStatusEvent.PayloadData>
    {

        public EMVClessReadStatusEvent(string RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public enum StatusEnum
            {
                NotReady,
                Idle,
                ReadyToRead,
                Processing,
                CardReadOk,
                ProcessingError,
            }

            public enum ValueQualifierEnum
            {
                Amount,
                Balance,
                NotApplicable,
            }


            public PayloadData(int? MessageId = null, StatusEnum? Status = null, int? HoldTime = null, ValueQualifierEnum? ValueQualifier = null, string Value = null, string CurrencyCode = null, string LanguagePreferenceData = null)
                : base()
            {
                this.MessageId = MessageId;
                this.Status = Status;
                this.HoldTime = HoldTime;
                this.ValueQualifier = ValueQualifier;
                this.Value = Value;
                this.CurrencyCode = CurrencyCode;
                this.LanguagePreferenceData = LanguagePreferenceData;
            }

            /// <summary>
            ///Represents the EMVCo defined message identifier that indicates the text string to be displayed, e.g., 0x1B is the “Authorising Please Wait” message (see EMVCo Contactless Specifications for Payment Systems Book A, Section 9.4).
            /// </summary>
            [DataMember(Name = "messageId")] 
            public int? MessageId { get; private set; }
            /// <summary>
            ///Represents the EMVCo defined transaction status value to be indicated through the Beep/LEDs as one of the following:**notReady**
            ////Contactless card reader is not able to communicate with a card. This status occurs towards the end of a contactless transaction or if the reader is not powered on.**idle**
            ////Contactless card reader is powered on, but the reader field is not yet active for communication with a card.**readyToRead**
            ////Contactless card reader is powered on and attempting to initiate communication with a card.**processing**
            ////Contactless card reader is in the process of reading the card.**cardReadOk**
            ////Contactless card reader was able to read a card successfully.**processingError**
            ////Contactless card reader was not able to process the card successfully.
            /// </summary>
            [DataMember(Name = "status")] 
            public StatusEnum? Status { get; private set; }
            /// <summary>
            ///Represents the hold time in units of 100 milliseconds for which the application should display the message before processing the next user interface data.
            /// </summary>
            [DataMember(Name = "holdTime")] 
            public int? HoldTime { get; private set; }
            /// <summary>
            ///Qualifies *value*. This data is defined by EMVCo as either “Amount” or “Balance” as one of the following:**amount**
            ////*value* is an Amount.**balance**
            ////*value* is a Balance.**notApplicable**
            ////*value* is neither of the above.
            /// </summary>
            [DataMember(Name = "valueQualifier")] 
            public ValueQualifierEnum? ValueQualifier { get; private set; }
            /// <summary>
            ///Represents the value of the amount or balance (as specified by *valueQualifier*) to be displayed where appropriate
            /// </summary>
            [DataMember(Name = "value")] 
            public string Value { get; private set; }
            /// <summary>
            ///Represents the numeric value of currency code as per ISO 4217.
            /// </summary>
            [DataMember(Name = "currencyCode")] 
            public string CurrencyCode { get; private set; }
            /// <summary>
            ///Represents the language preference (EMV Tag ‘5F2D’) if returned by the card. The application should use this data to display all messages in the specified language until the transaction concludes.
            /// </summary>
            [DataMember(Name = "languagePreferenceData")] 
            public string LanguagePreferenceData { get; private set; }
        }

    }
}
