/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Deposit.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "Deposit.Reset")]
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
                DepFull,
                DepJammed,
                EnvJammed,
                ShutterNotOpened,
                ShutterNotClosed,
                ContainerMissing
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```depFull``` - The deposit container is full.
            /// * ```depJammed``` - An envelope jam occurred in the deposit transport between the entry slot and the
            ///   deposit container.
            /// * ```envJammed``` - An envelope jam occurred in the dispenser transport between the envelope supply and
            ///   the output slot.
            /// * ```shutterNotOpened``` - The shutter failed to open.
            /// * ```shutterNotClosed``` - The shutter failed to close.
            /// * ```containerMissing``` - The deposit container is not present.
            /// <example>containerMissing</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
