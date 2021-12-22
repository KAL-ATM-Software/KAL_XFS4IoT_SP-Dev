/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass
    {
        public CardReaderServiceClass(IServiceProvider ServiceProvider,
                                      ICommonService CommonService,
                                      IStorageService StorageService,
                                      ILogger logger)
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(CardReaderServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(CardReaderServiceClass));

            StorageService.IsNotNull($"Unexpected parameter set in the " + nameof(CardReaderServiceClass));
            this.StorageService = StorageService.IsA<IStorageService>($"Invalid interface parameter specified for storage service. " + nameof(CardReaderServiceClass));

            GetCapabilities();
            GetStatus();
        }

        #region Common Service
        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set => CommonService.CardReaderCapabilities = value; }

        /// <summary>
        /// CardReader Status
        /// </summary>
        public CardReaderStatusClass CardReaderStatus { get => CommonService.CardReaderStatus; set => CommonService.CardReaderStatus = value; }

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


        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the CardReader Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage Service
        /// <summary>
        /// Storage service interface
        /// </summary>
        public IStorageService StorageService { get; init; }

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public async Task UpdateCardStorageCount(string storageId, int count, string preservedStorage) => await StorageService.UpdateCardStorageCount(storageId, count, preservedStorage);

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta, Dictionary<string, string> preservedStorage) => throw new NotSupportedException($"CardReader service provider doesn't support cash storage.");

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
        public Dictionary<string, CashUnitStorage> CashUnits { get => throw new NotSupportedException($"CardReader service provider doesn't support cash storage."); set { } }
        #endregion

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderCapabilities");
            CardReaderCapabilities = Device.CardReaderCapabilities;
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderCapabilities=");

            CardReaderCapabilities.IsNotNull($"The device class set CardReaderCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderStatus");
            CardReaderStatus = Device.CardReaderStatus;
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderStatus=");

            CardReaderStatus.IsNotNull($"The device class set CardReaderStatus property to null. The device class must report device status.");
        }
    }
}
