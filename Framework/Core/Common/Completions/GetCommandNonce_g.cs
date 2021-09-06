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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string CommandNonce = null, int? NonceTimeout = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.CommandNonce = CommandNonce;
                this.NonceTimeout = NonceTimeout;
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

            /// <summary>
            /// The length of time that the returned nonce value will be value for, in seconds. The value is given in 
            /// seconds but it should not be assumed that the timeout will be accurate to the nearest second. The nonce
            /// may also become invalid before the timeout, for example because of a power failure. 
            /// 
            /// If this value is not returned then the nonce has no fixed maximum life-time, but it may still become 
            /// invalid, for example because of a power failure or when explicitly cleared. 
            /// 
            /// For the best security the device should enforce a timeout which is short enough to stop invalid use 
            /// of the nonce, but long enough to not block valid applications. A one hour timeout might be reasonable. 
            /// </summary>
            [DataMember(Name = "nonceTimeout")]
            public int? NonceTimeout { get; init; }

        }
    }
}
