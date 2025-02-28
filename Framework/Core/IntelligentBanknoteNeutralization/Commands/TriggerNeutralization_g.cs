/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT IntelligentBanknoteNeutralization interface.
 * TriggerNeutralization_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.IntelligentBanknoteNeutralization.Commands
{
    //Original name = TriggerNeutralization
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "IntelligentBanknoteNeutralization.TriggerNeutralization")]
    public sealed class TriggerNeutralizationCommand : Command<TriggerNeutralizationCommand.PayloadData>
    {
        public TriggerNeutralizationCommand(int RequestId, TriggerNeutralizationCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(NeutralizationActionEnum? NeutralizationAction = null, string Token = null)
                : base()
            {
                this.NeutralizationAction = NeutralizationAction;
                this.Token = Token;
            }

            public enum NeutralizationActionEnum
            {
                Trigger
            }

            /// <summary>
            /// The new trigger state that the client requests.
            /// * ```trigger``` - trigger the banknote neutralization
            /// <example>trigger</example>
            /// </summary>
            [DataMember(Name = "neutralizationAction")]
            public NeutralizationActionEnum? NeutralizationAction { get; init; }

            /// <summary>
            /// The token that authorizes the operation, as created by the authorizing host. See 
            /// the section on [end-to-end security](#api.e2esecurity) for more information.
            /// 
            /// The token contains a nonce returned by [Common.GetCommandNonce](#common.getcommandnonce) which must match 
            /// the nonce stored in the hardware.
            /// 
            /// The hardware will also track the token being used and block any attempt to use multiple tokens with the 
            /// same nonce. Any attempt to use a different token will trigger an *invalidToken* error.
            /// 
            /// For maximum security the client should also explicitly clear the command nonce (and hence invalidate the 
            /// existing tokens,) with the [Common.ClearCommandNonce](#common.clearcommandnonce) command as soon as it's 
            /// finished using the current token.
            /// <example>NONCE=1234567890,TOKENFORMAT=1,TOKENLENGTH=0152,NEUTRALIZATIONACTION=TRIGGER,HMACSHA256=30AB309F2D5FDD4F88420BB680D5851AD7D34F16D666564E92B4CE91680B86AD</example>
            /// </summary>
            [DataMember(Name = "token")]
            [DataTypes(Pattern = @"^(?=[!-~]{0,1024}$)NONCE=[0-9A-F]+,TOKENFORMAT=1,TOKENLENGTH=[0-9]{4},(?:[A-Z0-9]+=[^,=]+?,)+HMACSHA256=[0-9A-F]{64}$")]
            public string Token { get; init; }

        }
    }
}
