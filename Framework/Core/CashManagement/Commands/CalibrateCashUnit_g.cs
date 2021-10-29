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
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = CalibrateCashUnit
    [DataContract]
    [Command(Name = "CashManagement.CalibrateCashUnit")]
    public sealed class CalibrateCashUnitCommand : Command<CalibrateCashUnitCommand.PayloadData>
    {
        public CalibrateCashUnitCommand(int RequestId, CalibrateCashUnitCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Unit = null, int? NumOfBills = null, ItemPositionClass Position = null)
                : base(Timeout)
            {
                this.Unit = Unit;
                this.NumOfBills = NumOfBills;
                this.Position = Position;
            }

            /// <summary>
            /// The object name of the storage unit as stated by the [Storage.GetStorage](#storage.getstorage) command.
            /// <example>20USD1</example>
            /// </summary>
            [DataMember(Name = "unit")]
            public string Unit { get; init; }

            /// <summary>
            /// The number of bills to be dispensed during the calibration process. If not specified or 0, the 
            /// service may decide how many bills are required. This may also be ignored if the device always dispenses a 
            /// default number of bills.
            /// <example>40</example>
            /// </summary>
            [DataMember(Name = "numOfBills")]
            [DataTypes(Minimum = 0)]
            public int? NumOfBills { get; init; }

            [DataMember(Name = "position")]
            public ItemPositionClass Position { get; init; }

        }
    }
}
