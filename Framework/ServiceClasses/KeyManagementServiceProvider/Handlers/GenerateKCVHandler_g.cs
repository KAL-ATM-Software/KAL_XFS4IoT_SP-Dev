/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GenerateKCVHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(GenerateKCVCommand))]
    public partial class GenerateKCVHandler : ICommandHandler
    {
        public GenerateKCVHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GenerateKCVHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GenerateKCVHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GenerateKCVHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var generateKCVCmd = command.IsA<GenerateKCVCommand>($"Invalid parameter in the GenerateKCV Handle method. {nameof(GenerateKCVCommand)}");
            generateKCVCmd.Header.RequestId.HasValue.IsTrue();

            IGenerateKCVEvents events = new GenerateKCVEvents(Connection, generateKCVCmd.Header.RequestId.Value);

            var result = await HandleGenerateKCV(events, generateKCVCmd, cancel);
            await Connection.SendMessageAsync(new GenerateKCVCompletion(generateKCVCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var generateKCVcommand = command.IsA<GenerateKCVCommand>();
            generateKCVcommand.Header.RequestId.HasValue.IsTrue();

            GenerateKCVCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GenerateKCVCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GenerateKCVCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GenerateKCVCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GenerateKCVCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GenerateKCVCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GenerateKCVCompletion(generateKCVcommand.Header.RequestId.Value, new GenerateKCVCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
