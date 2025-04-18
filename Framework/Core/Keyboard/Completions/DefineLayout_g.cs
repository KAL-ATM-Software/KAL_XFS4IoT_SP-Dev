/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DefineLayout_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Keyboard.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Keyboard.DefineLayout")]
    public sealed class DefineLayoutCompletion : Completion<DefineLayoutCompletion.PayloadData>
    {
        public DefineLayoutCompletion(int RequestId, DefineLayoutCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                ModeNotSupported,
                FrameCoordinate,
                KeyCoordinate,
                FrameOverlap,
                KeyOverlap,
                TooManyFrames,
                TooManyKeys,
                KeyAlreadyDefined
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```modeNotSupported``` - The device does not support the float action.
            /// * ```frameCoordinate``` - A frame coordinate or size field is out of range.
            /// * ```keyCoordinate``` - A key coordinate or size field is out of range.
            /// * ```frameOverlap``` - Frames are overlapping.
            /// * ```keyOverlap``` - Keys are overlapping.
            /// * ```tooManyFrames``` -There are more frames defined than allowed.
            /// * ```tooManyKeys``` - There are more keys defined than allowed.
            /// * ```keyAlreadyDefined``` - The values for *key* and *shiftKey* can only be used once per layout.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
