/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * SetActiveInterface_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.VendorApplication.Commands
{
    //Original name = SetActiveInterface
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "VendorApplication.SetActiveInterface")]
    public sealed class SetActiveInterfaceCommand : Command<SetActiveInterfaceCommand.PayloadData>
    {
        public SetActiveInterfaceCommand()
            : base()
        { }

        public SetActiveInterfaceCommand(int RequestId, SetActiveInterfaceCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ActiveInterfaceEnum? ActiveInterface = null)
                : base()
            {
                this.ActiveInterface = ActiveInterface;
            }

            public enum ActiveInterfaceEnum
            {
                Consumer,
                Operator
            }

            /// <summary>
            /// Specifies the active interface as one of the following values:
            /// 
            /// * ```consumer``` - The consumer interface.
            /// * ```operator``` - The operator interface.
            /// </summary>
            [DataMember(Name = "activeInterface")]
            public ActiveInterfaceEnum? ActiveInterface { get; init; }

        }
    }
}
