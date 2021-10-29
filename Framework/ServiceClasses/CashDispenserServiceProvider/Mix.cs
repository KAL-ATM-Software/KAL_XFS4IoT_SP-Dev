/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using XFS4IoT;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashDispenser
{
    /// <summary>
    /// MixTable
    ///  A mix defined by a fixed table. There is one denomination defined for each value.
    /// </summary>
    [Serializable()]
    public sealed class MixTable : Mix
    {
        private readonly ILogger Logger;

        public sealed class Table
        {
            public Table(double Amount, List<double> Values, List<int> Counts)
            {
                Contracts.Assert(Values.Count == Counts.Count, $"Supplied size for the Values and Counts are different. {Values.Count} {Counts.Count}");

                Amount = 0;
                this.Counts = new();
                for (int i=0; i<Values.Count; i++)
                {
                    Amount += Values[i] * Counts[i];
                    this.Counts.Add(Values[i], Counts[i]);
                }
            }

            public double Amount { get; init; }

            public Dictionary<double, int> Counts { get; init; }
        }

        public MixTable(int MixNumber,
                        string Name,
                        List<double> Cols,
                        Dictionary<double, List<Table>> Mixes, 
                        ILogger Logger )
            : base(TypeEnum.Table,
                   AlgorithmEnum.Table, 
                   Name)
        {
            this.MixNumber = MixNumber;
            this.Values = Cols;
            this.Mixes = Mixes;
            this.Logger = Logger.IsNotNull();
        }

        public override Denomination Calculate(Dictionary<string, double> CurrencyAmounts, Dictionary<string, CashUnitStorage> CashUnits, int MaxDispensableItems)
        {
            // Loop through the table looking for a mix with the given value.
            Dictionary<string, int> denom = new();

            foreach (var ca in CurrencyAmounts)
            {
                // Loop through all the mixes
                foreach (var mix in Mixes)
                {
                    // Check if this mix specifies the required amount.
                    if (mix.Key != ca.Value)
                        continue;

                    // Found a matching amount. 
                    // Now find the denomination for this mix, and check that it can be dispensed.
                    Dictionary<string, int> newDenom = new();
                    foreach (var value in mix.Value)
                    {
                        foreach (var count in value.Counts)
                        {
                            // Loop through cash units
                            foreach (var unit in CashUnits)
                            {
                                if (unit.Value.Unit.Configuration.Currency == ca.Key &&
                                    unit.Value.Unit.Configuration.Value == count.Key)
                                {
                                    // Found one and check there is enough cash
                                    if (count.Value > unit.Value.Unit.Status.Count)
                                    {
                                        // Maybe there are another units has save value
                                        continue;
                                    }
                                    else
                                    {
                                        if (newDenom.ContainsKey(unit.Key))
                                        {
                                            Logger.Warning(Constants.Framework, $"Invalid cash unit key. the key representing cash unit must be unique. {unit.Key}");
                                            return new Denomination(CurrencyAmounts);
                                        }
                                        newDenom.Add(unit.Key, count.Value);
                                    }
                                }
                            }
                        }
                        // Check the table is find or not
                        if (newDenom.Select(d => d.Value).Sum() == value.Counts.Select(v => v.Value).Sum())
                        {
                            // Found one
                            break;
                        }

                        newDenom.Clear();
                    }

                    if (newDenom.Count == 0)
                    {
                        // Could not find table can be dispensable for this amount
                        return new Denomination(CurrencyAmounts);
                    }

                    if (new Denominate(CurrencyAmounts, newDenom, Logger).IsDispensable(CashUnits) == Denominate.DispensableResultEnum.Good)
                    {
                        // Go to next currency and amount
                        foreach (var d in newDenom)
                        {
                            if (denom.ContainsKey(d.Key))
                                denom[d.Key] += d.Value;
                            else
                                denom.Add(d.Key, d.Value);
                        }

                        break;
                    }
                    else
                    {
                        // One of cash unit is not available to dispense
                        return new Denomination(CurrencyAmounts);
                    }
                }
            }

            return new Denomination(CurrencyAmounts, denom);
        }

        /// <summary>
        /// Number identifying the house mix table.
        /// </summary>
        public int MixNumber { get; init; }

        public List<double> Values { get; init; }

        /// <summary>
        /// Key is a amount and value is a table of conbination of the mixes
        /// Value - The quantity of each item denomination in the mix
        /// </summary>
        public Dictionary<double, List<Table>> Mixes { get; init; }
    }

    /// <summary>
    /// MinNumberMix
    /// Select a mix requiring the minimum possible number of items
    /// </summary>
    [Serializable()]
    public sealed class MinNumberMix : Mix
    {
        private readonly ILogger Logger;

        public MinNumberMix(ILogger Logger)
            : base(TypeEnum.Algorithm, 
                   AlgorithmEnum.minimumBills, 
                   "Minimum Number Of Bills")
        {
            this.Logger = Logger.IsNotNull();
        }

        /// <summary>
        /// Calculate a dispensable denomination with a given value, using the  number of bills.
        /// 
        ///     8 5 1
        /// 10  1   2   3
        /// 10  0 2 0   2
        ///
        ///     8 5
        /// 10    2
        ///
        ///     10 8 5 1
        /// 20   1 1   2   3
        /// 20   1 0 2 0   2
        /// </summary>
        /// <param name="CurrencyAmounts">Value to denominate and Currency to denominate in.</param>
        /// <param name="CashUnits">cash units to denominate from.</param>
        /// <param name="MaxDispensableItems"></param>
        /// <returns>Calculated denomination</returns>
        public override Denomination Calculate(Dictionary<string, double> CurrencyAmounts, Dictionary<string, CashUnitStorage> CashUnits, int MaxDispensableItems )
        {
            CurrencyAmounts.IsNotNull($"Currency and amounts should be provided to calculate mix.");
            CashUnits.IsNotNull($"Cash unit information should be provided to calculate mix.");

            //Array to make sure use cassettes only once
            List<string> CassetteUsed = new();

            Dictionary<string, int> totalSmallest = new();

            foreach (var ca in CurrencyAmounts)
            {
                // Find the greatest value note of this currency. This will be the first 
                // cash unit used.
                string currentCashUnit = FindNextGreatest(0, ca.Key, CashUnits, ref CassetteUsed);
                if (string.IsNullOrEmpty(currentCashUnit))
                {
                    return new Denomination(CurrencyAmounts);
                }

                // Find the greatest number from this cash unit which could be needed, and  can be 
                // dispensed. If we can't get enough notes we have to make up the rest with smaller
                // denominations.
                int greatestNumNotes;
                if (CashUnits[currentCashUnit].Unit.Configuration.Value != 0)
                {
                    greatestNumNotes = (int)(ca.Value / CashUnits[currentCashUnit].Unit.Configuration.Value);
                }
                else
                {
                    greatestNumNotes = 0;
                }
                if (greatestNumNotes > CashUnits[currentCashUnit].Unit.Status.Count)
                    greatestNumNotes = CashUnits[currentCashUnit].Unit.Status.Count;

                // We will have to dispense at least this many notes. If this is too many notes, 
                // there is no point in continuing the calculation.
                if (greatestNumNotes > MaxDispensableItems)
                {
                    return new Denomination(CurrencyAmounts);
                }

                double remainingAmount;
                int currentSmallestDenomNumNotes = 0;
                int currentSmallestNumNotes = greatestNumNotes;
                bool foundOne = false;
                int newDenomNumNotes;
                Dictionary<string, int> currentSmallest = new ();
                Dictionary<string, int> newDenom = new ();

                // Loop through all the posible dispenses from this cash unit to find the smallest.
                for (int numNotes = greatestNumNotes; numNotes >= 0; numNotes--)
                {
                    remainingAmount = ca.Value - (numNotes * CashUnits[currentCashUnit].Unit.Configuration.Value);
                    // If the remaining amount is zero, we know that the denomination will be null
                    if (remainingAmount == 0)
                    {
                        newDenomNumNotes = 0;
                        newDenom.Clear();
                    }
                    else
                    {
                        if (!CalculateR(remainingAmount, ca.Key, currentCashUnit, CashUnits, ref CassetteUsed, ref newDenom, Logger)) 
                            continue; // If we can't do this bit of the mix, ignore it and go on to the next possibility. 

                        newDenomNumNotes = currentSmallest.Select(d => d.Value).Sum();
                    }
                    // Always take the first one, else take it only if it is smaller than the previouse smallest
                    // and this number of notes can be dispensed from this cash unit
                    if (foundOne == false ||
                        (numNotes < CashUnits[currentCashUnit].Unit.Status.Count &&
                         newDenomNumNotes + numNotes < currentSmallestDenomNumNotes + currentSmallestNumNotes)
                        )
                    {
                        foundOne = true;
                        currentSmallestNumNotes = numNotes;
                        currentSmallestDenomNumNotes = newDenomNumNotes;
                        currentSmallest = newDenom;
                    }
                }

                if (currentSmallest.ContainsKey(currentCashUnit))
                    currentSmallest[currentCashUnit] = currentSmallestNumNotes;
                else
                    currentSmallest.Add(currentCashUnit, currentSmallestNumNotes);

                foreach (var smallest in currentSmallest)
                {
                    if (totalSmallest.ContainsKey(smallest.Key))
                        totalSmallest[smallest.Key] = smallest.Value;
                    else
                        totalSmallest.Add(smallest.Key, smallest.Value);
                }
            }

            // At this point. CurrentSmallestNumNotes is the number of notes from cash unit we should use
            // and CurrentSmallest is the smallest denomination required from the rest of the cash units
            // Add the notes from this cash unit to the denomination and return it.
            Denominate denom = new(CurrencyAmounts, totalSmallest, Logger);

            // Check that this denomination can actually be dispensed.
            if (denom.IsDispensable(CashUnits) != Denominate.DispensableResultEnum.Good)
            {
                return new Denomination(CurrencyAmounts);
            }

            return denom.Denomination;
        }

        /// <summary>
        /// Recursive version of Calculate.
        /// Calculates the smallest denomination for the given amount, (and currency and cash units) from the cash units with values lower than the value of the given cash unit.
        /// Smallest denomination is 
        /// Min(PosibleNumberOfCurrentCashUnit + CalcualteR(CurrentCashUnit, Remainder))
        /// </summary>
        /// <param name="Amount">Amount to find the minimum denomination for.</param>
        /// <param name="Currency">Currency of notes to use</param>
        /// <param name="LastCashUnit">Last cash unit that was used. </param>
        /// <param name="CashUnits">cash units to use</param>
        /// <param name="UnitsUsed">List of used cash units on mixing</param>
        /// <param name="Denom">Denomination to dispense</param>
        /// <param name="Logger">DI Logger</param>
        /// <returns></returns>
        public static bool CalculateR(double Amount, string Currency, string LastCashUnit, Dictionary<string, CashUnitStorage> CashUnits, ref List<string> UnitsUsed, ref Dictionary<string, int> Denom, ILogger Logger)
        {
            Contracts.Assert(Amount != 0, "Invalid parameter used for amount in CalculateR.");

            // Find the next cash unit to check
            string currentCashUnit = FindNextGreatest(CashUnits[LastCashUnit].Unit.Configuration.Value, Currency, CashUnits, ref UnitsUsed);
            // We've run out of cash units. The remaining amount shouldn't be zero so we can't dispense.
            if (string.IsNullOrEmpty(currentCashUnit))
                return false;

            // Check now if this is the last cash unit.
            bool finalCashUnit = false;
            List<string> dummy = new();
            string nextCashUnit = FindNextGreatest(CashUnits[currentCashUnit].Unit.Configuration.Value, Currency, CashUnits, ref dummy);
            if (string.IsNullOrEmpty(nextCashUnit))
                finalCashUnit = true;

            // Find the greatest number from this cash unit which could be needed, and  can be 
            // dispensed. If we can't get enough notes we have to make up the rest with smaller
            // denominations.
            int greatestNumNotes;
            if (CashUnits[currentCashUnit].Unit.Configuration.Value != 0)
            {
                greatestNumNotes = (int)(Amount / CashUnits[currentCashUnit].Unit.Configuration.Value);
            }
            else
            {
                greatestNumNotes = 0;
            }

            if (greatestNumNotes > CashUnits[currentCashUnit].Unit.Status.Count)
                greatestNumNotes = CashUnits[currentCashUnit].Unit.Status.Count;

            double remainingAmount;
            int currentSmallestDenomNumNotes = 0;
            int currentSmallestNumNotes = greatestNumNotes;
            bool foundOne = false;
            int newDenomNumNotes;
            Dictionary<string, int> newDenom = new();
            //  Loop through all the posible dispenses from this cash unit to find the 
            // smallest.
            for (int numNotes = greatestNumNotes; numNotes >= 0; numNotes--)
            {
                remainingAmount = Amount - (numNotes * CashUnits[currentCashUnit].Unit.Configuration.Value);
                // If the remaining amount isn't zero, and this is the last cash unit, then 
                // we can't mix the remaining amount, so this number of notes is invalid.
                if (remainingAmount != 0 && 
                    finalCashUnit == true)
                    continue;

                // If the remaining amount is zero, we know that the denomination will be null
                if (remainingAmount == 0)
                {
                    newDenom.Clear();
                    newDenomNumNotes = 0;
                }
                else
                {
                    if (!CalculateR(remainingAmount, Currency, currentCashUnit, CashUnits, ref UnitsUsed, ref newDenom, Logger)) 
                        continue; // If we can't do this bit of the mix, ignore it and go on to the next possibility. 
                    newDenomNumNotes = newDenom.Select(d => d.Value).Sum();
                }
                // Always take the first one, else take if only if it is smaller than the previouse smallest
                if (foundOne == false ||
                    newDenomNumNotes + numNotes < currentSmallestDenomNumNotes + currentSmallestNumNotes)
                {
                    foundOne = true;
                    currentSmallestNumNotes = numNotes;
                    currentSmallestDenomNumNotes = newDenomNumNotes;
                    Denom = newDenom;
                }
            }

            // At this point. CurrentSmallestNumNotes is the number of notes from cash unit we should use
            // and CurrentSmallest is the smallest denomination required from the rest of the cash units
            // Add the notes from this cash unit to the denomination and return it.
            if (Denom.ContainsKey(currentCashUnit))
                Denom[currentCashUnit] = currentSmallestNumNotes;
            else
                Denom.Add(currentCashUnit, currentSmallestNumNotes);

            if (!new Denominate(new Dictionary<string, double>() { { Currency, Amount } }, Denom, Logger).CheckTotalAmount(CashUnits))
            {
                if (UnitsUsed.Contains(currentCashUnit))
                    UnitsUsed.Remove(currentCashUnit);
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// EqualEmptyingMix
    /// Mix notes to give equal use of each logical cash unit.
    /// </summary>
    [Serializable()]
    public sealed class EqualEmptyingMix : Mix
    {
        private readonly ILogger Logger;

        public EqualEmptyingMix(ILogger Logger )
            : base(TypeEnum.Algorithm,
                   AlgorithmEnum.equalEmptying,
                   "Equal emptying of cash units")
        {
            this.Logger = Logger.IsNotNull();
        }

        /// <summary>
        /// Calculate a denomination with the given value, such that each cash unit will have equal usage.
        ///
        /// Algorithm:	Calculate sum of cash unit values.
        ///             If the amount is smaller than this sum, use a special case.
        ///             Otherwise, divide the amount by the sum, take that many notes from each cash unit.
        ///             Use the minnotes algorithm to make up the remainder.
        ///             Currently the low case also uses the minnotes algorithm.
        ///             
        /// </summary>
        /// <param name="CurrencyAmounts">Value to denominate and Currency to denominate in.</param>
        /// <param name="CashUnits">cash units to denominate from.</param>
        /// <param name="MaxDispensableItems"></param>
        /// <returns>denominate that matches the requirements</returns>
        public override Denomination Calculate(Dictionary<string, double> CurrencyAmounts, Dictionary<string, CashUnitStorage> CashUnits, int MaxDispensableItems )
        {
            CurrencyAmounts.IsNotNull($"Currency and amounts should be provided to calculate mix.");
            CashUnits.IsNotNull($"Cash unit information should be provided to calculate mix.");

            //Array to make sure use cassettes only once
            List<string> CassetteUsed = new();
            Dictionary<string, int> totalEqualEmptying = new();
            foreach (var ca in CurrencyAmounts)
            {
                Dictionary<string, int> newDenom = new();

                string biggestCashUnit = FindNextGreatest(0, ca.Key, CashUnits, ref CassetteUsed);
                string smallestCashUnit = FindNextLeast(0, ca.Key, CashUnits, ref CassetteUsed);

                if (string.IsNullOrEmpty(biggestCashUnit))
                {
                    Logger.Warning(Constants.Framework, "Unable to find upper bounds cash unit.");
                    return new Denomination(CurrencyAmounts);
                }
                if (string.IsNullOrEmpty(smallestCashUnit))
                {
                    Logger.Warning(Constants.Framework, "Unable to find lower bounds cash unit.");
                    return new Denomination(CurrencyAmounts);
                }

                // Find the sum of the cash unit values.
                double sum = 0;
                foreach (var unit in CashUnits)
                {
                    // If this cash unit isn't valid, skip it.
                    if (!unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) || 
                        unit.Value.Unit.Configuration.Currency != ca.Key ||
                        unit.Value.Status != CashUnitStorage.StatusEnum.Good ||
                        (unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                         unit.Value.Unit.Status.ReplenishmentStatus == CashStatusClass.ReplenishmentStatusEnum.Empty) ||
                        unit.Value.Unit.Configuration.AppLockOut)
                    {
                        continue;
                    }
                    else
                    {
                        sum += unit.Value.Unit.Configuration.Value;
                    }
                }

                if (ca.Value < sum)
                {
                    // If the amount is equal to the biggest cash unit, try to deliver this with smaller
                    // notes first.
                    if (ca.Value == CashUnits[biggestCashUnit].Unit.Configuration.Value)
                    {
                        if (!CalculateLow(ca.Value, ca.Key, biggestCashUnit, CashUnits, ref CassetteUsed, ref newDenom, Logger))
                        {
                            CalculateLow(ca.Value, ca.Key, string.Empty, CashUnits, ref CassetteUsed, ref newDenom, Logger);
                        }
                    }
                    else
                    {
                        CalculateLow(ca.Value, ca.Key, string.Empty, CashUnits, ref CassetteUsed, ref newDenom, Logger);
                    }

                }
                else
                {
                    int equalNumNotes = (int)(ca.Value / sum);
                    double remainder = 0;
                    bool mixFailure = false;

                    do
                    {
                        mixFailure = false;
                        newDenom.Clear();
                        // Attempt to remove this number of notes from each cash unit.  If a cash unit
                        // contains fewer notes, take all of them.
                        foreach (var unit in CashUnits)
                        {
                            if (!unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) ||
                                unit.Value.Unit.Configuration.Currency != ca.Key ||
                                unit.Value.Status != CashUnitStorage.StatusEnum.Good ||
                                (unit.Value.Status == CashUnitStorage.StatusEnum.Good &&
                                 unit.Value.Unit.Status.ReplenishmentStatus == CashStatusClass.ReplenishmentStatusEnum.Empty) ||
                                unit.Value.Unit.Configuration.AppLockOut)
                            {
                                // If this cash unit isn't valid, skip it.
                                continue;
                            }
                            else if (equalNumNotes > unit.Value.Unit.Status.Count)
                            {
                                if (newDenom.ContainsKey(unit.Key))
                                    newDenom[unit.Key] = unit.Value.Unit.Status.Count;
                                else
                                    newDenom.Add(unit.Key, unit.Value.Unit.Status.Count);
                            }
                            else
                            {
                                if (newDenom.ContainsKey(unit.Key))
                                    newDenom[unit.Key] = equalNumNotes;
                                else
                                    newDenom.Add(unit.Key, equalNumNotes);
                            }
                        }

                        if (Denominate.GetTotalAmount(ca.Key, newDenom, CashUnits) > ca.Value)
                        {
                            mixFailure = true;
                            // Check to ensure the Remainder is not negative
                            if (equalNumNotes > 0)
                            {
                                equalNumNotes--;
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Find the amount remaining to be allocated.  Don't use EqualNumNotes * sum, because
                        // some of the cash units might have fewer notes than EqualNumNotes
                        remainder = ca.Value - Denominate.GetTotalAmount(ca.Key, newDenom, CashUnits);


                        // Use the Low algorithm for the remainder
                        if (!CalculateLow(remainder, ca.Key, string.Empty, CashUnits, ref CassetteUsed, ref newDenom, Logger))
                        {
                            if (equalNumNotes > 0)
                            {
                                equalNumNotes--;
                            }
                            mixFailure = true;
                        }

                        // Check that this denomination can actually be dispensed.
                        if (mixFailure == false &&
                            new Denominate(new Dictionary<string, double>() { { ca.Key, ca.Value } }, newDenom, Logger).IsDispensable(CashUnits) != Denominate.DispensableResultEnum.Good)
                        {
                            if (equalNumNotes > 0)
                            {
                                equalNumNotes--;
                            }
                            mixFailure = true;
                        }

                        // Consistency check
                        if (Denominate.GetTotalAmount(ca.Key, newDenom, CashUnits) != ca.Value)
                        {
                            if (equalNumNotes > 0)
                            {
                                equalNumNotes--;
                            }
                            mixFailure = true;
                        }

                    } while (equalNumNotes > 0 && mixFailure == true);

                    if (mixFailure)
                    {
                        // We can't allocate notes based on the Equal mix.  Bail out and try the MinNotes
                        // algorithm for the entire amount.
                        Denomination tempDenom = new MinNumberMix(Logger).Calculate(new Dictionary<string, double>() { { ca.Key, ca.Value } }, CashUnits, MaxDispensableItems);
                        newDenom = tempDenom?.Values;
                    }
                }

                // Check that this denomination can actually be dispensed.
                if (new Denominate(new Dictionary<string, double>(){ { ca.Key, ca.Value } }, newDenom, Logger).IsDispensable(CashUnits) != Denominate.DispensableResultEnum.Good)
                {
                    return new Denomination(CurrencyAmounts);
                }

                foreach (var d in newDenom)
                {
                    if (totalEqualEmptying.ContainsKey(d.Key))
                        totalEqualEmptying[d.Key] += d.Value;
                    else
                        totalEqualEmptying.Add(d.Key, d.Value);
                }
            }

            // At this point. CurrentSmallestNumNotes is the number of notes from cash unit we should use
            // and CurrentSmallest is the smallest denomination required from the rest of the cash units
            // Add the notes from this cash unit to the denomination and return it.
            Denominate denom = new(CurrencyAmounts, totalEqualEmptying, Logger);

            // Check that this denomination can actually be dispensed.
            if (denom.IsDispensable(CashUnits) != Denominate.DispensableResultEnum.Good)
            {
                return new Denomination(CurrencyAmounts);
            }

            return denom.Denomination;
        }

        private bool CalculateLow(double Amount, string Currency, string LastCashUnit, Dictionary<string, CashUnitStorage> CashUnits, ref List<string> UnitsUsed, ref Dictionary<string, int> Denom, ILogger Logger)
        {
            CashUnits.IsNotNull($"Cash unit information should be provided to calculate mix.");
            Denom.IsNotNull($"An empty denomination passed in. " + nameof(CalculateLow));

            Dictionary<string, int> originalTotalDenomination = new(Denom);

            // Find the greatest value note of this currency. This will be the first 
            // note used.
            string currentCashUnit;
            if (string.IsNullOrEmpty(LastCashUnit))
                currentCashUnit = FindNextGreatest(0, Currency, CashUnits, ref UnitsUsed);
            else
                currentCashUnit = FindNextGreatest(CashUnits[LastCashUnit].Unit.Configuration.Value, Currency, CashUnits, ref UnitsUsed);
            if (string.IsNullOrEmpty(currentCashUnit))
            {
                Logger.Warning(Constants.Framework, "Failed to find cash unit to denominate in " + nameof(CalculateLow));
                return false;
            }

            double remainingAmount = Amount;

            //Find the cash unit with the next biggest value of bill of the correct 
            // currency in it.
            do
            {
                CashUnits.ContainsKey(currentCashUnit).IsTrue($"Unexpected value of key for the cash unit found. {currentCashUnit}");

                // Find the number of notes of this value which are needed.
                int numNotes = (int)(remainingAmount / CashUnits[currentCashUnit].Unit.Configuration.Value);
                // If we can't dispense this many notes, dispence what we can and make up the rest from 
                // the other cash units.
                if (Denom.ContainsKey(currentCashUnit) &&
                    (CashUnits[currentCashUnit].Unit.Status.Count + Denom[currentCashUnit] < numNotes))
                {
                    numNotes = CashUnits[currentCashUnit].Unit.Status.Count;
                }

                if (Denom.ContainsKey(currentCashUnit))
                    Denom[currentCashUnit] += numNotes;
                else
                    Denom.Add(currentCashUnit, numNotes);

                // Find how much will be left after these notes are included.
                remainingAmount -= numNotes * CashUnits[currentCashUnit].Unit.Configuration.Value;
                currentCashUnit = FindNextGreatest(CashUnits[currentCashUnit].Unit.Configuration.Value, Currency, CashUnits, ref UnitsUsed);
            }
            while (!string.IsNullOrEmpty(currentCashUnit));

            // If the amount remaining isn't zero, then we can't dispense this value.
            if (remainingAmount != 0)
            {
                MinNumberMix.CalculateR(Amount, Currency, LastCashUnit, CashUnits, ref UnitsUsed, ref Denom, Logger);
                foreach (var oriDenom in originalTotalDenomination)
                {
                    if (Denom.ContainsKey(oriDenom.Key))
                        Denom[oriDenom.Key] += oriDenom.Value;
                    else
                        Denom.Add(oriDenom.Key, oriDenom.Value);
                }
            }

            // Don't check the total value of the Denomination, because we do that in the caller.
            return Denom.Select(c => c.Value).Sum() != 0;
        }
    }
}
