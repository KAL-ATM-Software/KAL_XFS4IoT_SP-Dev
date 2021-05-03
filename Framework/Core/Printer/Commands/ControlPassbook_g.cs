/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Printer.ControlPassbook")]
    public sealed class ControlPassbookCommand : Command<ControlPassbookCommand.PayloadData>
    {
        public ControlPassbookCommand(string RequestId, ControlPassbookCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ActionEnum
            {
                Forward,
                Backward,
                CloseForward,
                CloseBackward,
            }


            public PayloadData(int Timeout, ActionEnum? Action = null, int? Count = null)
                : base(Timeout)
            {
                this.Action = Action;
                this.Count = Count;
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
            public ActionEnum? Action { get; private set; }
            /// <summary>
            /// Specifies the number of pages to be turned. In the case where
            /// [action](#printer.controlpassbook.command.properties.action) is *closeForward* or closeBackward*, this
            /// field will be ignored.
            /// </summary>
            [DataMember(Name = "count")] 
            public int? Count { get; private set; }

        }
    }
}
