/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StorageErrorEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Storage.Events
{

    [DataContract]
    [XFS4Version(Version = "2.1")]
    [Event(Name = "Storage.StorageErrorEvent")]
    public sealed class StorageErrorEvent : Event<StorageErrorEvent.PayloadData>
    {

        public StorageErrorEvent()
            : base()
        { }

        public StorageErrorEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(FailureEnum? Failure = null, Dictionary<string, StorageUnitClass> Unit = null)
                : base()
            {
                this.Failure = Failure;
                this.Unit = Unit;
            }

            public enum FailureEnum
            {
                Empty,
                Error,
                Full,
                Locked,
                Invalid,
                Config,
                NotConfigured,
                FeedModuleProblem,
                PhysicalLocked,
                PhysicalUnlocked
            }

            /// <summary>
            /// Specifies the kind of failure that occurred in the storage unit. Following values are possible:
            /// 
            /// * ```empty``` - Specified storage unit is empty.
            /// * ```error``` - Specified storage unit has malfunctioned.
            /// * ```full``` - Specified storage unit is full.
            /// * ```locked``` - Specified storage unit is locked.
            /// * ```invalid``` - Specified storage unit is invalid.
            /// * ```config``` - An attempt has been made to change the settings of a self-configuring storage unit.
            /// * ```notConfigured``` - Specified storage unit is not configured.
            /// * ```feedModuleProblem``` - A problem has been detected with the feeding module.
            /// * ```physicalLocked``` - The storage unit could not be unlocked and remains physically locked.
            /// * ```physicalUnlocked``` - The storage unit could not be locked and remains physically unlocked.
            /// </summary>
            [DataMember(Name = "failure")]
            public FailureEnum? Failure { get; init; }

            /// <summary>
            /// The storage unit object that caused the problem.
            /// </summary>
            [DataMember(Name = "unit")]
            public Dictionary<string, StorageUnitClass> Unit { get; init; }

        }

    }
}
