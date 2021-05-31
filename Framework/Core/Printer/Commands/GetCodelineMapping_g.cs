/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetCodelineMapping_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = GetCodelineMapping
    [DataContract]
    [Command(Name = "Printer.GetCodelineMapping")]
    public sealed class GetCodelineMappingCommand : Command<GetCodelineMappingCommand.PayloadData>
    {
        public GetCodelineMappingCommand(int RequestId, GetCodelineMappingCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, CodelineFormatEnum? CodelineFormat = null)
                : base(Timeout)
            {
                this.CodelineFormat = CodelineFormat;
            }

            public enum CodelineFormatEnum
            {
                Cmc7,
                E13b
            }

            /// <summary>
            /// Specifies the code-line format that the mapping for the special characters is required for. This field
            /// can be one of the following values:
            /// 
            /// * ```cmc7``` - Report the CMC7 mapping.
            /// * ```e13b``` - Report the E13B mapping.
            /// </summary>
            [DataMember(Name = "codelineFormat")]
            public CodelineFormatEnum? CodelineFormat { get; private set; }

        }
    }
}
