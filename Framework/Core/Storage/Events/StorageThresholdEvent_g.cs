/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StorageThresholdEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Storage.Events
{

    [DataContract]
    [Event(Name = "Storage.StorageThresholdEvent")]
    public sealed class StorageThresholdEvent : UnsolicitedEvent<StorageThresholdEvent.PayloadData>
    {

        public StorageThresholdEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData()
                : base()
            {
            }

            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            [System.Text.Json.Serialization.JsonIgnore]
            public Dictionary<string, StorageUnitClass> ExtendedProperties
            {
                get => MessageBase.ParseExtendedProperties<StorageUnitClass>(ExtensionData);
                set => ExtensionData = MessageBase.CreateExtensionData<StorageUnitClass>(value);
            }

        }

    }
}
