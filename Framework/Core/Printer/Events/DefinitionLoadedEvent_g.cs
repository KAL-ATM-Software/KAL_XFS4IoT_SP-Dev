/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * DefinitionLoadedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.DefinitionLoadedEvent")]
    public sealed class DefinitionLoadedEvent : UnsolicitedEvent<DefinitionLoadedEvent.PayloadData>
    {

        public DefinitionLoadedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Name = null, TypeEnum? Type = null)
                : base()
            {
                this.Name = Name;
                this.Type = Type;
            }

            /// <summary>
            /// Specifies the name of the form or media just loaded.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; private set; }

            public enum TypeEnum
            {
                Form,
                Name
            }

            /// <summary>
            /// Specifies the type of definition loaded. This field can be one of the following values:
            /// 
            /// * ```form``` - The form identified by [name](#printer.definitionloadedevent.event.properties.name) has
            ///   been loaded.
            /// * ```media``` - The media identified by *name* has been loaded.
            /// </summary>
            [DataMember(Name = "type")]
            public TypeEnum? Type { get; private set; }

        }

    }
}
