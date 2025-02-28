/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using XFS4IoT.Storage;
using XFS4IoTServer;
using XFS4IoT;

namespace XFS4IoTFramework.Storage
{
    [CommandHandlerAsync]
    public partial class GetStorageHandler
    {
        private Task<CommandResult<GetStorageCompletion.PayloadData>> HandleGetStorage(IGetStorageEvents events, GetStorageCommand getStorage, CancellationToken cancel)
        {
            List<string> unitIds = [];
            if (Storage.StorageType.HasFlag(StorageTypeEnum.Card))
            {
                unitIds.AddRange(from unit in Storage.CardUnits
                                 select unit.Key);
            }
            if (Storage.StorageType.HasFlag(StorageTypeEnum.Cash))
            {
                foreach (var unit in Storage.CashUnits)
                {
                    if (unitIds.Contains(unit.Key))
                    {
                        continue;
                    }
                    unitIds.Add(unit.Key);
                }
            }
            if (Storage.StorageType.HasFlag(StorageTypeEnum.Check))
            {
                foreach (var unit in Storage.CheckUnits)
                {
                    if (unitIds.Contains(unit.Key))
                    {
                        continue;
                    }
                    unitIds.Add(unit.Key);
                }
            }
            if (Storage.StorageType.HasFlag(StorageTypeEnum.Printer))
            {
                foreach (var unit in Storage.PrinterUnits)
                {
                    if (unitIds.Contains(unit.Key))
                    {
                        continue;
                    }
                    unitIds.Add(unit.Key);
                }
            }
            if (Storage.StorageType.HasFlag(StorageTypeEnum.IBNS))
            {
                foreach (var unit in Storage.IBNSUnits)
                {
                    if (unitIds.Contains(unit.Key))
                    {
                        continue;
                    }
                    unitIds.Add(unit.Key);
                }
            }
            if (Storage.StorageType.HasFlag(StorageTypeEnum.Deposit))
            {
                foreach (var unit in Storage.DepositUnits)
                {
                    if (unitIds.Contains(unit.Key))
                    {
                        continue;
                    }
                    unitIds.Add(unit.Key);
                }
            }

            Dictionary<string, StorageUnitClass> storageResponse = Storage.GetStorages(unitIds);

            return Task.FromResult(
                new CommandResult<GetStorageCompletion.PayloadData>(
                    (storageResponse is null || storageResponse.Count == 0) ? null : new(storageResponse),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
