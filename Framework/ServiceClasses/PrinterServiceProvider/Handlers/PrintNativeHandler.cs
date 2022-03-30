/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class PrintNativeHandler
    {
        private Task<PrintNativeCompletion.PayloadData> HandlePrintNative(IPrintNativeEvents events, PrintNativeCommand printNative, CancellationToken cancel)
        {
            return Task.FromResult(new PrintNativeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                         $"PrintNative command is not supported."));
        }
    }
}
