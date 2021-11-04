/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

namespace XFS4IoTServer
{
    public partial class TextTerminalServiceClass
    {
        public TextTerminalServiceClass(IServiceProvider ServiceProvider,
                                        ICommonService CommonService,
                                        ILogger logger)
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(TextTerminalServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid common interface specified in the " + nameof(TextTerminalServiceClass));

            GetStatus();
            GetCapabilities();
        }

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
        /// Stores TexTerminal interface capabilites internally
        /// </summary>
        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get => CommonService.TextTerminalCapabilities; set => CommonService.TextTerminalCapabilities = value; }

        /// <summary>
        /// Stores TexTerminal interface status internally
        /// </summary>
        public TextTerminalStatusClass TextTerminalStatus { get => CommonService.TextTerminalStatus; set => CommonService.TextTerminalStatus = value; }

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

            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetKeyDetail() -> {result.CompletionCode}");

            if(result.CompletionCode == XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success)
            {
                // Store the Keys and CommandKeys
                SupportedKeys = new(result.Keys, result.CommandKeys ?? new());
            }
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalStatus");
            TextTerminalStatus = Device.TextTerminalStatus;
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalStatus=");

            TextTerminalStatus.IsNotNull($"The device class set TextTerminalStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalCapabilities");
            TextTerminalCapabilities = Device.TextTerminalCapabilities;
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.TextTerminalCapabilities=");

            TextTerminalCapabilities.IsNotNull($"The device class set TextTerminalCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
