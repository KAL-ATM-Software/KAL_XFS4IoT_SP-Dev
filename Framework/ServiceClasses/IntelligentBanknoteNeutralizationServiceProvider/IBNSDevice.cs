/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT.Commands;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

namespace XFS4IoTFramework.IntelligentBanknoteNeutralization
{
    public interface IIntelligentBanknoteNeutralizationDevice : IDevice
    {
        /// <summary>
        /// This operation requests to activate (arming) or deactivates (disarming) the banknote protection. 
        /// The process of arming and disarming the banknote neutralization may be protected by end-to-end security.
        /// This means that the hardware will generate a nonce through GetCommandNonce common interface and
        /// the service application should create a security token that authorizes to set protection.
        /// </summary>
        Task<SetProtectionResult> SetProtection(SetProtectionRequest request, CancellationToken cancellation);

        /// <summary>
        /// This operation requests to activate the neutralization of the banknotes. 
        /// The process of triggering the banknote neutralization may be protected by end-to-end security. 
        /// This means that the hardware will generate a command nonce returned by GetCommandNonce common interface
        /// and the server application should  create a security token that authorizes trigering a neutralization.
        /// </summary>
        Task<TriggerNeutralizationResult> TriggerNeutralization(TriggerNeutralizationRequest request, CancellationToken cancellation);


        /// <summary>
        /// IBNS Status
        /// </summary>
        IBNSStatusClass IBNSStatus { get; set; }

        /// <summary>
        /// IBNS Capabilities
        /// </summary>
        IBNSCapabilitiesClass IBNSCapabilities { get; set; }

    }
}
