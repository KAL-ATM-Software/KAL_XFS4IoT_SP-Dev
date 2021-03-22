/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
