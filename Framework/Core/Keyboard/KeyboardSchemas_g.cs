/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyboardSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Keyboard
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(AutoBeepModeEnum? AutoBeepMode = null)
        {
            this.AutoBeepMode = AutoBeepMode;
        }

        public enum AutoBeepModeEnum
        {
            Active,
            InActive
        }

        /// <summary>
        /// Specifies whether automatic beep tone on key press is active or not. Active and in-active key beeping is reported 
        /// independently. autoBeepMode can take a combination of the following values, if the flag is not set auto beeping 
        /// is not activated (or not supported) for that key type (i.e. active or in-active keys)
        /// </summary>
        [DataMember(Name = "autoBeepMode")]
        public AutoBeepModeEnum? AutoBeepMode { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(AutoBeepClass AutoBeep = null, List<EtsCapsClass> EtsCaps = null)
        {
            this.AutoBeep = AutoBeep;
            this.EtsCaps = EtsCaps;
        }

        [DataContract]
        public sealed class AutoBeepClass
        {
            public AutoBeepClass(bool? ActiveAvailable = null, bool? ActiveSelectable = null, bool? InactiveAvailable = null, bool? InactiveSelectable = null)
            {
                this.ActiveAvailable = ActiveAvailable;
                this.ActiveSelectable = ActiveSelectable;
                this.InactiveAvailable = InactiveAvailable;
                this.InactiveSelectable = InactiveSelectable;
            }

            /// <summary>
            /// Automatic beep tone on active key key-press is supported.
            /// If this flag is not set then automatic beeping for active keys is not supported.
            /// </summary>
            [DataMember(Name = "activeAvailable")]
            public bool? ActiveAvailable { get; private set; }

            /// <summary>
            /// Automatic beeping for active keys can be controlled turned on and off by the application.
            /// If this flag is not set then automatic beeping for active keys cannot be controlled by an application.
            /// </summary>
            [DataMember(Name = "activeSelectable")]
            public bool? ActiveSelectable { get; private set; }

            /// <summary>
            /// Automatic beep tone on in-active key keypress is supported.
            /// If this flag is not set then automatic beeping for in-active keys is not supported.
            /// </summary>
            [DataMember(Name = "inactiveAvailable")]
            public bool? InactiveAvailable { get; private set; }

            /// <summary>
            /// Automatic beeping for in-active keys can be controlled turned on and off by the application.
            /// If this flag is not set then automatic beeping for in-active keys cannot be controlled by an application.
            /// </summary>
            [DataMember(Name = "inactiveSelectable")]
            public bool? InactiveSelectable { get; private set; }

        }

        /// <summary>
        /// Specifies whether the device will emit a key beep tone on key presses of active keys or inactive keys, and if so, which mode it supports
        /// </summary>
        [DataMember(Name = "autoBeep")]
        public AutoBeepClass AutoBeep { get; private set; }

        [DataContract]
        public sealed class EtsCapsClass
        {
            public EtsCapsClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, int? MaximumTouchFrames = null, int? MaximumTouchKeys = null, FloatFlagsClass FloatFlags = null)
            {
                this.XPos = XPos;
                this.YPos = YPos;
                this.XSize = XSize;
                this.YSize = YSize;
                this.MaximumTouchFrames = MaximumTouchFrames;
                this.MaximumTouchKeys = MaximumTouchKeys;
                this.FloatFlags = FloatFlags;
            }

            /// <summary>
            /// Specifies the position of the left edge of the ets in Windows virtual screen coordinates.
            /// This value may be negative because the of the monitor position on the virtual desktop.
            /// </summary>
            [DataMember(Name = "xPos")]
            public int? XPos { get; private set; }

            /// <summary>
            /// Specifies the position of the right edge of the ets in Windows virtual screen coordinates.
            /// This value may be negative because the of the monitor position on the virtual desktop.
            /// </summary>
            [DataMember(Name = "yPos")]
            public int? YPos { get; private set; }

            /// <summary>
            /// Specifies the width of the ets in Windows virtual screen coordinates.
            /// </summary>
            [DataMember(Name = "xSize")]
            public int? XSize { get; private set; }

            /// <summary>
            /// Specifies the height of the ets in Windows virtual screen coordinates.
            /// </summary>
            [DataMember(Name = "ySize")]
            public int? YSize { get; private set; }

            /// <summary>
            /// Specifies the maximum number of Touch-Frames that the device can support in a touch keyboard definition.
            /// </summary>
            [DataMember(Name = "maximumTouchFrames")]
            public int? MaximumTouchFrames { get; private set; }

            /// <summary>
            /// Specifies the maximum number of Touch-Keys that the device can support within any a touchframe.
            /// </summary>
            [DataMember(Name = "maximumTouchKeys")]
            public int? MaximumTouchKeys { get; private set; }

            [DataContract]
            public sealed class FloatFlagsClass
            {
                public FloatFlagsClass(bool? X = null, bool? Y = null)
                {
                    this.X = X;
                    this.Y = Y;
                }

                /// <summary>
                /// Specifies that the PIN device will randomly shift the layout in a horizontal direction
                /// </summary>
                [DataMember(Name = "x")]
                public bool? X { get; private set; }

                /// <summary>
                /// Specifies that the PIN device will randomly shift the layout in a vertical direction. 
                /// </summary>
                [DataMember(Name = "y")]
                public bool? Y { get; private set; }

            }

            /// <summary>
            /// Specifies if the device can float the touch keyboards. FloatNone if the PIN device cannot randomly shift the layout.
            /// </summary>
            [DataMember(Name = "floatFlags")]
            public FloatFlagsClass FloatFlags { get; private set; }

        }

        /// <summary>
        /// Specifies the capabilities of the ets device.
        /// </summary>
        [DataMember(Name = "etsCaps")]
        public List<EtsCapsClass> EtsCaps { get; private set; }

    }


    [DataContract]
    public sealed class FunctionKeysClass
    {
        public FunctionKeysClass(bool? Fk0 = null, bool? Fk1 = null, bool? Fk2 = null, bool? Fk3 = null, bool? Fk4 = null, bool? Fk5 = null, bool? Fk6 = null, bool? Fk7 = null, bool? Fk8 = null, bool? Fk9 = null, bool? FkA = null, bool? FkB = null, bool? FkC = null, bool? FkD = null, bool? FkE = null, bool? FkF = null, bool? FkEnter = null, bool? FkCancel = null, bool? FkClear = null, bool? FkBackspace = null, bool? FkHelp = null, bool? FkDecPoint = null, bool? Fk00 = null, bool? Fk000 = null, bool? FkShift = null, bool? FkRES01 = null, bool? FkRES02 = null, bool? FkRES03 = null, bool? FkRES04 = null, bool? FkRES05 = null, bool? FkRES06 = null, bool? FkRES07 = null, bool? FkRES08 = null, bool? FkOEM01 = null, bool? FkOEM02 = null, bool? FkOEM03 = null, bool? FkOEM04 = null, bool? FkOEM05 = null, bool? FkOEM06 = null)
        {
            this.Fk0 = Fk0;
            this.Fk1 = Fk1;
            this.Fk2 = Fk2;
            this.Fk3 = Fk3;
            this.Fk4 = Fk4;
            this.Fk5 = Fk5;
            this.Fk6 = Fk6;
            this.Fk7 = Fk7;
            this.Fk8 = Fk8;
            this.Fk9 = Fk9;
            this.FkA = FkA;
            this.FkB = FkB;
            this.FkC = FkC;
            this.FkD = FkD;
            this.FkE = FkE;
            this.FkF = FkF;
            this.FkEnter = FkEnter;
            this.FkCancel = FkCancel;
            this.FkClear = FkClear;
            this.FkBackspace = FkBackspace;
            this.FkHelp = FkHelp;
            this.FkDecPoint = FkDecPoint;
            this.Fk00 = Fk00;
            this.Fk000 = Fk000;
            this.FkShift = FkShift;
            this.FkRES01 = FkRES01;
            this.FkRES02 = FkRES02;
            this.FkRES03 = FkRES03;
            this.FkRES04 = FkRES04;
            this.FkRES05 = FkRES05;
            this.FkRES06 = FkRES06;
            this.FkRES07 = FkRES07;
            this.FkRES08 = FkRES08;
            this.FkOEM01 = FkOEM01;
            this.FkOEM02 = FkOEM02;
            this.FkOEM03 = FkOEM03;
            this.FkOEM04 = FkOEM04;
            this.FkOEM05 = FkOEM05;
            this.FkOEM06 = FkOEM06;
        }


        [DataMember(Name = "fk0")]
        public bool? Fk0 { get; private set; }


        [DataMember(Name = "fk1")]
        public bool? Fk1 { get; private set; }


        [DataMember(Name = "fk2")]
        public bool? Fk2 { get; private set; }


        [DataMember(Name = "fk3")]
        public bool? Fk3 { get; private set; }


        [DataMember(Name = "fk4")]
        public bool? Fk4 { get; private set; }


        [DataMember(Name = "fk5")]
        public bool? Fk5 { get; private set; }


        [DataMember(Name = "fk6")]
        public bool? Fk6 { get; private set; }


        [DataMember(Name = "fk7")]
        public bool? Fk7 { get; private set; }


        [DataMember(Name = "fk8")]
        public bool? Fk8 { get; private set; }


        [DataMember(Name = "fk9")]
        public bool? Fk9 { get; private set; }


        [DataMember(Name = "fkA")]
        public bool? FkA { get; private set; }


        [DataMember(Name = "fkB")]
        public bool? FkB { get; private set; }


        [DataMember(Name = "fkC")]
        public bool? FkC { get; private set; }


        [DataMember(Name = "fkD")]
        public bool? FkD { get; private set; }


        [DataMember(Name = "fkE")]
        public bool? FkE { get; private set; }


        [DataMember(Name = "fkF")]
        public bool? FkF { get; private set; }


        [DataMember(Name = "fkEnter")]
        public bool? FkEnter { get; private set; }


        [DataMember(Name = "fkCancel")]
        public bool? FkCancel { get; private set; }


        [DataMember(Name = "fkClear")]
        public bool? FkClear { get; private set; }


        [DataMember(Name = "fkBackspace")]
        public bool? FkBackspace { get; private set; }


        [DataMember(Name = "fkHelp")]
        public bool? FkHelp { get; private set; }


        [DataMember(Name = "fkDecPoint")]
        public bool? FkDecPoint { get; private set; }


        [DataMember(Name = "fk00")]
        public bool? Fk00 { get; private set; }


        [DataMember(Name = "fk000")]
        public bool? Fk000 { get; private set; }


        [DataMember(Name = "fkShift")]
        public bool? FkShift { get; private set; }


        [DataMember(Name = "fkRES01")]
        public bool? FkRES01 { get; private set; }


        [DataMember(Name = "fkRES02")]
        public bool? FkRES02 { get; private set; }


        [DataMember(Name = "fkRES03")]
        public bool? FkRES03 { get; private set; }


        [DataMember(Name = "fkRES04")]
        public bool? FkRES04 { get; private set; }


        [DataMember(Name = "fkRES05")]
        public bool? FkRES05 { get; private set; }


        [DataMember(Name = "fkRES06")]
        public bool? FkRES06 { get; private set; }


        [DataMember(Name = "fkRES07")]
        public bool? FkRES07 { get; private set; }


        [DataMember(Name = "fkRES08")]
        public bool? FkRES08 { get; private set; }


        [DataMember(Name = "fkOEM01")]
        public bool? FkOEM01 { get; private set; }


        [DataMember(Name = "fkOEM02")]
        public bool? FkOEM02 { get; private set; }


        [DataMember(Name = "fkOEM03")]
        public bool? FkOEM03 { get; private set; }


        [DataMember(Name = "fkOEM04")]
        public bool? FkOEM04 { get; private set; }


        [DataMember(Name = "fkOEM05")]
        public bool? FkOEM05 { get; private set; }


        [DataMember(Name = "fkOEM06")]
        public bool? FkOEM06 { get; private set; }

    }


    public enum AllKeysEnumEnum
    {
        Fk0,
        Fk1,
        Fk2,
        Fk3,
        Fk4,
        Fk5,
        Fk6,
        Fk7,
        Fk8,
        Fk9,
        FkA,
        FkB,
        FkC,
        FkD,
        FkE,
        FkF,
        FkEnter,
        FkCancel,
        FkClear,
        FkBackspace,
        FkHelp,
        FkDecPoint,
        Fk00,
        Fk000,
        FkShift,
        FkRES01,
        FkRES02,
        FkRES03,
        FkRES04,
        FkRES05,
        FkRES06,
        FkRES07,
        FkRES08,
        FkOEM01,
        FkOEM02,
        FkOEM03,
        FkOEM04,
        FkOEM05,
        FkOEM06,
        Fdk01,
        Fdk02,
        Fdk03,
        Fdk04,
        Fdk05,
        Fdk06,
        Fdk07,
        Fdk08,
        Fdk09,
        Fdk10,
        Fdk11,
        Fdk12,
        Fdk13,
        Fdk14,
        Fdk15,
        Fdk16,
        Fdk17,
        Fdk18,
        Fdk19,
        Fdk20,
        Fdk21,
        Fdk22,
        Fdk23,
        Fdk24,
        Fdk25,
        Fdk26,
        Fdk27,
        Fdk28,
        Fdk29,
        Fdk30,
        Fdk31,
        Fdk32
    }


    [DataContract]
    public sealed class FDKKeysClass
    {
        public FDKKeysClass(bool? Fdk01 = null, bool? Fdk02 = null, bool? Fdk03 = null, bool? Fdk04 = null, bool? Fdk05 = null, bool? Fdk06 = null, bool? Fdk07 = null, bool? Fdk08 = null, bool? Fdk09 = null, bool? Fdk10 = null, bool? Fdk11 = null, bool? Fdk12 = null, bool? Fdk13 = null, bool? Fdk14 = null, bool? Fdk15 = null, bool? Fdk16 = null, bool? Fdk17 = null, bool? Fdk18 = null, bool? Fdk19 = null, bool? Fdk20 = null, bool? Fdk21 = null, bool? Fdk22 = null, bool? Fdk23 = null, bool? Fdk24 = null, bool? Fdk25 = null, bool? Fdk26 = null, bool? Fdk27 = null, bool? Fdk28 = null, bool? Fdk29 = null, bool? Fdk30 = null, bool? Fdk31 = null, bool? Fdk32 = null)
        {
            this.Fdk01 = Fdk01;
            this.Fdk02 = Fdk02;
            this.Fdk03 = Fdk03;
            this.Fdk04 = Fdk04;
            this.Fdk05 = Fdk05;
            this.Fdk06 = Fdk06;
            this.Fdk07 = Fdk07;
            this.Fdk08 = Fdk08;
            this.Fdk09 = Fdk09;
            this.Fdk10 = Fdk10;
            this.Fdk11 = Fdk11;
            this.Fdk12 = Fdk12;
            this.Fdk13 = Fdk13;
            this.Fdk14 = Fdk14;
            this.Fdk15 = Fdk15;
            this.Fdk16 = Fdk16;
            this.Fdk17 = Fdk17;
            this.Fdk18 = Fdk18;
            this.Fdk19 = Fdk19;
            this.Fdk20 = Fdk20;
            this.Fdk21 = Fdk21;
            this.Fdk22 = Fdk22;
            this.Fdk23 = Fdk23;
            this.Fdk24 = Fdk24;
            this.Fdk25 = Fdk25;
            this.Fdk26 = Fdk26;
            this.Fdk27 = Fdk27;
            this.Fdk28 = Fdk28;
            this.Fdk29 = Fdk29;
            this.Fdk30 = Fdk30;
            this.Fdk31 = Fdk31;
            this.Fdk32 = Fdk32;
        }


        [DataMember(Name = "fdk01")]
        public bool? Fdk01 { get; private set; }


        [DataMember(Name = "fdk02")]
        public bool? Fdk02 { get; private set; }


        [DataMember(Name = "fdk03")]
        public bool? Fdk03 { get; private set; }


        [DataMember(Name = "fdk04")]
        public bool? Fdk04 { get; private set; }


        [DataMember(Name = "fdk05")]
        public bool? Fdk05 { get; private set; }


        [DataMember(Name = "fdk06")]
        public bool? Fdk06 { get; private set; }


        [DataMember(Name = "fdk07")]
        public bool? Fdk07 { get; private set; }


        [DataMember(Name = "fdk08")]
        public bool? Fdk08 { get; private set; }


        [DataMember(Name = "fdk09")]
        public bool? Fdk09 { get; private set; }


        [DataMember(Name = "fdk10")]
        public bool? Fdk10 { get; private set; }


        [DataMember(Name = "fdk11")]
        public bool? Fdk11 { get; private set; }


        [DataMember(Name = "fdk12")]
        public bool? Fdk12 { get; private set; }


        [DataMember(Name = "fdk13")]
        public bool? Fdk13 { get; private set; }


        [DataMember(Name = "fdk14")]
        public bool? Fdk14 { get; private set; }


        [DataMember(Name = "fdk15")]
        public bool? Fdk15 { get; private set; }


        [DataMember(Name = "fdk16")]
        public bool? Fdk16 { get; private set; }


        [DataMember(Name = "fdk17")]
        public bool? Fdk17 { get; private set; }


        [DataMember(Name = "fdk18")]
        public bool? Fdk18 { get; private set; }


        [DataMember(Name = "fdk19")]
        public bool? Fdk19 { get; private set; }


        [DataMember(Name = "fdk20")]
        public bool? Fdk20 { get; private set; }


        [DataMember(Name = "fdk21")]
        public bool? Fdk21 { get; private set; }


        [DataMember(Name = "fdk22")]
        public bool? Fdk22 { get; private set; }


        [DataMember(Name = "fdk23")]
        public bool? Fdk23 { get; private set; }


        [DataMember(Name = "fdk24")]
        public bool? Fdk24 { get; private set; }


        [DataMember(Name = "fdk25")]
        public bool? Fdk25 { get; private set; }


        [DataMember(Name = "fdk26")]
        public bool? Fdk26 { get; private set; }


        [DataMember(Name = "fdk27")]
        public bool? Fdk27 { get; private set; }


        [DataMember(Name = "fdk28")]
        public bool? Fdk28 { get; private set; }


        [DataMember(Name = "fdk29")]
        public bool? Fdk29 { get; private set; }


        [DataMember(Name = "fdk30")]
        public bool? Fdk30 { get; private set; }


        [DataMember(Name = "fdk31")]
        public bool? Fdk31 { get; private set; }


        [DataMember(Name = "fdk32")]
        public bool? Fdk32 { get; private set; }

    }


    public enum EntryCompletionEnum
    {
        Auto,
        Enter,
        Cancel,
        Continue,
        Clear,
        Backspace,
        Fdk,
        Help,
        Fk,
        ContFdk
    }


}
