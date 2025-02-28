/***********************************************************************************************\
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
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.PinPad;

namespace XFS4IoTServer
{
    public partial class PinPadServiceClass
    {
        public PinPadServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(PinPadServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IPinPadDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(PinPadServiceClass)}");
            KeyManagementService = ServiceProvider.IsA<IKeyManagementService>($"Invalid interface parameter specified for key management service. {nameof(PinPadServiceClass)}");

            Logger.Log(Constants.DeviceClass, "PinPadDev.GetPCIPTSDeviceId()");

            PCIPTSDeviceIdClass deviceId = Device.GetPCIPTSDeviceId();

            string log = "No information";
            if (deviceId is not null)
            {
                log = $"{nameof(deviceId.FirmwareIdentifier)}={deviceId.FirmwareIdentifier}," +
                      $"{nameof(deviceId.ApplicationIdentifier)}={deviceId.ApplicationIdentifier}," +
                      $"{nameof(deviceId.HardwareIdentifier)}={deviceId.HardwareIdentifier}," +
                      $"{nameof(deviceId.ManufacturerIdentifier)}={deviceId.ManufacturerIdentifier}," +
                      $"{nameof(deviceId.ModelIdentifier)}={deviceId.ModelIdentifier}";
            }
            Logger.Log(Constants.DeviceClass, "PinPadDev.GetPCIPTSDeviceId()-> " + log);

            PCIPTSDeviceId = deviceId;

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
        /// List of PCI Security Standards Council PIN transaction security (PTS) certification held by the PIN device
        /// </summary>
        public PCIPTSDeviceIdClass PCIPTSDeviceId { get; set; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "PinPadDev.PinPadCapabilities");
            CommonService.PinPadCapabilities = Device.PinPadCapabilities;
            Logger.Log(Constants.DeviceClass, "PinPadDev.PinPadCapabilities=");

            CommonService.PinPadCapabilities.IsNotNull($"The device class set PinPadCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
