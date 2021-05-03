/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ControlMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ControlMedia
    [DataContract]
    [Command(Name = "Printer.ControlMedia")]
    public sealed class ControlMediaCommand : Command<ControlMediaCommand.PayloadData>
    {
        public ControlMediaCommand(string RequestId, ControlMediaCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            /// <summary>
            /// Specifies the manner in which the media should be handled, as a combination of the following flags:
            /// 
            /// It is not possible to combine the flags eject, retract, park, expel and ejectToTransport with each
            /// other otherwise the command completes with errInvalidData.
            /// 
            /// It is not possible to combine the flag
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) with any other flags,
            /// otherwise the command completes with *invalidData*.
            /// 
            /// An application should be aware that the sequence of the actions is not guaranteed if more than one
            /// flag is specified in this parameter.
            /// </summary>
            public class MediaControlClass
            {
                [DataMember(Name = "eject")] 
                public bool? Eject { get; private set; }
                [DataMember(Name = "perforate")] 
                public bool? Perforate { get; private set; }
                [DataMember(Name = "cut")] 
                public bool? Cut { get; private set; }
                [DataMember(Name = "skip")] 
                public bool? Skip { get; private set; }
                [DataMember(Name = "flush")] 
                public bool? Flush { get; private set; }
                [DataMember(Name = "retract")] 
                public bool? Retract { get; private set; }
                [DataMember(Name = "stack")] 
                public bool? Stack { get; private set; }
                [DataMember(Name = "partialCut")] 
                public bool? PartialCut { get; private set; }
                [DataMember(Name = "alarm")] 
                public bool? Alarm { get; private set; }
                [DataMember(Name = "forward")] 
                public bool? Forward { get; private set; }
                [DataMember(Name = "backward")] 
                public bool? Backward { get; private set; }
                [DataMember(Name = "turnMedia")] 
                public bool? TurnMedia { get; private set; }
                [DataMember(Name = "stamp")] 
                public bool? Stamp { get; private set; }
                [DataMember(Name = "park")] 
                public bool? Park { get; private set; }
                [DataMember(Name = "expel")] 
                public bool? Expel { get; private set; }
                [DataMember(Name = "ejectToTransport")] 
                public bool? EjectToTransport { get; private set; }
                [DataMember(Name = "rotate180")] 
                public bool? Rotate180 { get; private set; }
                [DataMember(Name = "clearBuffer")] 
                public bool? ClearBuffer { get; private set; }

                public MediaControlClass (bool? Eject, bool? Perforate, bool? Cut, bool? Skip, bool? Flush, bool? Retract, bool? Stack, bool? PartialCut, bool? Alarm, bool? Forward, bool? Backward, bool? TurnMedia, bool? Stamp, bool? Park, bool? Expel, bool? EjectToTransport, bool? Rotate180, bool? ClearBuffer)
                {
                    this.Eject = Eject;
                    this.Perforate = Perforate;
                    this.Cut = Cut;
                    this.Skip = Skip;
                    this.Flush = Flush;
                    this.Retract = Retract;
                    this.Stack = Stack;
                    this.PartialCut = PartialCut;
                    this.Alarm = Alarm;
                    this.Forward = Forward;
                    this.Backward = Backward;
                    this.TurnMedia = TurnMedia;
                    this.Stamp = Stamp;
                    this.Park = Park;
                    this.Expel = Expel;
                    this.EjectToTransport = EjectToTransport;
                    this.Rotate180 = Rotate180;
                    this.ClearBuffer = ClearBuffer;
                }


            }


            public PayloadData(int Timeout, object MediaControl = null)
                : base(Timeout)
            {
                this.MediaControl = MediaControl;
            }

            /// <summary>
            /// Specifies the manner in which the media should be handled, as a combination of the following flags:
            /// 
            /// It is not possible to combine the flags eject, retract, park, expel and ejectToTransport with each
            /// other otherwise the command completes with errInvalidData.
            /// 
            /// It is not possible to combine the flag
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) with any other flags,
            /// otherwise the command completes with *invalidData*.
            /// 
            /// An application should be aware that the sequence of the actions is not guaranteed if more than one
            /// flag is specified in this parameter.
            /// </summary>
            [DataMember(Name = "mediaControl")] 
            public object MediaControl { get; private set; }

        }
    }
}
