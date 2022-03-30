/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * LoadDefinitionHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class LoadDefinitionHandler
    {

        private async Task<LoadDefinitionCompletion.PayloadData> HandleLoadDefinition(ILoadDefinitionEvents events, LoadDefinitionCommand loadDefinition, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(loadDefinition.Payload.Definition))
                return new LoadDefinitionCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "Definition cannot be null or whitespace.");

            
            if (Printer.LoadSingleDefinition(loadDefinition.Payload.Definition, loadDefinition.Payload.Overwrite ?? false, out var type, out var name, out var errorMsg))
            {
                await Provider.BroadcastEvent(new XFS4IoT.Printer.Events.DefinitionLoadedEvent(new(name, type)));
                return new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success, null);
            }
            else
            {
                LoadDefinitionCompletion.PayloadData.ErrorCodeEnum errorCode = type switch
                {
                    XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum.Form => LoadDefinitionCompletion.PayloadData.ErrorCodeEnum.FormInvalid,
                    XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum.Media => LoadDefinitionCompletion.PayloadData.ErrorCodeEnum.MediaInvalid,
                    _ => LoadDefinitionCompletion.PayloadData.ErrorCodeEnum.DefinitionExists
                };
                return new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode, errorMsg ?? "Failed to read definition.", errorCode);
            }
        }

    }
}
