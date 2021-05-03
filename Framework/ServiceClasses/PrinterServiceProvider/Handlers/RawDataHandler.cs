/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RawDataHandler.cs uses automatically generated parts.
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
    public partial class RawDataHandler
    {

        private Task<RawDataCompletion.PayloadData> HandleRawData(IRawDataEvents events, RawDataCommand rawData, CancellationToken cancel)
        {
            //ToDo: Implement HandleRawData for Printer.
            
            #if DEBUG
                throw new NotImplementedException("HandleRawData for Printer is not implemented in RawDataHandler.cs");
            #else
                #error HandleRawData for Printer is not implemented in RawDataHandler.cs
            #endif
        }

    }
}
