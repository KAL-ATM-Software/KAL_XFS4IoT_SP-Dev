/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * GetTransactionStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = GetTransactionStatus
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Check.GetTransactionStatus")]
    public sealed class GetTransactionStatusCommand : Command<GetTransactionStatusCommand.PayloadData>
    {
        public GetTransactionStatusCommand()
            : base()
        { }

        public GetTransactionStatusCommand(int RequestId, GetTransactionStatusCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(bool? IncludeImage = null)
                : base()
            {
                this.IncludeImage = IncludeImage;
            }

            /// <summary>
            /// Specifies whether image data is to be returned. If true, the [image](#check.gettransactionstatus.completion.properties.mediainfo.image.image) property 
            /// contains the image data, otherwise null. 
            /// The [Check.MediaDataEvent](#check.mediadataevent) contains all the image data requested for each item processed.
            /// </summary>
            [DataMember(Name = "includeImage")]
            public bool? IncludeImage { get; init; }

        }
    }
}
