/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * HSMInit_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.German.Commands
{
    //Original name = HSMInit
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "German.HSMInit")]
    public sealed class HSMInitCommand : Command<HSMInitCommand.PayloadData>
    {
        public HSMInitCommand(int RequestId, HSMInitCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(InitModeEnum? InitMode = null, string OnlineTime = null)
                : base()
            {
                this.InitMode = InitMode;
                this.OnlineTime = OnlineTime;
            }

            public enum InitModeEnum
            {
                Temp,
                Definite,
                Irreversible
            }

            /// <summary>
            /// Specifies the initialization mode. The following values are possible:
            /// 
            /// * ```temp``` - Initialize the HSM temporarily (K_UR remains loaded). For predefined key name K_UR see 'KUR' [[Ref. german-2](#ref-german-2)].
            /// * ```definite``` - Initialize the HSM definitely (K_UR is deleted). 
            /// * ```irreversible``` - Initialize the HSM irreversibly (can only be restored by the vendor).
            /// <example>temp</example>
            /// </summary>
            [DataMember(Name = "initMode")]
            public InitModeEnum? InitMode { get; init; }

            /// <summary>
            /// Specifies the online date and time in the format YYYYMMDDHHMMSS defined in ISO 8583 BMP 61 (see [[Ref. german-6](#ref-german-6)] and [[Ref. german-2](#ref-german-2)]) as BCD packed characters.
            /// This property is null when the *initMode* equals *definite* or *irreversible*.
            /// If the *initMode* equals *temp* and this property is null or all zeros, the online time will be set to a value in the past.
            /// <example>20221012123000</example>
            /// </summary>
            [DataMember(Name = "onlineTime")]
            [DataTypes(Pattern = @"^[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9][0-5][0-9]$")]
            public string OnlineTime { get; init; }

        }
    }
}
