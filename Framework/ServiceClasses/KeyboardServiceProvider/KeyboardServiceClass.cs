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
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    public partial class KeyboardServiceClass
    {
        public KeyboardServiceClass(IServiceProvider ServiceProvider,
                                    IKeyManagementServiceClass KeyManagement,
                                    ICommonService CommonService,
                                    ILogger logger)
        : this(ServiceProvider, logger)
        {
            this.KeyManagementService = KeyManagement.IsNotNull($"Unexpected parameter set in the " + nameof(KeyboardServiceClass));
            this.CommonService = CommonService.IsNotNull($"Unexpected parameter set in the " + nameof(KeyboardServiceClass));
            SupportedFunctionKeys = new();
            SupportedFunctionKeysWithShift = new();

            Logger.Log(Constants.DeviceClass, "KeyboardDev.GetLayoutInfo()");

            KeyboardLayouts = Device.GetLayoutInfo();

            Logger.Log(Constants.DeviceClass, "KeyboardDev.GetLayoutInfo()-> " + KeyboardLayouts is null || KeyboardLayouts.Count == 0 ? "0" : KeyboardLayouts.Count.ToString());

            KeyboardLayouts.IsNotNull($"The device class must provide keyboard layout information through GetLayoutInfo method call.");

            foreach (var entryType in KeyboardLayouts)
            {
                List<string> keys = new();
                List<string> shiftKeys = new();

                foreach (var frame in entryType.Value)
                {
                    foreach (var key in frame.FunctionKeys)
                    {
                        if (!string.IsNullOrEmpty(key.Key))
                            keys.Add(key.Key);
                        if (!string.IsNullOrEmpty(key.ShiftKey))
                            shiftKeys.Add(key.ShiftKey);
                    }
                }

                if (keys is not null && keys.Count != 0)
                {
                    SupportedFunctionKeys.Add(entryType.Key, keys);
                }
                if (shiftKeys is not null && shiftKeys.Count != 0)
                {
                    SupportedFunctionKeysWithShift.Add(entryType.Key, shiftKeys);
                }
            }
        }

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyboardCapabilitiesClass KeyboardCapabilitiesCapabilities { get => CommonService.KeyboardCapabilities; set => CommonService.KeyboardCapabilities = value; }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// KeyManagement service interface
        /// </summary>
        private IKeyManagementServiceClass KeyManagementService { get; init; }

        /// <summary>
        /// Function keys device supported
        /// </summary>
        public Dictionary<EntryModeEnum, List<string>> SupportedFunctionKeys { get; set; }

        /// <summary>
        /// Function keys device supported with shift key
        /// </summary>
        public Dictionary<EntryModeEnum, List<string>> SupportedFunctionKeysWithShift { get; set; }

        /// <summary>
        /// Keyboard layout device supported
        /// </summary>
        public Dictionary<EntryModeEnum, List<FrameClass>> KeyboardLayouts { get; set; }

        /// <summary>
        /// Return secure key entry component status
        /// </summary>
        public SecureKeyEntryStatusClass GetSecureKeyEntryStatus() => KeyManagementService.GetSecureKeyEntryStatus();
    }
}
