/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoT.CardReader.Events;
using XFS4IoT.Storage.Events;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Storage;
using System.ComponentModel;
using System.Threading;

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
    public class CardReaderServiceProvider : ServiceProvider, ICardReaderService, ICommonService, ILightsService, IStorageService
    {
        public CardReaderServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger, 
            IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CardReader, XFSConstants.ServiceClass.Storage],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            StorageService = new StorageServiceClass(this, logger, persistentData, StorageTypeEnum.Card);
            CardReader = new CardReaderServiceClass(this, logger);
        }

        private readonly CardReaderServiceClass CardReader;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService;

        #region CardReader unsolicited events
        public Task MediaRemovedEvent() => CardReader.MediaRemovedEvent();

        public Task CardActionEvent(MovePosition To, MovePosition From)
        {
            string to = To.Position switch
            {
                MovePosition.MovePositionEnum.Exit => "exit",
                MovePosition.MovePositionEnum.Transport => "transport",
                _ => To.StorageId,
            };
            string from = From.Position switch
            {
                MovePosition.MovePositionEnum.Exit => "exit",
                MovePosition.MovePositionEnum.Transport => "transport",
                _ => From.StorageId,
            };
            return CardReader.CardActionEvent(new CardActionEvent.PayloadData(to, from));
        }

        public Task MediaDetectedEvent(MovePosition Position)
        {
            return CardReader.MediaDetectedEvent(new MediaDetectedEvent.PayloadData(Position.Position switch
            {
                MovePosition.MovePositionEnum.Exit => "exit",
                MovePosition.MovePositionEnum.Transport => "transport",
                _ => Position.StorageId,
            }));
        }

        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(List<string> CardUnitIds)
        {
            StorageThresholdEvent.PayloadData paylod = new()
            {
                ExtendedProperties = GetStorages(CardUnitIds)
            };
            return StorageService.StorageThresholdEvent(paylod);
        }

        /// <summary>
        /// Sending status changed event.
        /// </summary>
        public Task StorageChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => StorageService.StorageChangedEvent(sender, propertyInfo);


        #endregion

        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the CardReader Service.");

        public Task ErrorEvent(
            CommonStatusClass.ErrorEventIdEnum EventId,
            CommonStatusClass.ErrorActionEnum Action,
            string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage Service

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public async Task UpdateCardStorageCount(string storageId, int count) => await StorageService.UpdateCardStorageCount(storageId, count);

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta) => throw new NotSupportedException($"CardReader service doesn't support cash storage.");

        /// <summary>
        /// Update managed check storage information in the framework.
        /// </summary>
        public Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta = null) => throw new NotSupportedException($"CardReader service class doesn't support check storage.");

        /// <summary>
        /// Update managed printer storage information in the framework.
        /// </summary>
        public Task UpdatePrinterStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CardReader service class doesn't support printer storage.");

        /// <summary>
        /// Update managed deposit storage information in the framework.
        /// </summary>
        public Task UpdateDepositStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CardReader service class doesn't support deposit storage.");


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
        /// Printer storage structure information of this device
        /// </summary>
        public Dictionary<string, PrinterUnitStorage> PrinterUnits { get => StorageService.PrinterUnits; init { } }

        /// <summary>
        /// IBNS storage structure information of this device
        /// </summary>
        public Dictionary<string, IBNSUnitStorage> IBNSUnits { get => StorageService.IBNSUnits; init { } }

        /// <summary>
        /// Deposit storage structure information of this device
        /// </summary>
        public Dictionary<string, DepositUnitStorage> DepositUnits { get => StorageService.DepositUnits; init { } }

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
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set => CommonService.CardReaderCapabilities = value; }

        /// <summary>
        /// CardReader Status
        /// </summary>
        public CardReaderStatusClass CardReaderStatus { get => CommonService.CardReaderStatus; set => CommonService.CardReaderStatus = value; }


        #endregion
    }
}
