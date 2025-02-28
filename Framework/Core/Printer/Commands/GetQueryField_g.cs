/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryField_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = GetQueryField
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.GetQueryField")]
    public sealed class GetQueryFieldCommand : Command<GetQueryFieldCommand.PayloadData>
    {
        public GetQueryFieldCommand(int RequestId, GetQueryFieldCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string FormName = null, string FieldName = null)
                : base()
            {
                this.FormName = FormName;
                this.FieldName = FieldName;
            }

            /// <summary>
            /// The form name.
            /// <example>Form 10</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// The field name. If null, all fields on the form are retrieved.
            /// <example>Field 3</example>
            /// </summary>
            [DataMember(Name = "fieldName")]
            public string FieldName { get; init; }

        }
    }
}
