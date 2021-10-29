/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Lights interface.
 * ILightsDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of lights. 
namespace XFS4IoTFramework.Lights
{
    public interface ILightsDevice : IDevice
    {

        /// <summary>
        /// This command is used to set the status of a light.      For guidelights, the slow and medium flash rates must not be greater than 2.0 Hz. It should be noted that in order to comply with American Disabilities Act guidelines only a slow or medium flash rate must be used.
        /// </summary>
        Task<XFS4IoT.Lights.Completions.SetLightCompletion.PayloadData> SetLight(ISetLightEvents events, 
                                                                                 XFS4IoT.Lights.Commands.SetLightCommand.PayloadData payload, 
                                                                                 CancellationToken cancellation);

    }
}
