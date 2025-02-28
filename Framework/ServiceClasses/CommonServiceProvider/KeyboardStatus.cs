/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    public sealed class KeyboardStatusClass(
        KeyboardStatusClass.AutoBeepModeEnum AutoBeepMode) : StatusBase
    {
        [Flags]
        public enum AutoBeepModeEnum
        {
            NotSupported = 0,
            Active = 1 << 0 ,
            InActive = 1 << 1,
        }

        /// <summary>
        /// Specifies whether automatic beep tone on key press is active or not. Active and in-active key beeping is reported 
        /// independently. autoBeepMode can take a combination of the following values, if the flag is not set auto beeping 
        /// is not activated (or not supported) for that key type (i.e. active or in-active keys).
        /// The following values are possible:
        /// * ```active``` - An automatic tone will be generated for all active keys.
        /// * ```inActive``` - An automatic tone will be generated for all in-active keys.
        /// </summary>
        public AutoBeepModeEnum AutoBeepMode
        {
            get { return autoBeepMode; }
            set
            {
                if (autoBeepMode != value)
                {
                    autoBeepMode = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AutoBeepModeEnum autoBeepMode = AutoBeepMode;
    }
}
