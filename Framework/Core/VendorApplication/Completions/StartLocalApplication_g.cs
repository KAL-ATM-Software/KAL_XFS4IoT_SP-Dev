/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * StartLocalApplication_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.VendorApplication.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "VendorApplication.StartLocalApplication")]
    public sealed class StartLocalApplicationCompletion : Completion<MessagePayload>
    {
        public StartLocalApplicationCompletion()
            : base()
        { }

        public StartLocalApplicationCompletion(int RequestId, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, null, CompletionCode, ErrorDescription)
        { }

    }
}
