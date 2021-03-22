/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * PowerSaveChangeEvent_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{

    [DataContract]
    [Event(Name = "Common.PowerSaveChangeEvent")]
    public sealed class PowerSaveChangeEvent : Event<PowerSaveChangeEvent.PayloadData>
    {

        public PowerSaveChangeEvent(string RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {


            public PayloadData(int? PowerSaveRecoveryTime = null)
                : base()
            {
                this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
            }

            /// <summary>
            ///Specifies the actual number of seconds required by the device to resume its normal operational state. This value is zero if the device exited the power saving mode
            /// </summary>
            [DataMember(Name = "powerSaveRecoveryTime")] 
            public int? PowerSaveRecoveryTime { get; private set; }
        }

    }
}
