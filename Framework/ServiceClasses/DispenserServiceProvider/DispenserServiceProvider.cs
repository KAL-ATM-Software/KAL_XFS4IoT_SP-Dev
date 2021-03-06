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
using XFS4IoT.Dispenser.Events;
using XFS4IoTFramework.Dispenser;
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
    public class DispenserServiceProvider : ServiceProvider, IDispenserServiceClass, ICashManagementServiceClass, ICommonServiceClass
    {
        public DispenserServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CashManagement, XFSConstants.ServiceClass.Dispenser },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            CashManagementService = new CashManagementServiceClass(this, CommonService, logger, persistentData);
            DispenserService = new DispenserServiceClass(this, CashManagementService, CommonService, logger, persistentData);
        }

        private readonly DispenserServiceClass DispenserService;
        private readonly CashManagementServiceClass CashManagementService;
        private readonly CommonServiceClass CommonService;


        #region Dispenser unsolicited events
        public Task ItemsTakenEvent(ItemsTakenEvent.PayloadData Payload) => DispenserService.ItemsTakenEvent(Payload);

        public Task ShutterStatusChangedEvent(ShutterStatusChangedEvent.PayloadData Payload) => DispenserService.ShutterStatusChangedEvent(Payload);

        public Task MediaDetectedEvent(MediaDetectedEvent.PayloadData Payload) => DispenserService.MediaDetectedEvent(Payload);

        public Task ItemsPresentedEvent() => DispenserService.ItemsPresentedEvent();
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
        public void AddMix(int mixNumber, Mix mix) => DispenserService.AddMix(mixNumber, mix);

        /// <summary>
        /// Return mix algorithm available
        /// </summary>
        /// <returns></returns>
        public Mix GetMix(int mixNumber) => DispenserService.GetMix(mixNumber);

        public IEnumerator GetMixAlgorithms() => DispenserService.GetMixAlgorithms();

        /// <summary>
        /// Keep last present status
        /// </summary>
        public Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PresentStatus> LastPresentStatus { get => DispenserService.LastPresentStatus; set => DispenserService.LastPresentStatus = value; }
    }
}
