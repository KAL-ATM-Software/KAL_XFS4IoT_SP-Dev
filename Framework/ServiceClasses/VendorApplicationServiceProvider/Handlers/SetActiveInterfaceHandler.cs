/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * SetActiveInterfaceHandler.cs uses automatically generated parts.
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
    public partial class SetActiveInterfaceHandler
    {

        private Task<SetActiveInterfaceCompletion.PayloadData> HandleSetActiveInterface(ISetActiveInterfaceEvents events, SetActiveInterfaceCommand setActiveInterface, CancellationToken cancel)
        {
            //ToDo: Implement HandleSetActiveInterface for VendorApplication.
            
            #if DEBUG
                throw new NotImplementedException("HandleSetActiveInterface for VendorApplication is not implemented in SetActiveInterfaceHandler.cs");
            #else
                #error HandleSetActiveInterface for VendorApplication is not implemented in SetActiveInterfaceHandler.cs
            #endif
        }

    }
}
