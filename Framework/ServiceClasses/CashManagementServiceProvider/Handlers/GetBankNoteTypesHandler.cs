/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

namespace XFS4IoTFramework.CashManagement
{
    public partial class GetBankNoteTypesHandler
    {
        private Task<GetBankNoteTypesCompletion.PayloadData> HandleGetBankNoteTypes(IGetBankNoteTypesEvents events, GetBankNoteTypesCommand getBankNoteTypes, CancellationToken cancel)
        {
            Dictionary<string, BankNoteClass> items = null;
            if (CashManagement.CashManagementCapabilities.AllBanknoteItems.Count > 0)
            {
                foreach (var item in CashManagement.CashManagementCapabilities.AllBanknoteItems)
                {
                    if (CashManagement.CashManagementStatus.AllBanknoteItems is null ||
                        CashManagement.CashManagementStatus.AllBanknoteItems.ContainsKey(item.Key))
                    {
                        return Task.FromResult(new GetBankNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                                                          $"The device specific class doesn't report status of the banknote type is enabled or not. {item.Key}"));
                    }

                    items.Add(item.Key, new BankNoteClass(new (item.Value.NoteId,
                                                               item.Value.Currency,
                                                               item.Value.Value,
                                                               item.Value.Release),
                                                          CashManagement.CashManagementStatus.AllBanknoteItems[item.Key]));
                }
            }

            return Task.FromResult(new GetBankNoteTypesCompletion.PayloadData(CompletionCode: MessagePayload.CompletionCodeEnum.Success,
                                                                              ErrorDescription: null,
                                                                              Items: items));
        }
    }
}
