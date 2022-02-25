/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CompareSignatureHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(CompareSignatureCommand))]
    public partial class CompareSignatureHandler : ICommandHandler
    {
        public CompareSignatureHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CompareSignatureHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CompareSignatureHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CompareSignatureHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CompareSignatureHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var compareSignatureCmd = command.IsA<CompareSignatureCommand>($"Invalid parameter in the CompareSignature Handle method. {nameof(CompareSignatureCommand)}");
            compareSignatureCmd.Header.RequestId.HasValue.IsTrue();

            ICompareSignatureEvents events = new CompareSignatureEvents(Connection, compareSignatureCmd.Header.RequestId.Value);

            var result = await HandleCompareSignature(events, compareSignatureCmd, cancel);
            await Connection.SendMessageAsync(new CompareSignatureCompletion(compareSignatureCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var compareSignaturecommand = command.IsA<CompareSignatureCommand>();
            compareSignaturecommand.Header.RequestId.HasValue.IsTrue();

            CompareSignatureCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CompareSignatureCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => CompareSignatureCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CompareSignatureCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CompareSignatureCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CompareSignatureCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CompareSignatureCompletion(compareSignaturecommand.Header.RequestId.Value, new CompareSignatureCompletion.PayloadData(errorCode, commandException.Message));

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
