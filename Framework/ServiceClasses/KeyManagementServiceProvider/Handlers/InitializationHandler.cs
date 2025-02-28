/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoT.KeyManagement;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class InitializationHandler
    {
        private async Task<CommandResult<InitializationCompletion.PayloadData>> HandleInitialization(IInitializationEvents events, InitializationCommand initialization, CancellationToken cancel)
        {
            AuthenticationData authData = null;
            if (initialization.Payload?.Authentication is not null)
            {
                if (initialization.Payload.Authentication.Method is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No authentication method specified.");
                }
                if (initialization.Payload.Authentication.Method is not null)
                {
                    if (initialization.Payload.Authentication.Data is null ||
                        initialization.Payload.Authentication.Data.Count == 0)
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"No authentication data specified.");
                    }
                    if (initialization.Payload.Authentication.Method is null)
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"No authentication method specified.");
                    }

                    KeyDetail keyDetail = KeyManagement.GetKeyDetail(initialization.Payload.Authentication.Key);
                    if (keyDetail is null)
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Specified authentication key doesn't exist. {initialization.Payload.Authentication.Key}");
                    }

                    authData = new AuthenticationData(
                        initialization.Payload.Authentication.Method switch
                        {
                            AuthenticationMethodEnum.Ca => AuthenticationData.SigningMethodEnum.CA,
                            AuthenticationMethodEnum.Cbcmac => AuthenticationData.SigningMethodEnum.CBCMAC,
                            AuthenticationMethodEnum.Certhost => AuthenticationData.SigningMethodEnum.CertHost,
                            AuthenticationMethodEnum.Cmac => AuthenticationData.SigningMethodEnum.CMAC,
                            AuthenticationMethodEnum.Hl => AuthenticationData.SigningMethodEnum.HL,
                            AuthenticationMethodEnum.Reserved1 => AuthenticationData.SigningMethodEnum.Reserved1,
                            AuthenticationMethodEnum.Reserved2 => AuthenticationData.SigningMethodEnum.Reserved2,
                            AuthenticationMethodEnum.Reserved3 => AuthenticationData.SigningMethodEnum.Reserved3,
                            _ => AuthenticationData.SigningMethodEnum.None,
                        },
                        initialization.Payload.Authentication.Key,
                        initialization.Payload.Authentication.Data);
                }
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.Initialization()");

            var result = await Device.Initialization(new InitializationRequest(authData),
                                                     cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.Initialization() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                KeyManagement.GetSecureKeyEntryStatus()?.Reset();

                // Delete internal key information
                var keyTable = KeyManagement.GetKeyTable();
                if (keyTable is not null)
                {
                    foreach (var key in keyTable)
                    {
                        if (!key.Preloaded)
                        {
                            KeyManagement.DeleteKey(key.KeyName);
                        }
                    }
                }
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
