/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * ICameraDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

// KAL specific implementation of camera. 
namespace XFS4IoTFramework.Camera
{
    public interface ICameraDevice : IDevice
    {

        /// <summary>
        /// This command is used to start the recording of the camera system. It is possible to select which camera or which camera position should be used to take a picture. Data to be displayed on the photo can be specified using the *camData* property.
        /// </summary>
        Task<TakePictureResponse> TakePictureAsync(TakePictureRequest request, CancellationToken cancellation);

        /// <summary>
        /// This command is used by the client to perform a hardware reset which will attempt to return the camera device to a known good state.
        /// </summary>
        Task<DeviceResult> ResetDeviceAsync(CancellationToken cancellation);

        /// <summary>
        /// Camera Status
        /// </summary>
        CameraStatusClass CameraStatus { get; set; }

        /// <summary>
        /// Camera Capabilities
        /// </summary>
        CameraCapabilitiesClass CameraCapabilities { get; set; }

    }
}
