/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * InsertCardEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [Event(Name = "CardReader.InsertCardEvent")]
    public sealed class InsertCardEvent : Event<MessagePayloadBase>
    {

        public InsertCardEvent(int RequestId)
            : base(RequestId)
        { }

    }
}
