/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;
using XFS4IoT.CashManagement.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using XFS4IoT.CashManagement;
using System.ComponentModel;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass
    {
        public CashManagementServiceClass(IServiceProvider ServiceProvider, 
                                          ILogger logger,
                                          IPersistentData PersistentData) 
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashManagementServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashManagementDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CashManagementServiceClass)}");
            StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(CashManagementServiceClass)}");

            GetStatus();
            GetCapabilities();

            this.PersistentData = PersistentData.IsNotNull($"No persistent data interface is set in the " + typeof(CashManagementServiceClass));
            
            CashInStatusManaged = PersistentData.Load<CashInStatusClass>(ServiceProvider.Name + typeof(CashInStatusClass).FullName);
            if (CashInStatusManaged is null)
            {
                CashInStatusManaged = new();
                StoreCashInStatus();
            }

            LastCashManagementPresentStatus = PersistentData.Load<Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashManagementPresentStatus>>(ServiceProvider.Name + typeof(CashManagementPresentStatus).FullName);
            if (LastCashManagementPresentStatus is null)
            {
                LastCashManagementPresentStatus = _LastCashManagementPresentStatus;
                StoreCashManagementPresentStatus();
            }

            ItemClassificationList = PersistentData.Load<ItemClassificationListClass>(ServiceProvider.Name + typeof(ItemClassificationListClass).FullName);
            if (ItemClassificationList is null)
            {
                ItemClassificationList = new();
                StoreItemClassificationList();
            }
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// Storage service interface
        /// </summary>
        private IStorageService StorageService { get; init; }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        public Task ItemsTakenEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches = null) => ItemsTakenEvent(
                    new ItemsTakenEvent.PayloadData(
                        Position: Position switch
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
                            _ => throw new InternalErrorException($"Invalid position is specified for the ItemsTakenEvent event {Position}"),
                        },
                        AdditionalBunches: AdditionalBunches)
                    );

        public Task ItemsInsertedEvent(CashManagementCapabilitiesClass.PositionEnum Position) => ItemsInsertedEvent(
            new ItemsInsertedEvent.PayloadData(Position switch
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
                _ => throw new InternalErrorException($"Invalid position is specified for the ItemsInsertedEvent event {Position}"),
            }));

        public Task ItemsPresentedEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches) => ItemsPresentedEvent(
                        new ItemsPresentedEvent.PayloadData(
                            Position: Position switch
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
                                _ => throw new InternalErrorException($"Invalid position is specified for the ItemsPresentedEvent event {Position}"),
                            },
                            AdditionalBunches: AdditionalBunches));

        public Task MediaDetectedEvent(string StorageId, ItemTargetEnum Target, int? Index = null)
        {
            (StorageService.CashUnits is null || StorageService.CashUnits.Count == 0).IsFalse($"MediaDetectedEvent is attempted to send when no storage unit setup.");

            string unitName = null;
            foreach (var unit in StorageService.CashUnits)
            {
                if (unit.Value.Id == StorageId)
                {
                    unitName = unit.Key;
                    break;
                }
            }

            if (string.IsNullOrEmpty(unitName) &&
                Target == ItemTargetEnum.SingleUnit)
            {
                Contracts.Assert(false, $"The items are moved into the single unit but there is no StorageId matches with specified type of unit. {StorageId} {Target}");
            }

            return MediaDetectedEvent(
                    new(Target: Target switch
                        {
                            ItemTargetEnum.SingleUnit => ItemTargetEnumEnum.SingleUnit,
                            ItemTargetEnum.Retract => ItemTargetEnumEnum.Retract,
                            ItemTargetEnum.Transport => ItemTargetEnumEnum.Transport,
                            ItemTargetEnum.Stacker => ItemTargetEnumEnum.Stacker,
                            ItemTargetEnum.Reject => ItemTargetEnumEnum.Reject,
                            ItemTargetEnum.ItemCassette => ItemTargetEnumEnum.ItemCassette,
                            ItemTargetEnum.CashIn => ItemTargetEnumEnum.CashIn,
                            ItemTargetEnum.OutDefault => ItemTargetEnumEnum.OutDefault,
                            ItemTargetEnum.OutLeft => ItemTargetEnumEnum.OutLeft,
                            ItemTargetEnum.OutRight => ItemTargetEnumEnum.OutRight,
                            ItemTargetEnum.OutCenter => ItemTargetEnumEnum.OutCenter,
                            ItemTargetEnum.OutTop => ItemTargetEnumEnum.OutTop,
                            ItemTargetEnum.OutBottom => ItemTargetEnumEnum.OutBottom,
                            ItemTargetEnum.OutFront => ItemTargetEnumEnum.OutFront,
                            ItemTargetEnum.OutRear => ItemTargetEnumEnum.OutRear,
                            _ => throw new InternalErrorException($"Unsupported target value specified for sending MediaDetectedEvent. {Target}"),
                        }, 
                        Unit: unitName,
                        Index: Index)
                    );
        }

        public Task ShutterStatusChangedEvent(CashManagementCapabilitiesClass.PositionEnum Position, CashManagementStatusClass.ShutterEnum Status)
        {
            ShutterStatusChangedEvent.PayloadData payload = new(
                Position: Position switch
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
                    _ => throw new InternalErrorException($"Invalid position is specified for the ShutterStatusChangedEvent event {Position}"),
                },
                Shutter: Status switch
                {
                    CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashManagement.ShutterEnum.Closed,
                    CashManagementStatusClass.ShutterEnum.JammedClosed => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.JammedOpen => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.JammedPartiallyOpen => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.JammedUnknown => XFS4IoT.CashManagement.ShutterEnum.Jammed,
                    CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashManagement.ShutterEnum.Open,
                    CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashManagement.ShutterEnum.Unknown,
                    _ => throw new InternalErrorException($"Invalid status is specified for the ShutterStatusChangedEvent event {Status}"),
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
            CommonService.CashManagementStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities");
            CommonService.CashManagementCapabilities = Device.CashManagementCapabilities;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities=");

            CommonService.CashManagementCapabilities.IsNotNull($"The device class set CashManagementCapabilities property to null. The device class must report device capabilities.");
        }

        /// <summary>
        /// The framework maintains cash-in status
        /// </summary>
        public CashInStatusClass CashInStatusManaged { get; init; }

        /// <summary>
        /// Store cash-in in status persistently
        /// </summary>
        public void StoreCashInStatus()
        {
            if (!PersistentData.Store<CashInStatusClass>(ServiceProvider.Name + typeof(CashInStatusClass).FullName, CashInStatusManaged))
            {
                Logger.Warning(Constants.Framework, $"Failed to save persistent data. {ServiceProvider.Name + typeof(CashInStatusClass).FullName}");
            }
        }

        /// <summary>
        /// The last status of the most recent attempt to present or return items to the customer. 
        /// </summary>
        public Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashManagementPresentStatus> LastCashManagementPresentStatus { get; init; }

        /// <summary>
        /// Keep last present status per position
        /// </summary>
        private readonly Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashManagementPresentStatus> _LastCashManagementPresentStatus = new()
        {
            { CashManagementCapabilitiesClass.PositionEnum.InBottom, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InCenter, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InDefault, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InFront, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InLeft, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InRear, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InRight, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.InTop, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutBottom, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutCenter, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutDefault, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutFront, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutLeft, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutRear, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutRight, new CashManagementPresentStatus() },
            { CashManagementCapabilitiesClass.PositionEnum.OutTop, new CashManagementPresentStatus() },
        };

        /// <summary>
        /// Store present status persistently
        /// </summary>
        public void StoreCashManagementPresentStatus()
        {
            if (!PersistentData.Store<Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashManagementPresentStatus>>(ServiceProvider.Name + typeof(CashManagementPresentStatus).FullName, LastCashManagementPresentStatus))
            {
                Logger.Warning(Constants.Framework, $"Failed to save persistent data. {ServiceProvider.Name + typeof(CashManagementPresentStatus).FullName}");
            }
        }

        /// <summary>
        /// This list provides the functionality to blacklist notes and allows additional flexibility, for example to specify that notes can be taken out of circulation
        /// by specifying them as unfit.Any items not returned in this list will be handled according to normal classification rules.
        /// </summary>
        public ItemClassificationListClass ItemClassificationList { get; init; }


        /// <summary>
        /// Store classification list persistently
        /// </summary>
        public void StoreItemClassificationList()
        {
            if (!PersistentData.Store<ItemClassificationListClass>(ServiceProvider.Name + typeof(ItemClassificationListClass).FullName, ItemClassificationList))
            {
                Logger.Warning(Constants.Framework, $"Failed to save persistent data. {ServiceProvider.Name + typeof(ItemClassificationListClass).FullName}");
            }
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
