/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteHandler.cs uses automatically generated parts.
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
    public partial class WriteHandler
    {

        private Task<WriteCompletion.PayloadData> HandleWrite(IWriteEvents events, WriteCommand write, CancellationToken cancel)
        {
            //ToDo: Implement HandleWrite for TextTerminal.
            
            #if DEBUG
                throw new NotImplementedException("HandleWrite for TextTerminal is not implemented in WriteHandler.cs");
            #else
                #error HandleWrite for TextTerminal is not implemented in WriteHandler.cs
            #endif
        }

    }
}
