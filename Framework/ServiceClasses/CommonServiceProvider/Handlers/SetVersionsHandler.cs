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
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    public partial class SetVersionsHandler
    {

        private Task<SetVersionsCompletion.PayloadData> HandleSetVersions(ISetVersionsEvents events, SetVersionsCommand setVersions, CancellationToken cancel)
        {
            if (setVersions.Payload is null)
            {
                Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                      $"No payload specified."));
            }

            if ((setVersions.Payload.Commands is null ||
                 setVersions.Payload.Commands is not null && setVersions.Payload.Commands.Count == 0) &&
                (setVersions.Payload.Events is null ||
                 setVersions.Payload.Events is not null && setVersions.Payload.Events.Count == 0))
            {
                Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                      $"No commands and events specified."));
            }

            if (setVersions.Payload.Commands is not null)
            {
                foreach (var cmd in setVersions.Payload.Commands)
                {
                    if (!Provider.GetMessagesSupported().ContainsKey(cmd.Key))
                    {
                        return Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedData,
                                                                                     $"Unsupported command specified. {cmd.Key}"));
                    }

                    if (!Provider.GetMessagesSupported()[cmd.Key].Versions.Contains($"{cmd.Value}.0"))
                    {
                        return Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedData,
                                                                                     $"Unsupported version specified. {cmd.Value}, The Service only support version {Provider.GetMessagesSupported()[cmd.Key].Versions[0]}"));
                    }
                }
            }

            if (setVersions.Payload.Events is not null)
            {
                foreach (var ev in setVersions.Payload.Events)
                {
                    if (!Provider.GetMessagesSupported().ContainsKey(ev.Key))
                    {
                        return Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedData,
                                                                                     $"Unsupported event specified. {ev.Key}"));
                    }

                    if (!Provider.GetMessagesSupported()[ev.Key].Versions.Contains($"{ev.Value}.0"))
                    {
                        return Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedData,
                                                                                     $"Unsupported event specified. {ev.Value}, The Service only support version {Provider.GetMessagesSupported()[ev.Key].Versions[0]}"));
                    }
                }
            }

            return Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty));
        }
    }
}
