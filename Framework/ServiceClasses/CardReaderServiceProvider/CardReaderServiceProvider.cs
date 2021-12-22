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
    public class CardReaderServiceProvider : ServiceProvider, ICardReaderService, ICommonService, ILightsService, IStorageService
    {
        public CardReaderServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CardReader, XFSConstants.ServiceClass.Storage },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            StorageService = new StorageServiceClass(this, CommonService, logger, persistentData, StorageTypeEnum.Card);
            CardReader = new CardReaderServiceClass(this, CommonService, StorageService, logger);
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
        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(List<string> CardUnitIds)
        {
            StorageThresholdEvent.PayloadData paylod = new();
            paylod.ExtendedProperties = GetStorages(CardUnitIds);
            return StorageService.StorageThresholdEvent(paylod);
        }

        public Task StorageChangedEvent(List<string> CardUnitIds)
        {
            StorageChangedEvent.PayloadData paylod = new();
            paylod.ExtendedProperties = GetStorages(CardUnitIds);
            return StorageService.StorageChangedEvent(paylod);
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CardUnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = GetStorages(CardUnitIds);
            return StorageService.StorageErrorEvent(new StorageErrorEvent.PayloadData(Failure switch
                                                                                     {
                                                                                         FailureEnum.Config => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Config,
                                                                                         FailureEnum.Empty => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Empty,
                                                                                         FailureEnum.Error => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Error,
                                                                                         FailureEnum.Full => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Full,
                                                                                         FailureEnum.Invalid => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Invalid,
                                                                                         FailureEnum.Locked => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Locked,
                                                                                         _ => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.NotConfigured,
                                                                                     },
                                                                                     storages));
        }

        private Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> CardUnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = new();

            foreach (var storageId in CardUnitIds)
            {
                if (!StorageService.CardUnits.ContainsKey(storageId))
                    continue;

                storages.Add(storageId,
                             new(PositionName: StorageService.CardUnits[storageId].PositionName,
                                 Capacity: StorageService.CardUnits[storageId].Capacity,
                                 Status: StorageService.CardUnits[storageId].Status switch
                                 {
                                     CardUnitStorage.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                     CardUnitStorage.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                     CardUnitStorage.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                     CardUnitStorage.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                     _ => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                 },
                                 SerialNumber: StorageService.CardUnits[storageId].SerialNumber,
                                 Cash: null,
                                 Card: new XFS4IoT.CardReader.StorageClass(
                                        new XFS4IoT.CardReader.StorageCapabilitiesClass(StorageService.CardUnits[storageId].Unit.Capabilities.Type switch
                                        {
                                            CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                            CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                            _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                        },
                                                                                        StorageService.CardUnits[storageId].Unit.Capabilities.HardwareSensors),
                                        new XFS4IoT.CardReader.StorageConfigurationClass(StorageService.CardUnits[storageId].Unit.Configuration.CardId,
                                                                                         StorageService.CardUnits[storageId].Unit.Configuration.Threshold),
                                        new XFS4IoT.CardReader.StorageStatusClass(StorageService.CardUnits[storageId].Unit.Status.InitialCount,
                                                                                  StorageService.CardUnits[storageId].Unit.Status.Count,
                                                                                  StorageService.CardUnits[storageId].Unit.Status.RetainCount,
                                                                                  StorageService.CardUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                                                                  {
                                                                                      CardStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.Storage.ReplenishmentStatusEnumEnum.Empty,
                                                                                      CardStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Storage.ReplenishmentStatusEnumEnum.Full,
                                                                                      CardStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Storage.ReplenishmentStatusEnumEnum.High,
                                                                                      CardStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.Storage.ReplenishmentStatusEnumEnum.Low,
                                                                                      _ => XFS4IoT.Storage.ReplenishmentStatusEnumEnum.Ok,
                                                                                  })
                                        )
                                 ));
            }

            return storages;
        }
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
