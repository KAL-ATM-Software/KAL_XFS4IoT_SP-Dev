/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteReaderHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class ConfigureNoteReaderHandler
    {

        private Task<ConfigureNoteReaderCompletion.PayloadData> HandleConfigureNoteReader(IConfigureNoteReaderEvents events, ConfigureNoteReaderCommand configureNoteReader, CancellationToken cancel)
        {
            //ToDo: Implement HandleConfigureNoteReader for CashAcceptor.
            
            #if DEBUG
                throw new NotImplementedException("HandleConfigureNoteReader for CashAcceptor is not implemented in ConfigureNoteReaderHandler.cs");
            #else
                #error HandleConfigureNoteReader for CashAcceptor is not implemented in ConfigureNoteReaderHandler.cs
            #endif
        }

    }
}
