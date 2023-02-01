/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [Command(Name = "Printer.DispensePaper")]
    public sealed class DispensePaperCommand : Command<DispensePaperCommand.PayloadData>
    {
        public DispensePaperCommand(int RequestId, DispensePaperCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string PaperSource = null)
                : base(Timeout)
            {
                this.PaperSource = PaperSource;
            }

            /// <summary>
            /// Specifes the paper source to be used. 
            /// This property can be omitted if any paper source to be used and the paper source is determined by the service.
            /// It can be one of the following:
            /// 
            /// * ```upper``` - Use the only paper source or the upper paper source, if there is more than one paper
            ///                 supply.
            /// * ```lower``` - Use the lower paper source.
            /// * ```external``` - Use the external paper.
            /// * ```aux``` - Use the auxiliary paper source.
            /// * ```aux2``` - Use the second auxiliary paper source.
            /// * ```park``` - Use the parking station paper source.
            /// * ```&lt;paper source identifier&gt;``` - The vendor specific paper source.
            /// </summary>
            [DataMember(Name = "paperSource")]
            [DataTypes(Pattern = @"^upper$|^lower$|^external$|^aux$|^aux2$|^park$|^[a-zA-Z]([a-zA-Z0-9]*)$")]
            public string PaperSource { get; init; }

        }
    }
}
