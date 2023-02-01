/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * SetDataPersistence_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = SetDataPersistence
    [DataContract]
    [Command(Name = "Biometric.SetDataPersistence")]
    public sealed class SetDataPersistenceCommand : Command<SetDataPersistenceCommand.PayloadData>
    {
        public SetDataPersistenceCommand(int RequestId, SetDataPersistenceCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PersistenceModeEnum? PersistenceMode = null)
                : base(Timeout)
            {
                this.PersistenceMode = PersistenceMode;
            }

            public enum PersistenceModeEnum
            {
                Persist,
                Clear
            }

            /// <summary>
            /// Specifies the data persistence mode. 
            /// This controls how biometric data that has been captured using the [Biometric.Read](#biometric.read) 
            /// command will persist. This value itself is persistent.
            /// Available values are described in the [persistenceModes](#common.capabilities.completion.properties.biometric.persistencemodes).
            /// The following values are possible:
            /// 
            /// * ```persist```\t- Biometric data captured using the [Biometric.Read](#biometric.read) can persist until all 
            ///                   sessions are closed, the device is power failed or rebooted, or the [Biometric.Read](#biometric.read) 
            ///                   is requested again. This captured biometric data can also be explicitly cleared using the 
            ///                   [Biometric.Clear](#biometric.clear) or [Biometric.Reset](#biometric.reset).
            /// * ```clear``` - Captured biometric data will not persist. Once the data has been either returned in the [Biometric.Read](#biometric.read) 
            ///                 command or used by the [Biometric.Match](#biometric.match), then the data is cleared from the device.
            /// </summary>
            [DataMember(Name = "persistenceMode")]
            public PersistenceModeEnum? PersistenceMode { get; init; }

        }
    }
}
