/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetCommandNonce_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [Completion(Name = "Common.GetCommandNonce")]
    public sealed class GetCommandNonceCompletion : Completion<GetCommandNonceCompletion.PayloadData>
    {
        public GetCommandNonceCompletion(int RequestId, GetCommandNonceCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string CommandNonce = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.CommandNonce = CommandNonce;
            }

            /// <summary>
            /// A nonce that should be included in the authorisation token in a command used to provide 
            /// end to end protection.
            /// 
            /// The nonce will be given as an integer string, or HEX (upper case.)
            /// </summary>
            [DataMember(Name = "commandNonce")]
            [DataTypes(Pattern = "^[0-9A-F]{32}$|^[0-9]*$")]
            public string CommandNonce { get; init; }

        }
    }
}
