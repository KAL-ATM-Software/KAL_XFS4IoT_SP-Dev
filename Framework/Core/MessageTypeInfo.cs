/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System.Collections.Generic;

namespace XFS4IoT
{
    /// <summary>
    /// Class to store supported message information
    /// </summary>
    public sealed class MessageTypeInfo(MessageTypeInfo.MessageTypeEnum Type,
                                        List<string> Versions)
    {
        public enum MessageTypeEnum
        {
            Command,
            Event,
        }

        public MessageTypeEnum Type { get; init; } = Type;

        public List<string> Versions { get; init; } = Versions;
    }

}
