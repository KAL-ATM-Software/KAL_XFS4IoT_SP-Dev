/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessPerformTransaction_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = EMVClessPerformTransaction
    [DataContract]
    [Command(Name = "CardReader.EMVClessPerformTransaction")]
    public sealed class EMVClessPerformTransactionCommand : Command<EMVClessPerformTransactionCommand.PayloadData>
    {
        public EMVClessPerformTransactionCommand(int RequestId, EMVClessPerformTransactionCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Data = null)
                : base(Timeout)
            {
                this.Data = Data;
            }

            /// <summary>
            /// Base64 encoded representation of the EMV data elements in a BER-TLV format required to perform a
            /// transaction. The types of object that could be included are:
            /// 
            /// * Transaction Type (9C)
            /// * Amount Authorized (9F02)
            /// * Transaction Date (9A)*
            /// * Transaction Time (9F21)*
            /// * Transaction Currency Code (5F2A)
            /// 
            /// Individual payment systems could define further data elements.
            /// 
            /// Tags are not mandatory with this command and this value can be omitted.
            /// 
            /// *Tags 9A and 9F21 could be managed internally by the reader. If tags are not supplied, tag values may
            /// be used from the configuration sent previously in the
            /// [CardReader.EMVClessConfigure](#cardreader.emvclessconfigure) command.
            /// </summary>
            [DataMember(Name = "data")]
            public string Data { get; init; }

        }
    }
}
