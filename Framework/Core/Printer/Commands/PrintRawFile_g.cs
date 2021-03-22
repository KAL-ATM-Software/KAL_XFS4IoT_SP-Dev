/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRawFile_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = PrintRawFile
    [DataContract]
    [Command(Name = "Printer.PrintRawFile")]
    public sealed class PrintRawFileCommand : Command<PrintRawFileCommand.PayloadData>
    {
        public PrintRawFileCommand(string RequestId, PrintRawFileCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            /// <summary>
            ///Specifies the manner in which the media should be handled after each page is printed, as a combination of the following flags. If no flags are set, no actions will be performed, as when printing multiple pages on a single media item. Note that the clearBuffer flag is not applicable to this this command and will be ignored.
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

            public enum PaperSourceEnum
            {
                Any,
                Upper,
                Lower,
                External,
                Aux,
                Aux2,
                Park,
            }


            public PayloadData(int Timeout, string FileName = null, object MediaControl = null, PaperSourceEnum? PaperSource = null)
                : base(Timeout)
            {
                this.FileName = FileName;
                this.MediaControl = MediaControl;
                this.PaperSource = PaperSource;
            }

            /// <summary>
            ///This is the full path and file name of the file to be printed. This value cannot contain UNICODE characters.
            /// </summary>
            [DataMember(Name = "fileName")] 
            public string FileName { get; private set; }
            /// <summary>
            ///Specifies the manner in which the media should be handled after each page is printed, as a combination of the following flags. If no flags are set, no actions will be performed, as when printing multiple pages on a single media item. Note that the clearBuffer flag is not applicable to this this command and will be ignored.
            /// </summary>
            [DataMember(Name = "mediaControl")] 
            public object MediaControl { get; private set; }
            /// <summary>
            ///Specifies the paper source to use when printing. If omitted, the Service Provider will determine the paper source that will be used. This parameter is ignored if there is already paper in the print position. Possible values are:**any*
            ////  Any paper source can be used; it is determined by the service.**upper*
            ////  Use the only paper source or the upper paper source, if there is more than one paper supply.**lower**
            ////  Use the lower paper source.**external**
            ////  Use the external paper source (such as envelope tray or single sheet feed).**aux**
            ////  Use the auxiliary paper source.**aux2**
            ////  Use the second auxiliary paper source.**park**
            ////  Use the parking station.
            /// </summary>
            [DataMember(Name = "paperSource")] 
            public PaperSourceEnum? PaperSource { get; private set; }

        }
    }
}
