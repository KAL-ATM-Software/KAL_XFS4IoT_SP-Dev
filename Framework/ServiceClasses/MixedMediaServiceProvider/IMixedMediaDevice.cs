/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT MixedMedia interface.
 * IMixedMediaDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of mixedmedia. 
namespace XFS4IoTFramework.MixedMedia
{
    public interface IMixedMediaDevice : IDevice
    {

        /// <summary>
        /// $ref: ../Docs/SetModeDescription.md
        /// </summary>
        Task<XFS4IoT.MixedMedia.Completions.SetModeCompletion.PayloadData> SetMode(ISetModeEvents events, 
                                                                                   XFS4IoT.MixedMedia.Commands.SetModeCommand.PayloadData payload, 
                                                                                   CancellationToken cancellation);

    }
}
