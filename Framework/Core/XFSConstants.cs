/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Collections.Generic;

namespace XFS4IoT
{
    /// <summary>
    /// Constant values related to XFS4IoT
    /// </summary>
    public static class XFSConstants
    {
        /// <summary>
        /// XFS4IoT mandated port options. 
        /// </summary>
        public static readonly int[] PortRanges = new int[] 
        { 
            80,  // Only for HTTP
            443, // Only for HTTPS
            5846, 
            5847, 
            5848, 
            5849, 
            5850, 
            5851, 
            5852, 
            5853, 
            5854, 
            5855, 
            5856 
        };

        /// <summary>
        /// Service classes 
        /// </summary>
        /// <remarks>
        /// Use .ToString() to get service class name strings. 
        /// </remarks>
        public enum ServiceClass 
        { 
            CardReader, 
            Publisher, 
            Printer, 
            TextTerminal, 
            PinPad, 
            Common, 
            CashDispenser, 
            CashManagement, 
            Crypto, 
            Keyboard, 
            KeyManagement, 
            Storage, 
            Lights,
            Auxiliaries,
            VendorApplication,
            VendorMode,
        }

        /// <summary>
        /// Service version
        /// </summary>
        public static readonly string ServiceVersion = "1.0";

        /// <summary>
        /// Command payload version range supported
        /// </summary>
        public static readonly List<string> SupportedVersionRange = new(){ "1.0" };
    }
}