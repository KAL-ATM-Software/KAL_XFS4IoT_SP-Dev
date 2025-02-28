/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DefineLayout_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = DefineLayout
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Keyboard.DefineLayout")]
    public sealed class DefineLayoutCommand : Command<DefineLayoutCommand.PayloadData>
    {
        public DefineLayoutCommand(int RequestId, DefineLayoutCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(LayoutClass Layout = null)
                : base()
            {
                this.Layout = Layout;
            }

            /// <summary>
            /// Specify layouts to define.
            /// </summary>
            [DataMember(Name = "layout")]
            public LayoutClass Layout { get; init; }

        }
    }
}
