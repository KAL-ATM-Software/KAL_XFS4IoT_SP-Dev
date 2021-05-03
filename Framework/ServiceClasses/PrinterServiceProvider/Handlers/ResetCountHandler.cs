/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ResetCountHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class ResetCountHandler
    {

        private Task<ResetCountCompletion.PayloadData> HandleResetCount(IResetCountEvents events, ResetCountCommand resetCount, CancellationToken cancel)
        {
            //ToDo: Implement HandleResetCount for Printer.
            
            #if DEBUG
                throw new NotImplementedException("HandleResetCount for Printer is not implemented in ResetCountHandler.cs");
            #else
                #error HandleResetCount for Printer is not implemented in ResetCountHandler.cs
            #endif
        }

    }
}
