/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * ErrorEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Common.ErrorEvent")]
    public sealed class ErrorEvent : UnsolicitedEvent<ErrorEvent.PayloadData>
    {

        public ErrorEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(EventIdEnum? EventId = null, ActionEnum? Action = null, string VendorDescription = null)
                : base()
            {
                this.EventId = EventId;
                this.Action = Action;
                this.VendorDescription = VendorDescription;
            }

            public enum EventIdEnum
            {
                Hardware,
                Software,
                User,
                FraudAttempt
            }

            /// <summary>
            /// Specifies the type of error. Following values are possible:
            /// 
            /// * ```hardware``` - The error is a hardware error.
            /// * ```software``` - The error is a software error.
            /// * ```user``` - The error is a user error.
            /// * ```fraudAttempt``` - Some devices are capable of identifying a malicious physical attack which attempts
            /// to defraud valuable information or media. In this circumstance, this is returned to indicate a fraud
            /// attempt has occurred.
            /// </summary>
            [DataMember(Name = "eventId")]
            public EventIdEnum? EventId { get; init; }

            public enum ActionEnum
            {
                Reset,
                SoftwareError,
                Configuration,
                Clear,
                Maintenance,
                Suspend
            }

            /// <summary>
            /// The action required to manage the error. This can be null if no action is required. Possible values
            /// are:
            /// 
            /// * ```reset``` - Reset device to attempt recovery using a *Reset* command, but should not be used
            /// excessively due to the potential risk of damage to the device. Intervention is not required, although if
            /// repeated attempts are unsuccessful then *maintenance* may be reported.
            /// * ```softwareError``` - A software error occurred. Contact software vendor.
            /// * ```configuration``` - A configuration error occurred. Check configuration.
            /// * ```clear``` - Recovery is not possible. A manual intervention for clearing the device is required. This
            /// value is typically returned when a hardware error has occurred which banking personnel may be able to
            /// clear without calling for technical maintenance, e.g. 'replace paper', or 'remove cards from retain bin'.
            /// * ```maintenance``` - Recovery is not possible. A technical maintenance intervention is required.
            /// This value is only used for hardware errors and fraud attempts. This value is typically returned when a
            /// hardware error or fraud attempt has occurred which requires field engineer specific maintenance activity.
            /// A *Reset* command may be used to attempt recovery after intervention, but should not be used excessively
            /// due to the potential risk of damage to the device. [Vendor Application](#vendorapplication) may be
            /// required to recover the device.
            /// * ```suspend``` - Device will attempt auto recovery and will advise any further action required via a
            /// [Common.StatusChangedEvent](#common.statuschangedevent) or another *Common.ErrorEvent*.
            /// </summary>
            [DataMember(Name = "action")]
            public ActionEnum? Action { get; init; }

            /// <summary>
            /// A vendor-specific description of the error. May be null if not applicable.
            /// <example>An error occurred in position B.</example>
            /// </summary>
            [DataMember(Name = "vendorDescription")]
            public string VendorDescription { get; init; }

        }

    }
}
