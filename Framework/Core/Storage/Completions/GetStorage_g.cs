/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * GetStorage_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Storage.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.1")]
    [Completion(Name = "Storage.GetStorage")]
    public sealed class GetStorageCompletion : Completion<GetStorageCompletion.PayloadData>
    {
        public GetStorageCompletion()
            : base()
        { }

        public GetStorageCompletion(int RequestId, GetStorageCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(Dictionary<string, StorageUnitClass> Storage = null)
                : base()
            {
                this.Storage = Storage;
            }

            /// <summary>
            /// Object containing storage unit information. The property name is the storage unit identifier.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, StorageUnitClass> Storage { get; init; }

        }
    }
}
