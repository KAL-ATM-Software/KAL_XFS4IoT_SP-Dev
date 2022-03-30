/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * TakePictureHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Camera.Commands;
using XFS4IoT.Camera.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Camera
{
    public partial class TakePictureHandler
    {

        private async Task<TakePictureCompletion.PayloadData> HandleTakePicture(ITakePictureEvents events, TakePictureCommand takePicture, CancellationToken cancel)
        {

            Logger.Log(Constants.DeviceClass, "CameraDev.TakePictureAsync()");
            var result = await Device.TakePictureAsync(new TakePictureRequest(CameraCapabilitiesClass.CameraEnum.Person), cancel);
            Logger.Log(Constants.DeviceClass, $"CameraDev.TakePictureAsync() -> {result.CompletionCode}");


            //ToDo: Return picture data.
            return new TakePictureCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription);
        }

    }
}
