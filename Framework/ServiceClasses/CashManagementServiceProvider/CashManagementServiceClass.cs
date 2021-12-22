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
        public CashManagementServiceClass(IServiceProvider ServiceProvider,
                                          ICommonService CommonService,
                                          IStorageService StorageService,
                                          ILogger logger) 
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(CashManagementServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(CashManagementServiceClass));

            StorageService.IsNotNull($"Unexpected parameter set for storage service in the " + nameof(CashManagementServiceClass));
            this.StorageService = StorageService.IsA<IStorageService>($"Invalid interface parameter specified for storage service. " + nameof(CashManagementServiceClass));

            GetStatus();
            GetCapabilities();
        }

        #region Common Service

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Capabilities of the CashManagement interface
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        /// <summary>
        /// Capabilities of the CashDispenser interface
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value; }

        /// <summary>
        /// CashManagement Status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get => CommonService.CashManagementStatus; set => CommonService.CashManagementStatus = value; }

        #endregion

        #region Common unsolicited events
        public Task StatusChangedEvent(CommonStatusClass.DeviceEnum? Device,
                                       CommonStatusClass.PositionStatusEnum? Position,
                                       int? PowerSaveRecoveryTime,
                                       CommonStatusClass.AntiFraudModuleEnum? AntiFraudModule,
                                       CommonStatusClass.ExchangeEnum? Exchange,
                                       CommonStatusClass.EndToEndSecurityEnum? EndToEndSecurity) => CommonService.StatusChangedEvent(Device,
                                                                                                                                     Position,
                                                                                                                                     PowerSaveRecoveryTime,
                                                                                                                                     AntiFraudModule,
                                                                                                                                     Exchange,
                                                                                                                                     EndToEndSecurity);
        public Task NonceClearedEvent(string ReasonDescription) => CommonService.NonceClearedEvent(ReasonDescription);

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage Service
        /// <summary>
        /// Storage service interface
        /// </summary>
        private IStorageService StorageService { get; init; }

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int countDelta, string preservedStorage) => throw new NotSupportedException($"The CashManagement interface doesn't aupport card unit information.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null, Dictionary<string, string> preservedStorage = null) => await StorageService.UpdateCashAccounting(countDelta, preservedStorage);

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => StorageService.StorageType; set { } }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() => StorageService.StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get => StorageService.CardUnits; set { } }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get => StorageService.CashUnits; set { } }

        #endregion

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
            CashManagementStatus = Device.CashManagementStatus;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashDispenserStatus=");

            CashManagementStatus.IsNotNull($"The device class set CashManagementStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities");
            CashManagementCapabilities = Device.CashManagementCapabilities;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities=");

            CashManagementCapabilities.IsNotNull($"The device class set CashManagementCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
