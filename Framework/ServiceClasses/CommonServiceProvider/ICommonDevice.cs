/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using System;

// KAL specific implementation of common. 
namespace XFS4IoTFramework.Common
{
    public interface ICommonDevice : IDevice
    {
        /// <summary>
        /// This method activates or deactivates the power-saving mode.If the Service Provider receives another execute command while in power saving mode, the Service Provider automatically exits the power saving mode, and executes the requested command. If the Service Provider receives an information command while in power saving mode, the Service Provider will not exit the power saving mode.
        /// </summary>
        [Obsolete("This method is no longer used by the common interface. Migrate power saving control to PowerManagement interface. this interface will be removed after version 4.")]
        Task<DeviceResult> PowerSaveControl(int MaxPowerSaveRecoveryTime, CancellationToken cancel);

        /// <summary>
        /// This method allows the application to specify the transaction state, which the Service Provider can then utilize in order to optimize performance. After receiving this command, this Service Provider can perform the necessary processing to start or end the customer transaction. This command should be called for every Service Provider that could be used in a customer transaction. The transaction state applies to every session.
        /// </summary>
        Task<DeviceResult> SetTransactionState(SetTransactionStateRequest request);

        /// <summary>
        /// This method can be used to get the transaction state.
        /// </summary>
        Task<GetTransactionStateResult> GetTransactionState();

        /// <summary>
        /// Get a nonce to be included in an Authorisation Token for a command that will be used to ensure end to end security.The hardware will overwrite any existing stored Command nonce with this new value. The value will be stored for future authentication. Any Authorisation Token received will be compared with this stored nonce and if the Token doesn't contain the same nonce it will be considered invalid and rejected, causing the command that contains that Authentication Token to fail.The nonce must match the algorithm used. For example, HMAC means the nonce must be 128 bit/16 bytes.
        /// </summary>
        Task<GetCommandNonceResult> GetCommandNonce();

        /// <summary>
        /// Clear generated nonce 
        /// </summary>
        Task<DeviceResult> ClearCommandNonce();

        /// <summary>
        /// CardReader Status
        /// </summary>
        CommonStatusClass CommonStatus { get; set; }

        /// <summary>
        /// Common Capabilities
        /// </summary>
        CommonCapabilitiesClass CommonCapabilities { get; set; }

    }
}
