/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * TextTerminalSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.TextTerminal
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(KeyboardEnum? Keyboard = null, KeyLockEnum? KeyLock = null, int? DisplaySizeX = null, int? DisplaySizeY = null, List<LedsClass> Leds = null)
        {
            this.Keyboard = Keyboard;
            this.KeyLock = KeyLock;
            this.DisplaySizeX = DisplaySizeX;
            this.DisplaySizeY = DisplaySizeY;
            this.Leds = Leds;
        }

        public enum KeyboardEnum
        {
            On,
            Off,
            Na
        }

        /// <summary>
        /// Specifies the state of the keyboard.
        /// </summary>
        [DataMember(Name = "keyboard")]
        public KeyboardEnum? Keyboard { get; private set; }

        public enum KeyLockEnum
        {
            On,
            Off,
            Na
        }

        /// <summary>
        /// Specifies the state of the keyboard lock.
        /// </summary>
        [DataMember(Name = "keyLock")]
        public KeyLockEnum? KeyLock { get; private set; }

        /// <summary>
        /// Specifies the horizontal size of the display of the text terminal unit.
        /// </summary>
        [DataMember(Name = "displaySizeX")]
        public int? DisplaySizeX { get; private set; }

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit.
        /// </summary>
        [DataMember(Name = "displaySizeY")]
        public int? DisplaySizeY { get; private set; }

        [DataContract]
        public sealed class LedsClass
        {
            public LedsClass(bool? Na = null, bool? Off = null, bool? SlowFlash = null, bool? MediumFlash = null, bool? QuickFlash = null, bool? Continuous = null, bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
            {
                this.Na = Na;
                this.Off = Off;
                this.SlowFlash = SlowFlash;
                this.MediumFlash = MediumFlash;
                this.QuickFlash = QuickFlash;
                this.Continuous = Continuous;
                this.Red = Red;
                this.Green = Green;
                this.Yellow = Yellow;
                this.Blue = Blue;
                this.Cyan = Cyan;
                this.Magenta = Magenta;
                this.White = White;
            }

            /// <summary>
            /// The Status is not available.
            /// Type A
            /// </summary>
            [DataMember(Name = "na")]
            public bool? Na { get; private set; }

            /// <summary>
            /// The LED is turned off.
            /// Type A
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; private set; }

            /// <summary>
            /// The LED is blinking.
            /// Type B
            /// </summary>
            [DataMember(Name = "slowFlash")]
            public bool? SlowFlash { get; private set; }

            /// <summary>
            /// The LED is blinking medium frequency.
            /// Type B
            /// </summary>
            [DataMember(Name = "mediumFlash")]
            public bool? MediumFlash { get; private set; }

            /// <summary>
            /// The LED is blinking quickly.
            /// Type B
            /// </summary>
            [DataMember(Name = "quickFlash")]
            public bool? QuickFlash { get; private set; }

            /// <summary>
            /// The LED is turned on continuous(steady).
            /// Type B
            /// </summary>
            [DataMember(Name = "continuous")]
            public bool? Continuous { get; private set; }

            /// <summary>
            /// The LED is red.
            /// Type C
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; private set; }

            /// <summary>
            /// The LED is green.
            /// Type C
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; private set; }

            /// <summary>
            /// The LED is yellow.
            /// Type C
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; private set; }

            /// <summary>
            /// The LED is blue.
            /// Type C
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; private set; }

            /// <summary>
            /// The LED is cyan.
            /// Type C
            /// </summary>
            [DataMember(Name = "cyan")]
            public bool? Cyan { get; private set; }

            /// <summary>
            /// The LED is magenta.
            /// Type C
            /// </summary>
            [DataMember(Name = "magenta")]
            public bool? Magenta { get; private set; }

            /// <summary>
            /// The LED is white.
            /// Type C
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; private set; }

        }

        /// <summary>
        /// Specifies array that specifies the state of each LED.
        /// Specifies the state of the na, off or a combination of the following flags consisting of one type B, 
        /// and optionally one type C
        /// </summary>
        [DataMember(Name = "leds")]
        public List<LedsClass> Leds { get; private set; }

    }


    [DataContract]
    public sealed class ResolutionClass
    {
        public ResolutionClass(int? SizeX = null, int? SizeY = null)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }

        /// <summary>
        /// TSpecifies the horizontal size of the display of the text terminal unit (the number of columns that can be displayed).
        /// </summary>
        [DataMember(Name = "sizeX")]
        public int? SizeX { get; private set; }

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit (the number of rows that can be displayed).
        /// </summary>
        [DataMember(Name = "sizeY")]
        public int? SizeY { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, List<ResolutionClass> Resolutions = null, bool? KeyLock = null, bool? DisplayLight = null, bool? Cursor = null, bool? Forms = null, CharSupportClass CharSupport = null, List<LedsClass> Leds = null)
        {
            this.Type = Type;
            this.Resolutions = Resolutions;
            this.KeyLock = KeyLock;
            this.DisplayLight = DisplayLight;
            this.Cursor = Cursor;
            this.Forms = Forms;
            this.CharSupport = CharSupport;
            this.Leds = Leds;
        }

        public enum TypeEnum
        {
            Fixed,
            Removable
        }

        /// <summary>
        /// Specifies the type of the text terminal unit.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; private set; }

        /// <summary>
        /// Array specifies the resolutions supported by the physical display device. (For the definition of Resolution see 
        /// the command [TextTerminal.SetResolution](#textterminal.setresolution)). The resolution indicated in the first 
        /// position is the default resolution and the device will be placed in this resolution when the Service Provider 
        /// is initialized or reset through the [TextTerminal.Reset](#textterminal.reset) command.
        /// </summary>
        [DataMember(Name = "resolutions")]
        public List<ResolutionClass> Resolutions { get; private set; }

        /// <summary>
        /// Specifies whether the text terminal unit has a key lock switch.
        /// </summary>
        [DataMember(Name = "keyLock")]
        public bool? KeyLock { get; private set; }

        /// <summary>
        /// Specifies whether the text terminal unit has a display light that can be switched ON and OFF with the 
        /// [TextTerminal.DispLight](#textterminal.displight) command.
        /// </summary>
        [DataMember(Name = "displayLight")]
        public bool? DisplayLight { get; private set; }

        /// <summary>
        /// Specifies whether the text terminal unit display supports a cursor.
        /// </summary>
        [DataMember(Name = "cursor")]
        public bool? Cursor { get; private set; }

        /// <summary>
        /// Specifies whether the text terminal unit service supports forms oriented input and output.
        /// </summary>
        [DataMember(Name = "forms")]
        public bool? Forms { get; private set; }

        [DataContract]
        public sealed class CharSupportClass
        {
            public CharSupportClass(bool? Ascii = null, bool? Unicode = null)
            {
                this.Ascii = Ascii;
                this.Unicode = Unicode;
            }

            /// <summary>
            /// Ascii is supported for forms.
            /// </summary>
            [DataMember(Name = "ascii")]
            public bool? Ascii { get; private set; }

            /// <summary>
            /// Unicode is supported for forms.
            /// </summary>
            [DataMember(Name = "unicode")]
            public bool? Unicode { get; private set; }

        }

        /// <summary>
        /// For charSupport, a Service Provider can support ONLY ascii forms or can support BOTH ascii and unicode forms.
        ///  A Service Provider can not support UNICODE forms without also supporting ASCII forms.\
        /// </summary>
        [DataMember(Name = "charSupport")]
        public CharSupportClass CharSupport { get; private set; }

        [DataContract]
        public sealed class LedsClass
        {
            public LedsClass(bool? Off = null, bool? SlowFlash = null, bool? MediumFlash = null, bool? QuickFlash = null, bool? Continuous = null, bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
            {
                this.Off = Off;
                this.SlowFlash = SlowFlash;
                this.MediumFlash = MediumFlash;
                this.QuickFlash = QuickFlash;
                this.Continuous = Continuous;
                this.Red = Red;
                this.Green = Green;
                this.Yellow = Yellow;
                this.Blue = Blue;
                this.Cyan = Cyan;
                this.Magenta = Magenta;
                this.White = White;
            }

            /// <summary>
            /// The LED can be off. 
            /// Type:(A)
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; private set; }

            /// <summary>
            /// The LED can be blinking. 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "slowFlash")]
            public bool? SlowFlash { get; private set; }

            /// <summary>
            /// The LED can be blinking medium frequency. 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "mediumFlash")]
            public bool? MediumFlash { get; private set; }

            /// <summary>
            /// The LED can be blinking quickly. 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "quickFlash")]
            public bool? QuickFlash { get; private set; }

            /// <summary>
            /// The LED can be turned on continuous(steady). 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "continuous")]
            public bool? Continuous { get; private set; }

            /// <summary>
            /// The LED can be red. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; private set; }

            /// <summary>
            /// The LED can be green. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; private set; }

            /// <summary>
            /// The LED can be yellow. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; private set; }

            /// <summary>
            /// The LED can be blue. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; private set; }

            /// <summary>
            /// The LED can be cyan. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "cyan")]
            public bool? Cyan { get; private set; }

            /// <summary>
            /// The LED can be magenta. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "magenta")]
            public bool? Magenta { get; private set; }

            /// <summary>
            /// The LED can be white. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; private set; }

        }

        /// <summary>
        /// Specifies which LEDs are available.
        /// The elements of this array are specified as a combination of the following flags and indicate all 
        /// of the possible flash rates (type B) and colors (type C) that the LED is capable of handling. 
        /// If the LED only supports one color then no value of type C is returned.
        /// </summary>
        [DataMember(Name = "leds")]
        public List<LedsClass> Leds { get; private set; }

    }


    public enum ModesEnum
    {
        Relative,
        Absolute
    }


}
