/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetPresentStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = GetPresentStatus
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashDispenser.GetPresentStatus")]
    public sealed class GetPresentStatusCommand : Command<GetPresentStatusCommand.PayloadData>
    {
        public GetPresentStatusCommand(int RequestId, GetPresentStatusCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CashManagement.OutputPositionEnum? Position = null, string Nonce = null)
                : base()
            {
                this.Position = Position;
                this.Nonce = Nonce;
            }

            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

            /// <summary>
            /// A nonce value to be used when creating the end-to-end security token in the
            /// response. If no token is requested this property should be null. See the
            /// generic end-to-end security documentation for more details.
            /// <example>646169ECDD0E440C2CECC8DDD7C27C22</example>
            /// </summary>
            [DataMember(Name = "nonce")]
            [DataTypes(Pattern = @"^[0-9A-F]{32}$|^[0-9]*$")]
            public string Nonce { get; init; }

        }
    }
}
