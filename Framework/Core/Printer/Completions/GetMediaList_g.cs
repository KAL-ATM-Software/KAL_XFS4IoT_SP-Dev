/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Printer.GetMediaList")]
    public sealed class GetMediaListCompletion : Completion<GetMediaListCompletion.PayloadData>
    {
        public GetMediaListCompletion(int RequestId, GetMediaListCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<string> MediaList = null)
                : base()
            {
                this.MediaList = MediaList;
            }

            /// <summary>
            /// The list of media definition names. This will be null if no media definitions are available.
            /// <example>["Media1", "Media2"]</example>
            /// </summary>
            [DataMember(Name = "mediaList")]
            public List<string> MediaList { get; init; }

        }
    }
}
