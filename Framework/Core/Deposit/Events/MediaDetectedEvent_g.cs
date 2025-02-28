/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Deposit.Events
{

    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Event(Name = "Deposit.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : UnsolicitedEvent<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(DispenseMediaEnum? DispenseMedia = null, DepositMediaEnum? DepositMedia = null)
                : base()
            {
                this.DispenseMedia = DispenseMedia;
                this.DepositMedia = DepositMedia;
            }

            public enum DispenseMediaEnum
            {
                NoMedia,
                Retracted,
                Dispenser,
                Ejected,
                Jammed,
                Unknown
            }

            /// <summary>
            /// Specifies the dispensed envelope position after the reset operation, as one of the following values or
            /// null if the device does not support dispensing envelopes:
            /// 
            /// * ```noMedia``` - No dispensed media was detected during the reset operation.
            /// * ```retracted``` - The media was retracted into the deposit container during the reset operation.
            /// * ```dispenser``` - The media was retracted into the envelope dispenser during the reset operation.
            /// * ```ejected``` - The media is in the exit slot.
            /// * ```jammed``` - The media is jammed in the device.
            /// * ```unknown``` - The media is in an unknown position.
            /// <example>ejected</example>
            /// </summary>
            [DataMember(Name = "dispenseMedia")]
            public DispenseMediaEnum? DispenseMedia { get; init; }

            public enum DepositMediaEnum
            {
                NoMedia,
                Retracted,
                Dispenser,
                Ejected,
                Jammed,
                Unknown
            }

            /// <summary>
            /// Specifies the deposited media position after the reset operation, as one of the following values:
            /// 
            /// * ```noMedia``` - No dispensed media was detected during the reset operation.
            /// * ```retracted``` - The media was retracted into the deposit container during the reset operation.
            /// * ```dispenser``` - The media was retracted into the envelope dispenser during the reset operation.
            /// * ```ejected``` - The media is in the exit slot.
            /// * ```jammed``` - The media is jammed in the device.
            /// * ```unknown``` - The media is in an unknown position.
            /// <example>jammed</example>
            /// </summary>
            [DataMember(Name = "depositMedia")]
            public DepositMediaEnum? DepositMedia { get; init; }

        }

    }
}
