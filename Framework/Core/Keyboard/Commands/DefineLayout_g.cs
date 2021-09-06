/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Keyboard.DefineLayout")]
    public sealed class DefineLayoutCommand : Command<DefineLayoutCommand.PayloadData>
    {
        public DefineLayoutCommand(int RequestId, DefineLayoutCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<LayoutClass> Layout = null)
                : base(Timeout)
            {
                this.Layout = Layout;
            }

            /// <summary>
            /// Specify layouts to define.  
            /// </summary>
            [DataMember(Name = "layout")]
            public List<LayoutClass> Layout { get; init; }

        }
    }
}
