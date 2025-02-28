/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Printer.ReadForm")]
    public sealed class ReadFormCompletion : Completion<ReadFormCompletion.PayloadData>
    {
        public ReadFormCompletion(int RequestId, ReadFormCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, List<string>> Fields = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                ReadNotSupported,
                FieldSpecFailure,
                FieldError,
                MediaNotFound,
                MediaSkewed,
                RetractBinFull,
                ShutterFail,
                MediaJammed,
                InkOut,
                LampInoperative,
                SequenceInvalid,
                MediaSize,
                MediaRejected,
                MsfError,
                NoMSF
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```readNotSupported``` - The device has no read capability.
            /// * ```fieldSpecFailure``` - The syntax of the
            ///   [fieldNames](#printer.readform.command.properties.fieldnames) member is invalid.
            /// * ```fieldError``` - An error occurred while processing a field, causing termination of the print
            ///   request. A [Printer.FieldErrorEvent](#printer.fielderrorevent) event is posted with the details.
            /// * ```mediaNotFound``` - The specified media definition cannot be found.
            /// * ```mediaSkewed``` - The media skew exceeded the limit in the form definition.
            /// * ```retractBinFull``` - The retract bin is full. No more media can be retracted. The current media is
            ///   still in the device.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```inkOut``` - No stamping possible, stamping ink supply empty.
            /// * ```lampInoperative``` - Imaging lamp is inoperative.
            /// * ```sequenceInvalid``` - Programming error. Invalid command sequence (e.g.
            ///   [mediaControl](#printer.readform.command.properties.mediacontrol) = park and park position is busy).
            /// * ```mediaSize``` - The media entered has an incorrect size.
            /// * ```mediaRejected``` - The media was rejected during the insertion phase. The
            ///   [Printer.MediaRejectedEvent](#printer.mediarejectedevent) event is posted with the details. The
            ///   device is still operational.
            /// * ```msfError``` - The MSF read operation specified by the form definition could not be completed
            ///   successfully due to invalid magnetic stripe data.
            /// * ```noMSF``` -  No magnetic stripe found; media may have been inserted or pulled through the wrong
            ///   way.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// An object containing fields read. If no fields were read, this is null.
            /// </summary>
            [DataMember(Name = "fields")]
            [System.Text.Json.Serialization.JsonConverter(typeof(StringOrArrayConverter))]
            public Dictionary<string, List<string>> Fields { get; init; }

        }
    }
}
