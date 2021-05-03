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
        public GetKeyDetailCompletion(string RequestId, GetKeyDetailCompletion.PayloadData Payload)
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
            /// e.g. “0123456789ABCabc” if those text terminal input keys are present. This field is not set if no keys 
            /// of this type are present on the device.
            /// </summary>
            [DataMember(Name = "keys")] 
            public string Keys { get; private set; }
            /// <summary>
            /// Array of command keys on the Text Terminal Unit.
            /// </summary>
            [DataMember(Name = "commandKeys")] 
            public List<string> CommandKeys{ get; private set; }

        }
    }
}
