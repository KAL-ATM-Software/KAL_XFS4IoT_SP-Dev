/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetQueryForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = GetQueryForm
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.GetQueryForm")]
    public sealed class GetQueryFormCommand : Command<GetQueryFormCommand.PayloadData>
    {
        public GetQueryFormCommand(int RequestId, GetQueryFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string FormName = null)
                : base()
            {
                this.FormName = FormName;
            }

            /// <summary>
            /// The form name for which to retrieve details.
            /// <example>Example form</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

        }
    }
}
