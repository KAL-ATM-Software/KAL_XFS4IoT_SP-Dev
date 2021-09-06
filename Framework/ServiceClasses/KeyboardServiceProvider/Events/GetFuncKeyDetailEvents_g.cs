/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * GetFuncKeyDetailEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Keyboard
{
    internal class GetFuncKeyDetailEvents : KeyboardEvents, IGetFuncKeyDetailEvents
    {

        public GetFuncKeyDetailEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

    }
}
