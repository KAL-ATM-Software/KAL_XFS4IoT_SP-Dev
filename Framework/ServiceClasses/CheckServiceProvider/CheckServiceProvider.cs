/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Check.Events;
using XFS4IoT.Storage.Events;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Check;
using XFS4IoTFramework.Storage;
using System.ComponentModel;
using XFS4IoT.Storage;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a Check Scanner service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical Check Scanner, which only implements the Check Scanner and Common interfaces. 
    /// It's possible to create other service provider types (i.e. Cash Recycler and Check Scanner mixed media device) by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CheckScannerServiceProvider : ServiceProvider, ICheckService, ICommonService, ILightsService, IStorageService
    {
        public CheckScannerServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            : base(endpointDetails,
                   ServiceName,
                   [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Check, XFSConstants.ServiceClass.Storage],
                   device,
                   logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            StorageService = new StorageServiceClass(this, logger, persistentData, StorageTypeEnum.Check);
            CheckScanner = new CheckServiceClass(this, logger, persistentData);
        }

        private readonly CheckServiceClass CheckScanner;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService;

        #region Check unsolicited events
        public Task MediaTakenEvent(MediaPresentedPositionEnum Postion)
        {
            return CheckScanner.MediaTakenEvent(new(Position: Postion switch
            {
                MediaPresentedPositionEnum.Input => XFS4IoT.Check.PositionEnum.Input,
                MediaPresentedPositionEnum.Refused => XFS4IoT.Check.PositionEnum.Refused,
                MediaPresentedPositionEnum.ReBuncher => XFS4IoT.Check.PositionEnum.Rebuncher,
                _ => throw new InternalErrorException($"Unexpected event parameter specified for {nameof(CheckScanner.MediaTakenEvent)}, {Postion}"),
            }));
        }

        public Task MediaDetectedEvent(MediaDetectedPositionEnum Position, string storageId = null)
        {
            if (Position == MediaDetectedPositionEnum.Unit &&
                !string.IsNullOrEmpty(storageId))
            {
                return CheckScanner.MediaDetectedEvent(new MediaDetectedEvent.PayloadData(storageId));
            }

            return CheckScanner.MediaDetectedEvent(new MediaDetectedEvent.PayloadData(Position switch
            {
                MediaDetectedPositionEnum.Device => "device",
                MediaDetectedPositionEnum.Position => "position",
                MediaDetectedPositionEnum.Customer => "customer",
                MediaDetectedPositionEnum.Jammed => "jammed",
                MediaDetectedPositionEnum.Unknown => "unknown",
                _ => throw new InternalErrorException($"Unexpected event parameter specified for {nameof(CheckScanner.MediaDetectedEvent)}, {Position}"),
            }));
        }

        /*
         * No ShutterStatusChangeEvent is supported as Common.StatusChanged event sends updated shutter status.
         * Specification after 2023-2 will be updated and ShutterStatusChangeEvent will be obsolete.
        */
        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(List<string> CheckUnitIds)
        {
            StorageThresholdEvent.PayloadData paylod = new()
            {
                ExtendedProperties = GetStorages(CheckUnitIds)
            };
            return StorageService.StorageThresholdEvent(paylod);
        }

        public Task StorageChangedEvent(List<string> CheckUnitIds)
        {
            StorageChangedEvent.PayloadData paylod = new()
            {
                ExtendedProperties = GetStorages(CheckUnitIds)
            };
            return StorageService.StorageChangedEvent(paylod);
        }
        #endregion

        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the CardReader Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage Service

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int count, string preservedStorage) => throw new NotSupportedException($"Check Scanner service provider doesn't support card storage.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta, Dictionary<string, string> preservedStorage) => throw new NotSupportedException($"Check Scanner service provider doesn't support cash storage.");

        /// <summary>
        /// Update managed check storage information in the framework.
        /// </summary>
        public Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta = null, Dictionary<string, string> preservedStorage = null) => StorageService.UpdateCheckStorageCount(countDelta, preservedStorage);

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => StorageService.StorageType; init { } }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() => StorageService.StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get => StorageService.CardUnits; init { } }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get => StorageService.CashUnits; init { } }
        
        /// <summary>
        /// Check storage structure information of this device
        /// </summary>
        public Dictionary<string, CheckUnitStorage> CheckUnits { get => StorageService.CheckUnits; init { } }

        /// <summary>
        /// Return XFS4IoT storage structured object.
        /// </summary>
        public Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> UnitIds) => StorageService.GetStorages(UnitIds);
        #endregion

        #region Common Service

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Stores Check Scanner interface capabilites internally
        /// </summary>
        public CheckScannerCapabilitiesClass CheckScannerCapabilities { get => CommonService.CheckScannerCapabilities; set => CommonService.CheckScannerCapabilities = value; }

        /// <summary>
        /// Check Scanner Status
        /// </summary>
        public CheckScannerStatusClass CheckScannerStatus { get => CommonService.CheckScannerStatus; set => CommonService.CheckScannerStatus = value; }

        #endregion

        #region Check Service

        /// <summary>
        /// Store transaction status
        /// </summary>
        public TransactionStatus LastTransactionStatus { get => CheckScanner.LastTransactionStatus; init { } }
        
        /// <summary>
        /// Store transaction status persistently
        /// </summary>
        public void StoreTransactionStatus() => CheckScanner.StoreTransactionStatus();

        #endregion
    }
}
