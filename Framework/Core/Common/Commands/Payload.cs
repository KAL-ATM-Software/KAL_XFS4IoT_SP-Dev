﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT.Commands
{
    /// <summary>
    /// A message object representing payload for commands
    /// </summary>
    [DataContract]
    public class MessagePayload : MessagePayloadBase
    {
        /// <summary>
        /// Constructor for the common command payload
        /// </summary>
        /// 
        
        public MessagePayload()
        { }

        // Add properties for common properties under payload when common properties
        // in payload introduced in the XFS4IoT specification.
    }
}