/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CreateP6Signature_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.CreateP6Signature")]
    public sealed class CreateP6SignatureCompletion : Completion<CreateP6SignatureCompletion.PayloadData>
    {
        public CreateP6SignatureCompletion(int RequestId, CreateP6SignatureCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? NoteId = null, OrientationClass Orientation = null, string Signature = null)
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
            /// "tooManyItems": There was more than one banknote inserted for creating a signature.
            /// 
            /// "noItems": There was no banknote to create a signature.
            /// 
            /// "cashInActive": A cash-in transaction is active.
            /// 
            /// "exchangeActive": The device is in the exchange state.
            /// 
            /// "positionNotEmpty": The output position is not empty so a banknote cannot be inserted.
            /// 
            /// "shutterNotOpen": Shutter failed to open.
            /// 
            /// "shutterNotClosed": Shutter failed to close.
            /// 
            /// "foreignItemsDetected": Foreign items have been detected in the input position.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Identification of note type.
            /// </summary>
            [DataMember(Name = "noteId")]
            public int? NoteId { get; init; }

            [DataContract]
            public sealed class OrientationClass
            {
                public OrientationClass(bool? FrontTop = null, bool? FrontBottom = null, bool? BackTop = null, bool? BackBottom = null, bool? Unknown = null, bool? NotSupported = null)
                {
                    this.FrontTop = FrontTop;
                    this.FrontBottom = FrontBottom;
                    this.BackTop = BackTop;
                    this.BackBottom = BackBottom;
                    this.Unknown = Unknown;
                    this.NotSupported = NotSupported;
                }

                /// <summary>
                /// If note is inserted wide side as the leading edge, the note was inserted with the front image 
                /// facing up and the top edge of the note was inserted first. If the note is inserted short side 
                /// as the leading edge, the note was inserted with the front image face up and the left edge was inserted first.
                /// </summary>
                [DataMember(Name = "frontTop")]
                public bool? FrontTop { get; init; }

                /// <summary>
                /// If note is inserted wide side as the leading edge, the note was inserted with the front image 
                /// facing up and the bottom edge of the note was inserted first. If the note is inserted short side 
                /// as the leading edge, the note was inserted with the front image face up and the right edge was inserted first.
                /// </summary>
                [DataMember(Name = "frontBottom")]
                public bool? FrontBottom { get; init; }

                /// <summary>
                /// If note is inserted wide side as the leading edge, the note was inserted with the back image facing up and 
                /// the top edge of the note was inserted first. If the note is inserted short side as the leading edge, the note 
                /// was inserted with the back image face up and the left edge was inserted first.
                /// </summary>
                [DataMember(Name = "backTop")]
                public bool? BackTop { get; init; }

                /// <summary>
                /// If note is inserted wide side as the leading edge, the note was inserted with the back image facing up and the 
                /// bottom edge of the note was inserted first. If the note is inserted short side as the leading edge, the note was 
                /// inserted with the back image face up and the right edge was inserted first.
                /// </summary>
                [DataMember(Name = "backBottom")]
                public bool? BackBottom { get; init; }

                /// <summary>
                /// The orientation for the inserted note can not be determined.
                /// </summary>
                [DataMember(Name = "unknown")]
                public bool? Unknown { get; init; }

                /// <summary>
                /// The hardware is not capable to determine the orientation.
                /// </summary>
                [DataMember(Name = "notSupported")]
                public bool? NotSupported { get; init; }

            }

            /// <summary>
            /// Orientation of the entered banknote.
            /// </summary>
            [DataMember(Name = "orientation")]
            public OrientationClass Orientation { get; init; }

            /// <summary>
            /// Base64 encoded signature data.
            /// </summary>
            [DataMember(Name = "signature")]
            public string Signature { get; init; }

        }
    }
}
