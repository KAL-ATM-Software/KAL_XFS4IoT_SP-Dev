/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ControlMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.ControlMedia")]
    public sealed class ControlMediaCompletion : Completion<ControlMediaCompletion.PayloadData>
    {
        public ControlMediaCompletion(int RequestId, ControlMediaCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                NoMediaPresent,
                FlushFail,
                RetractBinFull,
                StackerFull,
                PageTurnFail,
                MediaTurnFail,
                ShutterFail,
                MediaJammed,
                PaperJammed,
                PaperOut,
                InkOut,
                TonerOut,
                SequenceInvalid,
                MediaRetained,
                BlackMark,
                MediaRetracted
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```noMediaPresent``` - The control action could not be completed because there is no media in the
            ///   device, the media is not in a position where it can be controlled, or (in the case of *retract*) has
            ///   been removed.
            /// * ```flushFail``` - The device was not able to flush data.
            /// * ```retractBinFull``` - The retract bin is full. No more media can be retracted. The current media is
            ///   still in the device.
            /// * ```stackerFull``` - The internal stacker is full. No more media can be moved to the stacker.
            /// * ```pageTurnFail``` - The device was not able to turn the page.
            /// * ```mediaTurnFail``` - The device was not able to turn the inserted media.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed; operator intervention is required.
            /// * ```paperJammed``` - The paper is jammed.
            /// * ```paperOut``` - The paper supply is empty.
            /// * ```inkOut``` - No stamping possible, stamping ink supply empty.
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// * ```sequenceInvalid``` - Programming error. Invalid command sequence (e.g. *park* and the parking
            ///   station is busy).
            /// * ```mediaRetained``` - Media has been retracted in attempts to eject it. The device is clear and can
            ///   be used.
            /// * ```blackMark``` - Black mark detection has failed, nothing has been printed.
            /// * ```mediaRetracted``` - Presented media was automatically retracted before all wads could be
            ///   presented and before the command could complete successfully.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
