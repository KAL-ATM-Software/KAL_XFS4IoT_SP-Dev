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
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass
    {
        public CommonServiceClass(IServiceProvider ServiceProvider, ILogger logger, string ServiceName)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CommonServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICommonDevice>();

            GetCapabilities();
            GetStatus();
        }

        #region Device Capabilities
        /// <summary>
        /// Stores Common Capabilities
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get; set; } = null;

        /// <summary>
        /// Stores TextTerminal interface capabilites internally
        /// </summary>
        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get; set; } = null;

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get; set; } = null;

        /// <summary>
        /// Stores PinPad interface capabilites internally
        /// </summary>
        public PinPadCapabilitiesClass PinPadCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Keyboard interface capabilites internally
        /// </summary>
        public KeyboardCapabilitiesClass KeyboardCapabilities { get; set; } = null;
        #endregion

        #region Device Status
        /// <summary>
        /// Stores Commons status
        /// </summary>
        public CommonStatusClass CommonStatus { get; set; } = null;

        /// <summary>
        /// Storas CardReader status
        /// </summary>
        public CardReaderStatusClass CardReaderStatus { get; set; } = null;

        /// <summary>
        /// Storas CashDispenser status
        /// </summary>
        public CashDispenserStatusClass CashDispenserStatus { get; set; } = null;

        /// <summary>
        /// Storas CashManagement status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get; set; } = null;

        /// <summary>
        /// Storas KeyManagement status
        /// </summary>
        public KeyManagementStatusClass KeyManagementStatus { get; set; } = null;
        /// <summary>
        /// Storas Keyboard status
        /// </summary>
        public KeyboardStatusClass KeyboardStatus { get; set; } = null;

        /// <summary>
        /// Storas CardReader status
        /// </summary>
        public TextTerminalStatusClass TextTerminalStatus { get; set; } = null;

        /// <summary>
        /// Storas light status
        /// </summary>
        public LightsStatusClass LightsStatus { get; set; } = null;

        #endregion

        private void GetCapabilities()
        {
            if (CommonCapabilities is null)
            {
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonCapabilities");
                CommonCapabilities = Device.CommonCapabilities;
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonCapabilities=");

                CommonCapabilities.IsNotNull($"The device class set CommonCapabilities property to null. The device class must report device capabilities.");
            }
        }

        private void GetStatus()
        {
            if (CommonStatus is null)
            {
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonStatus");
                CommonStatus = Device.CommonStatus;
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonStatus=");

                CommonStatus.IsNotNull($"The device class set CommonStatus property to null. The device class must report device status.");
            }
        }
    }
}
