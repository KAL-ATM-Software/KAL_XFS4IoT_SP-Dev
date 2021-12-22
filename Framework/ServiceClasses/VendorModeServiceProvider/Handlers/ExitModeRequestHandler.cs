/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * ExitModeRequestHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;

namespace XFS4IoTFramework.VendorMode
{
    public partial class ExitModeRequestHandler
    {

        private Task<ExitModeRequestCompletion.PayloadData> HandleExitModeRequest(IExitModeRequestEvents events, ExitModeRequestCommand exitModeRequest, CancellationToken cancel)
        {
            //ToDo: Implement HandleExitModeRequest for VendorMode.
            
            #if DEBUG
                throw new NotImplementedException("HandleExitModeRequest for VendorMode is not implemented in ExitModeRequestHandler.cs");
            #else
                #error HandleExitModeRequest for VendorMode is not implemented in ExitModeRequestHandler.cs
            #endif
        }

    }
}
