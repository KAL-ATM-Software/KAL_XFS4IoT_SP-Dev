/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CreateSignature_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.CreateSignature")]
    public sealed class CreateSignatureCompletion : Completion<CreateSignatureCompletion.PayloadData>
    {
        public CreateSignatureCompletion(int RequestId, CreateSignatureCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, CashManagement.CashItemClass NoteId = null, CashManagement.OrientationEnum? Orientation = null, string Signature = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.NoteId = NoteId;
                this.Orientation = Orientation;
                this.Signature = Signature;
            }

            public enum ErrorCodeEnum
            {
                TooManyItems,
                NoItems,
                CashInActive,
                ExchangeActive,
                PositionNotEmpty,
                ShutterNotOpen,
                ShutterNotClosed,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```tooManyItems``` - There was more than one banknote inserted for creating a signature.
            /// * ```noItems``` - There was no banknote to create a signature.
            /// * ```cashInActive``` - A cash-in transaction is active.
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```positionNotEmpty``` - The output position is not empty so a banknote cannot be inserted.
            /// * ```shutterNotOpen``` - Shutter failed to open.
            /// * ```shutterNotClosed``` - Shutter failed to close.
            /// * ```foreignItemsDetected``` - Foreign items have been detected in the input position.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "noteId")]
            public CashManagement.CashItemClass NoteId { get; init; }

            [DataMember(Name = "orientation")]
            public CashManagement.OrientationEnum? Orientation { get; init; }

            /// <summary>
            /// Base64 encoded vendor specific signature data. If no signature is available or has not been requested then this is omitted.
            /// </summary>
            [DataMember(Name = "signature")]
            public string Signature { get; init; }

        }
    }
}
