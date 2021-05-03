/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ReadForm
    [DataContract]
    [Command(Name = "Printer.ReadForm")]
    public sealed class ReadFormCommand : Command<ReadFormCommand.PayloadData>
    {
        public ReadFormCommand(string RequestId, ReadFormCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            /// <summary>
            /// Specifies the manner in which the media should be handled after the reading was done and can be a
            /// combination of the following flags. The
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) flag is not
            /// applicable to this command.
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


            public PayloadData(int Timeout, string FormName = null, List<string> FieldNames = null, string MediaName = null, object MediaControl = null)
                : base(Timeout)
            {
                this.FormName = FormName;
                this.FieldNames = FieldNames;
                this.MediaName = MediaName;
                this.MediaControl = MediaControl;
            }

            /// <summary>
            /// The name of the form.
            /// </summary>
            [DataMember(Name = "formName")] 
            public string FormName { get; private set; }
            /// <summary>
            /// The field names from which to read input data. If this is omitted or empty, all input fields on the
            /// form will be read.
            /// </summary>
            [DataMember(Name = "fieldNames")] 
            public List<string> FieldNames{ get; private set; }
            /// <summary>
            /// The media name. If omitted or empty, no media definition applies.
            /// </summary>
            [DataMember(Name = "mediaName")] 
            public string MediaName { get; private set; }
            /// <summary>
            /// Specifies the manner in which the media should be handled after the reading was done and can be a
            /// combination of the following flags. The
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) flag is not
            /// applicable to this command.
            /// </summary>
            [DataMember(Name = "mediaControl")] 
            public object MediaControl { get; private set; }

        }
    }
}
