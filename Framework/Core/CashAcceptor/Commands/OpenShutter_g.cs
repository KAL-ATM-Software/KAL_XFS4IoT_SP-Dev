/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * OpenShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = OpenShutter
    [DataContract]
    [Command(Name = "CashAcceptor.OpenShutter")]
    public sealed class OpenShutterCommand : Command<OpenShutterCommand.PayloadData>
    {
        public OpenShutterCommand(int RequestId, OpenShutterCommand.PayloadData Payload)
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
            /// Position where the shutter is to be opened. 
            /// If the application does not need to specify the shutter, this field can be omitted or set to \"null\". 
            /// Otherwise this field should be set to one of the following values:
            /// 
            /// \"null\": The default configuration information should be used.
            /// 
            /// \"inLeft\": Open the shutter of the left input position.
            /// 
            /// \"inRight\": Open the shutter of the right input position.
            /// 
            /// \"inCenter\": Open the shutter of the center input position.
            /// 
            /// \"inTop\": Open the shutter of the top input position.
            /// 
            /// \"inBottom\": Open the shutter of the bottom input position.
            /// 
            /// \"inFront\": Open the shutter of the front input position.
            /// 
            /// \"inRear\": Open the shutter of the rear input position.
            /// 
            /// \"outLeft\": Open the shutter of the left output position.
            /// 
            /// \"outRight\": Open the shutter of the right output position.
            /// 
            /// \"outCenter\": Open the shutter of the center output position.
            /// 
            /// \"outTop\": Open the shutter of the top output position.
            /// 
            /// \"outBottom\": Open the shutter of the bottom output position.
            /// 
            /// \"outFront\": Open the shutter of the front output position.
            /// 
            /// \"outRear\": Open the shutter of the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

        }
    }
}
