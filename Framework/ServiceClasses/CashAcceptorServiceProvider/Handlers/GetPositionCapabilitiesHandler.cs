/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class GetPositionCapabilitiesHandler
    {
        private Task<GetPositionCapabilitiesCompletion.PayloadData> HandleGetPositionCapabilities(IGetPositionCapabilitiesEvents events, GetPositionCapabilitiesCommand getPositionCapabilities, CancellationToken cancel)
        {
            if (CashAcceptor.PositionCapabilities is null ||
                CashAcceptor.PositionCapabilities.Count == 0)
            {
                return Task.FromResult(new GetPositionCapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                                         $"The device class doesn't report position capabilities."));
            }

            List<GetPositionCapabilitiesCompletion.PayloadData.PosCapabilitiesClass> capabilities = new();
            foreach (var positionCaps in CashAcceptor.PositionCapabilities)
            {
                capabilities.Add(new GetPositionCapabilitiesCompletion.PayloadData.PosCapabilitiesClass(positionCaps.Key switch 
                                                                                                        { 
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InBottom => XFS4IoT.CashManagement.PositionEnum.InBottom,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InCenter => XFS4IoT.CashManagement.PositionEnum.InCenter,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InDefault => XFS4IoT.CashManagement.PositionEnum.InDefault,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InFront => XFS4IoT.CashManagement.PositionEnum.InFront,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InLeft => XFS4IoT.CashManagement.PositionEnum.InLeft,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InRear => XFS4IoT.CashManagement.PositionEnum.InRear,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InRight => XFS4IoT.CashManagement.PositionEnum.InRight,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.InTop => XFS4IoT.CashManagement.PositionEnum.InTop,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutBottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutCenter => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutDefault => XFS4IoT.CashManagement.PositionEnum.OutDefault,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutFront => XFS4IoT.CashManagement.PositionEnum.OutFront,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutLeft => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutRear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutRight => XFS4IoT.CashManagement.PositionEnum.OutRight,
                                                                                                            CashManagementCapabilitiesClass.PositionEnum.OutTop => XFS4IoT.CashManagement.PositionEnum.OutTop,
                                                                                                            _ => null,
                                                                                                        },
                                                                                                        new GetPositionCapabilitiesCompletion.PayloadData.PosCapabilitiesClass.UsageClass(positionCaps.Value.Usage.HasFlag(PositionCapabilitiesClass.UsageEnum.In),
                                                                                                                                                                                          positionCaps.Value.Usage.HasFlag(PositionCapabilitiesClass.UsageEnum.Refuse),
                                                                                                                                                                                          positionCaps.Value.Usage.HasFlag(PositionCapabilitiesClass.UsageEnum.Rollback)),
                                                                                                        positionCaps.Value.ShutterControl,
                                                                                                        positionCaps.Value.ItemsTakenSensor,
                                                                                                        positionCaps.Value.ItemsInsertedSensor,
                                                                                                        new GetPositionCapabilitiesCompletion.PayloadData.PosCapabilitiesClass.RetractAreasClass(positionCaps.Value.RetractAreas.HasFlag(PositionCapabilitiesClass.RetractAreaEnum.Retract),
                                                                                                                                                                                                 positionCaps.Value.RetractAreas.HasFlag(PositionCapabilitiesClass.RetractAreaEnum.Reject),
                                                                                                                                                                                                 positionCaps.Value.RetractAreas.HasFlag(PositionCapabilitiesClass.RetractAreaEnum.Transport),
                                                                                                                                                                                                 positionCaps.Value.RetractAreas.HasFlag(PositionCapabilitiesClass.RetractAreaEnum.Stacker),
                                                                                                                                                                                                 positionCaps.Value.RetractAreas.HasFlag(PositionCapabilitiesClass.RetractAreaEnum.BillCassettes),
                                                                                                                                                                                                 positionCaps.Value.RetractAreas.HasFlag(PositionCapabilitiesClass.RetractAreaEnum.CashIn)),
                                                                                                        positionCaps.Value.PresentControl,
                                                                                                        positionCaps.Value.PreparePresent
                                                                                                        ));
            }
            return Task.FromResult(new GetPositionCapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                     string.Empty,
                                                                                     capabilities));
        }
    }
}
