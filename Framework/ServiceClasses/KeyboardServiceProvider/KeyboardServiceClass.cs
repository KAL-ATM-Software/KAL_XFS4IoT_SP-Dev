﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using System.ComponentModel;

namespace XFS4IoTServer
{
    public partial class KeyboardServiceClass
    {
        public KeyboardServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(KeyboardServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IKeyboardDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(KeyboardServiceClass)}");
            KeyManagementService = ServiceProvider.IsA<IKeyManagementService>($"Invalid interface parameter specified for key management service. {nameof(KeyboardServiceClass)}");

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

            GetStatus();
            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// KeyManagement service interface
        /// </summary>
        private IKeyManagementService KeyManagementService { get; init; }

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

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "KeyboardDev.KeyboardStatus");
            CommonService.KeyboardStatus = Device.KeyboardStatus;
            Logger.Log(Constants.DeviceClass, "KeyboardDev.KeyboardStatus=");

            CommonService.KeyboardStatus.IsNotNull($"The device class set KeyboardStatus property to null. The device class must report device status.");
            CommonService.KeyboardStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "KeyboardDev.KeyboardCapabilities");
            CommonService.KeyboardCapabilities = Device.KeyboardCapabilities;
            Logger.Log(Constants.DeviceClass, "KeyboardDev.KeyboardCapabilities=");

            CommonService.KeyboardCapabilities.IsNotNull($"The device class set KeyboardCapabilities property to null. The device class must report device capabilities.");
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
