/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRaw_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Printer.PrintRaw")]
    public sealed class PrintRawCompletion : Completion<PrintRawCompletion.PayloadData>
    {
        public PrintRawCompletion()
            : base()
        { }

        public PrintRawCompletion(int RequestId, PrintRawCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> Data = null)
                : base()
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
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
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
            /// Base64 encoded device dependent data received from the device. This may be null if no data is requested or
            /// returned.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Data { get; init; }

        }
    }
}
