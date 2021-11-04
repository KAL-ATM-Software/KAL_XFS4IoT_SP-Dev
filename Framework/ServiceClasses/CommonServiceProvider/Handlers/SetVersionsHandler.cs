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
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    public partial class SetVersionsHandler
    {

        private Task<SetVersionsCompletion.PayloadData> HandleSetVersions(ISetVersionsEvents events, SetVersionsCommand setVersions, CancellationToken cancel)
        {
            // The version information to be transparent to the device class and handle version within the framework. TODO
            return Task.FromResult(new SetVersionsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                         $"SetVersion commnad is not supported yet."));
        }
    }
}
