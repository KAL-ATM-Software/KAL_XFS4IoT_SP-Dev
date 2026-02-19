/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;
using XFS4IoT.CardReader;

namespace XFS4IoT
{
    public sealed class MessageCollection
    {
        public static void Add(MessageHeader.TypeEnum messageType, string messageName, Type jsonMessageType)
        {
            if (messageType == MessageHeader.TypeEnum.Command)
            {
                commandMessageTypes.TryAdd(messageName, jsonMessageType);
            }
            else if (messageType == MessageHeader.TypeEnum.Completion)
            {
                completionMessageTypes.TryAdd(messageName, jsonMessageType);
            }
            else if (messageType == MessageHeader.TypeEnum.Event ||
                messageType == MessageHeader.TypeEnum.Unsolicited)
            {
                eventMessageTypes.TryAdd(messageName, jsonMessageType);
            }
        }

        public static bool TryGetMessageType(MessageHeader.TypeEnum messageType, string messageName, out Type jsonMessageType)
        {
            if (messageType == MessageHeader.TypeEnum.Acknowledge)
            {
                jsonMessageType = typeof(Acknowledge);
                return true;
            }
            else if (messageType == MessageHeader.TypeEnum.Command)
            {
                if (commandMessageTypes.TryGetValue(messageName, out jsonMessageType))
                {
                    return true;
                }
            }
            else if (messageType == MessageHeader.TypeEnum.Completion)
            {
                if (completionMessageTypes.TryGetValue(messageName, out jsonMessageType))
                {
                    return true;
                }
            }
            else if (messageType == MessageHeader.TypeEnum.Event ||
                messageType == MessageHeader.TypeEnum.Unsolicited)
            {
                if (eventMessageTypes.TryGetValue(messageName, out jsonMessageType))
                {
                    return true;
                }
            }

            jsonMessageType = null;
            return false;
        }

        private static readonly Dictionary<string, Type> commandMessageTypes = [];
        private static readonly Dictionary<string, Type> completionMessageTypes = [];
        private static readonly Dictionary<string, Type> eventMessageTypes = [];
    }
}
