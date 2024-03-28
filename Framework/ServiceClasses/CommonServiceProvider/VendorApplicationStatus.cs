/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// VendorApplicationStatusClass
    /// Store device status for the vendor application
    /// </summary>
    public sealed class VendorApplicationStatusClass(
        VendorApplicationStatusClass.AccessLevelEnum AccessLevel) : StatusBase
    {

        public enum AccessLevelEnum
        {
            NotActive,    //Access level is not supported
            Basic,        //The application is active for the basic access level.
            Intermediate, //The application is active for the intermediate access level.
            Full,         //The application is active for the full access level.
        }

        /// <summary>
        /// Reports the current access level
        /// </summary>
        public AccessLevelEnum AccessLevel 
        {
            get { return accessLevel; } 
            set
            {
                if (accessLevel != value)
                {
                    accessLevel = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AccessLevelEnum accessLevel = AccessLevel;
    }
}