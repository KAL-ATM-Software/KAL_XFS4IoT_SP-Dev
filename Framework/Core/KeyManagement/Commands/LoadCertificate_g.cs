/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * LoadCertificate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = LoadCertificate
    [DataContract]
    [Command(Name = "KeyManagement.LoadCertificate")]
    public sealed class LoadCertificateCommand : Command<LoadCertificateCommand.PayloadData>
    {
        public LoadCertificateCommand(int RequestId, LoadCertificateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, LoadOptionEnum? LoadOption = null, SignerEnum? Signer = null, string CertificateData = null)
                : base(Timeout)
            {
                this.LoadOption = LoadOption;
                this.Signer = Signer;
                this.CertificateData = CertificateData;
            }

            public enum LoadOptionEnum
            {
                NewHost,
                ReplaceHost
            }

            /// <summary>
            /// Specifies the method to use to load the certificate. The following values are possible:
            /// * ```newHost``` - Load a new Host certificate, where one has not already been loaded.
            /// * ```replaceHost``` - Replace (or rebind) the device to a new Host certificate, where the new Host certificate is signed by *signer*.
            /// </summary>
            [DataMember(Name = "loadOption")]
            public LoadOptionEnum? LoadOption { get; init; }

            public enum SignerEnum
            {
                CertHost,
                Ca,
                Hl
            }

            /// <summary>
            /// Specifies the signer of the certificate to be loaded. The following values are possible:
            /// * ```certHost``` - The certificate to be loaded is signed by the current Host. Cannot be combined with newHost.
            /// * ```ca``` - The certificate to be loaded is signed by the Certificate Authority (CA).
            /// * ```hl``` - The certificate to be loaded is signed by the Higher Level (HL) Authority.
            /// </summary>
            [DataMember(Name = "signer")]
            public SignerEnum? Signer { get; init; }

            /// <summary>
            /// The structure that contains the certificate that is to be loaded represented in DER encoded ASN.1
            /// notation. 
            /// 
            /// For loadNewHost, this data should be in a binary encoded PKCS #7 using the 'degenerate certificate only'
            /// case of the signed-data content type in which the inner content's data file is omitted and there are no
            /// signers.
            /// 
            /// For replaceHost, the message has an outer signedData content type with the signerInfo encryptedDigest 
            /// field containing the signature of signer. The inner content is binary encoded PKCS #7 using the
            /// degenerate certificate.
            /// 
            /// The optional CRL field may or may not be included in the PKCS #7 signedData structure.
            /// </summary>
            [DataMember(Name = "certificateData")]
            public string CertificateData { get; init; }

        }
    }
}
