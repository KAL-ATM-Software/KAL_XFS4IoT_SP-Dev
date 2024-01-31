/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * StartLocalApplication_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.VendorApplication.Commands
{
    //Original name = StartLocalApplication
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "VendorApplication.StartLocalApplication")]
    public sealed class StartLocalApplicationCommand : Command<StartLocalApplicationCommand.PayloadData>
    {
        public StartLocalApplicationCommand(int RequestId, StartLocalApplicationCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string AppName = null, AccessLevelEnum? AccessLevel = null)
                : base()
            {
                this.AppName = AppName;
                this.AccessLevel = AccessLevel;
            }

            /// <summary>
            /// Defines the vendor dependent application to start.
            /// <example>ACME vendor app</example>
            /// </summary>
            [DataMember(Name = "appName")]
            public string AppName { get; init; }

            public enum AccessLevelEnum
            {
                Basic,
                Intermediate,
                Full
            }

            /// <summary>
            /// If specified, this defines the access level for the vendor dependent application interface. If
            /// not specified (null) then the service will determine the level of access available. If the level of
            /// access is to be changed then an application exit should be performed, followed by a restart of
            /// the application specifying the new level of access. Specified as one of the following:
            /// 
            /// * ```basic``` - The vendor dependent application is active for the basic access level. Once the
            /// application is active it will show the user interface for the basic access level.
            /// * ```intermediate``` - The vendor dependent application is active for the intermediate access level.
            /// Once the application is active it will show the user interface for the intermediate
            /// access level.
            /// * ```full``` - The vendor dependent application is active for the full access
            /// level. Once the application is active it will show the user interface for the full
            /// access level.
            /// </summary>
            [DataMember(Name = "accessLevel")]
            public AccessLevelEnum? AccessLevel { get; init; }

        }
    }
}
