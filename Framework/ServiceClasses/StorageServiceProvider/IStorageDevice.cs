/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

namespace XFS4IoTFramework.Storage
{
    public interface IStorageDevice : IDevice
    {
        #region Card

        /// <summary>
        /// Return card storage information for current configuration and capabilities on the startup.
        /// </summary>
        /// <returns></returns>
        bool GetCardStorageConfiguration(out Dictionary<string, CardUnitStorageConfiguration> newCardUnits);

        /// <summary>
        /// This method is call after card is moved to the storage. Move or Reset command.
        /// </summary>
        /// <returns>Return true if the device maintains hardware counters for the card units</returns>
        bool GetCardUnitCounts(out Dictionary<string, CardUnitCount> unitCounts);

        /// <summary>
        /// Update card unit hardware status by device class. the maintaining status by the framework will be overwritten.
        /// The framework can't handle threshold event if the device class maintains hardware storage status on threshold value is not zero.
        /// </summary>
        /// <returns>Return true if the device maintains hardware card unit status</returns>
        bool GetCardUnitStatus(out Dictionary<string, CardStatusClass.ReplenishmentStatusEnum> unitStatus);

        /// <summary>
        /// Update card unit hardware storage status by device class.
        /// </summary>
        /// <returns>Return true if the device maintains hardware card storage status</returns>
        bool GetCardStorageStatus(out Dictionary<string, CardUnitStorage.StatusEnum> storageStatus);

        /// <summary>
        /// Set new configuration and counters
        /// </summary>
        /// <returns>Return operation is completed successfully or not and report updates storage information.</returns>
        Task<SetCardStorageResult> SetCardStorageAsync(SetCardStorageRequest request, CancellationToken cancellation);

        #endregion

        #region Cash

        /// <summary>
        /// Return cash storage information for current configuration and capabilities on the startup.
        /// </summary>
        /// <returns>Return true if the cash unit configuration or capabilities are changed, otherwise false</returns>
        bool GetCashStorageConfiguration(out Dictionary<string, CashUnitStorageConfiguration> newCardUnits);

        /// <summary>
        /// Return cash unit counts maintained by the device class
        /// </summary>
        /// <returns>Return true if the device class maintained counts, otherwise false</returns>
        bool GetCashUnitCounts(out Dictionary<string, CashUnitCountClass> unitCounts);

        /// <summary>
        /// Return cash unit initial counts maintained by the device class and only this method is called on the start of day
        /// </summary>
        /// <returns>Return true if the device class maintained initial counts, otherwise false</returns>
        bool GetCashUnitInitialCounts(out Dictionary<string, StorageCashCountClass> initialCounts);

        /// <summary>
        /// Return cash storage status.
        /// </summary>
        /// <returns>Return true if the device class uses hardware status, otherwise false</returns>
        bool GetCashStorageStatus(out Dictionary<string, CashUnitStorage.StatusEnum> storageStatus);

        /// <summary>
        /// Return cash unit status maintained by the device class
        /// </summary>
        /// <returns>Return true if the device class uses hardware status, otherwise false</returns>
        bool GetCashUnitStatus(out Dictionary<string, CashStatusClass.ReplenishmentStatusEnum> unitStatus);

        /// <summary>
        /// Return accuracy of counts. This method is called if the device class supports feature for count accuray
        /// </summary>
        void GetCashUnitAccuray(string storageId, out CashStatusClass.AccuracyEnum unitAccuracy);

        /// <summary>
        /// Set new configuration and counters for cash units
        /// </summary>
        /// <returns>Return operation is completed successfully or not and report updates storage information.</returns>
        Task<SetCashStorageResult> SetCashStorageAsync(SetCashStorageRequest request, CancellationToken cancellation);

        #endregion

        #region Check
        /// <summary>
        /// Return cheeck storage information for current configuration and capabilities on the startup.
        /// </summary>
        /// <returns>Return true if the check storage configuration or capabilities are changed, otherwise false</returns>
        bool GetCheckStorageConfiguration(out Dictionary<string, CheckUnitStorageConfiguration> newCheckUnits);

        /// <summary>
        /// Return check unit counts maintained by the device class
        /// </summary>
        /// <returns>Return true if the device class maintained counts, otherwise false</returns>
        bool GetCheckUnitCounts(out Dictionary<string, StorageCheckCountClass> unitCounts);

        /// <summary>
        /// Return check unit initial counts maintained by the device class and only this method is called on the start of day
        /// </summary>
        /// <returns>Return true if the device class maintained initial counts, otherwise false</returns>
        bool GetCheckUnitInitialCounts(out Dictionary<string, StorageCheckCountClass> initialCounts);

        /// <summary>
        /// Return check storage status.
        /// </summary>
        /// <returns>Return true if the device class uses hardware status, otherwise false</returns>
        bool GetCheckStorageStatus(out Dictionary<string, CheckUnitStorage.StatusEnum> storageStatus);

        /// <summary>
        /// Return check unit status maintained by the device class
        /// </summary>
        /// <returns>Return true if the device class uses hardware status, otherwise false</returns>
        bool GetCheckUnitStatus(out Dictionary<string, CheckStatusClass.ReplenishmentStatusEnum> unitStatus);

        /// <summary>
        /// Set new configuration and counters for check units
        /// </summary>
        /// <returns>Return operation is completed successfully or not and report updates storage information.</returns>
        Task<SetCheckStorageResult> SetCheckStorageAsync(SetCheckStorageRequest request, CancellationToken cancellation);

        #endregion

        #region Printer
        /// <summary>
        /// Return printer storage (retract bin, passbook storage) information for current configuration and capabilities on the startup.
        /// </summary>
        /// <returns>Return true if the storage configuration or capabilities are changed, otherwise false</returns>
        bool GetPrinterStorageConfiguration(out Dictionary<string, PrinterUnitStorageConfiguration> newPrinterUnits);

        /// <summary>
        /// Return printer storage counts maintained by the device class
        /// </summary>
        /// <returns>Return true if the device class maintained counts, otherwise false</returns>
        bool GetPrinterUnitCounts(out Dictionary<string, PrinterUnitCount> unitCounts);

        /// <summary>
        /// Return printer storage status (retract bin, passbook storage).
        /// </summary>
        /// <returns>Return true if the device class uses hardware status, otherwise false</returns>
        bool GetPrinterStorageStatus(out Dictionary<string, PrinterUnitStorage.StatusEnum> storageStatus);

        /// <summary>
        /// Return printer unit status (retract bin, passbook storage) maintained by the device class
        /// </summary>
        /// <returns>Return true if the device class uses hardware status, otherwise false</returns>
        bool GetPrinterUnitStatus(out Dictionary<string, PrinterStatusClass.ReplenishmentStatusEnum> unitStatus);

        /// <summary>
        /// Set new configuration and counters for printer storage.
        /// </summary>
        /// <returns>Return operation is completed successfully or not and report updates storage information.</returns>
        Task<SetPrinterStorageResult> SetPrinterStorageAsync(SetPrinterStorageRequest request, CancellationToken cancellation);

        #endregion

        #region IBNS
        /// <summary>
        /// Return IBNS storage (retract bin, passbook storage) information for current configuration and capabilities on the startup.
        /// Status object is a reference to report status changes.
        /// </summary>
        /// <returns>Return true if the storage configuration or capabilities are changed, otherwise false</returns>
        bool GetIBNSStorageInfo(out Dictionary<string, IBNSStorageInfo> newIBNSUnits);

        #endregion

        /// <summary>
        /// This command puts the device in an exchange state, i.e. a state in which storage units can be emptied, replenished,
        /// removed or replaced.The command will initiate any physical processes which may be necessary to make the storage units
        /// accessible.If this command returns a successful completion the device is in an exchange state.
        /// </summary>
        Task<StartExchangeResult> StartExchangeAsync(CancellationToken cancellation);

        /// <summary>
        /// This command will end the exchange state. If any physical action took place as a result of the Storage.StartExchange
        /// command then this command will cause the cash units to be returned to their normal physical state.Any necessary device testing will also be initiated.
        /// </summary>
        Task<EndExchangeResult> EndExchangeAsync(CancellationToken cancellation);

    }
}
