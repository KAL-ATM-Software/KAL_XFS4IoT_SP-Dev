/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * GetLayoutHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Keyboard, typeof(GetLayoutCommand))]
    public partial class GetLayoutHandler : ICommandHandler
    {
        public GetLayoutHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetLayoutHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetLayoutHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyboardDevice>();

            Keyboard = Provider.IsA<IKeyboardServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetLayoutHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getLayoutCmd = command.IsA<GetLayoutCommand>($"Invalid parameter in the GetLayout Handle method. {nameof(GetLayoutCommand)}");
            getLayoutCmd.Header.RequestId.HasValue.IsTrue();

            IGetLayoutEvents events = new GetLayoutEvents(Connection, getLayoutCmd.Header.RequestId.Value);

            var result = await HandleGetLayout(events, getLayoutCmd, cancel);
            await Connection.SendMessageAsync(new GetLayoutCompletion(getLayoutCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getLayoutcommand = command.IsA<GetLayoutCommand>();
            getLayoutcommand.Header.RequestId.HasValue.IsTrue();

            GetLayoutCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetLayoutCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetLayoutCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetLayoutCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetLayoutCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetLayoutCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetLayoutCompletion(getLayoutcommand.Header.RequestId.Value, new GetLayoutCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyboardDevice Device { get => Provider.Device.IsA<IKeyboardDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyboardServiceClass Keyboard { get; }
        private ILogger Logger { get; }
    }

}
