/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * LoadDefinition_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = LoadDefinition
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.LoadDefinition")]
    public sealed class LoadDefinitionCommand : Command<LoadDefinitionCommand.PayloadData>
    {
        public LoadDefinitionCommand(int RequestId, LoadDefinitionCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Definition = null, bool? Overwrite = null)
                : base()
            {
                this.Definition = Definition;
                this.Overwrite = Overwrite;
            }

            /// <summary>
            /// This contains the form (including sub-forms and frames) or media definition in text format as
            /// described in
            /// [Form, Sub-Form, Field, Frame, Table and Media Definitions](#printer.generalinformation.formandmediadefinitions).
            /// Only one form or media definition can be included in this property.
            /// <example>FormDefinition1</example>
            /// </summary>
            [DataMember(Name = "definition")]
            public string Definition { get; init; }

            /// <summary>
            /// Specifies if an existing form or media definition with the same name is to be replaced. If
            /// is true then an existing form or media definition with the same name will be replaced, unless the
            /// command fails with an error, where the definition will remain unchanged. If false this
            /// command will fail with an error if the form or media definition already exists.
            /// </summary>
            [DataMember(Name = "overwrite")]
            public bool? Overwrite { get; init; }

        }
    }
}
