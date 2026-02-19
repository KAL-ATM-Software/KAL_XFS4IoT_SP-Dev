/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * ModeExitedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.VendorMode.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "VendorMode.ModeExitedEvent")]
    public sealed class ModeExitedEvent : UnsolicitedEvent<ModeExitedEvent.PayloadData>
    {

        public ModeExitedEvent()
            : base()
        { }

        public ModeExitedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(List<string> ConnectedApplications = null)
                : base()
            {
                this.ConnectedApplications = ConnectedApplications;
            }

            /// <summary>
            /// List of applications that have not shut down.
            /// <example>["Application1", "Application2"]</example>
            /// </summary>
            [DataMember(Name = "connectedApplications")]
            public List<string> ConnectedApplications { get; init; }

        }

    }
}
