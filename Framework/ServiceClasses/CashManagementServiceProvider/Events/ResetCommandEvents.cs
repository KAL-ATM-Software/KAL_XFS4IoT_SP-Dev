/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;
using XFS4IoT.CashAcceptor.Events;

namespace XFS4IoTFramework.CashManagement
{
    public sealed class ResetCommandEvents(IStorageService storage, IResetEvents events) : StorageErrorCommonCommandEvents(storage, events)
    { }
}
