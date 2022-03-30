/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<byte> DataToSign = null, AuthenticationMethodEnum? Signers = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.DataToSign = DataToSign;
                this.Signers = Signers;
            }

            /// <summary>
            /// The data that must be authenticated by one of the authorities indicated by *methods* before the
            /// *command* can be executed. If the *command* does not require authentication, this property is omitted
            /// and the command result is success.
            /// <example>QXV0aGVudGljYXRpb24g ...</example>
            /// </summary>
            [DataMember(Name = "dataToSign")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> DataToSign { get; init; }

            [DataMember(Name = "signers")]
            public AuthenticationMethodEnum? Signers { get; init; }

        }
    }
}
