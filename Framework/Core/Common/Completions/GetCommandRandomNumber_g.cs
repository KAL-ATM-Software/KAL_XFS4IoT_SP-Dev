/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetCommandRandomNumber_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [Completion(Name = "Common.GetCommandRandomNumber")]
    public sealed class GetCommandRandomNumberCompletion : Completion<GetCommandRandomNumberCompletion.PayloadData>
    {
        public GetCommandRandomNumberCompletion(string RequestId, GetCommandRandomNumberCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string CommandRandomNumber = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.CommandRandomNumber = CommandRandomNumber;
            }

            /// <summary>
            /// A nonce that should be included in the authorisation token in a command used to provide 
            /// end to end protection.
            /// 
            /// The nonce will be given as HEX (upper case.)
            /// </summary>
            [DataMember(Name = "commandRandomNumber")] 
            public string CommandRandomNumber { get; private set; }

        }
    }
}
