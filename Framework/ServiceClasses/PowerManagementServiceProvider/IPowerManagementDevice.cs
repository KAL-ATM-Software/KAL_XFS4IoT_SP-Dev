/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

namespace XFS4IoTFramework.PowerManagement
{
    public interface IPowerManagementDevice : IDevice
    {
        /// <summary>
        /// This method activates or deactivates the power-saving mode.If the Service Provider receives another execute command while in power saving mode, the Service Provider automatically exits the power saving mode, and executes the requested command. If the Service Provider receives an information command while in power saving mode, the Service Provider will not exit the power saving mode.
        /// </summary>
        Task<DeviceResult> PowerSaveControl(int MaxPowerSaveRecoveryTime, CancellationToken cancel);

        /// <summary>
        /// Power Management Status
        /// </summary>
        PowerManagementStatusClass PowerManagementStatus { get; set; }

        /// <summary>
        /// Power Management Capabilities
        /// </summary>
        PowerManagementCapabilitiesClass PowerManagementCapabilities { get; set; }
    }
}
