/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRawFile_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.PrintRawFile")]
    public sealed class PrintRawFileCompletion : Completion<PrintRawFileCompletion.PayloadData>
    {
        public PrintRawFileCompletion(string RequestId, PrintRawFileCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                FileNotFound,
                ShutterFail,
                MediaJammed,
                PaperJammed,
                PaperOut,
                TonerOut,
                FileIOError,
                NoMediaPresent,
                FlushFail,
                RetractBinFull,
                StackerFull,
                PageTurnFail,
                MediaTurnFail,
                InkOut,
                SequenceInvalid,
                MediaOverflow,
                MediaRetained,
                BlackMark,
                SourceInvalid,
                MediaRejected,
                MediaRetracted,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(PrintRawFileCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**fileNotFound**
            ////  The specified file cannot be found.**shutterFail**
            ////  Open or close of the shutter failed due to manipulation or hardware error.**mediaJammed**
            ////  The media is jammed; operator intervention is required.**paperJammed**
            ////  The paper is jammed.**paperOut**
            ////  The paper supply is empty.**tonerOut**
            ////  Toner or ink supply is empty or printing contrast with ribbon is not sufficient.**fileIOError**
            ////  Directory does not exist or a File IO error occurred while processing the file.**noMediaPresent**
            ////  No media is present in the device.**flushFail**
            ////  The device was not able to flush data.**retractBinFull**
            ////  The retract bin is full. No more media can be retracted. The current media is still in the device.**stackerFull**
            ////  The internal stacker is full. No more media can be moved to the stacker.**pageTurnFail**
            ////  The device was not able to turn the page.**mediaTurnFail**
            ////  The device was not able to turn the inserted media.**inkOut**
            ////  No stamping possible, stamping ink supply empty.**sequenceInvalid**
            ////  Programming error. Invalid command sequence (e.g. *park* and the parking station is busy).**mediaOverflow**
            ////  The print request has overflowed the print media (e.g. print on a single sheet printer exceeded one  page).**mediaRetained**
            ////  Media has been retracted in attempts to eject it. The device is clear and can be used.**blackMark**
            ////  Black mark detection has failed, nothing has been printed.**sourceInvalid**
            ////  The selected paper source is not supported by the hardware.**mediaRejected**
            ////  The media was rejected during the insertion phase and no data has been printed. The  [Printer.MediaRejectedEvent](#message-Printer.MediaRejectedEvent) event is posted with the details.  The device is still operational.**mediaRetracted**
            ////  Presented media was automatically retracted before all wads could be presented and before the  command could complete successfully.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
