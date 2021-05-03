/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * GenerateRandom_g.cs uses automatically generated parts. 
 * created at 4/20/2021 12:28:05 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Crypto.Completions
{
    [DataContract]
    [Completion(Name = "Crypto.GenerateRandom")]
    public sealed class GenerateRandomCompletion : Completion<GenerateRandomCompletion.PayloadData>
    {
        public GenerateRandomCompletion(string RequestId, GenerateRandomCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                ModeNotSupported,
                AccessDenied,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string RandomNumber = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(GenerateRandomCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.RandomNumber = RandomNumber;
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```modeNotSupported``` - The mode specified by modeOfUse is not supported.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            /// The generated random number.
            /// </summary>
            [DataMember(Name = "randomNumber")] 
            public string RandomNumber { get; private set; }

        }
    }
}
