/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
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
            /// <example>Form 1</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

        }

    }
}
