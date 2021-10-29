/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "Storage.GetStorage")]
    public sealed class GetStorageCompletion : Completion<GetStorageCompletion.PayloadData>
    {
        public GetStorageCompletion(int RequestId, GetStorageCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, StorageUnitClass> Storage = null)
                : base(CompletionCode, ErrorDescription)
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
