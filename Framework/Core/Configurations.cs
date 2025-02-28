/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    /// <summary>
    /// Static class for framework configuration variables, the service provider can set XML configuration file xxx.exe.config
    /// <code><![CDATA[
    ///     <?xml version="1.0" encoding="utf-8" ?>
    ///     <configuration>
    ///         <appSettings>
    ///             <add key = "ServerAddressUri" value="http:://localhost" />
    ///         </appSettings>
    ///     </configuration>
    /// ]]></code>
    /// </summary>
    public static class Configurations
    {
        /// <summary>
        /// Server address for the service provider in URI format
        /// </summary>
        public static readonly string ServerAddressUri = "ServerAddressUri";

        /// <summary>
        /// Specific port to listen, must be within defined port in the XFS4IoT specification. 80, 443, 5846-5856.
        /// if this value is not configured or configured outside of range, first available port will be used.
        /// </summary>
        public static readonly string ServerPort = "ServerPort";

        /// <summary>
        /// Set of default values
        /// </summary>
        public static class Default
        {
            /// <summary>
            /// Default value of the server address
            /// </summary>
            public static readonly string ServerAddressUri = "http://localhost";
        }
    }
}
