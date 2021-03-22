/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * LampThresholdEvent_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.LampThresholdEvent")]
    public sealed class LampThresholdEvent : Event<LampThresholdEvent.PayloadData>
    {

        public LampThresholdEvent(string RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public enum StateEnum
            {
                Ok,
                Fading,
                Inop,
            }


            public PayloadData(StateEnum? State = null)
                : base()
            {
                this.State = State;
            }

            /// <summary>
            ///Specifies the current state of the imaging lamp as one of the following values:**ok**
            ////  The imaging lamp is in a good state.**fading**
            ////  The imaging lamp is fading and should be changed.**inop**
            ////  The imaging lamp is inoperative.
            /// </summary>
            [DataMember(Name = "state")] 
            public StateEnum? State { get; private set; }
        }

    }
}
