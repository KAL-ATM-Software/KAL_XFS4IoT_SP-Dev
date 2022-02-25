/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * DataClearedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Biometric.Events
{

    [DataContract]
    [Event(Name = "Biometric.DataClearedEvent")]
    public sealed class DataClearedEvent : UnsolicitedEvent<DataClearedEvent.PayloadData>
    {

        public DataClearedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ClearDataEnum? ClearData = null)
                : base()
            {
                this.ClearData = ClearData;
            }

            /// <summary>
            /// This parameter indicates the type of data that has been cleared from storage. 
            /// If this property is omitted, then all stored data has been cleared. 
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
