/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashDispenser;

namespace XFS4IoTFramework.CashManagement
{
    public sealed class ResetCommandEvents : ItemInfoAvailableCommandEvent
    {
        public ResetCommandEvents(IResetEvents events) :
            base(events)
        { }

        public Task MediaDetectedEvent(string StorageId, ItemPosition Position)
        {
            XFS4IoT.CashManagement.RetractClass retract = null;
            if (Position.RetractArea is not null)
            {
                XFS4IoT.CashManagement.RetractAreaEnum? retractArea = Position.RetractArea.RetractArea switch
                {
                    CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette => XFS4IoT.CashManagement.RetractAreaEnum.ItemCassette,
                    CashManagementCapabilitiesClass.RetractAreaEnum.Reject => XFS4IoT.CashManagement.RetractAreaEnum.Reject,
                    CashManagementCapabilitiesClass.RetractAreaEnum.Retract => XFS4IoT.CashManagement.RetractAreaEnum.Retract,
                    CashManagementCapabilitiesClass.RetractAreaEnum.Stacker => XFS4IoT.CashManagement.RetractAreaEnum.Stacker,
                    CashManagementCapabilitiesClass.RetractAreaEnum.Transport => XFS4IoT.CashManagement.RetractAreaEnum.Transport,
                    _ => null,
                };
                retract = new XFS4IoT.CashManagement.RetractClass(null, retractArea, Position.RetractArea.Index);
            }

            if (ResetEvents is not null)
            {
                return ResetEvents.MediaDetectedEvent(
                    new XFS4IoT.CashManagement.Events.MediaDetectedEvent.PayloadData(StorageId,
                                                                                     retract,
                                                                                     Position.OutputPosition switch
                                                                                     {
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutBottom => XFS4IoT.CashManagement.OutputPositionEnum.OutBottom,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutCenter => XFS4IoT.CashManagement.OutputPositionEnum.OutCenter,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutDefault => XFS4IoT.CashManagement.OutputPositionEnum.OutDefault,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutFront => XFS4IoT.CashManagement.OutputPositionEnum.OutFront,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutLeft => XFS4IoT.CashManagement.OutputPositionEnum.OutLeft,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutRear => XFS4IoT.CashManagement.OutputPositionEnum.OutRear,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutRight => XFS4IoT.CashManagement.OutputPositionEnum.OutRight,
                                                                                         CashManagementCapabilitiesClass.PositionEnum.OutTop => XFS4IoT.CashManagement.OutputPositionEnum.OutTop,
                                                                                         _ => null,
                                                                                     })
                    );
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(MediaDetectedEvent));
        }
    }
}
