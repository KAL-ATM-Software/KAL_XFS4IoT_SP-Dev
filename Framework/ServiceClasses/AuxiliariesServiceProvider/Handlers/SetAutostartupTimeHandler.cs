/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAutostartupTimeHandler.cs uses automatically generated parts.
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
    public partial class SetAutostartupTimeHandler
    {

        private Task<SetAutostartupTimeCompletion.PayloadData> HandleSetAutostartupTime(ISetAutostartupTimeEvents events, SetAutostartupTimeCommand setAutostartupTime, CancellationToken cancel)
        {
            //ToDo: Implement HandleSetAutostartupTime for Auxiliaries.
            
            #if DEBUG
                throw new NotImplementedException("HandleSetAutostartupTime for Auxiliaries is not implemented in SetAutostartupTimeHandler.cs");
            #else
                #error HandleSetAutostartupTime for Auxiliaries is not implemented in SetAutostartupTimeHandler.cs
            #endif
        }

    }
}
