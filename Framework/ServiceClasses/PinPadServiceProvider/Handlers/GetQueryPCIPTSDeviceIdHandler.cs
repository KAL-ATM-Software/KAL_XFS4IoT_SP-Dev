/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetQueryPCIPTSDeviceIdHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;

namespace XFS4IoTFramework.PinPad
{
    public partial class GetQueryPCIPTSDeviceIdHandler
    {

        private Task<GetQueryPCIPTSDeviceIdCompletion.PayloadData> HandleGetQueryPCIPTSDeviceId(IGetQueryPCIPTSDeviceIdEvents events, GetQueryPCIPTSDeviceIdCommand getQueryPCIPTSDeviceId, CancellationToken cancel)
        {
            //ToDo: Implement HandleGetQueryPCIPTSDeviceId for PinPad.
            
            #if DEBUG
                throw new NotImplementedException("HandleGetQueryPCIPTSDeviceId for PinPad is not implemented in GetQueryPCIPTSDeviceIdHandler.cs");
            #else
                #error HandleGetQueryPCIPTSDeviceId for PinPad is not implemented in GetQueryPCIPTSDeviceIdHandler.cs
            #endif
        }

    }
}
