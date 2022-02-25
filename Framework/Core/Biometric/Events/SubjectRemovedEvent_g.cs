/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * SubjectRemovedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Biometric.Events
{

    [DataContract]
    [Event(Name = "Biometric.SubjectRemovedEvent")]
    public sealed class SubjectRemovedEvent : UnsolicitedEvent<MessagePayloadBase>
    {

        public SubjectRemovedEvent()
            : base()
        { }

    }
}
