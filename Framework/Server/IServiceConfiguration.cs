/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    public interface IServiceConfiguration
    {
        /// <summary>
        /// Get configuration value associated with the key specified.
        /// Returns null if specified value doesn't exist in the configuration. 
        /// </summary>
        /// <param name="name">Name of the configuration value</param>
        /// <returns>Configuration value</returns>
        public string Get(string name);
    }
}

