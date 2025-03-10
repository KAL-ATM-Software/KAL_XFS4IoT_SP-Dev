/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = WriteForm
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.WriteForm")]
    public sealed class WriteFormCommand : Command<WriteFormCommand.PayloadData>
    {
        public WriteFormCommand(int RequestId, WriteFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string FormName = null, bool? ClearScreen = null, Dictionary<string, string> Fields = null)
                : base()
            {
                this.FormName = FormName;
                this.ClearScreen = ClearScreen;
                this.Fields = Fields;
            }

            /// <summary>
            /// Specifies the name of the form.
            /// <example>My form</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// Specifies whether the screen is cleared before displaying the form.
            /// </summary>
            [DataMember(Name = "clearScreen")]
            public bool? ClearScreen { get; init; }

            /// <summary>
            /// Details of the field(s) to write. The property is the field name and the value is the field value containing
            /// all the printable characters (numeric and alphanumeric) to display on the Text Terminal Unit keypad
            /// for this field. An example shows two fields to be written.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, string> Fields { get; init; }

        }
    }
}
