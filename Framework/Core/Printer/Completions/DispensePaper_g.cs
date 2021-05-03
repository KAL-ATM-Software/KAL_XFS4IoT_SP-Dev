/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * DispensePaper_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.DispensePaper")]
    public sealed class DispensePaperCompletion : Completion<DispensePaperCompletion.PayloadData>
    {
        public DispensePaperCompletion(string RequestId, DispensePaperCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                PaperJammed,
                PaperOut,
                SequenceInvalid,
                SourceInvalid,
                MediaRetracted,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```paperJammed``` - The paper is jammed.
            /// * ```paperOut``` - The paper supply is empty.
            /// * ```sequenceInvalid``` - Programming error. Invalid command sequence (e.g. there is already media in
            ///   the print position).
            /// * ```sourceInvalid``` - The selected paper source is not supported by the hardware.
            /// * ```mediaRetracted``` - Presented media was automatically retracted before all wads could be
            ///   presented and before the command could complete successfully.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
