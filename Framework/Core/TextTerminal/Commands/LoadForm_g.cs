/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * LoadForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = LoadForm
    [DataContract]
    [Command(Name = "TextTerminal.LoadForm")]
    public sealed class LoadFormCommand : Command<LoadFormCommand.PayloadData>
    {
        public LoadFormCommand(int RequestId, LoadFormCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Definition = null, bool? Overwrite = null)
                : base(Timeout)
            {
                this.Definition = Definition;
                this.Overwrite = Overwrite;
            }

            /// <summary>
            /// This contains the form definition in text format as described in
            /// [Form and Field Definitions](#textterminal.generalinformation.formfielddefinition).
            /// Only one form definition can be included in this property.
            /// <example>See form description</example>
            /// </summary>
            [DataMember(Name = "definition")]
            public string Definition { get; init; }

            /// <summary>
            /// Specifies if an existing form definition with the same name is to be replaced. If this
            /// is true then an existing form definition with the same name will be replaced, unless the
            /// command fails with an error, where the definition will remain unchanged. If this is false this
            /// command will fail with an error if the form definition already exists.
            /// </summary>
            [DataMember(Name = "overwrite")]
            public bool? Overwrite { get; init; }

        }
    }
}
