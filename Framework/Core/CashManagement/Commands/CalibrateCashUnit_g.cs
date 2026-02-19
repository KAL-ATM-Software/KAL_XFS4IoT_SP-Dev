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
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = CalibrateCashUnit
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.CalibrateCashUnit")]
    public sealed class CalibrateCashUnitCommand : Command<CalibrateCashUnitCommand.PayloadData>
    {
        public CalibrateCashUnitCommand()
            : base()
        { }

        public CalibrateCashUnitCommand(int RequestId, CalibrateCashUnitCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Unit = null, int? NumOfBills = null, ItemTargetDataClass Position = null)
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

            /// <summary>
            /// Defines where items are to be moved to as one of the following:
            /// 
            /// * A single storage unit, further specified by *unit*.
            /// * Internal areas of the device.
            /// * An output position.
            /// 
            /// This may be null if the Service is to determine where items are to be moved.
            /// </summary>
            [DataMember(Name = "position")]
            public ItemTargetDataClass Position { get; init; }

        }
    }
}
