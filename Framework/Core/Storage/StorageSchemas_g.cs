/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        public StorageUnitClass(string PositionName = null, int? Capacity = null, StatusEnum? Status = null, string SerialNumber = null, CashManagement.StorageCashClass Cash = null)
        {
            this.PositionName = PositionName;
            this.Capacity = Capacity;
            this.Status = Status;
            this.SerialNumber = SerialNumber;
            this.Cash = Cash;
        }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        [DataMember(Name = "positionName")]
        public string PositionName { get; init; }

        /// <summary>
        /// The nominal capacity of the unit. This may be an estimate as the quality and thickness of the items
        /// stored in the unit may affect how many items can be stored. 0 means the capacity is unknown.
        /// </summary>
        [DataMember(Name = "capacity")]
        [DataTypes(Minimum = 0)]
        public int? Capacity { get; init; }

        [DataMember(Name = "status")]
        public StatusEnum? Status { get; init; }

        /// <summary>
        /// The storage unit's serial number if it can be read electronically.
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; init; }

        /// <summary>
        /// The cash related contents, status and configuration of the unit.
        /// </summary>
        [DataMember(Name = "cash")]
        public CashManagement.StorageCashClass Cash { get; init; }

    }


    [DataContract]
    public sealed class SetStorageUnitClass
    {
        public SetStorageUnitClass(CashManagement.StorageSetCashClass Cash = null)
        {
            this.Cash = Cash;
        }

        /// <summary>
        /// The cash related contents and configuration of the unit.
        /// </summary>
        [DataMember(Name = "cash")]
        public CashManagement.StorageSetCashClass Cash { get; init; }

    }


    [DataContract]
    public sealed class StorageClass
    {
        public StorageClass(Dictionary<string, StorageUnitClass> Storage = null)
        {
            this.Storage = Storage;
        }

        /// <summary>
        /// Object containing storage unit information.
        /// </summary>
        [DataMember(Name = "storage")]
        public Dictionary<string, StorageUnitClass> Storage { get; init; }

    }


}
