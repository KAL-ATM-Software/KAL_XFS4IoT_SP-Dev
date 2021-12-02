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
using System.Drawing;
using XFS4IoTFramework.TextTerminal;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// TextTerminalCapabilitiesClass
    /// Store device capabilites for the text terminal unit
    /// </summary>
    public sealed class TextTerminalCapabilitiesClass
    {
        public TextTerminalCapabilitiesClass(TypeEnum Type,
                                             List<Size> Resolutions,
                                             bool KeyLock,
                                             bool Cursor,
                                             bool Forms)
        {
            this.Type = Type;
            this.Resolutions = Resolutions;
            this.KeyLock = KeyLock;
            this.Cursor = Cursor;
            this.Forms = Forms;
        }

        public enum TypeEnum
        {
            Fixed,
            Removable
        }

        /// <summary>
        /// Specifies the type of the text terminal unit as one of the following flags:
        /// * ```fixed``` - The text terminal unit is a fixed device.
        /// * ```removable``` - The text terminal unit is a removable device.
        /// </summary>
        public TypeEnum Type { get; init; }

        /// <summary>
        /// Array specifies the resolutions supported by the physical display device. (For the definition of Resolution see 
        /// the command TextTerminal.SetResolution. The resolution indicated in the first 
        /// position is the default resolution and the device will be placed in this resolution when the Service Provider 
        /// is initialized or reset through the TextTerminal.Reset command.
        /// </summary>
        public List<Size> Resolutions { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit has a key lock switch.
        /// </summary>
        public bool KeyLock { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit display supports a cursor.
        /// </summary>
        public bool Cursor { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit service supports forms oriented input and output.
        /// </summary>
        public bool Forms { get; init; }
    }
}