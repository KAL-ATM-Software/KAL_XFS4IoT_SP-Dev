/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using XFS4IoT;
using XFS4IoT.Commands;

namespace XFS4IoTServer
{

    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public sealed class CommandHandlerAttribute : Attribute
    {
        /// <summary>
        /// Tag a class to handle a message type for a service
        /// </summary>
        /// <param name="ServiceClass">Service class this handler is related to</param>
        /// <param name="HandledType">Type of message class this handler will handle</param>
        public CommandHandlerAttribute(XFS4IoT.XFSConstants.ServiceClass ServiceClass, Type HandledType)
        {
            HandledType.IsNotNull($"Invalid parameter received in {nameof(CommandHandlerAttribute)}. {nameof(HandledType)}");

            this.ServiceClass = ServiceClass;
            this.HandledType = HandledType;

            Contracts.IsTrue(typeof(Command<MessagePayload>).IsAssignableFrom(HandledType), $"Type {HandledType} is registered as a handled type but is not a {nameof(Command<MessagePayload>)}");
        }

        public XFS4IoT.XFSConstants.ServiceClass ServiceClass { get; }
        public Type HandledType { get; }
    }
}
