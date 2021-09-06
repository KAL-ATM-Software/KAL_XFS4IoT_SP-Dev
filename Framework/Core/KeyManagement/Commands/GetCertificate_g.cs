/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GetCertificate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = GetCertificate
    [DataContract]
    [Command(Name = "KeyManagement.GetCertificate")]
    public sealed class GetCertificateCommand : Command<GetCertificateCommand.PayloadData>
    {
        public GetCertificateCommand(int RequestId, GetCertificateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, GetCertificateEnum? GetCertificate = null)
                : base(Timeout)
            {
                this.GetCertificate = GetCertificate;
            }

            public enum GetCertificateEnum
            {
                Enckey,
                Verificationkey,
                Hostkey
            }

            /// <summary>
            /// Specifies which public key certificate is requested.
            /// If the [KeyManagement.Status](#common.status) command indicates Primary Certificates are accepted, then the Primary Public Encryption Key or the 
            /// Primary Public Verification Key will be read out. If the [KeyManagement.Status](#common.status) command indicates Secondary Certificates are 
            /// accepted, then the Secondary Public Encryption Key or the Secondary Public Verification Key will be read out.
            /// The following values are possible:
            /// 
            /// * ```enckey``` - The corresponding encryption key is to be returned.
            /// * ```verificationkey``` - The corresponding verification key is to be returned.
            /// * ```hostkey``` - The host public key is to be returned.
            /// </summary>
            [DataMember(Name = "getCertificate")]
            public GetCertificateEnum? GetCertificate { get; init; }

        }
    }
}
