/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GenerateRSAKeyPairHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(GenerateRSAKeyPairCommand))]
    public partial class GenerateRSAKeyPairHandler : ICommandHandler
    {
        public GenerateRSAKeyPairHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GenerateRSAKeyPairHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GenerateRSAKeyPairHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GenerateRSAKeyPairHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var generateRSAKeyPairCmd = command.IsA<GenerateRSAKeyPairCommand>($"Invalid parameter in the GenerateRSAKeyPair Handle method. {nameof(GenerateRSAKeyPairCommand)}");
            generateRSAKeyPairCmd.Header.RequestId.HasValue.IsTrue();

            IGenerateRSAKeyPairEvents events = new GenerateRSAKeyPairEvents(Connection, generateRSAKeyPairCmd.Header.RequestId.Value);

            var result = await HandleGenerateRSAKeyPair(events, generateRSAKeyPairCmd, cancel);
            await Connection.SendMessageAsync(new GenerateRSAKeyPairCompletion(generateRSAKeyPairCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var generateRSAKeyPaircommand = command.IsA<GenerateRSAKeyPairCommand>();
            generateRSAKeyPaircommand.Header.RequestId.HasValue.IsTrue();

            GenerateRSAKeyPairCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GenerateRSAKeyPairCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GenerateRSAKeyPairCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GenerateRSAKeyPairCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GenerateRSAKeyPairCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GenerateRSAKeyPairCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GenerateRSAKeyPairCompletion(generateRSAKeyPaircommand.Header.RequestId.Value, new GenerateRSAKeyPairCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
