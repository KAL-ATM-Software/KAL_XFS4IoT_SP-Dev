/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * FormLoadedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.TextTerminal.Events
{

    [DataContract]
    [Event(Name = "TextTerminal.FormLoadedEvent")]
    public sealed class FormLoadedEvent : Event<FormLoadedEvent.PayloadData>
    {

        public FormLoadedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Name = null)
                : base()
            {
                this.Name = Name;
            }

            /// <summary>
            /// Specifies the name of the form just loaded.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

        }

    }
}
