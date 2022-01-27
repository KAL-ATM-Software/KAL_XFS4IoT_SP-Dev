/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;
using XFS4IoT.CashManagement.Events;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass
    {
        public CashManagementServiceClass(IServiceProvider ServiceProvider, ILogger logger) 
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashManagementServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashManagementDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CashManagementServiceClass)}");
            StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(CashManagementServiceClass)}");

            GetStatus();
            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// Storage service interface
        /// </summary>
        private IStorageService StorageService { get; init; }

        public Task ItemsTakenEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Position, string AdditionalBunches = null) => ItemsTakenEvent(
                    new ItemsTakenEvent.PayloadData(
                        new XFS4IoT.CashManagement.PositionInfoClass(Position switch
                        {
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Center => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Default => XFS4IoT.CashManagement.PositionEnum.OutDefault,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Front => XFS4IoT.CashManagement.PositionEnum.OutFront,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Left => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Right => XFS4IoT.CashManagement.PositionEnum.OutRight,
                            CashDispenserCapabilitiesClass.OutputPositionEnum.Top => XFS4IoT.CashManagement.PositionEnum.OutTop,
                            _ => null,
                        },
                            AdditionalBunches))
                    );

        public Task ItemsInsertedEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Postion) => ItemsInsertedEvent(
            new ItemsInsertedEvent.PayloadData(Postion switch
            {
                CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Center => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Default => XFS4IoT.CashManagement.PositionEnum.OutDefault,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Front => XFS4IoT.CashManagement.PositionEnum.OutFront,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Left => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Right => XFS4IoT.CashManagement.PositionEnum.OutRight,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Top => XFS4IoT.CashManagement.PositionEnum.OutTop,
                _ => null,
            }));

        public Task ItemsPresentedEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Position, string AdditionalBunches) => ItemsPresentedEvent(
                        new ItemsPresentedEvent.PayloadData(
                            new XFS4IoT.CashManagement.PositionInfoClass(Position switch
                            {
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Center => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Default => XFS4IoT.CashManagement.PositionEnum.OutDefault,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Front => XFS4IoT.CashManagement.PositionEnum.OutFront,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Left => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Right => XFS4IoT.CashManagement.PositionEnum.OutRight,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Top => XFS4IoT.CashManagement.PositionEnum.OutTop,
                                _ => null,
                            },
                            AdditionalBunches)));

        public Task ShutterStatusChangedEvent(CashManagementCapabilitiesClass.PositionEnum Position, CashManagementStatusClass.ShutterEnum Status)
        {
            ShutterStatusChangedEvent.PayloadData payload = new(
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
                    CashManagementStatusClass.ShutterEnum.JammedClosed => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.JammedOpen => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.JammedPartiallyOpen => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.JammedUnknown => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashManagement.ShutterEnum.Closed,
                    CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashManagement.ShutterEnum.Closed,
                    _ => null,
                }
                );

            return ShutterStatusChangedEvent(payload);
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementStatus");
            CommonService.CashManagementStatus = Device.CashManagementStatus;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashDispenserStatus=");

            CommonService.CashManagementStatus.IsNotNull($"The device class set CashManagementStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities");
            CommonService.CashManagementCapabilities = Device.CashManagementCapabilities;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities=");

            CommonService.CashManagementCapabilities.IsNotNull($"The device class set CashManagementCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
