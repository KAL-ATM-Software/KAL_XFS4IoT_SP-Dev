/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.PinPad.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "PinPad.Reset")]
    public sealed class ResetCompletion : Completion<MessagePayload>
    {
        public ResetCompletion()
            : base()
        { }

        public ResetCompletion(int RequestId, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, null, CompletionCode, ErrorDescription)
        { }

    }
}
