/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
            /// This property specifies the target where items are to be moved to. Following values are possible:
            /// 
            /// * ```singleUnit``` - Move the items to a single storage unit defined by *unit*.
            /// * ```retract``` - Move the items to a retract storage unit.
            /// * ```transport``` - Move the items to the transport.
            /// * ```stacker``` - Move the items to the intermediate stacker area.
            /// * ```reject``` - Move the items to a reject storage unit.
            /// * ```itemCassette``` - Move the items to the storage units which would be used during a Cash In transaction including recycling storage units.
            /// * ```cashIn``` - Move the items to the storage units which would be used during a Cash In transaction but not including recycling storage units.
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
            /// [Storage.GetStorage](#storage.getstorage) command) of the single unit to
            /// be used for the storage of any items found.
            /// <example>unit4</example>
            /// </summary>
            [DataMember(Name = "unit")]
            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            public string Unit { get; init; }

            /// <summary>
            /// If *target* is set to *retract* this property defines the position inside the retract storage units into
            /// which the cash is to be retracted. *index* starts with a value of 1 for the first retract position
            /// and increments by one for each subsequent position. If there are several retract storage units
            /// (of type *retractCassette* in [Storage.GetStorage](#storage.getstorage)), *index* would be incremented from the
            /// first position of the first retract storage unit to the last position of the last retract storage unit.
            /// The maximum value of *index* is the sum of *maximum* of each retract storage unit. If *retractArea* is not
            /// set to *retract* the value of this property is ignored.
            /// </summary>
            [DataMember(Name = "index")]
            [DataTypes(Minimum = 1)]
            public int? Index { get; init; }

        }

    }
}
