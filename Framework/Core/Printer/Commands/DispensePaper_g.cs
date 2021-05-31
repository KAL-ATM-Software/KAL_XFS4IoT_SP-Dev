/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

            public PayloadData(int Timeout, PaperSourceEnum? PaperSource = null)
                : base(Timeout)
            {
                this.PaperSource = PaperSource;
            }

            public enum PaperSourceEnum
            {
                Any,
                Upper,
                Lower,
                External,
                Aux,
                Aux2,
                Park
            }

            /// <summary>
            /// The paper source to dispense from. It can be one of the following:
            /// 
            /// * ```any``` - Any paper source can be used; it is determined by the service.
            /// * ```upper``` - Use the only paper source or the upper paper source, if there is more than one paper
            ///   supply.
            /// * ```lower``` - Use the lower paper source.
            /// * ```internal``` - Use the external paper.
            /// * ```aux``` - Use the auxiliary paper source.
            /// * ```aux2``` - Use the second auxiliary paper source.
            /// * ```park``` - Use the parking station paper source.
            /// </summary>
            [DataMember(Name = "paperSource")]
            public PaperSourceEnum? PaperSource { get; private set; }

        }
    }
}
