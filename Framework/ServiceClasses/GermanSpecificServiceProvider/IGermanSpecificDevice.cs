/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using System.Runtime.Intrinsics.X86;
using System.Timers;

namespace XFS4IoTFramework.GermanSpecific
{
    public interface IGermanSpecificDevice : IDevice
    {
        /// <summary>
        /// Returns the current HSM terminal data.
        /// </summary>
        Task<GetHSMTDataResponse> GetHSMTData(CancellationToken cancellation);

        /// <summary>
        /// This operation allows the application to set the HSM terminal data 
        /// (except keys, trace number and session key index). 
        /// Terminal data that is included but not supported by the hardware will be ignored.
        /// </summary>
        Task<SetHSMTDataResponse> SetHSMTData(SetHSMTDataRequest request, CancellationToken cancellation);

        /// <summary>
        /// This operation handles all messages that should be sent through secure messaging to an authorization 
        /// or personalization system. The encryption module adds the security relevant fields to the message 
        /// and returns the modified message in the output structure. 
        /// All messages must be presented to the encryptor via this operation even if they do not contain 
        /// security fields in order to keep track of the transaction status in the internal state machine.          
        /// </summary>
        Task<SecureMsgSendResponse> SecureMsgSend(SecureMsgSendRequest request, CancellationToken cancellation);

        /// <summary>
        /// This operation handles all messages that are received through a secure messaging from an authorization 
        /// or personalization system.The encryption module checks the security relevant properties. 
        /// All messages must be presented to the encryptor via this operation even if they do not contain 
        /// security relevant properties in order to keep track of the transaction status in the internal 
        /// state machine.
        /// </summary>
        Task<SecureMsgReceiveResponse> SecureMsgReceive(SecureMsgReceiveRequest request, CancellationToken cancellation);

        /// <summary>
        /// This operation is used to set the HSM out of order (Außerbetriebnahme).
        /// At the same time the online time can be set to control when the OPT online dialog
        /// shall be started to initialize the HSM again.
        /// When this time is reached an German.OPTRequiredEvent could be sent.
        /// </summary>
        Task<HSMInitResponse> HSMInit(HSMInitRequest request, CancellationToken cancellation);

        /// <summary>
        /// German specific capabilities
        /// </summary>
        public GermanSpecificCapabilitiesClass GermanSpecificCapabilities { get; set; }
    }
}
