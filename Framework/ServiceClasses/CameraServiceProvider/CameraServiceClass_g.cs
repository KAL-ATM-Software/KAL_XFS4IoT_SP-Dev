/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * CameraServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Camera;

namespace XFS4IoTServer
{
    public partial class CameraServiceClass : ICameraServiceClass
    {

        public async Task MediaThresholdEvent(XFS4IoT.Camera.Events.MediaThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Camera.Events.MediaThresholdEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Camera.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Camera.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Camera.Commands.TakePictureCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Camera.TakePictureHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, TakePictureHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Camera.Reset", typeof(XFS4IoT.Camera.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Camera.TakePicture", typeof(XFS4IoT.Camera.Commands.TakePictureCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Camera.Reset", typeof(XFS4IoT.Camera.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Camera.TakePicture", typeof(XFS4IoT.Camera.Completions.TakePictureCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Camera.InvalidDataEvent", typeof(XFS4IoT.Camera.Events.InvalidDataEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Camera.MediaThresholdEvent", typeof(XFS4IoT.Camera.Events.MediaThresholdEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICameraDevice Device { get => ServiceProvider.Device.IsA<ICameraDevice>(); }
    }
}
