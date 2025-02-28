/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFS4IoT;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.TextTerminal;
using System.ComponentModel;

namespace XFS4IoTServer
{
    public partial class TextTerminalServiceClass
    {
        public TextTerminalServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(TextTerminalServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ITextTerminalDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(TextTerminalServiceClass)}");

            GetStatus();
            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// True when the SP process gets started and return false once the first GetKeyDetail command is handled.
        /// </summary>
        public bool FirstGetKeyDetailCommand { get; set; } = true;

        /// <summary>
        /// Keys supported by the TextTerminal device. Will be filled by the first GetKeyDetail command.
        /// </summary>
        public ITextTerminalService.KeyDetails SupportedKeys { get; set; }

        /// <summary>
        /// Update the SupportedKeys from the device through the GetKeyDetail() method.
        /// </summary>
        /// <returns></returns>
        public void UpdateKeyDetails()
        {
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.GetKeyDetail()");

            var result = Device.GetKeyDetail();

            string keys = string.Empty;
            if (result.Keys is not null)
            {
                keys = string.Join(",", [..result.Keys]);
            }
            string commandKeys = string.Empty;
            if (result.CommandKeys is not null)
            {
                commandKeys = string.Join(",", [..result.CommandKeys.Keys]);
            }
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetKeyDetail() -> Keys:{keys}, CommandKeys:{commandKeys}");

            // Store the Keys and CommandKeys
            SupportedKeys = new(result.Keys ?? [], result.CommandKeys ?? []);
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalStatus");
            CommonService.TextTerminalStatus = Device.TextTerminalStatus;
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalStatus=");

            CommonService.TextTerminalStatus.IsNotNull($"The device class set TextTerminalStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalCapabilities");
            CommonService.TextTerminalCapabilities = Device.TextTerminalCapabilities;
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalCapabilities=");

            CommonService.TextTerminalCapabilities.IsNotNull($"The device class set TextTerminalCapabilities property to null. The device class must report device capabilities.");
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
