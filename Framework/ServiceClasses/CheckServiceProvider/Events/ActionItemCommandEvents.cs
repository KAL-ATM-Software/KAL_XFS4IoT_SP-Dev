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
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.Check
{
    public sealed class ActionItemCommandEvents(IStorageService storage, ICheckService check, IActionItemEvents events) :
        MediaDataCommandEvent(check, events)
    {

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        public Task MediaPresentedEvent(
            MediaPresentedCommandEvent.PositionEnum Position,
            int BunchIndex,
            int TotalBunches = -1) => MediaPresentedCommandEvent?.MediaPresentedEvent(Position, BunchIndex, TotalBunches);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);

        private MediaPresentedCommandEvent MediaPresentedCommandEvent { get; init; } = new(events);
    }
}
