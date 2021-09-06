/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyboardEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTFramework.Keyboard
{
    internal abstract class KeyboardEvents
    {
        protected readonly IConnection connection;
        protected readonly int requestId;

        public KeyboardEvents(IConnection connection, int requestId)
        {
            this.connection = connection;
            this.requestId = requestId;
        }

    }
}
