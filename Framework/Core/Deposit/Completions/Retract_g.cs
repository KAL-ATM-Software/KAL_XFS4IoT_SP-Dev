/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * Retract_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Deposit.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "Deposit.Retract")]
    public sealed class RetractCompletion : Completion<RetractCompletion.PayloadData>
    {
        public RetractCompletion(int RequestId, RetractCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                NoEnv,
                PrinterFail,
                ShutterNotClosed,
                ContainerMissing,
                CharacterNotSupp,
                TonerOut
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```depFull``` - The deposit container is full.
            /// * ```depJammed``` - An envelope jam occurred in the deposit transport between the entry slot and the
            ///   deposit container.
            /// * ```envJammed``` - An envelope jam occurred between the entry slot and the envelope container (may only
            ///   occur with hardware that retracts to the envelope container).
            /// * ```noEnv``` - No envelope to retract.
            /// * ```printerFail``` - The printer failed.
            /// * ```shutterNotClosed``` - The shutter failed to close.
            /// * ```containerMissing``` - The deposit container is not present.
            /// * ```characterNotSupp``` - Characters specified in *printData* are not supported by the Service - see
            ///   [unicodeSupport](#common.capabilities.completion.description.deposit.unicodesupport).
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// <example>characterNotSupp</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
