/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * Digest_g.cs uses automatically generated parts. 
 * created at 4/20/2021 12:28:05 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Crypto.Completions
{
    [DataContract]
    [Completion(Name = "Crypto.Digest")]
    public sealed class DigestCompletion : Completion<DigestCompletion.PayloadData>
    {
        public DigestCompletion(string RequestId, DigestCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                AccessDenied,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string DigestOutput = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(DigestCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.DigestOutput = DigestOutput;
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for 
            ///                        any vendor specific reason.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            /// Contains the length and the data containing the calculated has.
            /// </summary>
            [DataMember(Name = "digestOutput")] 
            public string DigestOutput { get; private set; }

        }
    }
}
