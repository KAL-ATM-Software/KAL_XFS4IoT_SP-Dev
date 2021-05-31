/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [Event(Name = "CardReader.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : Event<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ResetOutEnum? ResetOut = null)
                : base()
            {
                this.ResetOut = ResetOut;
            }

            public enum ResetOutEnum
            {
                Ejected,
                Retained,
                ReadPosition,
                Jammed
            }

            /// <summary>
            /// Specifies the action that was performed on any card found within the device as one of the following:
            /// 
            /// * ```ejected``` - The card was ejected.
            /// * ```retained``` - The card was retained.
            /// * ```readPosition``` - The card is in read position.
            /// * ```jammed``` - The card is jammed in the device.
            /// </summary>
            [DataMember(Name = "resetOut")]
            public ResetOutEnum? ResetOut { get; private set; }

        }

    }
}
