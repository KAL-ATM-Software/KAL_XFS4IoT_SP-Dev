/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetKeyDetail_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.GetKeyDetail")]
    public sealed class GetKeyDetailCompletion : Completion<GetKeyDetailCompletion.PayloadData>
    {
        public GetKeyDetailCompletion(int RequestId, GetKeyDetailCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string Keys = null, List<string> CommandKeys = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Keys = Keys;
                this.CommandKeys = CommandKeys;
            }

            /// <summary>
            /// String which holds the printable characters (numeric and alphanumeric keys) on the Text Terminal Unit, 
            /// e.g. "0123456789ABCabc" if those text terminal input keys are present. This property is omitted if no keys 
            /// of this type are present on the device.
            /// <example>0123456789ABCabc</example>
            /// </summary>
            [DataMember(Name = "keys")]
            public string Keys { get; init; }

            /// <summary>
            /// Supporting command keys on the Text Terminal Unit. This property can be omitted if no command keys supported.
            /// <example>["enter", "cancel"]</example>
            /// </summary>
            [DataMember(Name = "commandKeys")]
            [DataTypes(Pattern = @"^(enter|cancel|clear|backspace|help|doubleZero|tripleZero|arrowUp|arrowDown|arrowLeft|arrowRight)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$|.+")]
            public List<string> CommandKeys { get; init; }

        }
    }
}
