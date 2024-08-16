/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ActionItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.ActionItem")]
    public sealed class ActionItemCompletion : Completion<ActionItemCompletion.PayloadData>
    {
        public ActionItemCompletion(int RequestId, ActionItemCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                InkOut,
                MediaBinError,
                MediaJammed,
                NoMediaPresent,
                PositionNotEmpty,
                RefusedItems,
                ScannerInop,
                ShutterFail,
                TonerOut
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaBinError``` - A storage unit caused a problem. A [Storage.StorageErrorEvent](#storage.storageerrorevent)
            /// will be posted with the details.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// * ```inkOut``` - No stamping possible, stamping ink supply empty.
            /// * ```noMediaPresent``` - There were no items present in the device.
            /// * ```scannerInop``` -\tThe scanner is inoperative.
            /// * ```refusedItems``` - Programming error, refused items that must be returned have not been presented.
            /// * ```positionNotEmpty``` - One of the input/output/refused positions is not empty.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
