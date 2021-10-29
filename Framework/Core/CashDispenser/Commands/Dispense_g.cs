/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Dispense_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = Dispense
    [DataContract]
    [Command(Name = "CashDispenser.Dispense")]
    public sealed class DispenseCommand : Command<DispenseCommand.PayloadData>
    {
        public DispenseCommand(int RequestId, DispenseCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, DenominateRequestClass Denomination = null, CashManagement.OutputPositionEnum? Position = null, string Token = null)
                : base(Timeout)
            {
                this.Denomination = Denomination;
                this.Position = Position;
                this.Token = Token;
            }

            /// <summary>
            /// Denomination object describing the contents of the denomination operation.
            /// </summary>
            [DataMember(Name = "denomination")]
            public DenominateRequestClass Denomination { get; init; }

            /// <summary>
            /// If specified, defines the [output position](#cashmanagement.outputposition) to which the items are to be 
            /// dispensed. If not specified, the items are dispensed to one of the following positions as applicable:
            /// 
            /// * teller position if the device is a Teller Dispenser
            /// * intermediate stacker if the device has one
            /// * the default position if there is no intermediate stacker.
            /// </summary>
            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

            /// <summary>
            /// The dispense token that authorizes the dispense operation, as created by the authorizing host. See 
            /// the section on [end to end security](#api.generalinformation.e2esecurity) for more information. 
            /// 
            /// The same token may be used multiple times with multiple calls to the CashDispenser.Dispense command as long 
            /// as the total value stacked does not exceed the value given in the token. The hardware will track the value 
            /// of the cash that has been dispensed and will raise an invalidToken error for any attempt to dispense more 
            /// cash than authorized by the token. 
            /// 
            /// The token contains a nonce returned by [Common.GetCommandNonce](#common.getcommandnonce) which must match 
            /// the nonce stored in the hardware. The nonce value stored in the hardware will be cleared when cash is 
            /// presented meaning that all tokens will become invalid after cash is presented. 
            /// 
            /// The dispense token will follow the standard token format, and will contain the following key: 
            /// 
            /// ```DISPENSE1```: The maximum value to be dispensed. This will be a number string that may contain a 
            /// fractional part. The decimal character will be ".". The value, including the fractional part, will be 
            /// defined by the ISO currency. The number will be followed by the ISO currency code. The currency 
            /// code will be upper case. 
            /// 
            /// For example, "123.45EUR" will be €123 and 45 cents.
            /// 
            /// The "DISPENSE" key may appear multiple times with a number suffix. For example, DISPENSE1, DISPENSE2, 
            /// DISPENSE3. The number will start at 1 and increment. Each key can only be given once. Each key must 
            /// have a value in a different currency. For example, DISPENSE1=100.00EUR,DISPENSE2=200.00USD   
            /// 
            /// The actual amount dispensed will be given by the denomination. The value in the token MUST be
            /// greater or equal to the amount in the denomination parameter. If the Token has a lower value, 
            /// or the Token is invalid for any reason, then the command will fail with an invalid data error code.
            /// <example>NONCE=254611E63B2531576314E86527338D61,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=50.00EUR,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2</example>
            /// </summary>
            [DataMember(Name = "token")]
            public string Token { get; init; }

        }
    }
}
