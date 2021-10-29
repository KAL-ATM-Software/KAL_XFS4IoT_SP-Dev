/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = Reset
    [DataContract]
    [Command(Name = "CashManagement.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand(int RequestId, ResetCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Unit = null, RetractClass RetractArea = null, OutputPositionEnum? OutputPosition = null)
                : base(Timeout)
            {
                this.Unit = Unit;
                this.RetractArea = RetractArea;
                this.OutputPosition = OutputPosition;
            }

            /// <summary>
            /// If defined, this value specifies the object name (as stated by the 
            /// [Storage.GetStorage](#storage.getstorage) command) of the single unit to 
            /// be used for the storage of any items found.
            /// <example>unit5</example>
            /// </summary>
            [DataMember(Name = "unit")]
            public string Unit { get; init; }

            /// <summary>
            /// This field is used if items are to be moved to internal areas of the device, including storage units, the
            /// intermediate stacker, or the transport.
            /// </summary>
            [DataMember(Name = "retractArea")]
            public RetractClass RetractArea { get; init; }

            [DataMember(Name = "outputPosition")]
            public OutputPositionEnum? OutputPosition { get; init; }

        }
    }
}
