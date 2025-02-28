/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * PowerSaveControl_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = PowerSaveControl
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.PowerSaveControl")]
    public sealed class PowerSaveControlCommand : Command<PowerSaveControlCommand.PayloadData>
    {
        public PowerSaveControlCommand(int RequestId, PowerSaveControlCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? MaxPowerSaveRecoveryTime = null)
                : base()
            {
                this.MaxPowerSaveRecoveryTime = MaxPowerSaveRecoveryTime;
            }

            /// <summary>
            /// Specifies the maximum number of seconds in which the device must be able to return to its normal operating state when exiting power save mode. The device will be set to the highest possible power save mode within this constraint. If set to 0 then the device will exit the power-saving mode.
            /// <example>5</example>
            /// </summary>
            [DataMember(Name = "maxPowerSaveRecoveryTime")]
            [DataTypes(Minimum = 0)]
            public int? MaxPowerSaveRecoveryTime { get; init; }

        }
    }
}
