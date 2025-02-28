/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PaperThresholdEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Printer.PaperThresholdEvent")]
    public sealed class PaperThresholdEvent : UnsolicitedEvent<PaperThresholdEvent.PayloadData>
    {

        public PaperThresholdEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string PaperSource = null, ThresholdEnum? Threshold = null)
                : base()
            {
                this.PaperSource = PaperSource;
                this.Threshold = Threshold;
            }

            /// <summary>
            /// Specifies the paper source as one of the following:
            /// 
            /// * ```upper``` - The only paper source or the upper paper source, if there is more than one paper
            ///                 supply.
            /// * ```lower``` - The lower paper source.
            /// * ```external``` - The external paper.
            /// * ```aux``` - The auxiliary paper source.
            /// * ```aux2``` - The second auxiliary paper source.
            /// * ```[paper source identifier]``` - The vendor specific paper source.
            /// <example>lower</example>
            /// </summary>
            [DataMember(Name = "paperSource")]
            [DataTypes(Pattern = @"^upper$|^lower$|^external$|^aux$|^aux2$|^[a-zA-Z]([a-zA-Z0-9]*)$")]
            public string PaperSource { get; init; }

            public enum ThresholdEnum
            {
                Full,
                Low,
                Out
            }

            /// <summary>
            /// Specifies the current state of the paper source as one of the following:
            /// 
            /// * ```full``` - The paper in the paper source is in a good state.
            /// * ```low``` - The paper in the paper source is low.
            /// * ```out``` - The paper in the paper source is out.
            /// <example>out</example>
            /// </summary>
            [DataMember(Name = "threshold")]
            public ThresholdEnum? Threshold { get; init; }

        }

    }
}
