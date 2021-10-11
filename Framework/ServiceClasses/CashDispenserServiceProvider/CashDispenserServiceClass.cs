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
            Dictionary<int, Mix> tableMixes = PersistentData.Load<Dictionary<int, Mix>>(typeof(Mix).FullName);
            if (tableMixes is not null)
            {
                // merge table mix set by the application
                foreach (var t in tableMixes)
                    AddMix(t.Key, t.Value);
            }

            this.CashManagementService = CashManagementService.IsNotNull($"Unexpected parameter set in the " + nameof(CashDispenserServiceClass));
            this.CommonService = CommonService.IsNotNull($"Unexpected parameter set in the " + nameof(CashDispenserServiceClass));

            this.Mixes = new()
            {
                { 1, new MinNumberMix(1, logger) },
                { 2, new EqualEmptyingMix(2, logger) }
            };
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICashManagementService CashManagementService { get; init; }

        /// <summary>
        /// ConstructCashUnits
        /// The method retreive cash unit structures from the device class. The device class must provide cash unit structure info
        /// </summary>
        public void ConstructCashUnits() => CashManagementService.ConstructCashUnits();
        /// <summary>
        /// UpdateCashUnitAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public void UpdateCashUnitAccounting(Dictionary<string, ItemMovement> MovementResult = null) => CashManagementService.UpdateCashUnitAccounting(MovementResult);

        /// <summary>
        /// Cash unit structure information of this device
        /// </summary>
        public Dictionary<string, CashUnit> CashUnits { get => CashManagementService.CashUnits; set => CashManagementService.CashUnits = value; }

        /// <summary>
        /// True when the SP process gets started and return false once the first CashUnitInfo command is handled.
        /// </summary>
        public bool FirstCashUnitInfoCommand { get => CashManagementService.FirstCashUnitInfoCommand; set => CashManagementService.FirstCashUnitInfoCommand = value; }

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
        /// Add vendor specific mix algorithm
        /// </summary>
        /// <param name="mixNumber"></param>
        /// <param name="mix">new mix algorithm to support for a customization</param>
        public void AddMix(int mixNumber, Mix mix)
        {
            if (Mixes.ContainsKey(mixNumber))
                Mixes.Remove(mixNumber);// replace algorithm
            Mixes.Add(mixNumber, mix);

            if (mix.Type == Mix.TypeEnum.Table)
            {
                // Save table mix set by the application
                Dictionary<int, Mix> tableMixes = PersistentData.Load<Dictionary<int, Mix>>(ServiceProvider.Name + typeof(Mix).FullName);
                if (tableMixes is null)
                    tableMixes = new();

                if (tableMixes.ContainsKey(mixNumber))
                    tableMixes.Remove(mixNumber);// Replace exiting one
                tableMixes.Add(mixNumber, mix);

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
        public Mix GetMix(int mixNumber)
        {
            if (Mixes.ContainsKey(mixNumber))
                return Mixes[mixNumber];
            return null;
        }

        public IEnumerator GetMixAlgorithms() => Mixes.Values.GetEnumerator();

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
        private readonly Dictionary<int, Mix> Mixes;
    }
}
