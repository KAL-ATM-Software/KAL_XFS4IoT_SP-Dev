/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ControlPassbook_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ControlPassbook
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.ControlPassbook")]
    public sealed class ControlPassbookCommand : Command<ControlPassbookCommand.PayloadData>
    {
        public ControlPassbookCommand(int RequestId, ControlPassbookCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ActionEnum? Action = null, int? Count = null)
                : base()
            {
                this.Action = Action;
                this.Count = Count;
            }

            public enum ActionEnum
            {
                Forward,
                Backward,
                CloseForward,
                CloseBackward
            }

            /// <summary>
            /// Specifies the direction of the page turn as one of the following values:
            /// 
            /// * ```forward``` - Turns forward the pages of the passbook.
            /// * ```backward``` - Turns backward the pages of the passbook.
            /// * ```closeForward``` - Close the passbook forward.
            /// * ```closeBackward``` - Close the passbook backward.
            /// </summary>
            [DataMember(Name = "action")]
            public ActionEnum? Action { get; init; }

            /// <summary>
            /// Specifies the number of pages to be turned. If
            /// [action](#printer.controlpassbook.command.properties.action) is *closeForward* or *closeBackward*,
            /// this will be ignored.
            /// <example>3</example>
            /// </summary>
            [DataMember(Name = "count")]
            [DataTypes(Minimum = 1)]
            public int? Count { get; init; }

        }
    }
}
