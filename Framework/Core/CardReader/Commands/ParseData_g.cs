/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ParseData_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = ParseData
    [DataContract]
    [Command(Name = "CardReader.ParseData")]
    public sealed class ParseDataCommand : Command<ParseDataCommand.PayloadData>
    {
        public ParseDataCommand(string RequestId, ParseDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            [DataContract]
            public sealed class DataClass
            {
                /// <summary>
                ///The status values applicable to all data sources. Possible values are:**ok**
                ////The data is OK.**dataMissing**
                ////The track/chip/memory chip is blank.**dataInvalid**
                ////The data contained on the track/chip/memory chip is invalid. This will typically be returned when *data* reports **badReadLevel** or **dataInvalid**.**dataTooLong**
                ////The data contained on the track/chip/memory chip is too long.**dataTooShort**
                ////The data contained on the track/chip/memory chip is too short.**dataSourceNotSupported**
                ////The data source to read from is not supported by the Service Provider.**dataSourceMissing**
                ////The data source to read from is missing on the card, or is unable to be read due to a hardware problem, or the module has not been initialized. For example, this will be returned on a request to read a Memory Card and the customer has entered a magnetic card without associated memory chip. This will also be reported when *data* reports **noData**, **notInitialized** or **hardwareError**. This will also be reported when the image reader could not create a BMP file due to the state of the image reader or due to a failure.
                /// </summary>
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }

                public string data;

                public DataClass(StatusEnum? Status = null, string Data = null)
                    : base()
                {
                    this.Status = Status;
                    this.Data = Data;
                }

                /// <summary>
                ///The status values applicable to all data sources. Possible values are:**ok**
                ////The data is OK.**dataMissing**
                ////The track/chip/memory chip is blank.**dataInvalid**
                ////The data contained on the track/chip/memory chip is invalid. This will typically be returned when *data* reports **badReadLevel** or **dataInvalid**.**dataTooLong**
                ////The data contained on the track/chip/memory chip is too long.**dataTooShort**
                ////The data contained on the track/chip/memory chip is too short.**dataSourceNotSupported**
                ////The data source to read from is not supported by the Service Provider.**dataSourceMissing**
                ////The data source to read from is missing on the card, or is unable to be read due to a hardware problem, or the module has not been initialized. For example, this will be returned on a request to read a Memory Card and the customer has entered a magnetic card without associated memory chip. This will also be reported when *data* reports **noData**, **notInitialized** or **hardwareError**. This will also be reported when the image reader could not create a BMP file due to the state of the image reader or due to a failure.
                /// </summary>
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }

                /// <summary>
                ///Base64 encoded representation of the data
                /// </summary>
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

            }


            public PayloadData(int Timeout, string FormName = null, List<DataClass> Data = null)
                : base(Timeout)
            {
                this.FormName = FormName;
                this.Data = Data;
            }

            /// <summary>
            ///The name of the form that defines the behavior for the reading of tracks.
            /// </summary>
            [DataMember(Name = "formName")] 
            public string FormName { get; private set; }
            /// <summary>
            ///An array of card data structures.
            /// </summary>
            [DataMember(Name = "data")] 
            public List<DataClass> Data{ get; private set; }

        }
    }
}
