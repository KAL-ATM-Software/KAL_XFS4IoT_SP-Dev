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
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.KeyManagement;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class InitializationHandler
    {
        private async Task<InitializationCompletion.PayloadData> HandleInitialization(IInitializationEvents events, InitializationCommand initialization, CancellationToken cancel)
        {
            AuthenticationData authData = null;
            if (initialization.Payload.Authentication is not null)
            {
                if (initialization.Payload.Authentication.Method is null)
                {
                    return new InitializationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"No authentication method specified.");
                }
                if (initialization.Payload.Authentication.Method != SigningMethodEnum.None)
                {
                    if (string.IsNullOrEmpty(initialization.Payload.Authentication.Data))
                    {
                        return new InitializationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"No authentication data specified.");
                    }
                    if (initialization.Payload.Authentication.Method is null)
                    {
                        return new InitializationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"No authentication method specified.");
                    }

                    KeyDetail keyDetail = KeyManagement.GetKeyDetail(initialization.Payload.Authentication.Key);
                    if (keyDetail is null)
                    {
                        return new InitializationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified authentication key doesn't exist. {initialization.Payload.Authentication.Key}");
                    }

                    authData = new AuthenticationData(initialization.Payload.Authentication.Method switch
                                                      {
                                                          SigningMethodEnum.Ca => AuthenticationData.SigningMethodEnum.CA,
                                                          SigningMethodEnum.Cbcmac => AuthenticationData.SigningMethodEnum.CBCMAC,
                                                          SigningMethodEnum.Certhost => AuthenticationData.SigningMethodEnum.CertHost,
                                                          SigningMethodEnum.Cmac => AuthenticationData.SigningMethodEnum.CMAC,
                                                          SigningMethodEnum.Hl => AuthenticationData.SigningMethodEnum.HL,
                                                          SigningMethodEnum.Reserved1 => AuthenticationData.SigningMethodEnum.Reserved1,
                                                          SigningMethodEnum.Reserved2 => AuthenticationData.SigningMethodEnum.Reserved2,
                                                          SigningMethodEnum.Reserved3 => AuthenticationData.SigningMethodEnum.Reserved3,
                                                          _ => AuthenticationData.SigningMethodEnum.None,
                                                      },
                                                      initialization.Payload.Authentication.Key,
                                                      Convert.FromBase64String(initialization.Payload.Authentication.Data).ToList());
                }
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.Initialization()");

            var result = await Device.Initialization(new InitializationRequest(authData),
                                                     cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.Initialization() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                KeyManagement.GetSecureKeyEntryStatus()?.Reset();

                // Delete internal key information
                foreach (var key in KeyManagement.GetKeyTable())
                {
                    if (!key.Preloaded)
                        KeyManagement.DeleteKey(key.KeyName);
                }
            }

            return new InitializationCompletion.PayloadData(result.CompletionCode,
                                                            result.ErrorDescription,
                                                            result.ErrorCode);
        }
    }
}
