/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = Reset
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Biometric.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand(int RequestId, ResetCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ClearDataEnum? ClearData = null)
                : base()
            {
                this.ClearData = ClearData;
            }

            public enum ClearDataEnum
            {
                ScannedData,
                ImportedData,
                SetMatchedData
            }

            /// <summary>
            /// This property indicates the type of data to be or which has been cleared from storage.
            /// If this property is null, then all stored data will be or has been cleared.
            /// Available values are described in the [clearData](#common.capabilities.completion.properties.biometric.cleardata).
            /// The following values are possible:
            /// 
            /// * ```scannedData``` - Raw image data that has been scanned using the [Biometric.Read](#biometric.read).
            /// * ```importedData``` - Template data that was imported using the [Biometric.Import](#biometric.import).
            /// * ```setMatchedData``` - Match criteria data that was set using the [Biometric.Match](#biometric.match).
            /// </summary>
            [DataMember(Name = "clearData")]
            public ClearDataEnum? ClearData { get; init; }

        }
    }
}
