/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * TextTerminalServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.TextTerminal;

namespace XFS4IoTServer
{
    public partial class TextTerminalServiceClass : ITextTerminalServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.BeepCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.BeepHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, BeepHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.ClearScreenCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.ClearScreenHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ClearScreenHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.DefineKeysCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.DefineKeysHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DefineKeysHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.GetFormListCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.GetFormListHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetFormListHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.GetKeyDetailCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.GetKeyDetailHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetKeyDetailHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.GetQueryFieldCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.GetQueryFieldHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetQueryFieldHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.GetQueryFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.GetQueryFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetQueryFormHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.ReadCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.ReadHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.ReadFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.ReadFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadFormHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.SetFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.SetFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetFormHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.SetResolutionCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.SetResolutionHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetResolutionHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.WriteCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.WriteHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, WriteHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.TextTerminal.Commands.WriteFormCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.TextTerminal.WriteFormHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, WriteFormHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.Beep", typeof(XFS4IoT.TextTerminal.Commands.BeepCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.ClearScreen", typeof(XFS4IoT.TextTerminal.Commands.ClearScreenCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.DefineKeys", typeof(XFS4IoT.TextTerminal.Commands.DefineKeysCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.GetFormList", typeof(XFS4IoT.TextTerminal.Commands.GetFormListCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.GetKeyDetail", typeof(XFS4IoT.TextTerminal.Commands.GetKeyDetailCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.GetQueryField", typeof(XFS4IoT.TextTerminal.Commands.GetQueryFieldCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.GetQueryForm", typeof(XFS4IoT.TextTerminal.Commands.GetQueryFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.Read", typeof(XFS4IoT.TextTerminal.Commands.ReadCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.ReadForm", typeof(XFS4IoT.TextTerminal.Commands.ReadFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.Reset", typeof(XFS4IoT.TextTerminal.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.SetForm", typeof(XFS4IoT.TextTerminal.Commands.SetFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.SetResolution", typeof(XFS4IoT.TextTerminal.Commands.SetResolutionCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.Write", typeof(XFS4IoT.TextTerminal.Commands.WriteCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "TextTerminal.WriteForm", typeof(XFS4IoT.TextTerminal.Commands.WriteFormCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.Beep", typeof(XFS4IoT.TextTerminal.Completions.BeepCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.ClearScreen", typeof(XFS4IoT.TextTerminal.Completions.ClearScreenCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.DefineKeys", typeof(XFS4IoT.TextTerminal.Completions.DefineKeysCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.GetFormList", typeof(XFS4IoT.TextTerminal.Completions.GetFormListCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.GetKeyDetail", typeof(XFS4IoT.TextTerminal.Completions.GetKeyDetailCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.GetQueryField", typeof(XFS4IoT.TextTerminal.Completions.GetQueryFieldCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.GetQueryForm", typeof(XFS4IoT.TextTerminal.Completions.GetQueryFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.Read", typeof(XFS4IoT.TextTerminal.Completions.ReadCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.ReadForm", typeof(XFS4IoT.TextTerminal.Completions.ReadFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.Reset", typeof(XFS4IoT.TextTerminal.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.SetForm", typeof(XFS4IoT.TextTerminal.Completions.SetFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.SetResolution", typeof(XFS4IoT.TextTerminal.Completions.SetResolutionCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.Write", typeof(XFS4IoT.TextTerminal.Completions.WriteCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "TextTerminal.WriteForm", typeof(XFS4IoT.TextTerminal.Completions.WriteFormCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "TextTerminal.FieldErrorEvent", typeof(XFS4IoT.TextTerminal.Events.FieldErrorEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "TextTerminal.FieldWarningEvent", typeof(XFS4IoT.TextTerminal.Events.FieldWarningEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "TextTerminal.KeyEvent", typeof(XFS4IoT.TextTerminal.Events.KeyEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ITextTerminalDevice Device { get => ServiceProvider.Device.IsA<ITextTerminalDevice>(); }
    }
}
