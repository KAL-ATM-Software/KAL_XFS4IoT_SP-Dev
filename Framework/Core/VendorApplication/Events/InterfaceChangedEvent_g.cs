/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * InterfaceChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.VendorApplication.Events
{

    [DataContract]
    [Event(Name = "VendorApplication.InterfaceChangedEvent")]
    public sealed class InterfaceChangedEvent : UnsolicitedEvent<InterfaceChangedEvent.PayloadData>
    {

        public InterfaceChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ActiveInterfaceEnum? ActiveInterface = null)
                : base()
            {
                this.ActiveInterface = ActiveInterface;
            }

            public enum ActiveInterfaceEnum
            {
                Consumer,
                Operator
            }

            /// <summary>
            /// Specifies the active interface as one of the following values:
            /// 
            /// * ```consumer``` - The consumer interface.
            /// * ```operator``` - The operator interface.
            /// </summary>
            [DataMember(Name = "activeInterface")]
            public ActiveInterfaceEnum? ActiveInterface { get; init; }

        }

    }
}
