/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Printer.GetQueryField")]
    public sealed class GetQueryFieldCommand : Command<GetQueryFieldCommand.PayloadData>
    {
        public GetQueryFieldCommand(int RequestId, GetQueryFieldCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string FormName = null, string FieldName = null)
                : base(Timeout)
            {
                this.FormName = FormName;
                this.FieldName = FieldName;
            }

            /// <summary>
            /// The form name.
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; private set; }

            /// <summary>
            /// The name of the field about which to retrieve details. If omitted, then details are retrieved for all
            /// fields on the form.
            /// </summary>
            [DataMember(Name = "fieldName")]
            public string FieldName { get; private set; }

        }
    }
}
