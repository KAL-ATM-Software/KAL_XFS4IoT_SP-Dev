/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * Entry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Deposit.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "Deposit.Entry")]
    public sealed class EntryCompletion : Completion<EntryCompletion.PayloadData>
    {
        public EntryCompletion(int RequestId, EntryCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                DepFull,
                DepJammed,
                EnvSize,
                PrinterFail,
                ShutterNotClosed,
                ShutterNotOpened,
                ContainerMissing,
                Unknown,
                CharacterNotSupp,
                TonerOut
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```depFull``` - The deposit container is full.
            /// * ```depJammed``` - An envelope jam occurred in the deposit transport between the entry slot and the deposit container.
            /// * ```envSize``` - The envelope entered has an incorrect size.
            /// * ```printerFail``` - The printer failed.
            /// * ```shutterNotClosed``` - The shutter failed to close.
            /// * ```shutterNotOpened``` - The shutter failed to open.
            /// * ```containerMissing``` - The deposit container is not present.
            /// * ```unknown``` - The result of the deposit is not known.
            /// * ```characterNotSupp``` - Characters specified in *printData* are not supported by the Service - see
            ///   [unicodeSupport](#common.capabilities.completion.description.deposit.unicodesupport).
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// <example>containerMissing</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
