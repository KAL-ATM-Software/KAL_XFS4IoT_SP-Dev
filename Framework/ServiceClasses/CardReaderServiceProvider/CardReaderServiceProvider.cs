/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * CardReaderServiceProvider.cs.cs uses automatically generated parts. 
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoT.CardReader.Events;
using XFS4IoT.Storage.Events;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a card reader service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical card reader, which only implements the CardReader and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CardReaderServiceProvider : ServiceProvider, ICardReaderServiceClass, ICommonServiceClass, ILightsServiceClass, IStorageServiceClass
    {
        public CardReaderServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CardReader, XFSConstants.ServiceClass.Storage },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            StorageService = new StorageServiceClass(this, CommonService, logger, persistentData, StorageTypeEnum.Card);
            CardReader = new CardReaderServiceClass(this, CommonService, StorageService, logger);
        }

        private readonly CardReaderServiceClass CardReader;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService;

        #region CardReader unsolicited events
        public Task MediaRemovedEvent() => CardReader.MediaRemovedEvent();

        public Task CardActionEvent(CardActionEvent.PayloadData Payload) => CardReader.CardActionEvent(Payload);
        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(StorageThresholdEvent.PayloadData Payload) => StorageService.StorageThresholdEvent(Payload);

        public Task StorageChangedEvent(StorageChangedEvent.PayloadData Payload) => StorageService.StorageChangedEvent(Payload);

        public Task StorageErrorEvent(StorageErrorEvent.PayloadData Payload) => StorageService.StorageErrorEvent(Payload);
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => CommonService.NonceClearedEvent(Payload);

        public Task ExchangeStateChangedEvent(ExchangeStateChangedEvent.PayloadData Payload) => CommonService.ExchangeStateChangedEvent(Payload);
        #endregion

        #region Storage Service

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
        public Dictionary<string, CashUnitStorage> CashUnits { get => StorageService.CashUnits; set { } }

        #endregion

        #region Common Service

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set => CommonService.CardReaderCapabilities = value; }

        /// <summary>
        /// Card storage information device supports 
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardStorages { get => StorageService.CardUnits; set => StorageService.CardUnits = value; }

        #endregion
    }
}
