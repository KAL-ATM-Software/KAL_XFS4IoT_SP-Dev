/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * RegisterHandler.cs uses automatically generated parts.
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
    public partial class RegisterHandler
    {

        private Task<RegisterCompletion.PayloadData> HandleRegister(IRegisterEvents events, RegisterCommand register, CancellationToken cancel)
        {
            //ToDo: Implement HandleRegister for VendorMode.
            
            #if DEBUG
                throw new NotImplementedException("HandleRegister for VendorMode is not implemented in RegisterHandler.cs");
            #else
                #error HandleRegister for VendorMode is not implemented in RegisterHandler.cs
            #endif
        }

    }
}
