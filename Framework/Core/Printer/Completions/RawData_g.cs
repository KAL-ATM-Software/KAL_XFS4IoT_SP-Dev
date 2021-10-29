/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RawData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.RawData")]
    public sealed class RawDataCompletion : Completion<RawDataCompletion.PayloadData>
    {
        public RawDataCompletion(int RequestId, RawDataCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Data = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Data = Data;
            }

            public enum ErrorCodeEnum
            {
                ShutterFail,
                MediaJammed,
                PaperJammed,
                PaperOut,
                TonerOut,
                MediaRetained,
                BlackMark,
                MediaRetracted
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```paperJammed``` - The paper is jammed.
            /// * ```paperOut``` - The paper supply is empty.
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// * ```mediaRetained``` - Media has been retracted in attempts to eject it. The device is clear and can
            ///   be used.
            /// * ```blackMark``` - Black mark detection has failed, nothing has been printed.
            /// * ```mediaRetracted``` - Presented media was automatically retracted before all wads could be
            ///   presented and before the command could complete successfully.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// BASE64 encoded device dependent data received from the device.
            /// <example>UmF3RGF0YQ==</example>
            /// </summary>
            [DataMember(Name = "data")]
            public string Data { get; init; }

        }
    }
}
