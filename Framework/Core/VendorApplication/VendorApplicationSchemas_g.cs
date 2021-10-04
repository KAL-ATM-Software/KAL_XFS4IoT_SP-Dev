/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * VendorApplicationSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.VendorApplication
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(AccessLevelEnum? AccessLevel = null)
        {
            this.AccessLevel = AccessLevel;
        }

        public enum AccessLevelEnum
        {
            NotActive,
            Basic,
            Intermediate,
            Full
        }

        /// <summary>
        /// Reports the current access level as one of the following values:
        /// 
        /// * ```notActive``` - The application is not active.
        /// * ```basic``` - The application is active for the basic access level.
        /// * ```intermediate``` - The application is active for the intermediate access level.
        /// * ```full``` - The application is active for the full access level.
        /// </summary>
        [DataMember(Name = "accessLevel")]
        public AccessLevelEnum? AccessLevel { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(SupportedAccessLevelsClass SupportedAccessLevels = null)
        {
            this.SupportedAccessLevels = SupportedAccessLevels;
        }

        [DataContract]
        public sealed class SupportedAccessLevelsClass
        {
            public SupportedAccessLevelsClass(bool? Basic = null, bool? Intermediate = null, bool? Full = null, bool? AccessNotSupported = null)
            {
                this.Basic = Basic;
                this.Intermediate = Intermediate;
                this.Full = Full;
                this.AccessNotSupported = AccessNotSupported;
            }

            /// <summary>
            /// The application supports the basic access level. Once the application is active it will
            /// show the user interface for the basic access level.
            /// </summary>
            [DataMember(Name = "basic")]
            public bool? Basic { get; init; }

            /// <summary>
            /// The application supports the intermediate access level. Once the application is active it will
            /// show the user interface for the intermediate access level.
            /// </summary>
            [DataMember(Name = "intermediate")]
            public bool? Intermediate { get; init; }

            /// <summary>
            /// The application supports the full access level. Once the application is active it will
            /// show the user interface for the full access level.
            /// </summary>
            [DataMember(Name = "full")]
            public bool? Full { get; init; }

            /// <summary>
            /// Access levels are not supported.
            /// </summary>
            [DataMember(Name = "accessNotSupported")]
            public bool? AccessNotSupported { get; init; }

        }

        /// <summary>
        /// Specifies the supported access levels. This allows the application to show a user interface with
        /// reduced or extended functionality depending on the access levels. The exact meaning or functionalities
        /// definition is left to the vendor. If no access levels are supported this field will be accessNotSupported.
        /// Otherwise this field will  be set to a combination of true or false value for supported or not supported
        /// respectively.
        /// </summary>
        [DataMember(Name = "supportedAccessLevels")]
        public SupportedAccessLevelsClass SupportedAccessLevels { get; init; }

    }


}
