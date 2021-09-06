/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * CertificateChangeEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.KeyManagement.Events
{

    [DataContract]
    [Event(Name = "KeyManagement.CertificateChangeEvent")]
    public sealed class CertificateChangeEvent : UnsolicitedEvent<CertificateChangeEvent.PayloadData>
    {

        public CertificateChangeEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(CertificateChangeEnum? CertificateChange = null)
                : base()
            {
                this.CertificateChange = CertificateChange;
            }

            public enum CertificateChangeEnum
            {
                Secondary
            }

            /// <summary>
            /// Specifies change of the certificate state inside of the KeyManagement.
            /// The following values are possible:
            /// * ```secondary``` - The certificate state of the encryptor is now Secondary and Primary Certificates will no longer be accepted.
            /// </summary>
            [DataMember(Name = "certificateChange")]
            public CertificateChangeEnum? CertificateChange { get; init; }

        }

    }
}
