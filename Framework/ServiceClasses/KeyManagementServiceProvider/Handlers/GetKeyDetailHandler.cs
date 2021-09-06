/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class GetKeyDetailHandler
    {
        private Task<GetKeyDetailCompletion.PayloadData> HandleGetKeyDetail(IGetKeyDetailEvents events, GetKeyDetailCommand getKeyDetail, CancellationToken cancel)
        {
            List<KeyDetail> keyDetails = KeyManagement.GetKeyTable();

            if (!string.IsNullOrEmpty(getKeyDetail.Payload.KeyName))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(getKeyDetail.Payload.KeyName);
                if (keyDetail is null)
                {
                    return Task.FromResult(new GetKeyDetailCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                  $"Specified key is not stored. {getKeyDetail.Payload.KeyName}",
                                                                                  GetKeyDetailCompletion.PayloadData.ErrorCodeEnum.KeyNotFound));
                }

                keyDetails.Clear();
                keyDetails.Add(keyDetail);
            }

            Dictionary<string, GetKeyDetailCompletion.PayloadData.KeyDetailsClass> keyInfo = new();
            foreach (var key in keyDetails)
            {
                GetKeyDetailCompletion.PayloadData.KeyDetailsClass.KeyBlockInfoClass keyblockInfo = new(key.KeyUsage,
                                                                                                        key.RestrictedKeyUsage,
                                                                                                        key.Algorithm,
                                                                                                        key.ModeOfUse,
                                                                                                        key.KeyVersionNumber,
                                                                                                        key.Exportability,
                                                                                                        key.OptionalKeyBlockHeader is null ? null : Convert.ToBase64String(key.OptionalKeyBlockHeader.ToArray()),
                                                                                                        key.KeyLength);

                keyInfo.Add(key.KeyName, new GetKeyDetailCompletion.PayloadData.KeyDetailsClass(key.Generation,
                                                                                                key.Version,
                                                                                                key.ActivatingDate?.ToString("yyyyMMdd"),
                                                                                                key.ExpiryDate?.ToString("yyyyMMdd"),
                                                                                                key.KeyStatus switch
                                                                                                {
                                                                                                    KeyDetail.KeyStatusEnum.Loaded => GetKeyDetailCompletion.PayloadData.KeyDetailsClass.LoadedEnum.Yes,
                                                                                                    KeyDetail.KeyStatusEnum.Construct => GetKeyDetailCompletion.PayloadData.KeyDetailsClass.LoadedEnum.Construct,
                                                                                                    KeyDetail.KeyStatusEnum.NotLoaded => GetKeyDetailCompletion.PayloadData.KeyDetailsClass.LoadedEnum.No,
                                                                                                    _ => GetKeyDetailCompletion.PayloadData.KeyDetailsClass.LoadedEnum.Unknown
                                                                                                },
                                                                                                keyblockInfo));
            }

            return Task.FromResult(new GetKeyDetailCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                          null,
                                                                          null,
                                                                          keyInfo.Count == 0 ? null : keyInfo));
        }
    }
}
