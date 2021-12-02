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
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashDispenser
{
    public sealed class DispenseCommandEvents : ItemErrorCommandEvents
    {
        public DispenseCommandEvents(IDispenseEvents events) :
            base(events)
        { }

        public Task ShutterStatusChangedEvent(CashManagementCapabilitiesClass.PositionEnum Position, CashManagementStatusClass.ShutterEnum Status)
        {
            XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent.PayloadData payload = new(
                Position switch
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
                Status switch
                {
                    CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashManagement.ShutterEnum.Closed,
                    CashManagementStatusClass.ShutterEnum.Jammed => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashManagement.ShutterEnum.Closed,
                    CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashManagement.ShutterEnum.Closed,
                    _ => null,
                }
                );

            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(ShutterStatusChangedEvent));
            
            return DispenseEvents.ShutterStatusChangedEvent(payload);
        }

        public Task DelayedDispenseEvent(double Delay)
        {
            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(DelayedDispenseEvent));
            return DispenseEvents.DelayedDispenseEvent(new(Delay));
        }

        public Task StartDispenseEvent()
        {
            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(StartDispenseEvent));
            return DispenseEvents.StartDispenseEvent();
        }

        public Task IncompleteDispenseEvent(Dictionary<string, double> Currencies, Dictionary<string, int> Values, Dictionary<string, double> CashBox)
        {
            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(IncompleteDispenseEvent));
            return DispenseEvents.IncompleteDispenseEvent(new(Currencies, Values, CashBox));
        }
    }
}
