/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
                public AuthenticationClass(SigningMethodEnum? Method = null, string Key = null, string Data = null)
                {
                    this.Method = Method;
                    this.Key = Key;
                    this.Data = Data;
                }


                [DataMember(Name = "method")]
                public SigningMethodEnum? Method { get; init; }

                /// <summary>
                /// If the *signer* is cbcmac or mac are specified, then this _signatureKey_ property is the name of a key with the key usage of key attribute is M0 to M8.
                /// If sigHost is specified for signer property, then this property signatureKey specifies the name of a previously loaded asymmetric key(i.e. an RSA Public Key).
                /// The default Signature Issuer public key (installed in a secure environment during manufacture) will be used, 
                /// if this signatureKey propery is omitted or contains the name of the default Signature Issuer as defined in the document [Default keys and securitry item loaded during manufacture](#keymanagement.generalinformation.rklprocess.defaultkeyandsecurity).
                /// Otherwise, this property should be omitted.
                /// </summary>
                [DataMember(Name = "key")]
                public string Key { get; init; }

                /// <summary>
                /// This property contains the signed version of the base64 encoded data that was provided by the KeyManagement device during the previous call to the StartExchange command.
                /// The signer specified by *signer* property is used to do the signing.
                /// Both the signature and the data that was signed must be verified before the operation is performed.
                /// If certHost, ca, or hl are specified for *signer* property, then _signedData_ is a PKCS #7 structure which includes the data that was returned by the StartAuthenticate command.
                /// The optional CRL field may or may not be included in the PKCS #7 _signedData_ structure.
                /// If the signer is certHostTr34, caTr34 or hlTr34, please refer to the X9 TR34-2012 [Ref. 42] for more details.
                /// If sigHost is specified for the *signer* property specified, then s is a PKCS #7 structure which includes the data that was returned by the StartAuthenticate command.
                /// If cmcmac or cmac are specified for the *signer* property specified, then _signatureKey_ must refer to a key loaded with the key usage of key attribute is M0 to M8.
                /// </summary>
                [DataMember(Name = "data")]
                public string Data { get; init; }

            }

            /// <summary>
            /// This property can be used to add authentication to perform an authenticated action.
            /// The [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate) command must be called before this property is used by the command required authenticated action. 
            /// If this property set without first calling the [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate) command, then this command will fail and [sequenceError](#api.generalinformation.commandsequence.completioncodes) will be returned.
            /// The [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate) command does not need to immediately precede the command with this property.
            /// It is acceptable for other commands to be executed until this property is set since [KeyManagement.StartAuthenticate](#keymanagement.startauthenticate) command is called, 
            /// except for any command that will clear from the KeyManagement interface the data that is being saved in order to verify the signed data provided in the command with this property. 
            /// If this occurs, then [sequenceError](#api.generalinformation.commandsequence.completioncodes) will be returned.
            /// </summary>
            [DataMember(Name = "authentication")]
            public AuthenticationClass Authentication { get; init; }

            /// <summary>
            /// Specifies the name of key being deleted. if this property is omitted. all keys are deleted.
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

        }
    }
}
