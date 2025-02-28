/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessIssuerUpdate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = EMVClessIssuerUpdate
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "CardReader.EMVClessIssuerUpdate")]
    public sealed class EMVClessIssuerUpdateCommand : Command<EMVClessIssuerUpdateCommand.PayloadData>
    {
        public EMVClessIssuerUpdateCommand(int RequestId, EMVClessIssuerUpdateCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<byte> Data = null)
                : base()
            {
                this.Data = Data;
            }

            /// <summary>
            /// Base64 encoded representation of the EMV data elements in a BER-TLV format received from the
            /// authorization response that are required to complete the transaction processing. The types of object
            /// that could be listed in *data* are:
            /// 
            /// * Authorization Code (if present)
            /// * Issuer Authentication Data (if present)
            /// * Issuer Scripts or proprietary payment system's data elements (if present) and any other data
            ///   elements if required.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Data { get; init; }

        }
    }
}
