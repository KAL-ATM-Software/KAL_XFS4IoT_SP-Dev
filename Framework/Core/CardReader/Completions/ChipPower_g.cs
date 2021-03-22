/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipPower_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.ChipPower")]
    public sealed class ChipPowerCompletion : Completion<ChipPowerCompletion.PayloadData>
    {
        public ChipPowerCompletion(string RequestId, ChipPowerCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                ChipPowerNotSupported,
                MediaJam,
                NoMedia,
                InvalidMedia,
                InvalidData,
                AtrNotObtained,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string ChipData = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ChipPowerCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.ChipData = ChipData;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**chipPowerNotSupported**
            ////The specified action is not supported by the hardware device.**mediaJam**
            ////The card is jammed (only applies to contact user chips). Operator intervention is required.**noMedia**
            ////There is no card inside the device (may not apply for contactless user chips).**invalidMedia**
            ////No chip found; card may have been inserted or pulled through the wrong way.**invalidData**
            ////An error occurred while communicating with the chip.**atrNotObtained**
            ////The ATR has not been obtained (only applies to user chips).
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///The Base64 encoded data received from the chip.
            /// </summary>
            [DataMember(Name = "chipData")] 
            public string ChipData { get; private set; }

        }
    }
}
