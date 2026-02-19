/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * CashDispenserServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CashDispenser;

namespace XFS4IoTServer
{
    public partial class CashDispenserServiceClass : ICashDispenserServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.CountCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.CountHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CountHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.DenominateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.DenominateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DenominateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.DispenseCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.DispenseHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DispenseHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.GetMixTableCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.GetMixTableHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetMixTableHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.GetMixTypesCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.GetMixTypesHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetMixTypesHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.GetPresentStatusCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.GetPresentStatusHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetPresentStatusHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.PrepareDispenseCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.PrepareDispenseHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PrepareDispenseHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.PresentCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.PresentHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PresentHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.RejectCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.RejectHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, RejectHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.SetMixTableCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.SetMixTableHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetMixTableHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashDispenser.Commands.TestCashUnitsCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashDispenser.TestCashUnitsHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, TestCashUnitsHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.Count", typeof(XFS4IoT.CashDispenser.Commands.CountCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.Denominate", typeof(XFS4IoT.CashDispenser.Commands.DenominateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.Dispense", typeof(XFS4IoT.CashDispenser.Commands.DispenseCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.GetMixTable", typeof(XFS4IoT.CashDispenser.Commands.GetMixTableCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.GetMixTypes", typeof(XFS4IoT.CashDispenser.Commands.GetMixTypesCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.GetPresentStatus", typeof(XFS4IoT.CashDispenser.Commands.GetPresentStatusCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.PrepareDispense", typeof(XFS4IoT.CashDispenser.Commands.PrepareDispenseCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.Present", typeof(XFS4IoT.CashDispenser.Commands.PresentCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.Reject", typeof(XFS4IoT.CashDispenser.Commands.RejectCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.SetMixTable", typeof(XFS4IoT.CashDispenser.Commands.SetMixTableCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashDispenser.TestCashUnits", typeof(XFS4IoT.CashDispenser.Commands.TestCashUnitsCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.Count", typeof(XFS4IoT.CashDispenser.Completions.CountCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.Denominate", typeof(XFS4IoT.CashDispenser.Completions.DenominateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.Dispense", typeof(XFS4IoT.CashDispenser.Completions.DispenseCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.GetMixTable", typeof(XFS4IoT.CashDispenser.Completions.GetMixTableCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.GetMixTypes", typeof(XFS4IoT.CashDispenser.Completions.GetMixTypesCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.GetPresentStatus", typeof(XFS4IoT.CashDispenser.Completions.GetPresentStatusCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.PrepareDispense", typeof(XFS4IoT.CashDispenser.Completions.PrepareDispenseCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.Present", typeof(XFS4IoT.CashDispenser.Completions.PresentCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.Reject", typeof(XFS4IoT.CashDispenser.Completions.RejectCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.SetMixTable", typeof(XFS4IoT.CashDispenser.Completions.SetMixTableCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashDispenser.TestCashUnits", typeof(XFS4IoT.CashDispenser.Completions.TestCashUnitsCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashDispenser.DelayedDispenseEvent", typeof(XFS4IoT.CashDispenser.Events.DelayedDispenseEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashDispenser.IncompleteDispenseEvent", typeof(XFS4IoT.CashDispenser.Events.IncompleteDispenseEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashDispenser.StartDispenseEvent", typeof(XFS4IoT.CashDispenser.Events.StartDispenseEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICashDispenserDevice Device { get => ServiceProvider.Device.IsA<ICashDispenserDevice>(); }
    }
}
