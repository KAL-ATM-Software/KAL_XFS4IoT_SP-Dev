/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * CashDispenserEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTFramework.CashDispenser
{
    internal abstract class CashDispenserEvents
    {
        protected readonly IConnection connection;
        protected readonly int requestId;

        public CashDispenserEvents(IConnection connection, int requestId)
        {
            this.connection = connection;
            this.requestId = requestId;
        }

    }
}
