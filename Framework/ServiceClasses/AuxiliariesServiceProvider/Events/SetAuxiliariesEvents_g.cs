/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAuxiliariesEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Auxiliaries
{
    internal class SetAuxiliariesEvents : AuxiliariesEvents, ISetAuxiliariesEvents
    {

        public SetAuxiliariesEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

    }
}
