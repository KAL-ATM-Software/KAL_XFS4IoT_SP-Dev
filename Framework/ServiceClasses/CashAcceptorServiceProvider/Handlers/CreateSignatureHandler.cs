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
        private async Task<CommandResult<CreateSignatureCompletion.PayloadData>> HandleCreateSignature(ICreateSignatureEvents events, CreateSignatureCommand createSignature, CancellationToken cancel)
        {
            if (!Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.Signature))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"The device does not support signature. {Common.CashManagementCapabilities.ItemInfoTypes}");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(CreateSignatureCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(CreateSignatureCompletion.PayloadData.ErrorCodeEnum.CashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CreateSignature()");

            var result = await Device.CreateSignature(new CreateSignatureCommandEvents(events), 
                                                      new CreateSignatureRequest(createSignature.Header.Timeout ?? 0), 
                                                      cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CreateSignature() -> {result.CompletionCode}, {result.ErrorCode}");

            CreateSignatureCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.SignatureCaptured is not null)
            {
                payload = new(
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

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
