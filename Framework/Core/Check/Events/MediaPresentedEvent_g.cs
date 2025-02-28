/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaPresentedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Event(Name = "Check.MediaPresentedEvent")]
    public sealed class MediaPresentedEvent : Event<MediaPresentedEvent.PayloadData>
    {

        public MediaPresentedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null, int? BunchIndex = null, string TotalBunches = null)
                : base()
            {
                this.Position = Position;
                this.BunchIndex = BunchIndex;
                this.TotalBunches = TotalBunches;
            }

            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            /// <summary>
            /// Specifies the index (starting from one) of the presented bunch (one or more items presented as a bunch).
            /// <example>2</example>
            /// </summary>
            [DataMember(Name = "bunchIndex")]
            [DataTypes(Minimum = 1)]
            public int? BunchIndex { get; init; }

            /// <summary>
            /// Specifies the total number of bunches to be returned from all positions. The total represents the number of
            /// bunches that will be returned as a result of a single command that presents media. The following values
            /// are possible:
            /// 
            ///   * ```[number]``` - The number of bunches to be presented.
            ///   * ```unknown``` - More than one bunch is required but the precise number is unknown.
            /// <example>1</example>
            /// </summary>
            [DataMember(Name = "totalBunches")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string TotalBunches { get; init; }

        }

    }
}
