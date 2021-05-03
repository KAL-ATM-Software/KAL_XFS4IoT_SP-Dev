/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * SetKeyEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CardReader
{
    internal class SetKeyEvents : CardReaderEvents, ISetKeyEvents
    {

        public SetKeyEvents(IConnection connection, string requestId)
            : base(connection, requestId)
        { }

    }
}
