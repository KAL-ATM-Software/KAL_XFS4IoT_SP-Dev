/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Printer.Reset")]
    public sealed class ResetCompletion : Completion<ResetCompletion.PayloadData>
    {
        public ResetCompletion(int RequestId, ResetCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                ShutterFail,
                RetractBinFull,
                MediaJammed,
                PaperJammed
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```retractBinFull``` - The retract bin is full; no more media can be retracted. The current media is
            ///   still in the device.
            /// * ```mediaJammed``` - The media is jammed; operator intervention is required.
            /// * ```paperJammed``` - The paper is jammed.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
