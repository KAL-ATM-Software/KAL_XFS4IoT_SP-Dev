/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipIO_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.ChipIO")]
    public sealed class ChipIOCompletion : Completion<ChipIOCompletion.PayloadData>
    {
        public ChipIOCompletion(string RequestId, ChipIOCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                MediaJam,
                NoMedia,
                InvalidMedia,
                InvalidData,
                ProtocolNotSupported,
                AtrNotObtained,
                CardCollision,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string ChipProtocol = null, string ChipData = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ChipIOCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.ChipProtocol = ChipProtocol;
                this.ChipData = ChipData;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**mediaJam**
            ////The card is jammed. Operator intervention is required.**noMedia**
            ////There is no card inside the device.**invalidMedia**
            ////No chip found; card may have been inserted the wrong way.**invalidData**
            ////An error occurred while communicating with the chip.**protocolNotSupported**
            ////The protocol used was not supported by the Service Provider.**atrNotObtained**
            ////The ATR has not been obtained.**cardCollision**
            ////There was an unresolved collision of two or more contactless card signals.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///Identifies the protocol that is used to communicate with the chip. This field contains the same value as the corresponding field in the payload. This field should be ignored in Memory Card dialogs and will contain **notSupported** when returned for any Memory Card dialog.
            /// </summary>
            [DataMember(Name = "chipProtocol")] 
            public string ChipProtocol { get; private set; }
            /// <summary>
            ///The Base64 encoded data received from the chip.
            /// </summary>
            [DataMember(Name = "chipData")] 
            public string ChipData { get; private set; }

        }
    }
}
