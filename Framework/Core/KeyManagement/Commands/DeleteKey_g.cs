/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * DeleteKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = DeleteKey
    [DataContract]
    [Command(Name = "KeyManagement.DeleteKey")]
    public sealed class DeleteKeyCommand : Command<DeleteKeyCommand.PayloadData>
    {
        public DeleteKeyCommand(int RequestId, DeleteKeyCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, AuthenticationClass Authentication = null, string Key = null)
                : base(Timeout)
            {
                this.Authentication = Authentication;
                this.Key = Key;
            }

            [DataContract]
            public sealed class AuthenticationClass
            {
                public AuthenticationClass(AuthenticationMethodEnum? Method = null, string Key = null, List<byte> Data = null)
                {
                    this.Method = Method;
                    this.Key = Key;
                    this.Data = Data;
                }

                [DataMember(Name = "method")]
                public AuthenticationMethodEnum? Method { get; init; }

                /// <summary>
                /// If *method* is cbcmac or mac, then this is the name of a key which has a MAC key usage e.g.
                /// M0.
                /// 
                /// If *method* is sigHost, then this specifies the name of a previously loaded asymmetric key (i.e. an RSA
                /// Public Key).
                /// 
                /// If this contains the name of the
                /// [default Signature Issuer](#keymanagement.generalinformation.rklprocess.defaultkeyandsecurity)
                /// or if omitted, the default Signature Issuer public key (installed in a secure environment during
                /// manufacture) will be used.
                /// <example>Key01</example>
                /// </summary>
                [DataMember(Name = "key")]
                public string Key { get; init; }

                /// <summary>
                /// This property contains the authenticated data (MAC, Signature) generated from the previous call to
                /// either the KeyManagement device during the previous call to
                /// [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate).
                /// 
                /// The authentication method specified by *method* is used to generate this data. Both this authentication
                /// data and the data used to generate the authentication data must be verified before the operation is
                /// performed.
                /// 
                /// If *certHost*, *ca*, or *hl* is specified in the *method* property, this contains a PKCS#7 signedData
                /// structure which includes the data that was returned by
                /// [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate). The optional CRL field may or may
                /// not be included in the PKCS#7 signedData structure.
                /// 
                /// If *certHostTr34*, *caTr34* or *hlTr34* is specified in the *method* property, please refer to the X9
                /// TR34-2019 [[Ref. keymanagement-9](#ref-keymanagement-9)] for more details.
                /// 
                /// If *sigHost* is specified in the *method* property, this is a PKCS#7 structure which includes the data
                /// that was returned by the *KeyManagement.StartAuthenticate* command.
                /// 
                /// If *cmcmac* or *cmac* is specified in the *method* property, then *key* must refer to a key with a MAC
                /// key usage key e.g. M0.
                /// <example>QXV0aGVudGljYXRpb24g ...</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
                public List<byte> Data { get; init; }

            }

            /// <summary>
            /// This property can be used to include authentication data if required by the command.
            /// 
            /// Additionally, if the command requires authentication:
            /// 
            /// * The [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate) command must be called before
            ///   this command to.
            /// * Commands which do not clear or modify the authentication data from the device may be executed between the
            ///   *KeyManagement.StartAuthenticate* and *keyManagement.Authenticate* command requests.
            /// * If prior to this command request, *KeyManagement.StartAuthenticate* is not called or a command clears the
            ///   authentication data from the device,
            ///   [sequenceError](#api.generalinformation.messagetypes.completionmessages.completioncodes) will be returned.
            /// </summary>
            [DataMember(Name = "authentication")]
            public AuthenticationClass Authentication { get; init; }

            /// <summary>
            /// Specifies the name of key being deleted. if this property is omitted. all keys are deleted.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

        }
    }
}
