/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ControlPassbook_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.ControlPassbook")]
    public sealed class ControlPassbookCompletion : Completion<ControlPassbookCompletion.PayloadData>
    {
        public ControlPassbookCompletion(string RequestId, ControlPassbookCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                NoMediaPresent,
                PageTurnFail,
                MediaJammed,
                PassbookClosed,
                LastOrFirstPageReached,
                MediaSize,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ControlPassbookCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**noMediaPresent**
            ////  No media present in a position where it should be or the media was removed during the operation.**pageTurnFail**
            ////  The device was not able to turn the page.**mediaJammed**
            ////  The media is jammed. Operator intervention is required.**passbookClosed**
            ////  There were fewer pages left than specified to turn. As a result of the operation, the passbook has  been closed.**lastOrFirstPageReached**
            ////  The printer cannot close the passbook because there were fewer pages left than specified to turn. As  a result of the operation, the last or the first page has been reached and is open.**mediaSize**
            ////  The media has an incorrect size.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
