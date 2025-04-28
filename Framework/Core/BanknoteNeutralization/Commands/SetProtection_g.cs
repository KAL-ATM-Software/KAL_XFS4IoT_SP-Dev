/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BanknoteNeutralization interface.
 * SetProtection_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.BanknoteNeutralization.Commands
{
    //Original name = SetProtection
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "BanknoteNeutralization.SetProtection")]
    public sealed class SetProtectionCommand : Command<SetProtectionCommand.PayloadData>
    {
        public SetProtectionCommand(int RequestId, SetProtectionCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(NewStateEnum? NewState = null, string Token = null)
                : base()
            {
                this.NewState = NewState;
                this.Token = Token;
            }

            public enum NewStateEnum
            {
                Arm,
                IgnoreAllSafeSensors,
                Disarm
            }

            /// <summary>
            /// The protection state that the client requests. Can be used for either enabling, partially deactivating or completely deactivating the banknote neutralization. One of the following values:
            /// 
            /// * ```arm``` - Activate the normal operating mode of the banknote neutralization. The banknote neutralization autonomously activates the protection when the safe door is closed and locked and deactivates it when the safe is legally opened. 
            /// * ```ignoreAllSafeSensors``` - Permanently deactivate all the safe sensors while the banknote neutralization of the Storage Units remain armed.
            /// * ```disarm``` - Permanently deactivate the whole banknote neutralization including the safe intrusion detection and the banknote neutralization in the Storage Units.
            /// <example>disarm</example>
            /// </summary>
            [DataMember(Name = "newState")]
            public NewStateEnum? NewState { get; init; }

            /// <summary>
            /// The SetProtection token that authorizes the operation, as created by the authorizing host. See 
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
            /// <example>NONCE=1234567890,TOKENFORMAT=1,TOKENLENGTH=0139,NEWSTATE=DISARM,HMACSHA256=66EDE19AEE10AFC8F1AC02A1EDA2854FBF5FD4B7EA0D2BC036DA804116689B55</example>
            /// </summary>
            [DataMember(Name = "token")]
            [DataTypes(Pattern = @"^(?=[!-~]{0,1024}$)NONCE=[0-9A-F]+,TOKENFORMAT=1,TOKENLENGTH=[0-9]{4},(?:[A-Z0-9]+=[^,=]+?,)+HMACSHA256=[0-9A-F]{64}$")]
            public string Token { get; init; }

        }
    }
}
