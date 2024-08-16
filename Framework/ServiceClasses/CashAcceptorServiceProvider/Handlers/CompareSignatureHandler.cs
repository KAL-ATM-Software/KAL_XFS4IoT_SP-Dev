/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class CompareSignatureHandler
    {
        private async Task<CommandResult<CompareSignatureCompletion.PayloadData>> HandleCompareSignature(ICompareSignatureEvents events, CompareSignatureCommand compareSignature, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(CompareSignatureCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(CompareSignatureCompletion.PayloadData.ErrorCodeEnum.CashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}");
            }

            if (!Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.Signature))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"The device does not support signature. {Common.CashManagementCapabilities.ItemInfoTypes}");
            }

            if (compareSignature.Payload.ReferenceSignatures is null ||
                compareSignature.Payload.ReferenceSignatures.Count == 0)
            {
                return new(
                    new(CompareSignatureCompletion.PayloadData.ErrorCodeEnum.InvalidReferenceSignature),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"No signature data supplied for the references.");
            }

            List<SignatureInfo> referenceSignatures = new();
            foreach (var reference in compareSignature.Payload.ReferenceSignatures)
            {
                if (!Common.CashManagementCapabilities.AllBanknoteItems.ContainsKey(reference.NoteType))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported note type supplied for references. {reference.NoteType}");
                }

                referenceSignatures.Add(new SignatureInfo(reference.NoteType,
                                                          reference.Orientation switch 
                                                          {
                                                              XFS4IoT.CashManagement.OrientationEnum.BackBottom => OrientationEnum.BackBottom,
                                                              XFS4IoT.CashManagement.OrientationEnum.BackTop => OrientationEnum.BackTop,
                                                              XFS4IoT.CashManagement.OrientationEnum.FrontBottom => OrientationEnum.FrontBottom,
                                                              XFS4IoT.CashManagement.OrientationEnum.FrontTop => OrientationEnum.FrontTop,
                                                              _ => OrientationEnum.Unknown,
                                                          },
                                                          reference.Signature));
            }

            Dictionary<int, SignatureInfo> signaturesToCompare = new();
            int index = 0;
            foreach (var signature in compareSignature.Payload.ReferenceSignatures)
            {
                if (!Common.CashManagementCapabilities.AllBanknoteItems.ContainsKey(signature.NoteType))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported note type supplied for signature to compare. {signature.NoteType}");
                }

                signaturesToCompare.Add(index++, new SignatureInfo(signature.NoteType,
                                                                   signature.Orientation switch
                                                                   {
                                                                       XFS4IoT.CashManagement.OrientationEnum.BackBottom => OrientationEnum.BackBottom,
                                                                       XFS4IoT.CashManagement.OrientationEnum.BackTop => OrientationEnum.BackTop,
                                                                       XFS4IoT.CashManagement.OrientationEnum.FrontBottom => OrientationEnum.FrontBottom,
                                                                       XFS4IoT.CashManagement.OrientationEnum.FrontTop => OrientationEnum.FrontTop,
                                                                       _ => OrientationEnum.Unknown,
                                                                   },
                                                                   signature.Signature));

            }

            if (compareSignature.Payload.Signatures is null ||
                compareSignature.Payload.Signatures.Count == 0)
            {
                return new(
                    new(CompareSignatureCompletion.PayloadData.ErrorCodeEnum.InvalidTransactionSignature),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"No signature data supplied for the search.");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CompareSignature()");

            var result = await Device.CompareSignature(new CompareSignatureRequest(referenceSignatures, signaturesToCompare),
                                                       cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CompareSignature() -> {result.CompletionCode}, {result.ErrorCode}");

            List<CompareSignatureCompletion.PayloadData.SignaturesIndexClass> sigReuslt = null;
            if (result.ConfidenceLevels?.Count > 0)
            {
                sigReuslt = [];
                foreach (var confLevel in result.ConfidenceLevels)
                {
                    if (confLevel.Value.ConfidenceLevel > 100)
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InternalError,
                            $"The device class reported unexpected confidence level. should be in the range of 0 to 100 inclusive. {confLevel.Value.ConfidenceLevel}");
                    }

                    sigReuslt.Add(new CompareSignatureCompletion.PayloadData.SignaturesIndexClass(confLevel.Key,
                                                                                                  confLevel.Value.ConfidenceLevel,
                                                                                                  confLevel.Value.ComparisonData));
                }
            }

            CompareSignatureCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                sigReuslt is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    SignaturesIndex: sigReuslt);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
