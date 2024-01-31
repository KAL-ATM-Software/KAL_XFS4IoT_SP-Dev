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

            if (takePicture.Payload is null)
                return new TakePictureCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "No Payload supplied for TakePicture command.");

            if (takePicture.Payload.Camera is null)
                return new TakePictureCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "No Camera supplied for TakePicture command.");

            CameraCapabilitiesClass.CameraEnum Camera = takePicture.Payload.Camera switch
            {
                TakePictureCommand.PayloadData.CameraEnum.Room => CameraCapabilitiesClass.CameraEnum.Room,
                TakePictureCommand.PayloadData.CameraEnum.Person => CameraCapabilitiesClass.CameraEnum.Person,
                TakePictureCommand.PayloadData.CameraEnum.ExitSlot => CameraCapabilitiesClass.CameraEnum.ExitSlot,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected Camera supplied for TakePicture command. {takePicture.Payload.Camera}"),
            };

            string CamData = null;

            if (takePicture.Payload.CamData is not null)
            {
                if (!Common.CameraCapabilities.CamData.HasFlag(CameraCapabilitiesClass.CamDataMethodsEnum.ManualAdd))
                    return new TakePictureCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "CamData specified when device does not support ManualAdd CamData.");

                CamData = takePicture.Payload.CamData;

                if(Common.CameraCapabilities.MaxDataLength.HasValue && CamData.Length > Common.CameraCapabilities.MaxDataLength)
                {
                    // We send InvalidDataEvent so the client knows we are altering the CamData.
                    // But we continue to take the picture anyway
                    await events.InvalidDataEvent();

                    CamData = CamData[..Common.CameraCapabilities.MaxDataLength.Value];
                }
            }

            Logger.Log(Constants.DeviceClass, "CameraDev.TakePictureAsync()");
            var result = await Device.TakePictureAsync(new TakePictureRequest(Camera, null, CamData), cancel);
            Logger.Log(Constants.DeviceClass, $"CameraDev.TakePictureAsync() -> {result.CompletionCode}");


            return new TakePictureCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription,
                                                         result.ErrorCode,
                                                         result.PictureData);
        }

    }
}
