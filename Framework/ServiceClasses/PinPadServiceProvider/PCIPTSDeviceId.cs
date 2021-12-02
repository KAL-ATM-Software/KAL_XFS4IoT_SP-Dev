using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.PinPad
{
    public sealed class PCIPTSDeviceIdClass
    {
        public PCIPTSDeviceIdClass(string ManufacturerIdentifier,
                                   string ModelIdentifier,
                                   string HardwareIdentifier,
                                   string FirmwareIdentifier,
                                   string ApplicationIdentifier)
        { 
            this.ManufacturerIdentifier = ManufacturerIdentifier;
            this.ModelIdentifier = ModelIdentifier;
            this.HardwareIdentifier = HardwareIdentifier;
            this.FirmwareIdentifier = FirmwareIdentifier;
            this.ApplicationIdentifier = ApplicationIdentifier;
        }

        /// <summary>
        /// Returns the manufacturer identifier of the PIN device.
        /// This value is not set if the manufacturer identifier is not available. 
        /// </summary>
        public string ManufacturerIdentifier { get; init; }

        /// <summary>
        /// Returns the model identifier of the PIN device. 
        /// This value is not set if the model identifier is not available.
        /// </summary>
        public string ModelIdentifier { get; init; }

        /// <summary>
        /// Returns the hardware identifier of the PIN device. 
        /// This value is not set if the hardware identifier is not available.
        /// </summary>
        public string HardwareIdentifier { get; init; }

        /// <summary>
        /// Returns the firmware identifier of the PIN device. 
        /// This value is not set if the firmware identifier is not available.
        /// </summary>
        public string FirmwareIdentifier { get; init; }

        /// <summary>
        /// Returns the application identifier of the PIN device.
        /// This value is not set if the application identifier is not available.
        /// </summary>
        public string ApplicationIdentifier { get; init; }
    }
}
