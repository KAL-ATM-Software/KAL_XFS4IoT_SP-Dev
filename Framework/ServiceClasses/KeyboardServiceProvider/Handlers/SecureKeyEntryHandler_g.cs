/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * SecureKeyEntryHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Keyboard
{
    [CommandHandler(XFSConstants.ServiceClass.Keyboard, typeof(SecureKeyEntryCommand))]
    public partial class SecureKeyEntryHandler : ICommandHandler
    {
        public SecureKeyEntryHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SecureKeyEntryHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SecureKeyEntryHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyboardDevice>();

            Keyboard = Provider.IsA<IKeyboardServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SecureKeyEntryHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var secureKeyEntryCmd = command.IsA<SecureKeyEntryCommand>($"Invalid parameter in the SecureKeyEntry Handle method. {nameof(SecureKeyEntryCommand)}");
            secureKeyEntryCmd.Header.RequestId.HasValue.IsTrue();

            ISecureKeyEntryEvents events = new SecureKeyEntryEvents(Connection, secureKeyEntryCmd.Header.RequestId.Value);

            var result = await HandleSecureKeyEntry(events, secureKeyEntryCmd, cancel);
            await Connection.SendMessageAsync(new SecureKeyEntryCompletion(secureKeyEntryCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var secureKeyEntrycommand = command.IsA<SecureKeyEntryCommand>();
            secureKeyEntrycommand.Header.RequestId.HasValue.IsTrue();

            SecureKeyEntryCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SecureKeyEntryCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SecureKeyEntryCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SecureKeyEntryCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SecureKeyEntryCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SecureKeyEntryCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SecureKeyEntryCompletion(secureKeyEntrycommand.Header.RequestId.Value, new SecureKeyEntryCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyboardDevice Device { get => Provider.Device.IsA<IKeyboardDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyboardServiceClass Keyboard { get; }
        private ILogger Logger { get; }
    }

}
