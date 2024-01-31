/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = GetQueryMedia
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.GetQueryMedia")]
    public sealed class GetQueryMediaCommand : Command<GetQueryMediaCommand.PayloadData>
    {
        public GetQueryMediaCommand(int RequestId, GetQueryMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string MediaName = null)
                : base()
            {
                this.MediaName = MediaName;
            }

            /// <summary>
            /// The media name for which to retrieve details.
            /// <example>example media</example>
            /// </summary>
            [DataMember(Name = "mediaName")]
            public string MediaName { get; init; }

        }
    }
}
