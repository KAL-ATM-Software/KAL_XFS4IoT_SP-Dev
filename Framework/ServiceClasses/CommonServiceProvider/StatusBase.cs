/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public abstract class StatusBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Handle common status changed event. the device class can set to null if the device class wants to disable it.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = null;

        /// <summary>
        /// Constructor
        /// The status changed event is enabled by default and possible to disable from device class if it's needed.
        /// </summary>
        public StatusBase()
        {
        }

        /// <summary>
        /// This method must be called in the derived object where the status property value is being changed.
        /// </summary>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
