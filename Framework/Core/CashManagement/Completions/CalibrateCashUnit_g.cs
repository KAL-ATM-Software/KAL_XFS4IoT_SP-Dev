/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "CashManagement.CalibrateCashUnit")]
    public sealed class CalibrateCashUnitCompletion : Completion<CalibrateCashUnitCompletion.PayloadData>
    {
        public CalibrateCashUnitCompletion(int RequestId, CalibrateCashUnitCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Unit = null, int? NumOfBills = null, ItemPositionClass Position = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Unit = Unit;
                this.NumOfBills = NumOfBills;
                this.Position = Position;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                UnsupportedPosition,
                ExchangeActive,
                InvalidCashUnit
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A cash unit caused an error. A 
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```unsupportedPosition``` - The position specified is not valid.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```invalidCashUnit``` - The cash unit number specified is not valid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The object name of the cash unit which has been calibrated as stated by the 
            /// [Storage.GetStorage](#storage.getstorage) command.
            /// <example>20USD1</example>
            /// </summary>
            [DataMember(Name = "unit")]
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
            /// Specifies where the items were moved to during the calibration process.
            /// </summary>
            [DataMember(Name = "position")]
            public ItemPositionClass Position { get; init; }

        }
    }
}
