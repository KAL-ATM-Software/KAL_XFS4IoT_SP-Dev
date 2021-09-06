/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
                                                       new StartAuthenticateRequest.InitializationInput(startAuthenticate.Payload.Command.Initialization.Key,
                                                                                                        string.IsNullOrEmpty(startAuthenticate.Payload.Command.Initialization.Ident) ? null : Convert.FromBase64String(startAuthenticate.Payload.Command.Initialization.Ident).ToList()));
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
                                                               result.DataToSign is null || result.DataToSign.Count == 0 ? null : Convert.ToBase64String(result.DataToSign.ToArray()),
                                                               result.SigningMethod switch
                                                               {
                                                                   AuthenticationData.SigningMethodEnum.CA => SigningMethodEnum.Ca,
                                                                   AuthenticationData.SigningMethodEnum.CBCMAC => SigningMethodEnum.Cbcmac,
                                                                   AuthenticationData.SigningMethodEnum.CertHost => SigningMethodEnum.Certhost,
                                                                   AuthenticationData.SigningMethodEnum.CMAC => SigningMethodEnum.Cmac,
                                                                   AuthenticationData.SigningMethodEnum.HL => SigningMethodEnum.Hl,
                                                                   AuthenticationData.SigningMethodEnum.SigHost => SigningMethodEnum.SigHost,
                                                                   AuthenticationData.SigningMethodEnum.Reserved1 => SigningMethodEnum.Reserved1,
                                                                   AuthenticationData.SigningMethodEnum.Reserved2 => SigningMethodEnum.Reserved2,
                                                                   AuthenticationData.SigningMethodEnum.Reserved3 => SigningMethodEnum.Reserved3,
                                                                   _ => SigningMethodEnum.None,
                                                               });
        }
    }
}
