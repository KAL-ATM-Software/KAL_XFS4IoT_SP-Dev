/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RawData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = RawData
    [DataContract]
    [Command(Name = "Printer.RawData")]
    public sealed class RawDataCommand : Command<RawDataCommand.PayloadData>
    {
        public RawDataCommand(int RequestId, RawDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, InputDataEnum? InputData = null, string Data = null)
                : base(Timeout)
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
            /// BASE64 encoded device dependent data to be sent to the device.
            /// <example>UmF3RGF0YQ==</example>
            /// </summary>
            [DataMember(Name = "data")]
            public string Data { get; init; }

        }
    }
}
