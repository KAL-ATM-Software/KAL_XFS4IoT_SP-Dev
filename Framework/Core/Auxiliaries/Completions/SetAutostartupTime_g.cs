/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAutoStartUpTime_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Auxiliaries.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Auxiliaries.SetAutoStartUpTime")]
    public sealed class SetAutoStartUpTimeCompletion : Completion<MessagePayload>
    {
        public SetAutoStartUpTimeCompletion(int RequestId, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, null, CompletionCode, ErrorDescription)
        { }

    }
}
