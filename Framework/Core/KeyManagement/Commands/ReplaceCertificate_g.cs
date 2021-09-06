/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ReplaceCertificate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = ReplaceCertificate
    [DataContract]
    [Command(Name = "KeyManagement.ReplaceCertificate")]
    public sealed class ReplaceCertificateCommand : Command<ReplaceCertificateCommand.PayloadData>
    {
        public ReplaceCertificateCommand(int RequestId, ReplaceCertificateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string ReplaceCertificate = null)
                : base(Timeout)
            {
                this.ReplaceCertificate = ReplaceCertificate;
            }

            /// <summary>
            /// The Base64 encoded PKCS # 7 message that will replace the current Certificate Authority.
            /// The outer content uses the signedData content type, the inner content is a degenerate certificate only content containing the new 
            /// ca certificate and Inner Signed Data type The certificate should be in a format represented in DER encoded ASN.1 notation.
            /// </summary>
            [DataMember(Name = "replaceCertificate")]
            public string ReplaceCertificate { get; init; }

        }
    }
}
