/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * CountsChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Storage.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Storage.CountsChangedEvent")]
    public sealed class CountsChangedEvent : UnsolicitedEvent<CountsChangedEvent.PayloadData>
    {

        public CountsChangedEvent()
            : base()
        { }

        public CountsChangedEvent(PayloadData Payload)
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
