/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * HSMInitEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.German
{
    internal class HSMInitEvents : GermanEvents, IHSMInitEvents
    {

        public HSMInitEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

    }
}
