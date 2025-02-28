/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Storage
{
    public enum StorageTypeEnum
    {
        Cash = 1 << 0,
        Card = 1 << 1,
        Check = 1 << 2,
        Printer = 1 << 3,
        IBNS = 1 << 4,
        Deposit = 1 << 5,
    }

    public interface IStorageService
    {
        /// <summary>
        /// Update managed card storage information in the framework.
        /// </summary>
        Task UpdateCardStorageCount(string storageId, int countDelta);

        /// <summary>
        /// Update managed cash storage information in the framework.
        /// </summary>
        Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null);

        /// <summary>
        /// Update managed check storage information in the framework.
        /// </summary>
        Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta = null);

        /// <summary>
        /// Update managed printer storage information in the framework.
        /// </summary>
        Task UpdatePrinterStorageCount(string storageId, int countDelta);

        /// <summary>
        /// Update managed deposit storage information in the framework.
        /// </summary>
        Task UpdateDepositStorageCount(string storageId, int countDelta);

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        StorageTypeEnum StorageType { get; init; }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        void StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        Dictionary<string, CardUnitStorage> CardUnits { get; init; }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        Dictionary<string, CashUnitStorage> CashUnits { get; init; }

        /// <summary>
        /// Check storage structure information of this device
        /// </summary>
        Dictionary<string, CheckUnitStorage> CheckUnits { get; init; }

        /// <summary>
        /// Printer storage structure information of this device
        /// </summary>
        Dictionary<string, PrinterUnitStorage> PrinterUnits { get; init; }

        /// <summary>
        /// IBNS storage structure information of this device
        /// </summary>
        Dictionary<string, IBNSUnitStorage> IBNSUnits { get; init; }

        /// <summary>
        /// Deposit storage structure information of this device
        /// </summary>
        Dictionary<string, DepositUnitStorage> DepositUnits { get; init; }

        /// <summary>
        /// Return XFS4IoT storage structured object.
        /// </summary>
        Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> UnitIds);

        /// <summary>
        /// Sending status changed event.
        /// </summary>
        Task StorageChangedEvent(object sender, PropertyChangedEventArgs propertyInfo);
    }

    public interface IStorageServiceClass : IStorageService, IStorageUnsolicitedEvents
    {
    }
}
