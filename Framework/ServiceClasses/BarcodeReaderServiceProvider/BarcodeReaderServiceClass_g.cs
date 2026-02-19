/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BarcodeReader interface.
 * BarcodeReaderServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.BarcodeReader;

namespace XFS4IoTServer
{
    public partial class BarcodeReaderServiceClass : IBarcodeReaderServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.BarcodeReader.Commands.ReadCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.BarcodeReader.ReadHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.BarcodeReader.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.BarcodeReader.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "BarcodeReader.Read", typeof(XFS4IoT.BarcodeReader.Commands.ReadCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "BarcodeReader.Reset", typeof(XFS4IoT.BarcodeReader.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "BarcodeReader.Read", typeof(XFS4IoT.BarcodeReader.Completions.ReadCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "BarcodeReader.Reset", typeof(XFS4IoT.BarcodeReader.Completions.ResetCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IBarcodeReaderDevice Device { get => ServiceProvider.Device.IsA<IBarcodeReaderDevice>(); }
    }
}
