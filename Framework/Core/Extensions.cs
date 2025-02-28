/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT.Common;

namespace XFS4IoT
{
    /// <summary>
    /// Collection of classes for extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts to the camelcase
        /// </summary>
        public static string ToCamelCase(this string toCamelCase)
        {
            if (string.IsNullOrEmpty(toCamelCase) || string.IsNullOrWhiteSpace(toCamelCase))
            {
                return toCamelCase;
            }
            if (toCamelCase?.Length == 1)
            {
                return toCamelCase.ToLowerInvariant();
            }
            return char.ToLowerInvariant(toCamelCase[0]) + toCamelCase[1..];
        }

        /// <summary>
        /// Adding matched command in the dictionary.
        /// </summary>
        /// <code><![CDATA[
        /// supportedMessages.AddMatches(
        ///     InterfaceName: InterfaceClass.NameEnum.CardReader,
        ///     Type: MessageTypeInfo.MessageTypeEnum.Command,
        ///     Messages: allMessages,
        ///     InterfaceCommands: CommonCapabilities.CardReaderInterface.Commands);
        /// ]]></code>
        /// <typeparam name="T">enum for commands or events defined in each device interfaces of the Capabilities class.
        /// i.e. CommonCapabilitiesClass.CardReaderInterface.CommandEnum or EventEnum
        /// </typeparam>
        /// <param name="ListToAdd">Adding matched command or event to the list</param>
        /// <param name="InterfaceName">Name of interface</param>
        /// <param name="Type">Message type to add, either command or event</param>
        /// <param name="Messages">List of all message to be compared</param>
        /// <param name="InterfaceCommands">List of CommandEnum or EventEnum supported</param>
        public static void AddMatches<T>(this Dictionary<string, MessageTypeInfo> ListToAdd, InterfaceClass.NameEnum InterfaceName, MessageTypeInfo.MessageTypeEnum Type, Dictionary<string, MessageTypeInfo> Messages, List<T> InterfaceCommands)
        {
            if (InterfaceCommands is null || 
                InterfaceCommands.Count == 0)
            {
                return;
            }
            foreach (var cmd in InterfaceCommands)
            {
                string msgName = $"{InterfaceName}.{cmd}";
                if (!Messages.ContainsKey(msgName))
                {
                    continue;
                }

                if (ListToAdd is null)
                {
                    ListToAdd = [];
                }
                else
                {
                    if (ListToAdd.ContainsKey(msgName))
                    {
                        continue;
                    }
                    if (Messages[msgName].Type != Type)
                    {
                        continue;
                    }
                }
                ListToAdd.Add(msgName, Messages[msgName]);
            }
        }
    }
}
