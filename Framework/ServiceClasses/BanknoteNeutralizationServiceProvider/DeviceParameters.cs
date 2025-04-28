/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.BanknoteNeutralization.Completions;

namespace XFS4IoTFramework.BanknoteNeutralization
{
    /// The classes used by the device interface for an Input/Output parameters

    /// <summary>
    /// SetProtectionRequest
    /// Activates (arming) or Deactivates (disarming) the banknote protection.
    /// </summary>
    public sealed class SetProtectionRequest(
        SetProtectionRequest.ProtectionEnum Protection,
        string E2EToken)
    {
        public enum ProtectionEnum
        {
            Arm, //Activate the normal operating mode of the banknote neutralization. The banknote neutralization autonomously activates the protection when the safe door is closed and locked and deactivates it when the safe is legally opened. 
            IgnoreAllSafeSensors, //Permanently deactivate all the safe sensors while the banknote neutralization of the Storage Units remain armed.
            Disarm, //deactivate the whole banknote neutralization including the safe intrusion detection and the banknote neutralization in the Storage Units.
        }

        /// <summary>
        /// Activates (arming) or Deactivates (disarming) the banknote protection.
        /// </summary>
        public ProtectionEnum Protection { get; init; } = Protection;

        /// <summary>
        /// The token requires for authorizing the operation
        /// </summary>
        public string E2EToken { get; init; } = E2EToken;
    }

    /// <summary>
    /// SetProtectionResult
    /// Return result of changing protection status.
    /// </summary>
    public sealed class SetProtectionResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        SetProtectionCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {
        public enum ErrorCodeEnum
        {
            SensorNotReady
        }

        /// <summary>
        /// Specifies the error code on changing protection status.
        /// </summary>
        public SetProtectionCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// TriggerNeutralizationRequest
    /// Activates the neutralization of the banknotes.
    /// </summary>
    public sealed class TriggerNeutralizationRequest(
        TriggerNeutralizationRequest.TriggerEnum Trigger,
        string E2EToken)
    {
        public enum TriggerEnum
        {
            Trigger, //trigger the banknote neutralization
        }

        /// <summary>
        /// Activates (arming) or Deactivates (disarming) the banknote protection.
        /// </summary>
        public TriggerEnum Trigger { get; init; } = Trigger;

        /// <summary>
        /// The token requires for authorizing the operation
        /// </summary>
        public string E2EToken { get; init; } = E2EToken;
    }

    /// <summary>
    /// TriggerNeutralizationResult
    /// Return result of activates the neutralization of the banknotes
    /// </summary>
    public sealed class TriggerNeutralizationResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null) : DeviceResult(CompletionCode, ErrorDescription)
    {
    }
}
