/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * IAuxiliariesDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;

// KAL specific implementation of auxiliaries. 
namespace XFS4IoTFramework.Auxiliaries
{
    public interface IAuxiliariesDevice : IDevice
    {

        /// <summary>
        /// This command is used to retrieve the current AutoStartupTime configured in the device.
        /// This will only be called if the device reports AutoStartupMode in the capabilities.
        /// </summary>
        Task<GetAutostartupTimeResult> GetAutoStartupTime(CancellationToken cancellation);

        /// <summary>
        /// This command is used to clear the time at which the machine will automatically start.
        /// This will only be called if the device reports AutoStartupMode in the capabilities.
        /// </summary>
        Task<DeviceResult> ClearAutoStartupTime(CancellationToken cancellation);

        /// <summary>
        /// This command is used to set or clear one or more device auxiliaries.
        /// </summary>
        Task<DeviceResult> SetAuxiliaries(SetAuxiliariesRequest request, CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the time at which the machine will automatically start. 
        /// It is also used to disable automatic start-up.If a new start-up time is set by this command it will replace any previously set start-up time.
        /// Before the auto start-up can take place the operating system must be shut down.
        /// This will only be called if the device reports AutoStartupMode in the capabilities.
        /// </summary>
        Task<DeviceResult> SetAutostartupTime(SetAutostartupTimeRequest autoStartupInfo, CancellationToken cancellation);


        /// <summary>
        /// Auxiliaries Capabilities
        /// </summary>
        AuxiliariesCapabilities AuxiliariesCapabilities { get; set; }

        /// <summary>
        /// Stores Auxiliaries status
        /// </summary>
        AuxiliariesStatus AuxiliariesStatus { get; set; }

    }
}
