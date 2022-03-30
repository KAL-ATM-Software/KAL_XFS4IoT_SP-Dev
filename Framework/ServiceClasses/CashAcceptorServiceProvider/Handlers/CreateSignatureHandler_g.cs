/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CreateSignatureHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashAcceptor
{
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(CreateSignatureCommand))]
    public partial class CreateSignatureHandler : ICommandHandler
    {
        public CreateSignatureHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CreateSignatureHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CreateSignatureHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CreateSignatureHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CreateSignatureHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var createSignatureCmd = command.IsA<CreateSignatureCommand>($"Invalid parameter in the CreateSignature Handle method. {nameof(CreateSignatureCommand)}");
            createSignatureCmd.Header.RequestId.HasValue.IsTrue();

            ICreateSignatureEvents events = new CreateSignatureEvents(Connection, createSignatureCmd.Header.RequestId.Value);

            var result = await HandleCreateSignature(events, createSignatureCmd, cancel);
            await Connection.SendMessageAsync(new CreateSignatureCompletion(createSignatureCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var createSignaturecommand = command.IsA<CreateSignatureCommand>();
            createSignaturecommand.Header.RequestId.HasValue.IsTrue();

            CreateSignatureCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CreateSignatureCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => CreateSignatureCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CreateSignatureCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CreateSignatureCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CreateSignatureCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CreateSignatureCompletion(createSignaturecommand.Header.RequestId.Value, new CreateSignatureCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashAcceptorDevice Device { get => Provider.Device.IsA<ICashAcceptorDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashAcceptorService CashAcceptor { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
