/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Printer
{
    [CommandHandlerAsync]
    public partial class GetFormListHandler
    {
        private Task<CommandResult<GetFormListCompletion.PayloadData>> HandleGetFormList(IGetFormListEvents events, GetFormListCommand getFormList, CancellationToken cancel)
        {
            Dictionary<string, Form> forms = Printer.GetForms();
            return Task.FromResult(
                new CommandResult<GetFormListCompletion.PayloadData>(
                    forms.Count == 0 ? null : new(new List<string>(forms.Keys)),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
