/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ClearScreen_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.ClearScreen")]
    public sealed class ClearScreenCompletion : Completion<MessagePayload>
    {
        public ClearScreenCompletion(int RequestId, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, null, CompletionCode, ErrorDescription)
        { }

    }
}
