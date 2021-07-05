/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CloseShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = CloseShutter
    [DataContract]
    [Command(Name = "CashAcceptor.CloseShutter")]
    public sealed class CloseShutterCommand : Command<CloseShutterCommand.PayloadData>
    {
        public CloseShutterCommand(int RequestId, CloseShutterCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PositionEnum? Position = null)
                : base(Timeout)
            {
                this.Position = Position;
            }

            public enum PositionEnum
            {
                Null,
                InLeft,
                InRight,
                InCenter,
                InTop,
                InBottom,
                InFront,
                InRear,
                OutLeft,
                OutRight,
                OutCenter,
                OutTop,
                OutBottom,
                OutFront,
                OutRear
            }

            /// <summary>
            /// Position where the shutter is to be closed. 
            /// If the application does not need to specify the shutter, this field can be omitted or set to \"null\". 
            /// Otherwise this field should be set to one of the following values:
            /// 
            /// \"null\": The default configuration information should be used.
            /// 
            /// \"inLeft\": Close the shutter of the left input position.
            /// 
            /// \"inRight\": Close the shutter of the right input position.
            /// 
            /// \"inCenter\": Close the shutter of the center input position.
            /// 
            /// \"inTop\": Close the shutter of the top input position.
            /// 
            /// \"inBottom\": Close the shutter of the bottom input position.
            /// 
            /// \"inFront\": Close the shutter of the front input position.
            /// 
            /// \"inRear\": Close the shutter of the rear input position.
            /// 
            /// \"outLeft\": Close the shutter of the left output position.
            /// 
            /// \"outRight\": Close the shutter of the right output position.
            /// 
            /// \"outCenter\": Close the shutter of the center output position.
            /// 
            /// \"outTop\": Close the shutter of the top output position.
            /// 
            /// \"outBottom\": Close the shutter of the bottom output position.
            /// 
            /// \"outFront\": Close the shutter of the front output position.
            /// 
            /// \"outRear\": Close the shutter of the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

        }
    }
}
