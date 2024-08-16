/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static XFS4IoT.MessageHeader;

namespace XFS4IoT
{
    [DataContract]
    [Acknowledge(Name = "Common.Acknowledge")]
    public class Acknowledge : Message<MessagePayloadBase>
    {
        /// <summary>
        /// Initialise any response object
        /// </summary>
        /// <param name="RequestId">request id</param>
        /// <param name="CommandName">acknowledge command name</param>
        /// <param name="Version">version of command, completion or event</param>
        /// <param name="Status">acknowledge status or null</param>
        public Acknowledge(int RequestId, string CommandName, string Version, StatusEnum? Status) :
            base(new MessageHeader(CommandName, RequestId, Version, MessageHeader.TypeEnum.Acknowledge, null, Status), null)
        { }
    }
}
