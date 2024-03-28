/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

namespace XFS4IoTFramework.MixedMedia
{
    public interface IMixedMediaDevice : IDevice
    {
        /// <summary>
        /// $ref: ../Docs/SetModeDescription.md
        /// </summary>
        Task<SetModeResult> SetModeAsync(SetModeRequest request, CancellationToken cancellation);

        /// <summary>
        /// Mixed media Status
        /// </summary>
        MixedMediaStatusClass MixedMediaStatus { get; set; }

        /// <summary>
        /// MixedMedia Capabilities
        /// </summary>
        MixedMediaCapabilitiesClass MixedMediaCapabilities { get; set; }
    }
}
