/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * DispLight_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.DispLight")]
    public sealed class DispLightCompletion : Completion<DispLightCompletion.PayloadData>
    {
        public DispLightCompletion(int RequestId, DispLightCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, bool? Mode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Mode = Mode;
            }

            /// <summary>
            /// Specifies whether the lighting of the text terminal unit is switched on (true) or off (false).
            /// </summary>
            [DataMember(Name = "mode")]
            public bool? Mode { get; private set; }

        }
    }
}
