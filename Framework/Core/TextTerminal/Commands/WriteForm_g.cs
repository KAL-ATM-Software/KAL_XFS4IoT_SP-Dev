/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteForm_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = WriteForm
    [DataContract]
    [Command(Name = "TextTerminal.WriteForm")]
    public sealed class WriteFormCommand : Command<WriteFormCommand.PayloadData>
    {
        public WriteFormCommand(string RequestId, WriteFormCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string FormName = null, bool? ClearScreen = null, List<string> Fields = null)
                : base(Timeout)
            {
                this.FormName = FormName;
                this.ClearScreen = ClearScreen;
                this.Fields = Fields;
            }

            /// <summary>
            ///Form name.
            /// </summary>
            [DataMember(Name = "formName")] 
            public string FormName { get; private set; }
            /// <summary>
            ///Specifies whether the screen is cleared before displaying the form (TRUE) or not (FALSE).
            /// </summary>
            [DataMember(Name = "clearScreen")] 
            public bool? ClearScreen { get; private set; }
            /// <summary>
            ///Specifies \"<fieldName>=<fieldValue>\" string. e.g. Field1=123. The <fieldValue> stands for a string containing all the printable characters (numeric and alphanumeric) to display on the text terminal unit key pad for this field.
            /// </summary>
            [DataMember(Name = "fields")] 
            public List<string> Fields{ get; private set; }

        }
    }
}
