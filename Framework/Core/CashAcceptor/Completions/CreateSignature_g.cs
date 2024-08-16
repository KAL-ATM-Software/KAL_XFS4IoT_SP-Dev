/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.CreateSignature")]
    public sealed class CreateSignatureCompletion : Completion<CreateSignatureCompletion.PayloadData>
    {
        public CreateSignatureCompletion(int RequestId, CreateSignatureCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, string NoteType = null, CashManagement.OrientationEnum? Orientation = null, List<byte> Signature = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.NoteType = NoteType;
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
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
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

            /// <summary>
            /// A cash item as reported by [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes). This is null
            /// if the item was not identified as a cash item.
            /// <example>type20USD1</example>
            /// </summary>
            [DataMember(Name = "noteType")]
            [DataTypes(Pattern = @"^type[0-9A-Z]+$")]
            public string NoteType { get; init; }

            [DataMember(Name = "orientation")]
            public CashManagement.OrientationEnum? Orientation { get; init; }

            /// <summary>
            /// Base64 encoded vendor specific signature data. If no signature is available or has not been requested then this is null.
            /// <example>MAA5ADgANwA2ADUANAAz ...</example>
            /// </summary>
            [DataMember(Name = "signature")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Signature { get; init; }

        }
    }
}
