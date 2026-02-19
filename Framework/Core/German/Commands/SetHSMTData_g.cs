/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * SetHSMTData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.German.Commands
{
    //Original name = SetHSMTData
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "German.SetHSMTData")]
    public sealed class SetHSMTDataCommand : Command<SetHSMTDataCommand.PayloadData>
    {
        public SetHSMTDataCommand()
            : base()
        { }

        public SetHSMTDataCommand(int RequestId, SetHSMTDataCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string TerminalId = null, string BankCode = null, string OnlineDateAndTime = null)
                : base()
            {
                this.TerminalId = TerminalId;
                this.BankCode = BankCode;
                this.OnlineDateAndTime = OnlineDateAndTime;
            }

            /// <summary>
            /// Terminal ID. ISO 8583 BMP 41 (see [[Ref. german-2](#ref-german-2)]). A data source is the EPP.
            /// This property is null if not applicable.
            /// <example>00054321</example>
            /// </summary>
            [DataMember(Name = "terminalId")]
            [DataTypes(Pattern = @"^[0-9]{8}$")]
            public string TerminalId { get; init; }

            /// <summary>
            /// Bank code. ISO 8583 BMP 42 (rightmost 4 bytes see [[Ref. german-2](#ref-german-2)])
            /// Account data for terminal account. A data source is the EPP.
            /// This property is null if not applicable.
            /// <example>00000414</example>
            /// </summary>
            [DataMember(Name = "bankCode")]
            [DataTypes(Pattern = @"^[0-9]{8}$")]
            public string BankCode { get; init; }

            /// <summary>
            /// Online date and time. ISO 8583 BMP 61 (YYYYMMDDHHMMSS) see [[Ref. german-2](#ref-german-2)].
            /// A data source is the HSM.
            /// This property is null if not applicable.
            /// <example>20240919105500</example>
            /// </summary>
            [DataMember(Name = "onlineDateAndTime")]
            [DataTypes(Pattern = @"^20\\d{2}(0[1-9]|1[0,1,2])(0[1-9]|[12][0-9]|3[01])(0[0-9]|1[0-9]|2[0-3])[0-5][0-9][0-5][0-9]$")]
            public string OnlineDateAndTime { get; init; }

        }
    }
}
