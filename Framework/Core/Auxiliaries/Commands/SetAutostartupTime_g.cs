/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAutoStartupTime_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Auxiliaries.Commands
{
    //Original name = SetAutoStartupTime
    [DataContract]
    [Command(Name = "Auxiliaries.SetAutoStartupTime")]
    public sealed class SetAutoStartupTimeCommand : Command<SetAutoStartupTimeCommand.PayloadData>
    {
        public SetAutoStartupTimeCommand(int RequestId, SetAutoStartupTimeCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ModeEnum? Mode = null, SystemTimeClass StartTime = null)
                : base(Timeout)
            {
                this.Mode = Mode;
                this.StartTime = StartTime;
            }

            public enum ModeEnum
            {
                Clear,
                Specific,
                Daily,
                Weekly
            }

            /// <summary>
            /// Specifies the current or desired auto start-up control mode configured. The following values are possible:
            /// 
            /// * ```clear``` - No auto start-up time configured.
            /// * ```specific``` - In the *startTime* object, only *year*, *month, *day*, *hour* and *minute* are relevant. 
            ///   All other properties must be ignored.
            /// * ```daily``` - Auto start-up every day has been configured. In the *startTime* object, only *hour* and 
            ///   *minute* are relevant. All other properties must be ignored.
            /// * ```weekly``` - Auto start-up at a specified time on a specific day of every week has been configured. 
            ///   In the *startTime* parameter, only *dayOfWeek*, *hour* and *minute* are relevant. All other properties must be ignored.
            /// </summary>
            [DataMember(Name = "mode")]
            public ModeEnum? Mode { get; init; }

            /// <summary>
            /// Specifies the current or desired auto start-up time configuration.
            /// </summary>
            [DataMember(Name = "startTime")]
            public SystemTimeClass StartTime { get; init; }

        }
    }
}
