/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * TestCashUnits_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = TestCashUnits
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashDispenser.TestCashUnits")]
    public sealed class TestCashUnitsCommand : Command<TestCashUnitsCommand.PayloadData>
    {
        public TestCashUnitsCommand()
            : base()
        { }

        public TestCashUnitsCommand(int RequestId, TestCashUnitsCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CashManagement.ItemTargetDataClass Target = null)
                : base()
            {
                this.Target = Target;
            }

            /// <summary>
            /// Defines where items are to be moved to as one of the following:
            /// 
            /// * A single storage unit, further specified by *unit*.
            /// * Internal areas of the device.
            /// * An output position.
            /// 
            /// This may be null if the Service is to determine where items are to be moved.
            /// </summary>
            [DataMember(Name = "target")]
            public CashManagement.ItemTargetDataClass Target { get; init; }

        }
    }
}
