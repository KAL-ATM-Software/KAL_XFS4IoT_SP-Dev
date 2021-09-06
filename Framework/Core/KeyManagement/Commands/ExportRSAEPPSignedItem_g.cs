/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSAEPPSignedItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = ExportRSAEPPSignedItem
    [DataContract]
    [Command(Name = "KeyManagement.ExportRSAEPPSignedItem")]
    public sealed class ExportRSAEPPSignedItemCommand : Command<ExportRSAEPPSignedItemCommand.PayloadData>
    {
        public ExportRSAEPPSignedItemCommand(int RequestId, ExportRSAEPPSignedItemCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, TypeDataItemToExportEnum? ExportItemType = null, string Name = null, string SigKey = null, RSASignatureAlgorithmEnum? SignatureAlgorithm = null)
                : base(Timeout)
            {
                this.ExportItemType = ExportItemType;
                this.Name = Name;
                this.SigKey = SigKey;
                this.SignatureAlgorithm = SignatureAlgorithm;
            }

            /// <summary>
            /// Defines the type of data item to be exported from the device
            /// </summary>
            [DataMember(Name = "exportItemType")]
            public TypeDataItemToExportEnum? ExportItemType { get; init; }

            /// <summary>
            /// Specifies the name of the public key to be exported.
            /// This can either be the name of a key-pair generated through [KeyManagement.GenerateRsaKeyPair](#keymanagement.generatersakeypair) or 
            /// the name of one of the default key-pairs installed during manufacture.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            /// <summary>
            /// Specifies the name of the private key to use to sign the exported item.
            /// </summary>
            [DataMember(Name = "sigKey")]
            public string SigKey { get; init; }

            /// <summary>
            /// Specifies the algorithm to use to generate the Signature, returned in both the selfSignature and
            /// signature fields, as one of the following:
            /// 
            /// * ```na``` - No signature will be provided in selfSignature or signature. The requested item may still
            ///   be exported.  
            /// * ```rsassaPkcs1V15``` - RSASSA-PKCS1-v1.5 algorithm used.  
            /// * ```rsassaPss``` - RSASSA-PSS algorithm used.
            /// </summary>
            [DataMember(Name = "signatureAlgorithm")]
            public RSASignatureAlgorithmEnum? SignatureAlgorithm { get; init; }

        }
    }
}
