/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSAIssuerSignedItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = ExportRSAIssuerSignedItem
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.ExportRSAIssuerSignedItem")]
    public sealed class ExportRSAIssuerSignedItemCommand : Command<ExportRSAIssuerSignedItemCommand.PayloadData>
    {
        public ExportRSAIssuerSignedItemCommand()
            : base()
        { }

        public ExportRSAIssuerSignedItemCommand(int RequestId, ExportRSAIssuerSignedItemCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(TypeDataItemToExportEnum? ExportItemType = null, string Name = null)
                : base()
            {
                this.ExportItemType = ExportItemType;
                this.Name = Name;
            }

            [DataMember(Name = "exportItemType")]
            public TypeDataItemToExportEnum? ExportItemType { get; init; }

            /// <summary>
            /// Specifies the name of the public key to be exported.
            /// 
            /// The private/public key pair was installed during manufacture; see section [Default Keys and Security
            /// Item loaded during manufacture](#keymanagement.generalinformation.rklprocess.defaultkeyandsecurity)
            /// for a definition of these default keys.
            /// If this is null, then the default EPP public key that is used for symmetric key encryption
            /// is exported.
            /// <example>PKey01</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

        }
    }
}
