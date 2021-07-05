/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass
    {
        public CashManagementServiceClass(IServiceProvider ServiceProvider,
                                          ICommonService CommonService,
                                          ILogger logger, 
                                          IPersistentData PersistentData) 
            : this(ServiceProvider, logger)
        {
            this.PersistentData = PersistentData.IsNotNull($"No persistent data interface is set. " + nameof(CashManagementServiceClass));

            // Load persistent data
            CashUnits = PersistentData.Load<Dictionary<string, CashUnit>>(typeof(CashUnit).FullName);
            if (CashUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data. It could be a first run and no persistent exists on the file system.");
                CashUnits = new Dictionary<string, CashUnit>();
            }

            FirstCashUnitInfoCommand = true;
            this.CommonService = CommonService.IsNotNull($"Unexpected parameter set in the " + nameof(CashManagementServiceClass));
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value; }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        /// <summary>
        /// ConstructCashUnits
        /// The method retreive cash unit structures from the device class. The device class must provide cash unit structure info
        /// </summary>
        public void ConstructCashUnits()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.GetCashUnitConfiguration()");

            bool newConfiguration = Device.GetCashUnitConfiguration(out Dictionary<string, CashUnitConfiguration> newCashUnits);

            Logger.Log(Constants.DeviceClass, $"CashManagementDev.GetCashUnitConfiguration()-> {newConfiguration}");

            newCashUnits.IsNotNull("The device class returned an empty cash unit structure information on the GetCashUnitStructure.");

            if (newConfiguration)
            {
                CashUnits.Clear();
                foreach (var unit in newCashUnits)
                {
                    CashUnits.Add(unit.Key, new CashUnit(unit.Value));
                }
            }
            else
            {
                bool identical = newCashUnits.Count == CashUnits.Count;
                foreach (var unit in newCashUnits)
                {
                    identical = CashUnits.ContainsKey(unit.Key);
                    if (!identical)
                    {
                        Logger.Warning(Constants.Framework, $"Existing cash unit information doesn't contain key specified by the device class. {unit.Key}. Construct new cash unit infomation.");
                        break;
                    }

                    identical = CashUnits[unit.Key].Configuration == unit.Value;
                    if (!identical)
                    {
                        Logger.Warning(Constants.Framework, $"Existing cash unit information doesn't have an identical cash unit structure information specified by the device class. {unit.Key}. Construct new cash unit infomation.");
                        break;
                    }
                }

                if (!identical)
                {
                    CashUnits.Clear();
                    foreach (var unit in newCashUnits)
                    {
                        CashUnits.Add(unit.Key, new CashUnit(unit.Value));
                    }
                }
            }

            if (!PersistentData.Store(typeof(CashUnit).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// UpdateCashUnitAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public void UpdateCashUnitAccounting(Dictionary<string, ItemMovement> MovementResult = null)
        {
            // First to update item movement reported by the device class, then update entire counts if the device class maintains cash unit counts.
            if (MovementResult is not null)
            {
                foreach (var movement in MovementResult)
                {
                    if (!CashUnits.ContainsKey(movement.Key) ||
                        movement.Value != null)
                    {
                        continue;
                    }
                    // update counts
                    int dispensedCount = movement.Value.DispensedCount ?? 0;
                    CashUnits[movement.Key].Count -= dispensedCount;
                    if (CashUnits[movement.Key].Count < 0)
                        CashUnits[movement.Key].Count = 0;
                    CashUnits[movement.Key].DispensedCount += dispensedCount;
                    CashUnits[movement.Key].RejectCount += movement.Value.RejectCount ?? 0;
                    CashUnits[movement.Key].RetractedCount += movement.Value.RetractedCount ?? 0;
                    CashUnits[movement.Key].PresentedCount += movement.Value.PresentedCount ?? 0;
                    if (movement.Value.StoredBankNoteList is not null &&
                        movement.Value.StoredBankNoteList.Count > 0)
                    {
                        foreach (var (bkMoved, bkCount) in from BankNoteNumber bkMoved in movement.Value.StoredBankNoteList
                                                           from BankNoteNumber bkCount in CashUnits[movement.Key].BankNoteNumberList
                                                           where bkMoved.NoteID == bkCount.NoteID
                                                           select (bkMoved, bkCount))
                        {
                            bkCount.Count += bkMoved.Count;
                            CashUnits[movement.Key].CashInCount += bkMoved.Count;
                            break;
                        }
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashManagementDev.GetCashUnitAccounting()");

            Dictionary<string, CashUnitAccounting> cashUnitAccounting = Device.GetCashUnitAccounting();

            Logger.Log(Constants.DeviceClass, $"CashManagementDev.GetCashUnitAccounting()-> {cashUnitAccounting}");

            if (cashUnitAccounting is not null)
            {
                bool identical = cashUnitAccounting.Count == CashUnits.Count;
                foreach (var accounting in cashUnitAccounting)
                {
                    identical = CashUnits.ContainsKey(accounting.Key);
                    if (!identical)
                    {
                        Logger.Warning(Constants.Framework, $"Existing cash unit information doesn't contain key specified by the device class. {accounting.Key}. Reset count to zero.");
                        break;
                    }
                }

                if (identical)
                {
                    foreach (var unit in cashUnitAccounting)
                        CashUnits[unit.Key].Accounting = unit.Value;
                }
                else
                {
                    foreach (var unit in CashUnits)
                    {
                        unit.Value.Accounting = new CashUnitAccounting();
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashManagementDev.GetCashUnitStatus()");

            Dictionary<string, CashUnit.StatusEnum> cashUnitStatus = Device.GetCashUnitStatus();

            Logger.Log(Constants.DeviceClass, $"CashManagementDev.GetCashUnitStatus()-> {cashUnitStatus}");

            if (cashUnitStatus is not null)
            {
                foreach (var unit in cashUnitStatus)
                {
                    if (CashUnits.ContainsKey(unit.Key))
                        CashUnits[unit.Key].Status = unit.Value;
                }
            }
            else
            {
                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Type == CashUnit.TypeEnum.NotApplicable)
                    {
                        unit.Value.Status = CashUnit.StatusEnum.NoValue;
                        continue;
                    }

                    unit.Value.Status = CashUnit.StatusEnum.Ok;

                    if (unit.Value.Type == CashUnit.TypeEnum.BillCassette ||
                        unit.Value.Type == CashUnit.TypeEnum.CoinCylinder ||
                        unit.Value.Type == CashUnit.TypeEnum.CoinDispenser ||
                        unit.Value.Type == CashUnit.TypeEnum.Coupon ||
                        unit.Value.Type == CashUnit.TypeEnum.Document ||
                        unit.Value.Type == CashUnit.TypeEnum.Recycling)
                    {
                        
                        if (unit.Value.Count == 0)
                        {
                            unit.Value.Status = CashUnit.StatusEnum.Empty;
                        }
                        else if (unit.Value.Minimum != 0 &&
                                 unit.Value.Minimum >= unit.Value.Count)
                        {
                            unit.Value.Status = CashUnit.StatusEnum.Low;
                        }
                    }
                    else
                    {
                        if (unit.Value.MaximumCapacity != 0 &&
                            unit.Value.MaximumCapacity >= unit.Value.Count)
                        {
                            unit.Value.Status = CashUnit.StatusEnum.Full;
                        }
                        else if (unit.Value.Maximum != 0 &&
                                 unit.Value.Maximum >= unit.Value.Count)
                        {
                            unit.Value.Status = CashUnit.StatusEnum.High;
                        }
                    }
                }
            }

            if (!PersistentData.Store(typeof(CashUnit).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// Cash unit structure information of this device
        /// </summary>
        public Dictionary<string, CashUnit> CashUnits { get; set; }

        /// <summary>
        /// True when the SP process gets started and return false once the first CashUnitInfo command is handled.
        /// </summary>
        public bool FirstCashUnitInfoCommand { get; set; }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;
    }
}
