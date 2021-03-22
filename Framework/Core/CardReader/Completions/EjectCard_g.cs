/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EjectCard_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.EjectCard")]
    public sealed class EjectCardCompletion : Completion<EjectCardCompletion.PayloadData>
    {
        public EjectCardCompletion(string RequestId, EjectCardCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                MediaJam,
                ShutterFail,
                NoMedia,
                MediaRetained,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(EjectCardCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**mediaJam**
            ////The card is jammed. Operator intervention is required. A possible scenario is also when an attempt to retain the card was made during attempts to eject it. The retain bin is full; no more cards can be retained. The current card is still in the device.**shutterFail**
            ////The open of the shutter failed due to manipulation or hardware error. Operator intervention is required.**noMedia**
            ////No card is present.**mediaRetained**
            ////The card has been retained during attempts to eject it. The device is clear and can be used.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
