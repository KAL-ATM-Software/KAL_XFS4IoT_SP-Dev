/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * InitializedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.KeyManagement.Events
{

    [DataContract]
    [Event(Name = "KeyManagement.InitializedEvent")]
    public sealed class InitializedEvent : UnsolicitedEvent<MessagePayloadBase>
    {

        public InitializedEvent()
            : base()
        { }

    }
}
