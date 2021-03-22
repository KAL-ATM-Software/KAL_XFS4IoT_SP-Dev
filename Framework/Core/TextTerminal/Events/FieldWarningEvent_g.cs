/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * FieldWarningEvent_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.TextTerminal.Events
{

    [DataContract]
    [Event(Name = "TextTerminal.FieldWarningEvent")]
    public sealed class FieldWarningEvent : Event<MessagePayloadBase>
    {

        public FieldWarningEvent(string RequestId)
            : base(RequestId)
        { }

    }
}
