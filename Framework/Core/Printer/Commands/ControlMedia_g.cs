/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.ControlMedia")]
    public sealed class ControlMediaCommand : Command<ControlMediaCommand.PayloadData>
    {
        public ControlMediaCommand(int RequestId, ControlMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(MediaControlClass MediaControl = null)
                : base()
            {
                this.MediaControl = MediaControl;
            }

            /// <summary>
            /// Specifies the manner in which the media should be handled.
            /// 
            /// In the descriptions, *flush data* means flush any data to the printer that has not yet been printed from
            /// previous [Printer.PrintForm](#printer.printform) or [Printer.PrintNative](#printer.printnative) commands.
            /// 
            /// An application should be aware that the sequence of the actions is not guaranteed if more than one
            /// property is specified in this parameter.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlClass MediaControl { get; init; }

        }
    }
}
