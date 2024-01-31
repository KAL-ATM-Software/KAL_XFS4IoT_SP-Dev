/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.CalibrateCashUnit")]
    public sealed class CalibrateCashUnitCommand : Command<CalibrateCashUnitCommand.PayloadData>
    {
        public CalibrateCashUnitCommand(int RequestId, CalibrateCashUnitCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Unit = null, int? NumOfBills = null, ItemTargetClass Position = null)
                : base()
            {
                this.Unit = Unit;
                this.NumOfBills = NumOfBills;
                this.Position = Position;
            }

            /// <summary>
            /// The object name of the storage unit as stated by the [Storage.GetStorage](#storage.getstorage) command.
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "unit")]
            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            public string Unit { get; init; }

            /// <summary>
            /// The number of bills to be dispensed during the calibration process. If null, the
            /// Service may decide how many bills are required. This may also be ignored if the device always dispenses a
            /// default number of bills.
            /// <example>40</example>
            /// </summary>
            [DataMember(Name = "numOfBills")]
            [DataTypes(Minimum = 1)]
            public int? NumOfBills { get; init; }

            [DataMember(Name = "position")]
            public ItemTargetClass Position { get; init; }

        }
    }
}
