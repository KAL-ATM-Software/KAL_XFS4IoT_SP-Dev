/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "KeyManagement.ExportRSAIssuerSignedItem")]
    public sealed class ExportRSAIssuerSignedItemCommand : Command<ExportRSAIssuerSignedItemCommand.PayloadData>
    {
        public ExportRSAIssuerSignedItemCommand(int RequestId, ExportRSAIssuerSignedItemCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, TypeDataItemToExportEnum? ExportItemType = null, string Name = null)
                : base(Timeout)
            {
                this.ExportItemType = ExportItemType;
                this.Name = Name;
            }

            /// <summary>
            /// Defines the type of data item to be exported from the device. 
            /// </summary>
            [DataMember(Name = "exportItemType")]
            public TypeDataItemToExportEnum? ExportItemType { get; init; }

            /// <summary>
            /// Specifies the name of the public key to be exported.
            /// The private/public key pair was installed during manufacture; see section 8.1.8 (Default Keys and Security Item loaded during manufacture) 
            /// for a definition of these default keys. If name is an empty string, then the default EPP public key that is used for symmetric key encryption is exported.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

        }
    }
}
