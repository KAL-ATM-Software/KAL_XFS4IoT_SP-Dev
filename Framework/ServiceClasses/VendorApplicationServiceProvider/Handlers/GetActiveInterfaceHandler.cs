/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * GetActiveInterfaceHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.VendorApplication.Commands;
using XFS4IoT.VendorApplication.Completions;

namespace XFS4IoTFramework.VendorApplication
{
    public partial class GetActiveInterfaceHandler
    {

        private Task<GetActiveInterfaceCompletion.PayloadData> HandleGetActiveInterface(IGetActiveInterfaceEvents events, GetActiveInterfaceCommand getActiveInterface, CancellationToken cancel)
        {
            //ToDo: Implement HandleGetActiveInterface for VendorApplication.
            
            #if DEBUG
                throw new NotImplementedException("HandleGetActiveInterface for VendorApplication is not implemented in GetActiveInterfaceHandler.cs");
            #else
                #error HandleGetActiveInterface for VendorApplication is not implemented in GetActiveInterfaceHandler.cs
            #endif
        }

    }
}
