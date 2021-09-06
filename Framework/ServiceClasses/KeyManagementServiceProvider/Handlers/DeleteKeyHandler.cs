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
using XFS4IoT.KeyManagement;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class DeleteKeyHandler
    {
        private async Task<DeleteKeyCompletion.PayloadData> HandleDeleteKey(IDeleteKeyEvents events, DeleteKeyCommand deleteKey, CancellationToken cancel)
        {
            if (!string.IsNullOrEmpty(deleteKey.Payload.Key))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(deleteKey.Payload.Key);
                if (keyDetail is null)
                {
                    return new DeleteKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Specified key doesn't exist. {deleteKey.Payload.Key}");
                }
            }

            AuthenticationData authData = null;
            if (deleteKey.Payload.Authentication is not null)
            {
                if (deleteKey.Payload.Authentication.Method is null)
                {
                    return new DeleteKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No authentication method specified.");
                }
                if (deleteKey.Payload.Authentication.Method != SigningMethodEnum.None)
                {
                    if (string.IsNullOrEmpty(deleteKey.Payload.Authentication.Data))
                    {
                        return new DeleteKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"No authentication data specified.");
                    }
                    if (deleteKey.Payload.Authentication.Method is null)
                    {
                        return new DeleteKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"No authentication method specified.");
                    }

                    KeyDetail keyDetail = KeyManagement.GetKeyDetail(deleteKey.Payload.Authentication.Key);
                    if (keyDetail is null)
                    {
                        return new DeleteKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified authentication key doesn't exist. {deleteKey.Payload.Authentication.Key}");
                    }

                    authData = new AuthenticationData(deleteKey.Payload.Authentication.Method switch
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
                                                      deleteKey.Payload.Authentication.Key,
                                                      Convert.FromBase64String(deleteKey.Payload.Authentication.Data).ToList());
                }
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.DeleteKey()");

            var result = await Device.DeleteKey(new DeleteKeyRequest(deleteKey.Payload.Key, authData), cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.DeleteKey() -> {result.CompletionCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                // Delete internal key information
                if (string.IsNullOrEmpty(deleteKey.Payload.Key))
                {
                    foreach (var key in KeyManagement.GetKeyTable())
                    {
                        if (!key.Preloaded)
                            KeyManagement.DeleteKey(key.KeyName);
                    }
                }
                else
                {
                    KeyManagement.DeleteKey(deleteKey.Payload.Key);
                }
            }

            return new DeleteKeyCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription);
        }
    }
}
