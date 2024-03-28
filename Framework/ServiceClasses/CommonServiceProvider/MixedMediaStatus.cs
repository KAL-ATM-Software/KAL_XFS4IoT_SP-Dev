/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTServer;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// Mixed media status class
    /// </summary>
    public sealed class MixedMediaStatusClass(
        MixedMedia.ModeTypeEnum CurrentModes) : StatusBase
    {

        /// <summary>
        /// Specifies the state of the transaction modes supported by the Service.
        /// </summary>
        public MixedMedia.ModeTypeEnum CurrentModes
        {
            get { return currentModes; }
            set
            {
                if (currentModes != value)
                {
                    currentModes = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MixedMedia.ModeTypeEnum currentModes = CurrentModes;
    }
}
