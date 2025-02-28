/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintNative_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = PrintNative
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.PrintNative")]
    public sealed class PrintNativeCommand : Command<PrintNativeCommand.PayloadData>
    {
        public PrintNativeCommand(int RequestId, PrintNativeCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<byte> Data = null, MediaControlNullableClass MediaControl = null, string PaperSource = null)
                : base()
            {
                this.Data = Data;
                this.MediaControl = MediaControl;
                this.PaperSource = PaperSource;
            }

            /// <summary>
            /// The data to be printed.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Data { get; init; }

            /// <summary>
            /// Specifies the manner in which the media should be handled after each page is printed.
            /// If null or no options are set, no actions will be performed, as when printing multiple
            /// pages on a single media item.
            /// 
            /// In the descriptions, *flush data* means flush any data to the printer that has not yet been printed from
            /// previous [Printer.PrintForm](#printer.printform) or [Printer.PrintNative](#printer.printnative) commands.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlNullableClass MediaControl { get; init; }

            /// <summary>
            /// Specifies the paper source to be used. For commands which print, this parameter is ignored if there is already
            /// paper in the print position. It can be one of the following:
            /// 
            /// * ```upper``` - Use the only paper source or the upper paper source, if there is more than one paper
            ///                 supply.
            /// * ```lower``` - Use the lower paper source.
            /// * ```external``` - Use the external paper.
            /// * ```aux``` - Use the auxiliary paper source.
            /// * ```aux2``` - Use the second auxiliary paper source.
            /// * ```park``` - Use the parking station paper source.
            /// * ```any``` - Use any paper source, it is determined by the service.
            /// * ```[paper source identifier]``` - The vendor specific paper source.
            /// <example>lower</example>
            /// </summary>
            [DataMember(Name = "paperSource")]
            [DataTypes(Pattern = @"^upper$|^lower$|^external$|^aux$|^aux2$|^park$|^any$|^[a-zA-Z]([a-zA-Z0-9]*)$")]
            public string PaperSource { get; init; }

        }
    }
}
