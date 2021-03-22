/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;

namespace XFS4IoTServer
{
    public interface ICommandHandler
    {
        // Must have a constructor of the form: 
        // ICommandHandler( ICommandDispatcher, ILogger )

        Task Handle(IConnection Connection, object Command, CancellationToken Cancel);

        Task HandleError(IConnection Connection, object Command, Exception CommandException);
    }
}