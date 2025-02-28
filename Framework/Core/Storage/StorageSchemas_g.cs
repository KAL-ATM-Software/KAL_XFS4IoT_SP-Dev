/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StorageSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Storage
{

    public enum StatusEnum
    {
        Ok,
        Inoperative,
        Missing,
        NotConfigured,
        Manipulated
    }


    [DataContract]
    public sealed class StorageUnitClass
    {
        public StorageUnitClass(string Id = null, string PositionName = null, int? Capacity = null, StatusEnum? Status = null, string SerialNumber = null, CashManagement.StorageCashClass Cash = null, CardReader.StorageClass Card = null, Check.StorageClass Check = null, Deposit.StorageClass Deposit = null, IntelligentBanknoteNeutralization.StorageUnitStatusClass BanknoteNeutralization = null, Printer.StorageClass Printer = null)
        {
            this.Id = Id;
            this.PositionName = PositionName;
            this.Capacity = Capacity;
            this.Status = Status;
            this.SerialNumber = SerialNumber;
            this.Cash = Cash;
            this.Card = Card;
            this.Check = Check;
            this.Deposit = Deposit;
            this.BanknoteNeutralization = BanknoteNeutralization;
            this.Printer = Printer;
        }

        /// <summary>
        /// An identifier which can be used for cUnitID in CDM/CIM XFS 3.x migration. May be null if not applicable.
        /// <example>RC1</example>
        /// </summary>
        [DataMember(Name = "id")]
        [DataTypes(Pattern = @"^.{1,5}$")]
        public string Id { get; init; }

        /// <summary>
        /// Fixed physical name for the position. May be null if not applicable.
        /// <example>Top Right</example>
        /// </summary>
        [DataMember(Name = "positionName")]
        public string PositionName { get; init; }

        /// <summary>
        /// The nominal capacity of the unit. This may be an estimate as the quality and thickness of the items
        /// stored in the unit may affect how many items can be stored. 0 means the capacity is unknown; null means capacity
        /// is not applicable.
        /// <example>100</example>
        /// </summary>
        [DataMember(Name = "capacity")]
        [DataTypes(Minimum = 0)]
        public int? Capacity { get; init; }

        [DataMember(Name = "status")]
        public StatusEnum? Status { get; init; }

        /// <summary>
        /// The storage unit's serial number if it can be read electronically. May be null if not applicable.
        /// <example>ABCD1234</example>
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; init; }

        [DataMember(Name = "cash")]
        public CashManagement.StorageCashClass Cash { get; init; }

        [DataMember(Name = "card")]
        public CardReader.StorageClass Card { get; init; }

        [DataMember(Name = "check")]
        public Check.StorageClass Check { get; init; }

        [DataMember(Name = "deposit")]
        public Deposit.StorageClass Deposit { get; init; }

        [DataMember(Name = "banknoteNeutralization")]
        public IntelligentBanknoteNeutralization.StorageUnitStatusClass BanknoteNeutralization { get; init; }

        [DataMember(Name = "printer")]
        public Printer.StorageClass Printer { get; init; }

    }


    [DataContract]
    public sealed class SetStorageUnitClass
    {
        public SetStorageUnitClass(CashManagement.StorageSetCashClass Cash = null, CardReader.StorageSetClass Card = null, Check.StorageSetClass Check = null, Deposit.StorageSetClass Deposit = null, Printer.StorageSetClass Printer = null)
        {
            this.Cash = Cash;
            this.Card = Card;
            this.Check = Check;
            this.Deposit = Deposit;
            this.Printer = Printer;
        }

        [DataMember(Name = "cash")]
        public CashManagement.StorageSetCashClass Cash { get; init; }

        [DataMember(Name = "card")]
        public CardReader.StorageSetClass Card { get; init; }

        [DataMember(Name = "check")]
        public Check.StorageSetClass Check { get; init; }

        [DataMember(Name = "deposit")]
        public Deposit.StorageSetClass Deposit { get; init; }

        [DataMember(Name = "printer")]
        public Printer.StorageSetClass Printer { get; init; }

    }


}
