/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Read_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = Read
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Biometric.Read")]
    public sealed class ReadCommand : Command<ReadCommand.PayloadData>
    {
        public ReadCommand()
            : base()
        { }

        public ReadCommand(int RequestId, ReadCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<DataTypeClass> DataTypes = null, int? NumCaptures = null, ModeEnum? Mode = null)
                : base()
            {
                this.DataTypes = DataTypes;
                this.NumCaptures = NumCaptures;
                this.Mode = Mode;
            }

            /// <summary>
            /// Array of data types, each data element of which represents the data type(s) in which the data should be returned
            /// in the completion payload. If no data is to be returned *dataTypes* can be null. Single or multiple
            /// formats can be returned, or no data can be returned in the case where the scan is to be followed by a
            /// subsequent matching operation.
            /// </summary>
            [DataMember(Name = "dataTypes")]
            public List<DataTypeClass> DataTypes { get; init; }

            /// <summary>
            /// This property indicates the number of times to attempt capture of the biometric data from the subject.
            /// If this is zero, then the device determines how many attempts will be made. The maximum number of captures
            /// possible is indicated by the [maxCapture](#common.capabilities.completion.properties.biometric.maxcapture) capability.
            /// </summary>
            [DataMember(Name = "numCaptures")]
            [DataTypes(Minimum = 0)]
            public int? NumCaptures { get; init; }

            public enum ModeEnum
            {
                Scan,
                Match
            }

            /// <summary>
            /// This optional property indicates the reason why the [Biometric.Read](#biometric.read) has been issued,
            /// to allow for any necessary optimization.
            /// Available values are detailed in the [scanModes](#common.capabilities.completion.properties.biometric.scanmodes).
            /// The following values are possible:
            /// 
            /// * ```scan``` - Scan data only, for example to enroll a user or collect data for matching in an external biometric system.
            /// * ```match``` - Scan data for a match operation using the [Biometric.Match](#biometric.match).
            /// </summary>
            [DataMember(Name = "mode")]
            public ModeEnum? Mode { get; init; }

        }
    }
}
