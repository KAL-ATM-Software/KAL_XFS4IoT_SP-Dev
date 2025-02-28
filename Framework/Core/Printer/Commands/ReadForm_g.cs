/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ReadForm
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.ReadForm")]
    public sealed class ReadFormCommand : Command<ReadFormCommand.PayloadData>
    {
        public ReadFormCommand(int RequestId, ReadFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string FormName = null, List<string> FieldNames = null, string MediaName = null, MediaControlNullableClass MediaControl = null)
                : base()
            {
                this.FormName = FormName;
                this.FieldNames = FieldNames;
                this.MediaName = MediaName;
                this.MediaControl = MediaControl;
            }

            /// <summary>
            /// The name of the form.
            /// <example>Form1</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// The field names from which to read input data. If this is null, all input fields on the
            /// form will be read.
            /// <example>["FieldName1"]</example>
            /// </summary>
            [DataMember(Name = "fieldNames")]
            public List<string> FieldNames { get; init; }

            /// <summary>
            /// The media definition name. If null, no media definition applies.
            /// <example>MediaName1</example>
            /// </summary>
            [DataMember(Name = "mediaName")]
            public string MediaName { get; init; }

            /// <summary>
            /// Specifies the manner in which the media should be handled after the reading was done. If null, it means do
            /// none of these actions.
            /// 
            /// In the descriptions, *flush data* means flush any data to the printer that has not yet been printed from
            /// previous [Printer.PrintForm](#printer.printform) or [Printer.PrintNative](#printer.printnative) commands.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlNullableClass MediaControl { get; init; }

        }
    }
}
