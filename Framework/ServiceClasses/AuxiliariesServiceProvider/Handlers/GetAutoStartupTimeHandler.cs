/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * GetAutoStartupTimeHandler.cs uses automatically generated parts.
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
    public partial class GetAutoStartupTimeHandler
    {

        private Task<GetAutoStartupTimeCompletion.PayloadData> HandleGetAutoStartupTime(IGetAutoStartupTimeEvents events, GetAutoStartupTimeCommand getAutoStartupTime, CancellationToken cancel)
        {
            //ToDo: Implement HandleGetAutoStartupTime for Auxiliaries.
            
            #if DEBUG
                throw new NotImplementedException("HandleGetAutoStartupTime for Auxiliaries is not implemented in GetAutoStartupTimeHandler.cs");
            #else
                #error HandleGetAutoStartupTime for Auxiliaries is not implemented in GetAutoStartupTimeHandler.cs
            #endif
        }

    }
}
