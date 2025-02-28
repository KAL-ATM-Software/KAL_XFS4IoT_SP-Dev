/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Runtime.Serialization;

namespace XFS4IoT.Completions
{
    /// <summary>
    /// A message object representing payload on responses
    /// </summary>
    [DataContract]
    public class MessagePayload : MessagePayloadBase
    {
        /// <summary>
        /// Constructor of the common payload for response
        /// </summary>
        public MessagePayload()
        { }
    }
}
