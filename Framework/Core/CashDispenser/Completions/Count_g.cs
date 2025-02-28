/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Count_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashDispenser.Count")]
    public sealed class CountCompletion : Completion<CountCompletion.PayloadData>
    {
        public CountCompletion(int RequestId, CountCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, CountedCashUnitsClass> CountedCashUnits = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.CountedCashUnits = CountedCashUnits;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                UnsupportedPosition,
                SafeDoorOpen,
                ExchangeActive
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A storage unit caused a problem. A [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```safeDoorOpen``` - The safe door is open. This device requires the safe door to be closed in order to perform this operation
            ///   (see [Common.Status](#common.status.completion.properties.auxiliaries.safedoor) property).
            /// * ```exchangeActive``` - The device is in an exchange state (see command
            ///   [Storage.StartExchange](#storage.startexchange)).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class CountedCashUnitsClass
            {
                public CountedCashUnitsClass(int? Dispensed = null, int? Counted = null, CashManagement.ReplenishmentStatusEnum? ReplenishmentStatus = null, Storage.StatusEnum? Status = null)
                {
                    this.Dispensed = Dispensed;
                    this.Counted = Counted;
                    this.ReplenishmentStatus = ReplenishmentStatus;
                    this.Status = Status;
                }

                /// <summary>
                /// The number of items that were dispensed during the emptying of the storage unit.
                /// <example>100</example>
                /// </summary>
                [DataMember(Name = "dispensed")]
                [DataTypes(Minimum = 1)]
                public int? Dispensed { get; init; }

                /// <summary>
                /// The number of items that were counted during the emptying of the storage unit.
                /// <example>100</example>
                /// </summary>
                [DataMember(Name = "counted")]
                [DataTypes(Minimum = 1)]
                public int? Counted { get; init; }

                [DataMember(Name = "replenishmentStatus")]
                public CashManagement.ReplenishmentStatusEnum? ReplenishmentStatus { get; init; }

                [DataMember(Name = "status")]
                public Storage.StatusEnum? Status { get; init; }

            }

            /// <summary>
            /// List of counted storage unit objects. Will be null if no units were counted.
            /// </summary>
            [DataMember(Name = "countedCashUnits")]
            public Dictionary<string, CountedCashUnitsClass> CountedCashUnits { get; init; }

        }
    }
}
