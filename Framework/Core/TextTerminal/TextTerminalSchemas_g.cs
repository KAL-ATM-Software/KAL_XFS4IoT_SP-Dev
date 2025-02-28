/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        public StatusClass(KeyboardEnum? Keyboard = null, KeyLockEnum? KeyLock = null, int? DisplaySizeX = null, int? DisplaySizeY = null)
        {
            this.Keyboard = Keyboard;
            this.KeyLock = KeyLock;
            this.DisplaySizeX = DisplaySizeX;
            this.DisplaySizeY = DisplaySizeY;
        }

        public enum KeyboardEnum
        {
            On,
            Off
        }

        /// <summary>
        /// Specifies the state of the keyboard in the text terminal unit. This property will be null in
        /// [Common.Status](#common.status) if the keyboard is not available, otherwise one of the following values:
        /// 
        /// * ```on``` - The keyboard is activated.
        /// * ```off``` - The keyboard is not activated.
        /// </summary>
        [DataMember(Name = "keyboard")]
        public KeyboardEnum? Keyboard { get; init; }

        public enum KeyLockEnum
        {
            On,
            Off
        }

        /// <summary>
        /// Specifies the state of the keyboard lock of the text terminal unit. This property will be null in
        /// [Common.Status](#common.status) if the keyboard lock switch is not available, otherwise one of the following
        /// values:
        /// 
        /// * ```on``` - The keyboard lock switch is activated.
        /// * ```off``` - The keyboard lock switch is not activated.
        /// </summary>
        [DataMember(Name = "keyLock")]
        public KeyLockEnum? KeyLock { get; init; }

        /// <summary>
        /// Specifies the horizontal size of the display of the text terminal unit (the number of columns that can be
        /// displayed). This property will be null in [Common.StatusChangedEvent](#common.statuschangedevent) if
        /// unchanged.
        /// </summary>
        [DataMember(Name = "displaySizeX")]
        [DataTypes(Minimum = 0)]
        public int? DisplaySizeX { get; init; }

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit (the number of rows that can be
        /// displayed).  This property will be null in [Common.StatusChangedEvent](#common.statuschangedevent) if
        /// unchanged.
        /// </summary>
        [DataMember(Name = "displaySizeY")]
        [DataTypes(Minimum = 0)]
        public int? DisplaySizeY { get; init; }

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
        /// Specifies the horizontal size of the display of the Text Terminal Unit (the number of columns that can be displayed).
        /// </summary>
        [DataMember(Name = "sizeX")]
        [DataTypes(Minimum = 0)]
        public int? SizeX { get; init; }

        /// <summary>
        /// Specifies the vertical size of the display of the Text Terminal Unit (the number of rows that can be displayed).
        /// </summary>
        [DataMember(Name = "sizeY")]
        [DataTypes(Minimum = 0)]
        public int? SizeY { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, List<ResolutionClass> Resolutions = null, bool? KeyLock = null, bool? Cursor = null, bool? Forms = null)
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
        /// Specifies the type of the text terminal unit as one of the following:
        /// * ```fixed``` - The text terminal unit is a fixed device.
        /// * ```removable``` - The text terminal unit is a removable device.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Array specifies the resolutions supported by the physical display device. (For the definition of Resolution see 
        /// the command [TextTerminal.SetResolution](#textterminal.setresolution)). The resolution indicated in the first 
        /// position is the default resolution and the device will be placed in this resolution when the Service 
        /// is initialized or reset through the [TextTerminal.Reset](#textterminal.reset) command.
        /// </summary>
        [DataMember(Name = "resolutions")]
        public List<ResolutionClass> Resolutions { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit has a key lock switch.
        /// </summary>
        [DataMember(Name = "keyLock")]
        public bool? KeyLock { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit display supports a cursor.
        /// </summary>
        [DataMember(Name = "cursor")]
        public bool? Cursor { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit service supports forms oriented input and output.
        /// </summary>
        [DataMember(Name = "forms")]
        public bool? Forms { get; init; }

    }


    [DataContract]
    public sealed class SizeClass
    {
        public SizeClass(int? Width = null, int? Height = null)
        {
            this.Width = Width;
            this.Height = Height;
        }

        /// <summary>
        /// Specifies the width in terms of the base horizontal resolution.
        /// <example>50</example>
        /// </summary>
        [DataMember(Name = "width")]
        [DataTypes(Minimum = 1)]
        public int? Width { get; init; }

        /// <summary>
        /// Specifies the height in terms of the base vertical resolution. For media definitions, 0 means unlimited, i.e.,
        /// paper roll.
        /// <example>100</example>
        /// </summary>
        [DataMember(Name = "height")]
        [DataTypes(Minimum = 1)]
        public int? Height { get; init; }

    }


    [DataContract]
    public sealed class PositionClass
    {
        public PositionClass(int? X = null, int? Y = null)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        /// Horizontal position relative to left side.
        /// <example>20</example>
        /// </summary>
        [DataMember(Name = "x")]
        [DataTypes(Minimum = 0)]
        public int? X { get; init; }

        /// <summary>
        /// Vertical position relative to the top.
        /// <example>12</example>
        /// </summary>
        [DataMember(Name = "y")]
        [DataTypes(Minimum = 0)]
        public int? Y { get; init; }

    }


    [DataContract]
    public sealed class KeyClass
    {
        public KeyClass(bool? Terminate = null)
        {
            this.Terminate = Terminate;
        }

        /// <summary>
        /// The key is a terminate key.
        /// </summary>
        [DataMember(Name = "terminate")]
        public bool? Terminate { get; init; }

    }


    public enum ModesEnum
    {
        Relative,
        Absolute
    }


}
