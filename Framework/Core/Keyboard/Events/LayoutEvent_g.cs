/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
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
            /// The layout for the [Keyboard.DataEntry](#keyboard.dataentry) command.
            /// 
            /// There can be one or more frames included.
            /// 
            /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
            /// see the diagram for an example.
            /// </summary>
            [DataMember(Name = "data")]
            public List<LayoutFrameClass> Data { get; init; }

            /// <summary>
            /// The layout for the [Keyboard.PinEntry](#keyboard.pinentry) command.
            /// 
            /// There can be one or more frames included.
            /// 
            /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
            /// see the diagram for an example.
            /// </summary>
            [DataMember(Name = "pin")]
            public List<LayoutFrameClass> Pin { get; init; }

            /// <summary>
            /// The layout for the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command.
            /// 
            /// There can be one or more frames included.
            /// 
            /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
            /// see the diagram for an example.
            /// </summary>
            [DataMember(Name = "secure")]
            public List<LayoutFrameClass> Secure { get; init; }

        }

    }
}
