/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashDispenser
{
    [Serializable()]
    public abstract class Mix
    {
        /// <summary>
        /// The mix type is an algorithm or a house mix table
        /// </summary>
        public enum TypeEnum
        {
            Table, 
            Algorithm,
            Individual ,
        }

        /// <summary>
        /// XFS Predefined mix algorithm
        /// </summary>
        public enum AlgorithmEnum
        {
            Table,
            VendorSpecific,
            minimumBills,
            equalEmptying,
            maxCashUnits,
        }

        /// <summary>
        /// Mix Constructor
        /// </summary>
        public Mix(TypeEnum Type,
                   AlgorithmEnum Algorithm, 
                   string Name)
        {
            this.Type = Type;
            this.Algorithm = Algorithm;
            this.Name = Name;
        }

        /// <summary>
        /// Calculate mix depending on the algorithm derived
        /// </summary>
        /// <param name="CurrencyAmounts">Currency and amounts to denominate</param>
        /// <param name="CashUnits">Cash units to go through</param>
        /// <param name="MaxDispensableItems">Maximum number of items can be dispensed to the stacker.</param>
        /// <returns>Calculated denomination matching the requirements</returns>
        public abstract Denomination Calculate(Dictionary<string, double> CurrencyAmounts, Dictionary<string, CashUnitStorage> CashUnits, int MaxDispensableItems );

        /// <summary>
        /// Specifies whether the mix type is an algorithm or a house mix table
        /// </summary>
        public TypeEnum Type { get; init; }

        /// <summary>
        /// Specifies predefined mix algorithm
        /// Individual vendor-defined mix algorithms are defined above hexadecimal 7FFF. 
        /// Mix algorithms which are provided by the Service Provider are in the range hexadecimal 8000 - 8FFF. 
        /// Application defined mix algorithms start at hexadecimal 9000. 
        /// All numbers below 8000 hexadecimal are reserved
        /// Predefined 1-3
        /// 1 = MinimumBills
        /// 2 = EqualEmptying
        /// 3 = MaxCashUnits
        /// </summary>
        public AlgorithmEnum Algorithm { get; init; }

        /// <summary>
        /// Name of the mix table or algorithm
        /// </summary>
        public string Name { get; init; }


        /// <summary>
        /// Find the cash unit with the highest value notes which are lower that the given value. If CurrentGreatest is zero, find the greatest of all notes.
        /// </summary>
        /// <param name="CurrentGreatest">Find the next value smaller than this. If this is zero, the greatest value available.</param>
        /// <param name="CurrencyID">Currency to use. Ignore other currencies.</param>
        /// <param name="CashUnits">The cash units to search through.</param>
        /// <param name="UnitsUsed"></param>
        /// <returns></returns>
        protected static string FindNextGreatest(double CurrentGreatest, string CurrencyID, Dictionary<string, CashUnitStorage> CashUnits, ref List<string> UnitsUsed)
        {
            double currentBiggest = 0;
            string biggestCashUnit = string.Empty;

            // Find the greatest value note of this currency. This will be the first 
            // note used.
            if (CurrentGreatest == 0)
            {
                UnitsUsed.Clear();

                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Configuration.Currency == CurrencyID &&
                        //This check is done elsewhere
                        unit.Value.Unit.Configuration.Value > currentBiggest &&
                        unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Empty &&
                        !unit.Value.Unit.Configuration.AppLockOut)
                    {
                        currentBiggest = unit.Value.Unit.Configuration.Value;
                        biggestCashUnit = unit.Key;
                    }
                }
            }
            else
            {
                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Configuration.Currency == CurrencyID &&
                        //This check is done elsewhere
                        unit.Value.Unit.Configuration.Value > 0 &&
                        unit.Value.Unit.Configuration.Value >= currentBiggest &&
                        unit.Value.Unit.Configuration.Value <= CurrentGreatest &&
                        !UnitsUsed.Contains(unit.Key) &&
                        unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Empty &&
                        !unit.Value.Unit.Configuration.AppLockOut)
                    {
                        currentBiggest = unit.Value.Unit.Configuration.Value;
                        biggestCashUnit = unit.Key;
                    }

                }
            }

            if (!string.IsNullOrEmpty(biggestCashUnit) &&
                !UnitsUsed.Contains(biggestCashUnit))
            {
                UnitsUsed.Add(biggestCashUnit);
            }

            return biggestCashUnit;
        }

        /// <summary>
        /// Find the cash unit with the smallest value notes which are lower that the given value. If CurrentGreatest is zero, find the greatest of all notes.
        /// </summary>
        /// <param name="CurrentLeast">Find the next value smaller than this. If this is zero, find the greatest value available.</param>
        /// <param name="CurrencyID">Currency to use. Ignore other currencies.</param>
        /// <param name="CashUnits">The cash units to search through.</param>
        /// <param name="UnitsUsed"></param>
        /// <returns></returns>
        protected static string FindNextLeast(double CurrentLeast, string CurrencyID, Dictionary<string, CashUnitStorage> CashUnits, ref List<string> UnitsUsed)
        {
            double currentSmallest = double.MaxValue;
            string smallestCashUnit = string.Empty;

            // Find the greatest value note of this currency. This will be the first 
            // note used.
            if (CurrentLeast == 0)
            {
                UnitsUsed.Clear();

                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Configuration.Currency == CurrencyID &&
                        //This check is done elsewhere
                        unit.Value.Unit.Configuration.Value < currentSmallest &&
                        unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Empty &&
                        !unit.Value.Unit.Configuration.AppLockOut)
                    {
                        currentSmallest = unit.Value.Unit.Configuration.Value;
                        smallestCashUnit = unit.Key;
                    }
                }
            }
            else
            {
                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Configuration.Currency == CurrencyID &&
                        //This check is done elsewhere
                        unit.Value.Unit.Configuration.Value > 0 &&
                        unit.Value.Unit.Configuration.Value <= currentSmallest &&
                        unit.Value.Unit.Configuration.Value >= CurrentLeast &&
                        !UnitsUsed.Contains(unit.Key) &&
                        unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Empty &&
                        !unit.Value.Unit.Configuration.AppLockOut)
                    {
                        currentSmallest = unit.Value.Unit.Configuration.Value;
                        smallestCashUnit = unit.Key;
                    }
                }
            }

            if (!string.IsNullOrEmpty(smallestCashUnit) &&
                !UnitsUsed.Contains(smallestCashUnit))
            {
                UnitsUsed.Add(smallestCashUnit);
            }

            return smallestCashUnit;
        }

        /// <summary>
        /// Find the cash unit with the most full notes are lower that the given value. If CurrentGreatest is zero, find the greatest of all notes.
        /// </summary>
        /// <param name="LastMostFull">Find the next value smaller than this. If this is zero, find the greatest value available.</param>
        /// <param name="CurrencyID">Currency to use. Ignore other currencies.</param>
        /// <param name="CashUnits">The cash units to search through.</param>
        /// <param name="UnitsUsed"></param>
        /// <returns></returns>
        protected static string FindNextMostFull(double LastMostFull, string CurrencyID, Dictionary<string, CashUnitStorage> CashUnits, List<string> UnitsUsed)
        {
            double currentMostFull = 0;
            string mostFullCashUnit = string.Empty;

            // Find the greatest value note of this currency. This will be the first 
            // note used.
            if (LastMostFull == 0)
            {
                UnitsUsed.Clear();

                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Configuration.Currency == CurrencyID &&
                        //This check is done elsewhere
                        unit.Value.Unit.Status.Count > currentMostFull &&
                        unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Empty &&
                        !unit.Value.Unit.Configuration.AppLockOut)
                        {
                        currentMostFull = unit.Value.Unit.Status.Count;
                        mostFullCashUnit = unit.Key;
                    }
                }
            }
            else
            {
                foreach (var unit in CashUnits)
                {
                    if (unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Configuration.Currency == CurrencyID &&
                        //This check is done elsewhere
                        unit.Value.Unit.Status.Count > 0 &&
                        unit.Value.Unit.Status.Count >= currentMostFull &&
                        unit.Value.Unit.Status.Count <= LastMostFull &&
                        !UnitsUsed.Contains(unit.Key) &&
                        unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Empty &&
                        !unit.Value.Unit.Configuration.AppLockOut)
                    {
                        currentMostFull = unit.Value.Unit.Status.Count;
                        mostFullCashUnit = unit.Key;
                    }
                }
            }

            if (!string.IsNullOrEmpty(mostFullCashUnit) &&
                !UnitsUsed.Contains(mostFullCashUnit))
            {
                UnitsUsed.Add(mostFullCashUnit);
            }

            return mostFullCashUnit;
        }
    }
}
