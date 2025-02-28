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

// KAL specific implementation of lights. 
namespace XFS4IoTFramework.Lights
{
    public interface ILightsDevice : IDevice
    {

        /// <summary>
        /// This command is used to set the status of a light.
        /// For guidelights, the slow and medium flash rates must not be greater than 2.0 Hz. 
        /// It should be noted that in order to comply with American Disabilities Act guidelines only a slow or medium flash rate must be used.
        /// </summary>
        Task<SetLightResult> SetLightAsync(SetLightRequest request, CancellationToken cancellation);

        /// <summary>
        /// CardReader Capabilities
        /// </summary>
        LightsCapabilitiesClass LightsCapabilities { get; set; }

        /// <summary>
        /// Storas light status
        /// </summary>
        LightsStatusClass LightsStatus { get; set; }
    }
}
