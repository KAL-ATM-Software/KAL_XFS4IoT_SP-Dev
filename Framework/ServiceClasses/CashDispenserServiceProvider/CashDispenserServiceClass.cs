/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
                                         ILogger logger, 
                                         IPersistentData PersistentData)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashDispenserServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashDispenserDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CashDispenserServiceClass)}");
            CashManagementService = ServiceProvider.IsA<ICashManagementService>($"Invalid interface parameter specified for cash management service. {nameof(CashDispenserServiceClass)}");

            this.PersistentData = PersistentData.IsNotNull($"No persistent data interface is set. " + typeof(Mix).FullName);

            // Load persistent data
            Dictionary<string, Mix> tableMixes = PersistentData.Load<Dictionary<string, Mix>>(typeof(Mix).FullName);
            if (tableMixes is not null)
            {
                // merge table mix set by the application
                foreach (var t in tableMixes)
                    AddMix(t.Key, t.Value);
            }

            LastCashDispenserPresentStatus = PersistentData.Load<Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashDispenserPresentStatus>>(ServiceProvider.Name + typeof(CashDispenserPresentStatus).FullName);
            if (LastCashDispenserPresentStatus is null)
            {
                LastCashDispenserPresentStatus = _LastCashDispenserPresentStatus;
                StoreCashDispenserPresentStatus();
            }

            Mixes = new()
            {
                { "mix1", new MinNumberMix(logger) },
                { "mix2", new EqualEmptyingMix(logger) }
            };

            GetStatus();
            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// CashManagement service interface
        /// </summary>
        private ICashManagementService CashManagementService { get; init; }

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
        public Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashDispenserPresentStatus> LastCashDispenserPresentStatus { get; init; }

        /// <summary>
        /// Keep last present status per position
        /// </summary>
        private readonly Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashDispenserPresentStatus> _LastCashDispenserPresentStatus = new()
        {
            { CashManagementCapabilitiesClass.OutputPositionEnum.Bottom,  new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Center,  new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Default, new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Front,   new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Left,    new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Rear,    new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Right,   new CashDispenserPresentStatus() },
            { CashManagementCapabilitiesClass.OutputPositionEnum.Top,     new CashDispenserPresentStatus() }
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
            CommonService.CashDispenserStatus = Device.CashDispenserStatus;
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserStatus=");

            CommonService.CashDispenserStatus.IsNotNull($"The device class set CashDispenserStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserCapabilities");
            CommonService.CashDispenserCapabilities = Device.CashDispenserCapabilities;
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CashDispenserCapabilities=");

            CommonService.CashDispenserCapabilities.IsNotNull($"The device class set CashDispenserCapabilities property to null. The device class must report device capabilities.");
        }

        /// <summary>
        /// Store present status persistently
        /// </summary>
        public void StoreCashDispenserPresentStatus()
        {
            if (!PersistentData.Store<Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashDispenserPresentStatus>>(ServiceProvider.Name + typeof(CashDispenserPresentStatus).FullName, LastCashDispenserPresentStatus))
            {
                Logger.Warning(Constants.Framework, $"Failed to save persistent data. {ServiceProvider.Name + typeof(CashDispenserPresentStatus).FullName}");
            }
        }
    }
}
