/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * Count_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Dispenser.Completions
{
    [DataContract]
    [Completion(Name = "Dispenser.Count")]
    public sealed class CountCompletion : Completion<CountCompletion.PayloadData>
    {
        public CountCompletion(int RequestId, CountCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Dictionary<string, CountedCashUnitsClass> CountedCashUnits = null)
                : base(CompletionCode, ErrorDescription)
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
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A cash unit caused a problem. A CashManagement.CashUnitErrorEvent will be posted with the details.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```safeDoorOpen``` - The safe door is open. This device requires the safe door to be closed in order to perform this operation.
            /// * ```exchangeActive``` - The device is in an exchange state (see 
            /// [CashManagement.StartExchange](#cashmanagement.startexchange)).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class CountedCashUnitsClass
            {
                public CountedCashUnitsClass(string PhysicalPositionName = null, string UnitId = null, int? Dispensed = null, int? Counted = null, StatusEnum? Status = null)
                {
                    this.PhysicalPositionName = PhysicalPositionName;
                    this.UnitId = UnitId;
                    this.Dispensed = Dispensed;
                    this.Counted = Counted;
                    this.Status = Status;
                }

                /// <summary>
                /// Specifies which cash unit was emptied and counted. This name is the same as the 
                /// *physicalPositionName* in the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) completion message.
                /// </summary>
                [DataMember(Name = "physicalPositionName")]
                public string PhysicalPositionName { get; init; }

                /// <summary>
                /// Cash unit ID. This is the identifier defined in the *unitID* field in the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) completion message.
                /// </summary>
                [DataMember(Name = "unitId")]
                public string UnitId { get; init; }

                /// <summary>
                /// The number of items that were dispensed during the emptying of the cash unit.
                /// </summary>
                [DataMember(Name = "dispensed")]
                public int? Dispensed { get; init; }

                /// <summary>
                /// The number of items that were counted during the emptying of the cash unit.
                /// </summary>
                [DataMember(Name = "counted")]
                public int? Counted { get; init; }

                public enum StatusEnum
                {
                    Ok,
                    Full,
                    High,
                    Low,
                    Empty,
                    Inoperative,
                    Missing,
                    NoValue,
                    NoReference,
                    Manipulated
                }

                /// <summary>
                /// Supplies the status of the cash unit. Following values are possible:
                /// 
                /// * ```ok``` - The cash unit is in a good state.
                /// * ```full``` - The cash unit is full.
                /// * ```high``` - The cash unit is almost full (i.e. reached or exceeded the threshold defined by *maximum*). 
                /// * ```low``` - The cash unit is almost empty (i.e. reached or below the threshold defined by *minimum*). 
                /// * ```empty``` - The cash unit is empty, or insufficient items in the cash unit are preventing further dispense operations.
                /// * ```inoperative``` - The cash unit is inoperative.
                /// * ```missing``` - The cash unit is missing.
                /// * ```noValue``` - The values of the specified cash unit are not available.
                /// * ```noReference``` - There is no reference value available for the notes in this cash unit. The cash unit has not been calibrated.
                /// * ```manipulated``` - The cash unit has been inserted (including removal followed by a reinsertion) when the device 
                /// was not in the exchange state. This cash unit cannot be dispensed from.
                /// </summary>
                [DataMember(Name = "status")]
                public StatusEnum? Status { get; init; }

            }

            /// <summary>
            /// List of counted cash unit objects.
            /// </summary>
            [DataMember(Name = "countedCashUnits")]
            public Dictionary<string, CountedCashUnitsClass> CountedCashUnits { get; init; }

        }
    }
}
