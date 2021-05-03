/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.PrintForm")]
    public sealed class PrintFormCompletion : Completion<PrintFormCompletion.PayloadData>
    {
        public PrintFormCompletion(string RequestId, PrintFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                FormNotFound,
                FlushFail,
                MediaOverflow,
                FieldSpecFailure,
                FieldError,
                MediaNotFound,
                MediaInvalid,
                FormInvalid,
                MediaSkewed,
                RetractBinFull,
                StackerFull,
                PageTurnFail,
                MediaTurnFail,
                ShutterFail,
                MediaJammed,
                CharSetData,
                PaperJammed,
                PaperOut,
                InkOut,
                TonerOut,
                SequenceInvalid,
                SourceInvalid,
                MediaRetained,
                BlackMark,
                MediaSize,
                MediaRejected,
                MediaRetracted,
                MsfError,
                NoMSF,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```formNotFound``` - The specified form definition cannot be found.
            /// * ```flushFail``` - The device was not able to flush data.
            /// * ```mediaOverflow``` - The form overflowed the media.
            /// * ```fieldSpecFailure``` - The syntax of the [fields](#printer.printform.command.properties.fields)
            ///   member is invalid.
            /// * ```fieldError``` - An error occurred while processing a field, causing termination of the print
            ///   request. A [Printer.FieldErrorEvent](#printer.fielderrorevent) event is posted with the details.
            /// * ```mediaNotFound``` - The specified media definition cannot be found.
            /// * ```mediaInvalid``` - The specified media definition is invalid.
            /// * ```formInvalid``` - The specified form definition is invalid.
            /// * ```mediaSkewed``` - The media skew exceeded the limit in the form definition.
            /// * ```retractBinFull``` - The retract bin is full. No more media can be retracted. The current media is
            ///   still in the device.
            /// * ```stackerFull``` - The internal stacker is full. No more media can be moved to the stacker.
            /// * ```pageTurnFail``` - The device was not able to turn the page.
            /// * ```mediaTurnFail``` - The device was not able to turn the inserted media.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed; operator intervention is required.
            /// * ```charSetData``` - Character set(s) supported by Service Provider is inconsistent with use of
            ///   *fields*.
            /// * ```paperJammed``` - The paper is jammed.
            /// * ```paperOut``` - The paper supply is empty.
            /// * ```inkOut``` - No stamping possible, stamping ink supply empty.
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// * ```sequenceInvalid``` - Programming error. Invalid command sequence (e.g.
            ///   [mediaControl](#printer.printform.command.properties.mediacontrol) = park and park position is
            ///   busy).
            /// * ```sourceInvalid``` - The selected paper source is not supported by the hardware.
            /// * ```mediaRetained``` - Media has been retracted in attempts to eject it. The device is clear and can
            ///   be used.
            /// * ```blackMark``` - Black mark detection has failed, nothing has been printed.
            /// * ```mediaSize``` - The media entered has an incorrect size and the media remains inside the device.
            /// * ```mediaRejected``` - The media was rejected during the insertion phase and no data has been
            ///   printed. The [Printer.MediaRejectedEvent](#printer.mediarejectedevent) event is posted with the
            ///   details. The device is still operational.
            /// * ```mediaRetracted``` - Presented media was automatically retracted before all wads could be
            ///   presented and before the command could complete successfully.
            /// * ```msfError``` - An error occurred while writing the magnetic stripe data.
            /// * ```noMSF``` - No magnetic stripe found; media may have been inserted or pulled through the wrong
            ///   way.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
