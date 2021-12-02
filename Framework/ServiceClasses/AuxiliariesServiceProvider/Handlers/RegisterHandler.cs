/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * RegisterHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;

namespace XFS4IoTFramework.Auxiliaries
{
    public partial class RegisterHandler
    {

        private Task<RegisterCompletion.PayloadData> HandleRegister(IRegisterEvents events, RegisterCommand register, CancellationToken cancel)
        {
            //ToDo: Implement HandleRegister for Auxiliaries.
            
            #if DEBUG
                throw new NotImplementedException("HandleRegister for Auxiliaries is not implemented in RegisterHandler.cs");
            #else
                #error HandleRegister for Auxiliaries is not implemented in RegisterHandler.cs
            #endif
        }

    }
}
