/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetMediaList_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.GetMediaList")]
    public sealed class GetMediaListCompletion : Completion<GetMediaListCompletion.PayloadData>
    {
        public GetMediaListCompletion(int RequestId, GetMediaListCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<string> MediaList = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.MediaList = MediaList;
            }

            /// <summary>
            /// The list of media names.
            /// </summary>
            [DataMember(Name = "mediaList")]
            public List<string> MediaList { get; private set; }

        }
    }
}
