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
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public partial class GetItemInfoHandler
    {
        private Task<CommandResult<GetItemInfoCompletion.PayloadData>> HandleGetItemInfo(IGetItemInfoEvents events, GetItemInfoCommand getItemInfo, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.GetItemInfoInfo()");

            ItemInfoTypeEnum itemInfoTypes = ItemInfoTypeEnum.All;
            if (getItemInfo.Payload.ItemInfoType?.Image is not null && (bool)getItemInfo.Payload.ItemInfoType?.Image)
            {
                itemInfoTypes |= ItemInfoTypeEnum.Image;
            }
            if (getItemInfo.Payload.ItemInfoType?.SerialNumber is not null && (bool)getItemInfo.Payload.ItemInfoType?.SerialNumber)
            {
                itemInfoTypes |= ItemInfoTypeEnum.SerialNumber;
            }
            if (getItemInfo.Payload.ItemInfoType?.Signature is not null && (bool)getItemInfo.Payload.ItemInfoType?.Signature)
            {
                itemInfoTypes |= ItemInfoTypeEnum.Signature;
            }

            var result = Device.GetItemInfoInfo(
                new GetItemInfoRequest(
                    getItemInfo.Payload.Items is null ? 0 : (int)getItemInfo.Payload.Items.Index,
                    getItemInfo.Payload.Items is null ? 
                        NoteLevelEnum.All : 
                        getItemInfo.Payload.Items.Level switch 
                        {
                            XFS4IoT.CashManagement.NoteLevelEnum.Counterfeit => NoteLevelEnum.Counterfeit,
                            XFS4IoT.CashManagement.NoteLevelEnum.Fit => NoteLevelEnum.Counterfeit,
                            XFS4IoT.CashManagement.NoteLevelEnum.Inked => NoteLevelEnum.Counterfeit,
                            XFS4IoT.CashManagement.NoteLevelEnum.Suspect => NoteLevelEnum.Counterfeit,
                            XFS4IoT.CashManagement.NoteLevelEnum.Unfit => NoteLevelEnum.Counterfeit,
                            XFS4IoT.CashManagement.NoteLevelEnum.Unrecognized => NoteLevelEnum.Counterfeit,
                            _ => NoteLevelEnum.All,
                        },
                    itemInfoTypes)
                );

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetItemInfoInfo() -> {result.CompletionCode}");

            List<XFS4IoT.CashManagement.ItemInfoClass> itemList = null;

            if (result.ItemInfos?.Count > 0)
            {
                itemList = [];
                foreach (var item in result.ItemInfos)
                {
                    if (string.IsNullOrEmpty(item.Key))
                    {
                        continue;
                    }

                    itemList.Add(
                        new XFS4IoT.CashManagement.ItemInfoClass(
                            item.Key,
                            item.Value?.Orientation switch 
                            {
                                OrientationEnum.BackBottom => XFS4IoT.CashManagement.OrientationEnum.BackBottom,
                                OrientationEnum.BackTop => XFS4IoT.CashManagement.OrientationEnum.BackTop,
                                OrientationEnum.FrontBottom => XFS4IoT.CashManagement.OrientationEnum.FrontBottom,
                                OrientationEnum.FrontTop => XFS4IoT.CashManagement.OrientationEnum.FrontTop,
                                OrientationEnum.Unknown => XFS4IoT.CashManagement.OrientationEnum.Unknown,
                                _ => null,
                            },
                            item.Value?.Signature,
                            item.Value?.Level switch 
                            { 
                                NoteLevelEnum.Counterfeit => XFS4IoT.CashManagement.NoteLevelEnum.Counterfeit,
                                NoteLevelEnum.Fit => XFS4IoT.CashManagement.NoteLevelEnum.Fit,
                                NoteLevelEnum.Inked => XFS4IoT.CashManagement.NoteLevelEnum.Inked,
                                NoteLevelEnum.Suspect => XFS4IoT.CashManagement.NoteLevelEnum.Suspect,
                                NoteLevelEnum.Unfit => XFS4IoT.CashManagement.NoteLevelEnum.Unfit,
                                NoteLevelEnum.Unrecognized => XFS4IoT.CashManagement.NoteLevelEnum.Unrecognized,
                                _ => null,
                            },
                            item.Value?.SerialNumber,
                            item.Value?.Image,
                            item.Value?.ClassificationList switch 
                            {
                                ClassificationListEnum.ClassificationListUnknown => XFS4IoT.CashManagement.OnClassificationListEnum.ClassificationListUnknown,
                                ClassificationListEnum.NotOnClassificationList => XFS4IoT.CashManagement.OnClassificationListEnum.NotOnClassificationList,
                                ClassificationListEnum.OnClassificationList => XFS4IoT.CashManagement.OnClassificationListEnum.OnClassificationList,
                                _ => null,
                            },
                            item.Value?.ItemLocation)
                        );
                }
            }

            return Task.FromResult(
                new CommandResult<GetItemInfoCompletion.PayloadData>(
                    itemList is not null ? new(itemList) : null,
                    result.CompletionCode,
                    result.ErrorDescription)
                );
        }
    }
}
