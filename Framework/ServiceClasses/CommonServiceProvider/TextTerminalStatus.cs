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
    public sealed class TextTerminalStatusClass(
        TextTerminalStatusClass.KeyboardEnum Keyboard,
        TextTerminalStatusClass.KeyLockEnum KeyLock,
        int DisplaySizeX,
        int DisplaySizeY) : StatusBase
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

        /// <summary>
        /// Specifies the state of the keyboard in the text terminal unit as one of the following values:
        /// * ```on``` - The keyboard is activated.
        /// * ```off``` - The keyboard is not activated.
        /// * ```notAvailable``` - The keyboard is not available.
        /// </summary>
        public KeyboardEnum Keyboard 
        {
            get { return keyboard; }
            set
            {
                if (keyboard != value)
                {
                    keyboard = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private KeyboardEnum keyboard = Keyboard;

        /// <summary>
        /// Specifies the state of the keyboard lock of the text terminal unit as one of the following values:
        /// * ```on``` - The keyboard lock switch is activated.
        /// * ```off``` - The keyboard lock switch is not activated.
        /// * ```notAvailable``` - The keyboard lock switch is not available.
        /// </summary>
        public KeyLockEnum KeyLock 
        { 
            get { return keyLock; }
            set
            {
                if (keyLock != value)
                {
                    keyLock = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private KeyLockEnum keyLock = KeyLock;

        /// <summary>
        /// Specifies the horizontal size of the display of the text terminal unit.
        /// </summary>
        public int DisplaySizeX 
        {
            get { return displaySizeX; } 
            set
            {
                if (displaySizeX != value)
                {
                    displaySizeX = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int displaySizeX = DisplaySizeX;

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit.
        /// </summary>
        public int DisplaySizeY 
        {
            get { return displaySizeY; }
            set
            {
                if (displaySizeY != value)
                {
                    displaySizeY = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int displaySizeY = DisplaySizeY;
    }
}
