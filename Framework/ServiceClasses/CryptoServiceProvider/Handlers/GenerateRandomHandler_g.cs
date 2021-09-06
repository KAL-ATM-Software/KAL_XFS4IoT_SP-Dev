/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * GenerateRandomHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Crypto
{
    [CommandHandler(XFSConstants.ServiceClass.Crypto, typeof(GenerateRandomCommand))]
    public partial class GenerateRandomHandler : ICommandHandler
    {
        public GenerateRandomHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GenerateRandomHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GenerateRandomHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICryptoDevice>();

            Crypto = Provider.IsA<ICryptoServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GenerateRandomHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var generateRandomCmd = command.IsA<GenerateRandomCommand>($"Invalid parameter in the GenerateRandom Handle method. {nameof(GenerateRandomCommand)}");
            generateRandomCmd.Header.RequestId.HasValue.IsTrue();

            IGenerateRandomEvents events = new GenerateRandomEvents(Connection, generateRandomCmd.Header.RequestId.Value);

            var result = await HandleGenerateRandom(events, generateRandomCmd, cancel);
            await Connection.SendMessageAsync(new GenerateRandomCompletion(generateRandomCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var generateRandomcommand = command.IsA<GenerateRandomCommand>();
            generateRandomcommand.Header.RequestId.HasValue.IsTrue();

            GenerateRandomCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GenerateRandomCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GenerateRandomCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => GenerateRandomCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => GenerateRandomCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GenerateRandomCompletion(generateRandomcommand.Header.RequestId.Value, new GenerateRandomCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICryptoDevice Device { get => Provider.Device.IsA<ICryptoDevice>(); }
        private IServiceProvider Provider { get; }
        private ICryptoServiceClass Crypto { get; }
        private ILogger Logger { get; }
    }

}
