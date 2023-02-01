/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Clear_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = Clear
    [DataContract]
    [Command(Name = "Biometric.Clear")]
    public sealed class ClearCommand : Command<ClearCommand.PayloadData>
    {
        public ClearCommand(int RequestId, ClearCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ClearDataEnum? ClearData = null)
                : base(Timeout)
            {
                this.ClearData = ClearData;
            }

            /// <summary>
            /// This property indicates the type of data to be cleared from storage. 
            /// If this property is omitted, then all stored data will be cleared.
            /// Available values are described in the [clearData](#common.capabilities.completion.properties.biometric.cleardata).
            /// The following values are possible:
            /// 
            /// * ```scannedData``` - Raw image data that has been scanned using the [Biometric.Read](#biometric.read) can be cleared.
            /// * ```importedData``` - Template data that was imported using the [Biometric.Import](#biometric.import) can be cleared.
            /// * ```setMatchedData``` - Match criteria data that was set using the [Biometric.Match](#biometric.match) can be cleared.
            /// </summary>
            [DataMember(Name = "clearData")]
            public ClearDataEnum? ClearData { get; init; }

        }
    }
}
