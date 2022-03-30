/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class GetDeviceLockStatusHandler
    {
        private Task<GetDeviceLockStatusCompletion.PayloadData> HandleGetDeviceLockStatus(IGetDeviceLockStatusEvents events, GetDeviceLockStatusCommand getDeviceLockStatus, CancellationToken cancel)
        {
            if (CashAcceptor.DeviceLockStatus is null)
            {
                return Task.FromResult(new GetDeviceLockStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                                     $"The device doesn't support lock status."));
            }

            return Task.FromResult(new GetDeviceLockStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                 string.Empty,
                                                                                 CashAcceptor.DeviceLockStatus.LockStatus switch
                                                                                 {
                                                                                     DeviceLockStatusClass.DeviceLockStatusEnum.Lock => GetDeviceLockStatusCompletion.PayloadData.DeviceLockStatusEnum.Lock,
                                                                                     DeviceLockStatusClass.DeviceLockStatusEnum.Unlock => GetDeviceLockStatusCompletion.PayloadData.DeviceLockStatusEnum.Unlock,
                                                                                     _ => GetDeviceLockStatusCompletion.PayloadData.DeviceLockStatusEnum.LockUnknown,
                                                                                 },
                                                                                 CashAcceptor.DeviceLockStatus.UnitLock?.Select(l => new GetDeviceLockStatusCompletion.PayloadData.UnitLockClass(l.Key, l.Value switch
                                                                                                                                        { 
                                                                                                                                             DeviceLockStatusClass.UnitLockStatusEnum.Lock => GetDeviceLockStatusCompletion.PayloadData.UnitLockClass.UnitLockStatusEnum.Lock,
                                                                                                                                             DeviceLockStatusClass.UnitLockStatusEnum.Unlock => GetDeviceLockStatusCompletion.PayloadData.UnitLockClass.UnitLockStatusEnum.Unlock,
                                                                                                                                             _ => GetDeviceLockStatusCompletion.PayloadData.UnitLockClass.UnitLockStatusEnum.LockUnknown,
                                                                                                                                         })).ToList()
                                                                                ));
        }
    }
}
