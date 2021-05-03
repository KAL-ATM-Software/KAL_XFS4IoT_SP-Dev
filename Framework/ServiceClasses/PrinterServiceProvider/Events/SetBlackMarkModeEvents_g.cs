/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * SetBlackMarkModeEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Printer
{
    internal class SetBlackMarkModeEvents : PrinterEvents, ISetBlackMarkModeEvents
    {

        public SetBlackMarkModeEvents(IConnection connection, string requestId)
            : base(connection, requestId)
        { }

    }
}
