/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "CashManagement.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : UnsolicitedEvent<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ItemTargetEnumEnum? Target = null, string Unit = null, int? Index = null)
                : base()
            {
                this.Target = Target;
                this.Unit = Unit;
                this.Index = Index;
            }

            /// <summary>
            /// This property specifies the target. Following values are possible:
            /// 
            /// * ```singleUnit``` - A single storage unit defined by *unit*.
            /// * ```retract``` - A retract storage unit defined by *index*.
            /// * ```transport``` - The transport.
            /// * ```stacker``` - Intermediate stacker area.
            /// * ```reject``` - Reject storage unit.
            /// * ```itemCassette``` - Storage units which would be used during a cash-in transaction including recycling storage units.
            /// * ```cashIn``` - Storage units which would be used during a cash-in transaction but not including recycling storage units.
            /// * ```outDefault``` - Default output position.
            /// * ```outLeft``` - Left output position.
            /// * ```outRight``` - Right output position.
            /// * ```outCenter``` - Center output position.
            /// * ```outTop``` - Top output position.
            /// * ```outBottom``` - Bottom output position.
            /// * ```outFront``` - Front output position.
            /// * ```outRear``` - Rear output position.
            /// </summary>
            [DataMember(Name = "target")]
            public ItemTargetEnumEnum? Target { get; init; }

            /// <summary>
            /// If *target* is set to *singleUnit*, this property specifies the object name (as stated by the
            /// [Storage.GetStorage](#storage.getstorage) command) of a single storage unit. Ignored and may be null
            /// for all other cases.
            /// <example>unit4</example>
            /// </summary>
            [DataMember(Name = "unit")]
            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            public string Unit { get; init; }

            /// <summary>
            /// If *target* is set to *retract* this property defines a position inside the retract storage units. *index*
            /// starts with a value of 1 for the first retract position and increments by one for each subsequent position. If
            /// there are several retract storage units (of type *cashInRetract* or *cashOutRetract* as appropriate to the
            /// operation and as reported by *types* in
            /// [Storage.GetStorage](#storage.getstorage.completion.description.storage.unit1.cash.configuration)), *index*
            /// would be incremented from the first position of the first retract storage unit to the last position of the last
            /// retract storage unit. The maximum value of *index* is the sum of *maximum* of each retract storage unit. If
            /// *retractArea* is not set to *retract* the value of this property is ignored and may be null in command data.
            /// </summary>
            [DataMember(Name = "index")]
            [DataTypes(Minimum = 1)]
            public int? Index { get; init; }

        }

    }
}
