/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Common.PowerSaveControl")]
    public sealed class PowerSaveControlCommand : Command<PowerSaveControlCommand.PayloadData>
    {
        public PowerSaveControlCommand(int RequestId, PowerSaveControlCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? MaxPowerSaveRecoveryTime = null)
                : base(Timeout)
            {
                this.MaxPowerSaveRecoveryTime = MaxPowerSaveRecoveryTime;
            }

            /// <summary>
            /// Specifies the maximum number of seconds in which the device must be able to return to its normal operating state when exiting power save mode. The device will be set to the highest possible power save mode within this constraint. If usMaxPowerSaveRecoveryTime is set to zero then the device will exit the power saving mode. 
            /// </summary>
            [DataMember(Name = "maxPowerSaveRecoveryTime")]
            public int? MaxPowerSaveRecoveryTime { get; private set; }

        }
    }
}
