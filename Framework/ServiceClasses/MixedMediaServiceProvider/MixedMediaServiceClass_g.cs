/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT MixedMedia interface.
 * MixedMediaServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.MixedMedia;

namespace XFS4IoTServer
{
    public partial class MixedMediaServiceClass : IMixedMediaServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.MixedMedia.Commands.SetModeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.MixedMedia.SetModeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetModeHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "MixedMedia.SetMode", typeof(XFS4IoT.MixedMedia.Commands.SetModeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "MixedMedia.SetMode", typeof(XFS4IoT.MixedMedia.Completions.SetModeCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IMixedMediaDevice Device { get => ServiceProvider.Device.IsA<IMixedMediaDevice>(); }
    }
}
