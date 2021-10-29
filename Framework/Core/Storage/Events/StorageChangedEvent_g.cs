/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StorageChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Storage.Events
{

    [DataContract]
    [Event(Name = "Storage.StorageChangedEvent")]
    public sealed class StorageChangedEvent : UnsolicitedEvent<StorageChangedEvent.PayloadData>
    {

        public StorageChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(Dictionary<string, StorageUnitClass> Storage = null)
                : base()
            {
                this.Storage = Storage;
            }

            /// <summary>
            /// Object containing information for a single storage unit.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, StorageUnitClass> Storage { get; init; }

        }

    }
}
