/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Command(Name = "KeyManagement.ReplaceCertificate")]
    public sealed class ReplaceCertificateCommand : Command<ReplaceCertificateCommand.PayloadData>
    {
        public ReplaceCertificateCommand(int RequestId, ReplaceCertificateCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<byte> ReplaceCertificate = null)
                : base()
            {
                this.ReplaceCertificate = ReplaceCertificate;
            }

            /// <summary>
            /// The PKCS#7 (See [[Ref. keymanagement-1](#ref-keymanagement-1)]) message that will replace the current
            /// Certificate Authority. The outer content uses the Signed-data content type, the inner content is a
            /// degenerate certificate only content containing the new CA certificate and Inner Signed Data type. The
            /// certificate should be in a format represented in DER encoded ASN.1 notation.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "replaceCertificate")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> ReplaceCertificate { get; init; }

        }
    }
}
