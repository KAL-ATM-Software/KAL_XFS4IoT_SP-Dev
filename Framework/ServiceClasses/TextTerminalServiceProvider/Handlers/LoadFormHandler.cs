/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * LoadFormHandler.cs uses automatically generated parts.
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
    public partial class LoadFormHandler
    {

        private Task<LoadFormCompletion.PayloadData> HandleLoadForm(ILoadFormEvents events, LoadFormCommand loadForm, CancellationToken cancel)
        {
            //ToDo: Implement HandleLoadForm for TextTerminal.
            
            #if DEBUG
                throw new NotImplementedException("HandleLoadForm for TextTerminal is not implemented in LoadFormHandler.cs");
            #else
                #error HandleLoadForm for TextTerminal is not implemented in LoadFormHandler.cs
            #endif
        }

    }
}
