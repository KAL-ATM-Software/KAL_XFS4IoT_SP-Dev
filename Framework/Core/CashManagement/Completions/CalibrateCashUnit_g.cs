/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CalibrateCashUnit_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashManagement.CalibrateCashUnit")]
    public sealed class CalibrateCashUnitCompletion : Completion<CalibrateCashUnitCompletion.PayloadData>
    {
        public CalibrateCashUnitCompletion(int RequestId, CalibrateCashUnitCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, ResultClass Result = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Result = Result;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                UnsupportedPosition,
                ExchangeActive,
                InvalidCashUnit
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A storage unit caused an error. A
            ///   [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```unsupportedPosition``` - The position specified is not valid.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```invalidCashUnit``` - The storage unit number specified is not valid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class ResultClass
            {
                public ResultClass(string Unit = null, int? NumOfBills = null, ItemTargetDataClass Position = null)
                {
                    this.Unit = Unit;
                    this.NumOfBills = NumOfBills;
                    this.Position = Position;
                }

                /// <summary>
                /// The object name of the storage unit which has been calibrated as stated by
                /// [Storage.GetStorage](#storage.getstorage).
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "unit")]
                [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
                public string Unit { get; init; }

                /// <summary>
                /// Number of items that were actually dispensed during the calibration process. This value may be different
                /// from that passed in using the input structure if the device always dispenses a default number of
                /// bills. When bills are presented to an output position this is the count of notes presented to the output
                /// position, any other notes rejected during the calibration process are not included in this count as they
                /// will be accounted for within the storage unit counts.
                /// <example>20</example>
                /// </summary>
                [DataMember(Name = "numOfBills")]
                [DataTypes(Minimum = 0)]
                public int? NumOfBills { get; init; }

                /// <summary>
                /// Defines where items have been moved to as one of the following:
                /// 
                /// * A single storage unit, further specified by *unit*.
                /// * Internal areas of the device.
                /// * An output position.
                /// 
                /// This may be null if no items were moved.
                /// </summary>
                [DataMember(Name = "position")]
                public ItemTargetDataClass Position { get; init; }

            }

            /// <summary>
            /// The result of the command, detailing where items were moved from and to. May be null if no items were
            /// moved.
            /// </summary>
            [DataMember(Name = "result")]
            public ResultClass Result { get; init; }

        }
    }
}
