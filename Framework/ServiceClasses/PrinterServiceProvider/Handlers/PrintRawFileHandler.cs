/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRawFileHandler.cs uses automatically generated parts.
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
    public partial class PrintRawFileHandler
    {

        private Task<PrintRawFileCompletion.PayloadData> HandlePrintRawFile(IPrintRawFileEvents events, PrintRawFileCommand printRawFile, CancellationToken cancel)
        {
            //ToDo: Implement HandlePrintRawFile for Printer.
            
            #if DEBUG
                throw new NotImplementedException("HandlePrintRawFile for Printer is not implemented in PrintRawFileHandler.cs");
            #else
                #error HandlePrintRawFile for Printer is not implemented in PrintRawFileHandler.cs
            #endif
        }

    }
}
