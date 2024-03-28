/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Storage
{
    /// <summary>
    /// Base class for all type of item units.
    /// </summary>
    public abstract record UnitStorageBase
    {
        public enum StatusEnum
        {
            Good,
            Inoperative,
            Missing,
            NotConfigured,
            Manipulated,
        }

        public UnitStorageBase(
            string PositionName,
            int Capacity,
            StatusEnum Status,
            string SerialNumber)
        {
            this.PositionName = PositionName;
            this.Capacity = Capacity;
            this.Status = Status;
            this.SerialNumber = SerialNumber;
        }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public string PositionName { get; init; }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public int Capacity { get; init; }

        /// <summary>
        /// Status of this storage
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// The storage unit's serial number if it can be read electronically.
        /// </summary>
        public string SerialNumber { get; init; }
    }
}
