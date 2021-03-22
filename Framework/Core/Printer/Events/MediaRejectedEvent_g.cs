/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaRejectedEvent_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.MediaRejectedEvent")]
    public sealed class MediaRejectedEvent : Event<MediaRejectedEvent.PayloadData>
    {

        public MediaRejectedEvent(string RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public enum ReasonEnum
            {
                Short,
                Long,
                Multiple,
                Align,
                MoveToAlign,
                Shutter,
                Escrow,
                Thick,
                Other,
            }


            public PayloadData(ReasonEnum? Reason = null)
                : base()
            {
                this.Reason = Reason;
            }

            /// <summary>
            ///Specifies the reason for rejecting the media as one of the following values:**short**
            ////  The rejected media was too short.**long**
            ////  The rejected media was too long.**multiple**
            ////  The media was rejected due to insertion of multiple documents.**align**
            ////  The media could not be aligned and was rejected.**moveToAlign**
            ////  The media could not be transported to the align area and was rejected.**shutter**
            ////  The media was rejected due to the shutter failing to close.**escrow**
            ////  The media was rejected due to problems transporting media to the escrow position.**thick**
            ////  The rejected media was too thick.**other**
            ////  The media was rejected due to a reason other than those listed above.
            /// </summary>
            [DataMember(Name = "reason")] 
            public ReasonEnum? Reason { get; private set; }
        }

    }
}
