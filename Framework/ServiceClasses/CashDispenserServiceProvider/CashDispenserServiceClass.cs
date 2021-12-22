/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class CashDispenserServiceClass
    {
        public CashDispenserServiceClass(IServiceProvider ServiceProvider,
                                         ICashManagementService CashManagementService,
                                         ICommonService CommonService,
                                         ILogger logger, 
                                         IPersistentData PersistentData)
            : this(ServiceProvider, logger)
        {
            this.PersistentData = PersistentData.IsNotNull($"No persistent data interface is set. " + typeof(Mix).FullName);

            // Load persistent data
            Dictionary<string, Mix> tableMixes = PersistentData.Load<Dictionary<string, Mix>>(typeof(Mix).FullName);
            if (tableMixes is not null)
            {
                // merge table mix set by the application
                foreach (var t in tableMixes)
                    AddMix(t.Key, t.Value);
            }

            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(CashDispenserServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(CashDispenserServiceClass));

            CashManagementService.IsNotNull($"Unexpected parameter set for cash management service in the " + nameof(CashDispenserServiceClass));
            this.CashManagementService = CashManagementService.IsA<ICashManagementService>($"Invalid interface parameter specified for cash management service. " + nameof(CashDispenserServiceClass));

            Mixes = new()
            {
                { "mix1", new MinNumberMix(logger) },
                { "mix2", new EqualEmptyingMix(logger) }
            };

            GetStatus();
            GetCapabilities();
        }

        #region CashManagement unsolicited events

        public Task ItemsTakenEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Position, string AdditionalBunches = null) => CashManagementService.ItemsTakenEvent(Position, AdditionalBunches);

        public Task ItemsInsertedEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Postion) => CashManagementService.ItemsInsertedEvent(Postion);

        public Task ItemsPresentedEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Position, string AdditionalBunches) => CashManagementService.ItemsPresentedEvent(Position, AdditionalBunches);

        public Task ShutterStatusChangedEvent(CashManagementCapabilitiesClass.PositionEnum Position, CashManagementStatusClass.ShutterEnum Status) => CashManagementService.ShutterStatusChangedEvent(Position, Status);

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
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value; }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        /// <summary>
        /// CashDispenser Status
        /// </summary>
        public CashDispenserStatusClass CashDispenserStatus { get => CommonService.CashDispenserStatus; set => CommonService.CashDispenserStatus = value; }

        /// <summary>
        /// CashManagement Status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get => CommonService.CashManagementStatus; set => CommonService.CashManagementStatus = value; }

        #endregion

        #region Cash Management Service
        /// <summary>
        /// Common service interface
        /// </summary>
        private ICashManagementService CashManagementService { get; init; }

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int countDelta, string preservedStorage) => throw new NotSupportedException($"The CashManagement interface doesn't aupport card unit information.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null, Dictionary<string, string> preservedStorage = null) => await CashManagementService.UpdateCashAccounting(countDelta, preservedStorage);

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => CashManagementService.StorageType; set { } }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() => CashManagementService.StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get => CashManagementService.CardUnits; set { } }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get => CashManagementService.CashUnits; set { } }

        #endregion

        /// <summary>
        /// Add vendor specific mix algorithm
        /// </summary>
        /// <param name="mixId"></param>
        /// <param name="mix">new mix algorithm to support for a customization</param>
        public void AddMix(string mixId, Mix mix)
        {
            if (Mixes.ContainsKey(mixId))
                Mixes.Remove(mixId);// replace algorithm
            Mixes.Add(mixId, mix);

            if (mix.Type == Mix.TypeEnum.Table)
            {
                // Save table mix set by the application
                Dictionary<string, Mix> tableMixes = PersistentData.Load<Dictionary<string, Mix>>(ServiceProvider.Name + typeof(Mix).FullName);
                if (tableMixes is null)
                    tableMixes = new();

                if (tableMixes.ContainsKey(mixId))
                    tableMixes.Remove(mixId);// Existing table is tested by the SetMixTable command handler and report error to the application. but here could be an internal usage and can be udated.
                tableMixes.Add(mixId, mix);

                if (!PersistentData.Store(ServiceProvider.Name + typeof(Mix).FullName, tableMixes))
                {
                    Logger.Warning(Constants.Framework, "Failed to save persistent data." + typeof(Mix).FullName);
                }
            }
        }

        /// <summary>
        /// Return mix algorithm available
        /// </summary>
        /// <returns></returns>
        public Mix GetMix(string mixId)
        {
            if (Mixes.ContainsKey(mixId))
                return Mixes[mixId];
            return null;
        }

        /// <summary>
        /// Return mix algorithm supported by the framework or the application set mix tables
        /// </summary>
        public Dictionary<string, Mix> GetMixAlgorithms() => Mixes;

        /// <summary>
        /// Keep last present status
        /// </summary>
        public Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PresentStatus> LastPresentStatus { get => _LastPresentStatus; set => _LastPresentStatus = value; }

        /// <summary>
        /// Keep last present status per position
        /// </summary>
        private Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PresentStatus> _LastPresentStatus = new()
        {
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,  new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Center,  new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Default, new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Front,   new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Left,    new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,    new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Right,   new PresentStatus() },
            { CashDispenserCapabilitiesClass.OutputPositionEnum.Top,     new PresentStatus() }
        };

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        /// <summary>
        /// Supported Mix algorithm
        /// </summary>
        private readonly Dictionary<string, Mix> Mixes;

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserStatus");
            CashDispenserStatus = Device.CashDispenserStatus;
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserStatus=");

            CashDispenserStatus.IsNotNull($"The device class set CashDispenserStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserCapabilities");
            CashDispenserCapabilities = Device.CashDispenserCapabilities;
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserCapabilities=");

            CashDispenserCapabilities.IsNotNull($"The device class set CashDispenserCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
