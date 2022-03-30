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
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashManagement
{
    public partial class SetClassificationListHandler
    {
        private Task<SetClassificationListCompletion.PayloadData> HandleSetClassificationList(ISetClassificationListEvents events, SetClassificationListCommand setClassificationList, CancellationToken cancel)
        {
            if (setClassificationList.Payload.ClassificationElements is null ||
                setClassificationList.Payload.ClassificationElements.Count == 0)
            {
                Task.FromResult(new SetClassificationListCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                                $"No classification list supplied."));
            }

            // Clear list first to update new list
            CashManagement.ItemClassificationList.ItemClassifications.Clear();

            CashManagement.ItemClassificationList.Version = setClassificationList.Payload.Version;

            foreach (var classificationItem in setClassificationList.Payload.ClassificationElements)
            {
                if (!string.IsNullOrEmpty(classificationItem.Currency))
                {
                    if (!Regex.IsMatch(classificationItem.Currency, "^[A-Z]{3}$"))
                    {
                        Task.FromResult(new SetClassificationListCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                        $"Invalid currency specified. {classificationItem.Currency}"));
                    }
                }
                if (classificationItem.Level is null)
                {
                    Task.FromResult(new SetClassificationListCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                                    $"No item level is specified."));
                }

                CashManagement.ItemClassificationList.ItemClassifications.Add(
                    new ItemClassificationClass(classificationItem.SerialNumber,
                                                classificationItem.Currency,
                                                classificationItem.Value ?? 0,
                                                classificationItem.Level switch 
                                                { 
                                                    XFS4IoT.CashManagement.NoteLevelEnum.Counterfeit => NoteLevelEnum.Counterfeit,
                                                    XFS4IoT.CashManagement.NoteLevelEnum.Fit => NoteLevelEnum.Fit,
                                                    XFS4IoT.CashManagement.NoteLevelEnum.Inked => NoteLevelEnum.Inked,
                                                    XFS4IoT.CashManagement.NoteLevelEnum.Suspect => NoteLevelEnum.Suspect,
                                                    XFS4IoT.CashManagement.NoteLevelEnum.Unfit => NoteLevelEnum.Unfit,
                                                    XFS4IoT.CashManagement.NoteLevelEnum.Unrecognized => NoteLevelEnum.Unrecognized,
                                                    _ => NoteLevelEnum.All,
                                                }));
            }

            CashManagement.StoreItemClassificationList();

            return Task.FromResult(new SetClassificationListCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty));
        }
    }
}
