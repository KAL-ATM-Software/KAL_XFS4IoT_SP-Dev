/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class StartAuthenticateHandler
    {
        private async Task<CommandResult<StartAuthenticateCompletion.PayloadData>> HandleStartAuthenticate(IStartAuthenticateEvents events, StartAuthenticateCommand startAuthenticate, CancellationToken cancel)
        {
            if (startAuthenticate.Payload.Command is null ||
                (startAuthenticate.Payload.Command.DeleteKey is null &&
                 startAuthenticate.Payload.Command.Initialization is null))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No command information specified.");
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.StartAuthenticate()");

            StartAuthenticateRequest request = null;

            if (startAuthenticate.Payload.Command.Initialization is not null)
            {
                request = new StartAuthenticateRequest(StartAuthenticateRequest.CommandEnum.Initialization,
                                                       new StartAuthenticateRequest.InitializationInput());
            }
            else
            {
                request = new StartAuthenticateRequest(StartAuthenticateRequest.CommandEnum.Deletekey,
                                                       new StartAuthenticateRequest.DeleteKeyInput(startAuthenticate.Payload.Command.DeleteKey.Key));
            }

            var result = await Device.StartAuthenticate(request, cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.StartAuthenticate() -> {result.CompletionCode}");

            StartAuthenticateCompletion.PayloadData payload = null;
            if (result.DataToSign?.Count > 0)
            {
                payload = new(
                    DataToSign: result.DataToSign,
                    Signers: result.SigningMethod == AuthenticationData.SigningMethodEnum.None ?
                        null :
                        new(CertHost: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.CertHost),
                            SigHost: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.SigHost),
                            Ca: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.CA),
                            Hl: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.HL),
                            Cbcmac: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.CBCMAC),
                            Cmac: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.CMAC),
                            CertHostTr34: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.CertHost_TR34),
                            CaTr34: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.CA_TR34),
                            HlTr34: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.HL_TR34),
                            Reserved1: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.Reserved1),
                            Reserved2: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.Reserved2),
                            Reserved3: result.SigningMethod.HasFlag(AuthenticationData.SigningMethodEnum.Reserved3)
                            ));
            }

            return new(
                payload,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
