/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetLedHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class SetLedHandler
    {

        private Task<SetLedCompletion.PayloadData> HandleSetLed(ISetLedEvents events, SetLedCommand setLed, CancellationToken cancel)
        {
            //ToDo: Implement HandleSetLed for TextTerminal.
            
            #if DEBUG
                throw new NotImplementedException("HandleSetLed for TextTerminal is not implemented in SetLedHandler.cs");
            #else
                #error HandleSetLed for TextTerminal is not implemented in SetLedHandler.cs
            #endif
        }

    }
}
