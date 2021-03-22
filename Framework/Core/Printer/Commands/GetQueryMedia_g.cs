/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryMedia_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = GetQueryMedia
    [DataContract]
    [Command(Name = "Printer.GetQueryMedia")]
    public sealed class GetQueryMediaCommand : Command<GetQueryMediaCommand.PayloadData>
    {
        public GetQueryMediaCommand(string RequestId, GetQueryMediaCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string MediaName = null)
                : base(Timeout)
            {
                this.MediaName = MediaName;
            }

            /// <summary>
            ///The media name for which to retrieve details.
            /// </summary>
            [DataMember(Name = "mediaName")] 
            public string MediaName { get; private set; }

        }
    }
}
