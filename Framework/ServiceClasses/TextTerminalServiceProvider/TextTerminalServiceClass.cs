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
            this.CommonService = CommonService.IsNotNull($"Unexpected parameter set in the " + nameof(TextTerminalServiceClass));
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores TexTerminal interface capabilites internally
        /// </summary>
        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get => CommonService.TextTerminalCapabilities; set => CommonService.TextTerminalCapabilities = value; }

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
                List<string> SupportedCommandKeys = new();
                foreach (XFS4IoT.TextTerminal.Completions.GetKeyDetailCompletion.PayloadData.CommandKeysClass keys in result.CommandKeys)
                {
                    // Get the key name from the propertyName in the object.
                    foreach (var prop in typeof(XFS4IoT.TextTerminal.Completions.GetKeyDetailCompletion.PayloadData.CommandKeysClass).GetProperties())
                    {
                        var val = prop.GetValue(keys);
                        if (val is not null && (bool)val) SupportedCommandKeys.Add(Char.ToLower(prop.Name[0]) + prop.Name[1..]);
                    }
                }
                // Store the Keys and CommandKeys. Keep CommandKeysClass to respond to GetKeyDetail request.
                SupportedKeys = new(result.Keys, SupportedCommandKeys, result.CommandKeys != null && result.CommandKeys.Count > 0 ? result.CommandKeys[0] : new());
            }
        }
    }
}
