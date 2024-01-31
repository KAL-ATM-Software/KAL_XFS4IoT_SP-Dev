/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRaw_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = PrintRaw
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.PrintRaw")]
    public sealed class PrintRawCommand : Command<PrintRawCommand.PayloadData>
    {
        public PrintRawCommand(int RequestId, PrintRawCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(InputDataEnum? InputData = null, List<byte> Data = null)
                : base()
            {
                this.InputData = InputData;
                this.Data = Data;
            }

            public enum InputDataEnum
            {
                No,
                Yes
            }

            /// <summary>
            /// Specifies that input data from the device is expected in response to sending the raw data (i.e. the
            /// data contains a command requesting data). Possible values are:
            /// 
            /// * ```no``` - No input data is expected.
            /// * ```yes``` - Input data is expected.
            /// </summary>
            [DataMember(Name = "inputData")]
            public InputDataEnum? InputData { get; init; }

            /// <summary>
            /// Base64 encoded device dependent data to be sent to the device.
            /// <example>UmF3RGF0YQ==</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Data { get; init; }

        }
    }
}
