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
using XFS4IoT.CashManagement.Events;
using XFS4IoT.Common.Events;
using XFS4IoT.CashDispenser.Events;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a dispenser service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical dispenser, which only implements the Dispenser, CashManagement and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CashDispenserServiceProvider : ServiceProvider, ICashDispenserServiceClass, ICashManagementServiceClass, ICommonServiceClass
    {
        public CashDispenserServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CashManagement, XFSConstants.ServiceClass.CashDispenser },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            CashManagementService = new CashManagementServiceClass(this, CommonService, logger, persistentData);
            CashDispenserService = new CashDispenserServiceClass(this, CashManagementService, CommonService, logger, persistentData);
        }

        private readonly CashDispenserServiceClass CashDispenserService;
        private readonly CashManagementServiceClass CashManagementService;
        private readonly CommonServiceClass CommonService;


        #region CashDispenser unsolicited events
        public Task ItemsTakenEvent(ItemsTakenEvent.PayloadData Payload) => CashDispenserService.ItemsTakenEvent(Payload);

        public Task ShutterStatusChangedEvent(ShutterStatusChangedEvent.PayloadData Payload) => CashDispenserService.ShutterStatusChangedEvent(Payload);

        public Task MediaDetectedEvent(MediaDetectedEvent.PayloadData Payload) => CashDispenserService.MediaDetectedEvent(Payload);

        public Task ItemsPresentedEvent() => CashDispenserService.ItemsPresentedEvent();
        #endregion

        #region CashManagement unsolicited events
        public Task TellerInfoChangedEvent(TellerInfoChangedEvent.PayloadData Payload) => CashManagementService.TellerInfoChangedEvent(Payload);

        public Task CashUnitThresholdEvent(CashUnitThresholdEvent.PayloadData Payload) => CashManagementService.CashUnitThresholdEvent(Payload);

        public Task CashUnitInfoChangedEvent(CashUnitInfoChangedEvent.PayloadData Payload) => CashManagementService.CashUnitInfoChangedEvent(Payload);

        public Task SafeDoorOpenEvent() => CashManagementService.SafeDoorOpenEvent();

        public Task SafeDoorClosedEvent() => CashManagementService.SafeDoorClosedEvent();
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => CommonService.NonceClearedEvent(Payload);

        public Task ExchangeStateChangedEvent(ExchangeStateChangedEvent.PayloadData Payload) => CommonService.ExchangeStateChangedEvent(Payload);
        #endregion

        /// <summary>
        /// Construct cash unit information given by the device class
        /// </summary>
        public void ConstructCashUnits() => CashManagementService.ConstructCashUnits();

        /// <summary>
        /// Update various counts from the device class
        /// </summary>
        public void UpdateCashUnitAccounting(Dictionary<string, ItemMovement> MovementResult) => CashManagementService.UpdateCashUnitAccounting(MovementResult);

        /// <summary>
        /// Cash unit structure information of this device
        /// </summary>
        public Dictionary<string, CashUnit> CashUnits { get => CashManagementService.CashUnits; set => CashManagementService.CashUnits = value; }

        /// <summary>
        /// This property is set to true once the framework processed first GetCashUnitInfo command on the start of the day.
        /// </summary>
        public bool FirstCashUnitInfoCommand { get => CashManagementService.FirstCashUnitInfoCommand; set => CashManagementService.FirstCashUnitInfoCommand = value; }

        /// <summary>
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value;  }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }


        /// <summary>
        /// Add vendor specific mix algorithm
        /// </summary>
        /// <param name="mixNumber"></param>
        /// <param name="mix">new mix algorithm to support for a customization</param>
        public void AddMix(int mixNumber, Mix mix) => CashDispenserService.AddMix(mixNumber, mix);

        /// <summary>
        /// Return mix algorithm available
        /// </summary>
        /// <returns></returns>
        public Mix GetMix(int mixNumber) => CashDispenserService.GetMix(mixNumber);

        public IEnumerator GetMixAlgorithms() => CashDispenserService.GetMixAlgorithms();

        /// <summary>
        /// Keep last present status
        /// </summary>
        public Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PresentStatus> LastPresentStatus { get => CashDispenserService.LastPresentStatus; set => CashDispenserService.LastPresentStatus = value; }
    }
}
