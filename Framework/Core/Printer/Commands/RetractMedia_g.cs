/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RetractMedia_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = RetractMedia
    [DataContract]
    [Command(Name = "Printer.RetractMedia")]
    public sealed class RetractMediaCommand : Command<RetractMediaCommand.PayloadData>
    {
        public RetractMediaCommand(string RequestId, RetractMediaCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? BinNumber = null)
                : base(Timeout)
            {
                this.BinNumber = BinNumber;
            }

            /// <summary>
            ///This number has to be between one and the number of bins supported by this device. If omitted, the media will be retracted to the transport. After it has been retracted to the transport, in a subsequent operation the media can be ejected again, or retracted to one of the retract bins.
            /// </summary>
            [DataMember(Name = "binNumber")] 
            public int? BinNumber { get; private set; }

        }
    }
}
