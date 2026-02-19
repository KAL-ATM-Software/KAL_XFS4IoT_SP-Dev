/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * DispensePaper_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = DispensePaper
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.DispensePaper")]
    public sealed class DispensePaperCommand : Command<DispensePaperCommand.PayloadData>
    {
        public DispensePaperCommand()
            : base()
        { }

        public DispensePaperCommand(int RequestId, DispensePaperCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string PaperSource = null)
                : base()
            {
                this.PaperSource = PaperSource;
            }

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
