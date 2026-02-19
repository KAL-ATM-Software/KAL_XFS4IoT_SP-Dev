/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ReadForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = ReadForm
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.ReadForm")]
    public sealed class ReadFormCommand : Command<ReadFormCommand.PayloadData>
    {
        public ReadFormCommand()
            : base()
        { }

        public ReadFormCommand(int RequestId, ReadFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string FormName = null, List<string> Fields = null)
                : base()
            {
                this.FormName = FormName;
                this.Fields = Fields;
            }

            /// <summary>
            /// Specifies the name of the form.
            /// <example>My form</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// Specifies the field names from which to read input data. The fields
            /// are edited by the user in the order that the fields are specified
            /// within this parameter. If this property is null, data is read
            /// from all input fields on the form in the order they appear in the
            /// form (independent of the field screen position).
            /// <example>["Field1", "Field2"]</example>
            /// </summary>
            [DataMember(Name = "fields")]
            public List<string> Fields { get; init; }

        }
    }
}
