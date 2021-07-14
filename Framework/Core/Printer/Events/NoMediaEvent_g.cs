/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * NoMediaEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.NoMediaEvent")]
    public sealed class NoMediaEvent : Event<NoMediaEvent.PayloadData>
    {

        public NoMediaEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string UserPrompt = null)
                : base()
            {
                this.UserPrompt = UserPrompt;
            }

            /// <summary>
            /// The user prompt from the form definition. This will be omitted if either a form does not define a value
            /// for the user prompt or the event is being generated as the result of a command that does not use forms.
            /// 
            /// The application may use the this in any manner it sees fit, for example it might display the string to the
            /// operator, along with a message that the media should be inserted.
            /// </summary>
            [DataMember(Name = "userPrompt")]
            public string UserPrompt { get; init; }

        }

    }
}
