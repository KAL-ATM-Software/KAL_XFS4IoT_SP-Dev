/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class CreateSignatureHandler
    {
        private async Task<CreateSignatureCompletion.PayloadData> HandleCreateSignature(ICreateSignatureEvents events, CreateSignatureCommand createSignature, CancellationToken cancel)
        {
            if (!Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.Signature))
            {
                return new CreateSignatureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"The device does not support signature. {Common.CashManagementCapabilities.ItemInfoTypes}");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new CreateSignatureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"The exchange state is already in active.",
                                                                 CreateSignatureCompletion.PayloadData.ErrorCodeEnum.ExchangeActive);
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new CreateSignatureCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}",
                                                                 CreateSignatureCompletion.PayloadData.ErrorCodeEnum.CashInActive);
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CreateSignature()");

            var result = await Device.CreateSignature(new CashInCommonCommandEvents(events), 
                                                      new CashInRequest(createSignature.Payload.Timeout), 
                                                      cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CreateSignature() -> {result.CompletionCode}, {result.ErrorCode}");

            return new CreateSignatureCompletion.PayloadData(result.CompletionCode,
                                                             result.ErrorDescription,
                                                             result.ErrorCode,
                                                             result.SignatureCaptured.NoteType,
                                                             result.SignatureCaptured.Orientation switch
                                                             {
                                                                 OrientationEnum.BackBottom => XFS4IoT.CashManagement.OrientationEnum.BackBottom,
                                                                 OrientationEnum.BackTop => XFS4IoT.CashManagement.OrientationEnum.BackTop,
                                                                 OrientationEnum.FrontBottom => XFS4IoT.CashManagement.OrientationEnum.FrontBottom,
                                                                 OrientationEnum.FrontTop => XFS4IoT.CashManagement.OrientationEnum.FrontTop,
                                                                 _ => XFS4IoT.CashManagement.OrientationEnum.Unknown,
                                                             },
                                                             result.SignatureCaptured.Signature);
        }
    }
}
