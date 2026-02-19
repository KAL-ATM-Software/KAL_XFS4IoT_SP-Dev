/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Lights interface.
 * LightsServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Lights;

namespace XFS4IoTServer
{
    public partial class LightsServiceClass : ILightsServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Lights.Commands.SetLightCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Lights.SetLightHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetLightHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Lights.SetLight", typeof(XFS4IoT.Lights.Commands.SetLightCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Lights.SetLight", typeof(XFS4IoT.Lights.Completions.SetLightCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ILightsDevice Device { get => ServiceProvider.Device.IsA<ILightsDevice>(); }
    }
}
