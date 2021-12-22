/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * StartLocalApplicationHandler.cs uses automatically generated parts.
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
    public partial class StartLocalApplicationHandler
    {

        private Task<StartLocalApplicationCompletion.PayloadData> HandleStartLocalApplication(IStartLocalApplicationEvents events, StartLocalApplicationCommand startLocalApplication, CancellationToken cancel)
        {
            //ToDo: Implement HandleStartLocalApplication for VendorApplication.
            
            #if DEBUG
                throw new NotImplementedException("HandleStartLocalApplication for VendorApplication is not implemented in StartLocalApplicationHandler.cs");
            #else
                #error HandleStartLocalApplication for VendorApplication is not implemented in StartLocalApplicationHandler.cs
            #endif
        }

    }
}
