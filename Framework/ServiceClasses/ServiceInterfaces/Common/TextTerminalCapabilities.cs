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
                                             bool DisplayLight,
                                             bool Cursor,
                                             bool Forms,
                                             List<LEDClass> LEDSupported)
        {
            this.Type = Type;
            this.Resolutions = Resolutions;
            this.KeyLock = KeyLock;
            this.DisplayLight = DisplayLight;
            this.Cursor = Cursor;
            this.Forms = Forms;
            this.LEDSupported = LEDSupported;
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
        /// Specifies whether the text terminal unit has a display light that can be switched ON and OFF with the 
        /// TextTerminal.DispLight command.
        /// </summary>
        public bool DisplayLight { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit display supports a cursor.
        /// </summary>
        public bool Cursor { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit service supports forms oriented input and output.
        /// </summary>
        public bool Forms { get; init; }

        public sealed class LEDClass
        {
            [Flags]
            public enum LEDColorsEnum
            {
                None = 0,
                Red = 0x00000100,
                Green = 0x00000200,
                Yellow = 0x00000400,
                Blue = 0x00000800,
                Cyan = 0x00001000,
                Magenta = 0x00002000,
                White = 0x00004000
            }

            [Flags]
            public enum LEDLightControlsEnum
            {
                None = 0,
                Off = 0x0001,
                SlowFlash = 0x0002,
                MediumFlash = 0x0004,
                QuickFlash = 0x0008,
                Continuous = 0x0080,
            }

            public LEDClass(LEDColorsEnum Colors, LEDLightControlsEnum LightControl)
            {
                this.Color = Colors;
                this.LightControl = LightControl;
            }

            public LEDColorsEnum Color { get; init; }

            public LEDLightControlsEnum LightControl { get; init; }
        }

        /// <summary>
        /// Specifies which LEDs are available.
        /// The elements of this array are specified as a combination of the following flags and indicate all 
        /// of the possible flash rates (type B) and colors (type C) that the LED is capable of handling. 
        /// If the LED only supports one color then no value of type C is returned.
        /// </summary>
        public List<LEDClass> LEDSupported { get; private set; }
    }
}