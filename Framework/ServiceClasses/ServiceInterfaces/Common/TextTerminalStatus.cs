/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public sealed class TextTerminalStatusClass
    {
        public enum KeyboardEnum
        {
            On,
            Off,
            NotAvailable
        }

        public enum KeyLockEnum
        {
            On,
            Off,
            NotAvailable
        }

        public TextTerminalStatusClass(KeyboardEnum Keyboard, 
                                       KeyLockEnum KeyLock , 
                                       int DisplaySizeX, 
                                       int DisplaySizeY)
        {
            this.Keyboard = Keyboard;
            this.KeyLock = KeyLock;
            this.DisplaySizeX = DisplaySizeX;
            this.DisplaySizeY = DisplaySizeY;
        }

        

        /// <summary>
        /// Specifies the state of the keyboard in the text terminal unit as one of the following values:
        /// * ```on``` - The keyboard is activated.
        /// * ```off``` - The keyboard is not activated.
        /// * ```notAvailable``` - The keyboard is not available.
        /// </summary>
        public KeyboardEnum Keyboard { get; set; }

        /// <summary>
        /// Specifies the state of the keyboard lock of the text terminal unit as one of the following values:
        /// * ```on``` - The keyboard lock switch is activated.
        /// * ```off``` - The keyboard lock switch is not activated.
        /// * ```notAvailable``` - The keyboard lock switch is not available.
        /// </summary>
        public KeyLockEnum KeyLock { get; set; }

        /// <summary>
        /// Specifies the horizontal size of the display of the text terminal unit.
        /// </summary>
        public int DisplaySizeX { get; set; }

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit.
        /// </summary>
        public int DisplaySizeY { get; set; }
    }
}
