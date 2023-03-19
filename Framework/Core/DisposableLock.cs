/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace XFS4IoT;

public class DisposableLock : IDisposable
{
    private readonly SemaphoreSlim Semaphore;

    public DisposableLock(SemaphoreSlim semaphore)
    {
        Semaphore = semaphore;
    }

    public async Task Aquire(CancellationToken token)
    {
        await Semaphore.WaitAsync(token);
    }

    public void Dispose()
    {
        Semaphore.Release();
    }

    public static async Task<DisposableLock> Create(SemaphoreSlim semaphore, CancellationToken? token = null)
    {
        DisposableLock disposableLock = new(semaphore);
        await disposableLock.Aquire(token ?? CancellationToken.None);
        return disposableLock;
    }
}
