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
using XFS4IoT.Completions;
using XFS4IoT.KeyManagement;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class StartAuthenticateHandler
    {
        private async Task<StartAuthenticateCompletion.PayloadData> HandleStartAuthenticate(IStartAuthenticateEvents events, StartAuthenticateCommand startAuthenticate, CancellationToken cancel)
        {
            if (startAuthenticate.Payload.Command is null ||
                (startAuthenticate.Payload.Command.DeleteKey is null &&
                 startAuthenticate.Payload.Command.Initialization is null))
            {
                return new StartAuthenticateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
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

            return new StartAuthenticateCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.DataToSign,
                                                               result.SigningMethod switch
                                                               {
                                                                   AuthenticationData.SigningMethodEnum.CA => AuthenticationMethodEnum.Ca,
                                                                   AuthenticationData.SigningMethodEnum.CBCMAC => AuthenticationMethodEnum.Cbcmac,
                                                                   AuthenticationData.SigningMethodEnum.CertHost => AuthenticationMethodEnum.Certhost,
                                                                   AuthenticationData.SigningMethodEnum.CMAC => AuthenticationMethodEnum.Cmac,
                                                                   AuthenticationData.SigningMethodEnum.HL => AuthenticationMethodEnum.Hl,
                                                                   AuthenticationData.SigningMethodEnum.SigHost => AuthenticationMethodEnum.SigHost,
                                                                   AuthenticationData.SigningMethodEnum.Reserved1 => AuthenticationMethodEnum.Reserved1,
                                                                   AuthenticationData.SigningMethodEnum.Reserved2 => AuthenticationMethodEnum.Reserved2,
                                                                   AuthenticationData.SigningMethodEnum.Reserved3 => AuthenticationMethodEnum.Reserved3,
                                                                   _ => AuthenticationMethodEnum.None,
                                                               });
        }
    }
}
