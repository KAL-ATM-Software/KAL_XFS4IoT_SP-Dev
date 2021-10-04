/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * LayoutEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Keyboard.Events
{

    [DataContract]
    [Event(Name = "Keyboard.LayoutEvent")]
    public sealed class LayoutEvent : Event<LayoutEvent.PayloadData>
    {

        public LayoutEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(List<LayoutFrameClass> Data = null, List<LayoutFrameClass> Pin = null, List<LayoutFrameClass> Secure = null)
                : base()
            {
                this.Data = Data;
                this.Pin = Pin;
                this.Secure = Secure;
            }

            /// <summary>
            /// The layout for the DataEntry command.
            /// </summary>
            [DataMember(Name = "data")]
            public List<LayoutFrameClass> Data { get; init; }

            /// <summary>
            /// The layout for the PinEntry command.
            /// </summary>
            [DataMember(Name = "pin")]
            public List<LayoutFrameClass> Pin { get; init; }

            /// <summary>
            /// The layout for the SecureKeyEntry command.
            /// </summary>
            [DataMember(Name = "secure")]
            public List<LayoutFrameClass> Secure { get; init; }

        }

    }
}
