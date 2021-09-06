/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DataEntryHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;

namespace XFS4IoTFramework.Keyboard
{
    public partial class DataEntryHandler
    {

        private Task<DataEntryCompletion.PayloadData> HandleDataEntry(IDataEntryEvents events, DataEntryCommand dataEntry, CancellationToken cancel)
        {
            //ToDo: Implement HandleDataEntry for Keyboard.
            
            #if DEBUG
                throw new NotImplementedException("HandleDataEntry for Keyboard is not implemented in DataEntryHandler.cs");
            #else
                #error HandleDataEntry for Keyboard is not implemented in DataEntryHandler.cs
            #endif
        }

    }
}
