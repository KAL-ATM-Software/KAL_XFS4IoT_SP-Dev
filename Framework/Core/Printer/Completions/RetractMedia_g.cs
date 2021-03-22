/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RetractMedia_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.RetractMedia")]
    public sealed class RetractMediaCompletion : Completion<RetractMediaCompletion.PayloadData>
    {
        public RetractMediaCompletion(string RequestId, RetractMediaCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                NoMediaPresent,
                RetractBinFull,
                MediaJammed,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? BinNumber = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(RetractMediaCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.BinNumber = BinNumber;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**noMediaPresent**
            ////  No media present on retract. Either there was no media present (in a position to be retracted from)  when the command was called or the media was removed during the retract.**retractBinFull**
            ////  The retract bin is full; no more media can be retracted. The current media is still in the device.**mediaJammed**
            ////  The media is jammed; operator intervention is required.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///The number of the retract bin where the media has actually been deposited.
            /// </summary>
            [DataMember(Name = "binNumber")] 
            public int? BinNumber { get; private set; }

        }
    }
}
