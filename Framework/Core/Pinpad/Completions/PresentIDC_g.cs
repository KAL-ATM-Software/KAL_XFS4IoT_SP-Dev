/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PresentIDC_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.PinPad.Completions
{
    [DataContract]
    [Completion(Name = "PinPad.PresentIDC")]
    public sealed class PresentIDCCompletion : Completion<PresentIDCCompletion.PayloadData>
    {
        public PresentIDCCompletion(int RequestId, PresentIDCCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string ChipProtocol = null, string ChipData = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.ChipProtocol = ChipProtocol;
                this.ChipData = ChipData;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                NoPin,
                ProtocolNotSupported,
                InvalidData
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```noPin``` - The PIN has not been entered was not long enough or has been cleared.
            /// * ```protocolNotSupported``` - The specified protocol is not supported by the Service Provider.
            /// * ```invalidData``` - An error occurred while communicating with the chip.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Identifies the protocol that was used to communicate with the chip. 
            /// This property contains the same value as the corresponding property in the input.
            /// </summary>
            [DataMember(Name = "chipProtocol")]
            public string ChipProtocol { get; init; }

            /// <summary>
            /// The Base64 encoded data responded from the chip.
            /// </summary>
            [DataMember(Name = "chipData")]
            public string ChipData { get; init; }

        }
    }
}
