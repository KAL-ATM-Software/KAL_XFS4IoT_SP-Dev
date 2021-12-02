/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * ClearAutoStartupTimeHandler.cs uses automatically generated parts.
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
    public partial class ClearAutoStartupTimeHandler
    {

        private Task<ClearAutoStartupTimeCompletion.PayloadData> HandleClearAutoStartupTime(IClearAutoStartupTimeEvents events, ClearAutoStartupTimeCommand clearAutoStartupTime, CancellationToken cancel)
        {
            //ToDo: Implement HandleClearAutoStartupTime for Auxiliaries.
            
            #if DEBUG
                throw new NotImplementedException("HandleClearAutoStartupTime for Auxiliaries is not implemented in ClearAutoStartupTimeHandler.cs");
            #else
                #error HandleClearAutoStartupTime for Auxiliaries is not implemented in ClearAutoStartupTimeHandler.cs
            #endif
        }

    }
}
