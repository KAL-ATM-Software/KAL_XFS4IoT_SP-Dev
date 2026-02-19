/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyboardServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Keyboard;

namespace XFS4IoTServer
{
    public partial class KeyboardServiceClass : IKeyboardServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.DataEntryCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.DataEntryHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DataEntryHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.DefineLayoutCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.DefineLayoutHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DefineLayoutHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.GetLayoutCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.GetLayoutHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetLayoutHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.KeypressBeepCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.KeypressBeepHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, KeypressBeepHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.PinEntryCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.PinEntryHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PinEntryHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Keyboard.Commands.SecureKeyEntryCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Keyboard.SecureKeyEntryHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SecureKeyEntryHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.DataEntry", typeof(XFS4IoT.Keyboard.Commands.DataEntryCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.DefineLayout", typeof(XFS4IoT.Keyboard.Commands.DefineLayoutCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.GetLayout", typeof(XFS4IoT.Keyboard.Commands.GetLayoutCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.KeypressBeep", typeof(XFS4IoT.Keyboard.Commands.KeypressBeepCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.PinEntry", typeof(XFS4IoT.Keyboard.Commands.PinEntryCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.Reset", typeof(XFS4IoT.Keyboard.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Keyboard.SecureKeyEntry", typeof(XFS4IoT.Keyboard.Commands.SecureKeyEntryCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.DataEntry", typeof(XFS4IoT.Keyboard.Completions.DataEntryCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.DefineLayout", typeof(XFS4IoT.Keyboard.Completions.DefineLayoutCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.GetLayout", typeof(XFS4IoT.Keyboard.Completions.GetLayoutCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.KeypressBeep", typeof(XFS4IoT.Keyboard.Completions.KeypressBeepCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.PinEntry", typeof(XFS4IoT.Keyboard.Completions.PinEntryCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.Reset", typeof(XFS4IoT.Keyboard.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Keyboard.SecureKeyEntry", typeof(XFS4IoT.Keyboard.Completions.SecureKeyEntryCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Keyboard.EnterDataEvent", typeof(XFS4IoT.Keyboard.Events.EnterDataEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Keyboard.KeyEvent", typeof(XFS4IoT.Keyboard.Events.KeyEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Keyboard.LayoutEvent", typeof(XFS4IoT.Keyboard.Events.LayoutEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IKeyboardDevice Device { get => ServiceProvider.Device.IsA<IKeyboardDevice>(); }
    }
}
