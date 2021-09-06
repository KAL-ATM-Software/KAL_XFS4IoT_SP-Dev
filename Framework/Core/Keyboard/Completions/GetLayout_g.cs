/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * GetLayout_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Keyboard.Completions
{
    [DataContract]
    [Completion(Name = "Keyboard.GetLayout")]
    public sealed class GetLayoutCompletion : Completion<GetLayoutCompletion.PayloadData>
    {
        public GetLayoutCompletion(int RequestId, GetLayoutCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<LayoutClass> Layout = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Layout = Layout;
            }

            public enum ErrorCodeEnum
            {
                ModeNotSupported
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```modeNotSupported``` - The specified entry mode is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Return supported layouts specified by the *entryMode* property. 
            /// </summary>
            [DataMember(Name = "layout")]
            public List<LayoutClass> Layout { get; init; }

        }
    }
}
