/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandlerAsync]
    public partial class GetClassificationListHandler
    {
        private Task<CommandResult<GetClassificationListCompletion.PayloadData>> HandleGetClassificationList(IGetClassificationListEvents events, GetClassificationListCommand getClassificationList, CancellationToken cancel)
        {
            if (CashManagement.ItemClassificationList.ItemClassifications.Count == 0)
            {
                Task.FromResult(
                    new CommandResult<GetClassificationListCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.Success,
                        $"No classification list stored.")
                    );
            }

            return Task.FromResult(
                new CommandResult<GetClassificationListCompletion.PayloadData>(
                    new(
                        CashManagement.ItemClassificationList.Version,
                        CashManagement.ItemClassificationList.ItemClassifications.Select(c =>
                        new XFS4IoT.CashManagement.ClassificationElementClass(
                            c.SerialNumber,
                            c.Currency,
                            c.Value,
                            c.Level switch
                            {
                                NoteLevelEnum.Counterfeit => XFS4IoT.CashManagement.NoteLevelEnum.Counterfeit,
                                NoteLevelEnum.Fit => XFS4IoT.CashManagement.NoteLevelEnum.Fit,
                                NoteLevelEnum.Inked => XFS4IoT.CashManagement.NoteLevelEnum.Inked,
                                NoteLevelEnum.Suspect => XFS4IoT.CashManagement.NoteLevelEnum.Suspect,
                                NoteLevelEnum.Unfit => XFS4IoT.CashManagement.NoteLevelEnum.Unfit,
                                NoteLevelEnum.Unrecognized => XFS4IoT.CashManagement.NoteLevelEnum.Unrecognized,
                                _ => null,
                            })
                        ).ToList()),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
