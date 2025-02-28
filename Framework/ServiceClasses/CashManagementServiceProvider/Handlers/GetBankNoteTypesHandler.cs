/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.CashManagement;
using XFS4IoT.Completions;
using XFS4IoTServer;
using XFS4IoT;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandlerAsync]
    public partial class GetBankNoteTypesHandler
    {
        private Task<CommandResult<GetBankNoteTypesCompletion.PayloadData>> HandleGetBankNoteTypes(IGetBankNoteTypesEvents events, GetBankNoteTypesCommand getBankNoteTypes, CancellationToken cancel)
        {
            Dictionary<string, BankNoteClass> items = null;
            if (Common.CashManagementCapabilities.AllBanknoteItems?.Count > 0)
            {
                items = [];
                foreach (var item in Common.CashManagementCapabilities.AllBanknoteItems)
                {
                    items.Add(
                        item.Key, 
                        new BankNoteClass(
                            new(item.Value.NoteId,
                                item.Value.Currency,
                                item.Value.Value,
                                item.Value.Release),
                                item.Value.Enabled)
                        );
                }
            }

            return Task.FromResult(
                new CommandResult<GetBankNoteTypesCompletion.PayloadData>(
                    items is not null ? new(Items: items) : null,
                    CompletionCode: MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
