/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RetractMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = RetractMedia
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.RetractMedia")]
    public sealed class RetractMediaCommand : Command<RetractMediaCommand.PayloadData>
    {
        public RetractMediaCommand(int RequestId, RetractMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string MediaControl = null)
                : base()
            {
                this.MediaControl = MediaControl;
            }

            /// <summary>
            /// Specifies the manner in which the media should be handled, as one of the following:
            /// 
            /// * ```transport``` - Retract the media to the transport. After it has been retracted to the transport, in a
            /// subsequent operation the media can be ejected again, or retracted to one of the retract bins.
            /// * ```unit&lt;retract bin number&gt;``` - Retract the media to retract bin number specified. This number has
            /// to be between 1 and the [number of bins](#common.capabilities.completion.properties.printer.retractbins)
            /// supported by this device.
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "mediaControl")]
            [DataTypes(Pattern = @"^transport$|^unit[0-9]+$")]
            public string MediaControl { get; init; }

        }
    }
}
