/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * MaintainPinEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.PinPad
{
    internal class MaintainPinEvents : PinPadEvents, IMaintainPinEvents
    {

        public MaintainPinEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

    }
}
