/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Event(Name = "Printer.PaperThresholdEvent")]
    public sealed class PaperThresholdEvent : UnsolicitedEvent<PaperThresholdEvent.PayloadData>
    {

        public PaperThresholdEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PaperSourceEnum? PaperSource = null, ThresholdEnum? Threshold = null)
                : base()
            {
                this.PaperSource = PaperSource;
                this.Threshold = Threshold;
            }

            public enum PaperSourceEnum
            {
                Upper,
                Lower,
                External,
                Aux,
                Aux2
            }

            /// <summary>
            /// Specifies the paper source as one of the following:
            /// 
            /// * ```upper``` - The only paper source or the upper paper source, if there is more than one paper supply.
            /// * ```lower``` - The lower paper source.
            /// * ```external``` - The external paper source (such as envelope tray or single sheet feed).
            /// * ```aux``` - The auxiliary paper source.
            /// * ```aux2``` - The second auxiliary paper source.
            /// </summary>
            [DataMember(Name = "paperSource")]
            public PaperSourceEnum? PaperSource { get; init; }

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
            /// </summary>
            [DataMember(Name = "threshold")]
            public ThresholdEnum? Threshold { get; init; }

        }

    }
}
