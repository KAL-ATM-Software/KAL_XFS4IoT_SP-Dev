/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    public interface ICommandDispatcher
    {
        Task Dispatch(IConnection Connection, MessageBase Command, CancellationToken token);

        Task DispatchError(IConnection Connection, MessageBase Command, Exception CommandException);

        Task RunAsync(CancellationSource cancellationSource);

        Task<bool> CancelCommandsAsync(IConnection Connection, List<int> RequestIds, CancellationToken token);
    }
}