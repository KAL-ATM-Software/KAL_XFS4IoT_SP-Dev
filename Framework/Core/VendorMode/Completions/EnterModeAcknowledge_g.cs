/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * EnterModeAcknowledge_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.VendorMode.Completions
{
    [DataContract]
    [Completion(Name = "VendorMode.EnterModeAcknowledge")]
    public sealed class EnterModeAcknowledgeCompletion : Completion<EnterModeAcknowledgeCompletion.PayloadData>
    {
        public EnterModeAcknowledgeCompletion(int RequestId, EnterModeAcknowledgeCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription)
                : base(CompletionCode, ErrorDescription)
            {
            }

        }
    }
}
