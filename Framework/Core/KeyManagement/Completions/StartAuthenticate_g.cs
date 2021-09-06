/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartAuthenticate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.StartAuthenticate")]
    public sealed class StartAuthenticateCompletion : Completion<StartAuthenticateCompletion.PayloadData>
    {
        public StartAuthenticateCompletion(int RequestId, StartAuthenticateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string DataToSign = null, SigningMethodEnum? Signers = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.DataToSign = DataToSign;
                this.Signers = Signers;
            }

            /// <summary>
            /// The Base64 encoded data that must be signed by one of the authorities indicated by signers before the command referenced by 
            /// execution command can be executed. If the command specified by execution command does not require authentication, 
            /// then this property is omitted and the command result is success.
            /// </summary>
            [DataMember(Name = "dataToSign")]
            public string DataToSign { get; init; }


            [DataMember(Name = "signers")]
            public SigningMethodEnum? Signers { get; init; }

        }
    }
}
