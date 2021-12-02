/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class GetCodelineMappingHandler
    {
        private Task<GetCodelineMappingCompletion.PayloadData> HandleGetCodelineMapping(IGetCodelineMappingEvents events, GetCodelineMappingCommand getCodelineMapping, CancellationToken cancel)
        {
            if (getCodelineMapping.Payload.CodelineFormat is null)
            {
                return Task.FromResult(new GetCodelineMappingCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                    $"No codeline format specified."));
            }

            CodelineFormatEnum codelineFormat = CodelineFormatEnum.CMC7;
            if (getCodelineMapping.Payload.CodelineFormat == GetCodelineMappingCommand.PayloadData.CodelineFormatEnum.E13b)
                codelineFormat = CodelineFormatEnum.E13B;

            if ((codelineFormat == CodelineFormatEnum.CMC7 &&
                !Printer.PrinterCapabilities.CodelineFormats.HasFlag(PrinterCapabilitiesClass.CodelineFormatEnum.CMC7)) ||
                (codelineFormat == CodelineFormatEnum.E13B &&
                !Printer.PrinterCapabilities.CodelineFormats.HasFlag(PrinterCapabilitiesClass.CodelineFormatEnum.E13B)))
            {
                return Task.FromResult(new GetCodelineMappingCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                    $"Specified codeline format is not supported by the device."));
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetCodelineMapping()");
            var result = Device.GetCodelineMapping(codelineFormat);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetCodelineMapping() -> {result.Count}");

            if (result is null ||
                result.Count == 0)
            {
                return Task.FromResult(new GetCodelineMappingCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                                                    $"Failed to map specified codeline format."));
            }

            return Task.FromResult(new GetCodelineMappingCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                ErrorDescription: string.Empty,
                                                                                CodelineFormat: codelineFormat switch
                                                                                {
                                                                                    CodelineFormatEnum.CMC7 => GetCodelineMappingCompletion.PayloadData.CodelineFormatEnum.Cmc7,
                                                                                    _ => GetCodelineMappingCompletion.PayloadData.CodelineFormatEnum.E13b
                                                                                },
                                                                                CharMapping: Convert.ToBase64String(result.ToArray())));
        }
    }
}
