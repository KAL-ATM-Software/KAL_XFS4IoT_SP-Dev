/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetKeyDetail_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.GetKeyDetail")]
    public sealed class GetKeyDetailCompletion : Completion<GetKeyDetailCompletion.PayloadData>
    {
        public GetKeyDetailCompletion(int RequestId, GetKeyDetailCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string Keys = null, CommandKeysClass CommandKeys = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Keys = Keys;
                this.CommandKeys = CommandKeys;
            }

            /// <summary>
            /// String which holds the printable characters (numeric and alphanumeric keys) on the Text Terminal Unit, 
            /// e.g. “0123456789ABCabc” if those text terminal input keys are present. This field is not set if no keys 
            /// of this type are present on the device.
            /// </summary>
            [DataMember(Name = "keys")]
            public string Keys { get; init; }

            [DataContract]
            public sealed class CommandKeysClass
            {
                public CommandKeysClass(bool? Enter = null, bool? Cancel = null, bool? Clear = null, bool? Backspace = null, bool? Help = null, bool? DoubleZero = null, bool? TripleZero = null, bool? ArrowUp = null, bool? ArrowDown = null, bool? ArrowLeft = null, bool? ArrowRight = null, bool? Oem1 = null, bool? Oem2 = null, bool? Oem3 = null, bool? Oem4 = null, bool? Oem5 = null, bool? Oem6 = null, bool? Oem7 = null, bool? Oem8 = null, bool? Oem9 = null, bool? Oem10 = null, bool? Oem11 = null, bool? Oem12 = null, bool? Fdk01 = null, bool? Fdk02 = null, bool? Fdk03 = null, bool? Fdk04 = null, bool? Fdk05 = null, bool? Fdk06 = null, bool? Fdk07 = null, bool? Fdk08 = null, bool? Fdk09 = null, bool? Fdk10 = null, bool? Fdk11 = null, bool? Fdk12 = null, bool? Fdk13 = null, bool? Fdk14 = null, bool? Fdk15 = null, bool? Fdk16 = null, bool? Fdk17 = null, bool? Fdk18 = null, bool? Fdk19 = null, bool? Fdk20 = null, bool? Fdk21 = null, bool? Fdk22 = null, bool? Fdk23 = null, bool? Fdk24 = null, bool? Fdk25 = null, bool? Fdk26 = null, bool? Fdk27 = null, bool? Fdk28 = null, bool? Fdk29 = null, bool? Fdk30 = null, bool? Fdk31 = null, bool? Fdk32 = null)
                {
                    this.Enter = Enter;
                    this.Cancel = Cancel;
                    this.Clear = Clear;
                    this.Backspace = Backspace;
                    this.Help = Help;
                    this.DoubleZero = DoubleZero;
                    this.TripleZero = TripleZero;
                    this.ArrowUp = ArrowUp;
                    this.ArrowDown = ArrowDown;
                    this.ArrowLeft = ArrowLeft;
                    this.ArrowRight = ArrowRight;
                    this.Oem1 = Oem1;
                    this.Oem2 = Oem2;
                    this.Oem3 = Oem3;
                    this.Oem4 = Oem4;
                    this.Oem5 = Oem5;
                    this.Oem6 = Oem6;
                    this.Oem7 = Oem7;
                    this.Oem8 = Oem8;
                    this.Oem9 = Oem9;
                    this.Oem10 = Oem10;
                    this.Oem11 = Oem11;
                    this.Oem12 = Oem12;
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


                [DataMember(Name = "enter")]
                public bool? Enter { get; init; }


                [DataMember(Name = "cancel")]
                public bool? Cancel { get; init; }


                [DataMember(Name = "clear")]
                public bool? Clear { get; init; }


                [DataMember(Name = "backspace")]
                public bool? Backspace { get; init; }


                [DataMember(Name = "help")]
                public bool? Help { get; init; }


                [DataMember(Name = "doubleZero")]
                public bool? DoubleZero { get; init; }


                [DataMember(Name = "tripleZero")]
                public bool? TripleZero { get; init; }


                [DataMember(Name = "arrowUp")]
                public bool? ArrowUp { get; init; }


                [DataMember(Name = "arrowDown")]
                public bool? ArrowDown { get; init; }


                [DataMember(Name = "arrowLeft")]
                public bool? ArrowLeft { get; init; }


                [DataMember(Name = "arrowRight")]
                public bool? ArrowRight { get; init; }


                [DataMember(Name = "oem1")]
                public bool? Oem1 { get; init; }


                [DataMember(Name = "oem2")]
                public bool? Oem2 { get; init; }


                [DataMember(Name = "oem3")]
                public bool? Oem3 { get; init; }


                [DataMember(Name = "oem4")]
                public bool? Oem4 { get; init; }


                [DataMember(Name = "oem5")]
                public bool? Oem5 { get; init; }


                [DataMember(Name = "oem6")]
                public bool? Oem6 { get; init; }


                [DataMember(Name = "oem7")]
                public bool? Oem7 { get; init; }


                [DataMember(Name = "oem8")]
                public bool? Oem8 { get; init; }


                [DataMember(Name = "oem9")]
                public bool? Oem9 { get; init; }


                [DataMember(Name = "oem10")]
                public bool? Oem10 { get; init; }


                [DataMember(Name = "oem11")]
                public bool? Oem11 { get; init; }


                [DataMember(Name = "oem12")]
                public bool? Oem12 { get; init; }


                [DataMember(Name = "fdk01")]
                public bool? Fdk01 { get; init; }


                [DataMember(Name = "fdk02")]
                public bool? Fdk02 { get; init; }


                [DataMember(Name = "fdk03")]
                public bool? Fdk03 { get; init; }


                [DataMember(Name = "fdk04")]
                public bool? Fdk04 { get; init; }


                [DataMember(Name = "fdk05")]
                public bool? Fdk05 { get; init; }


                [DataMember(Name = "fdk06")]
                public bool? Fdk06 { get; init; }


                [DataMember(Name = "fdk07")]
                public bool? Fdk07 { get; init; }


                [DataMember(Name = "fdk08")]
                public bool? Fdk08 { get; init; }


                [DataMember(Name = "fdk09")]
                public bool? Fdk09 { get; init; }


                [DataMember(Name = "fdk10")]
                public bool? Fdk10 { get; init; }


                [DataMember(Name = "fdk11")]
                public bool? Fdk11 { get; init; }


                [DataMember(Name = "fdk12")]
                public bool? Fdk12 { get; init; }


                [DataMember(Name = "fdk13")]
                public bool? Fdk13 { get; init; }


                [DataMember(Name = "fdk14")]
                public bool? Fdk14 { get; init; }


                [DataMember(Name = "fdk15")]
                public bool? Fdk15 { get; init; }


                [DataMember(Name = "fdk16")]
                public bool? Fdk16 { get; init; }


                [DataMember(Name = "fdk17")]
                public bool? Fdk17 { get; init; }


                [DataMember(Name = "fdk18")]
                public bool? Fdk18 { get; init; }


                [DataMember(Name = "fdk19")]
                public bool? Fdk19 { get; init; }


                [DataMember(Name = "fdk20")]
                public bool? Fdk20 { get; init; }


                [DataMember(Name = "fdk21")]
                public bool? Fdk21 { get; init; }


                [DataMember(Name = "fdk22")]
                public bool? Fdk22 { get; init; }


                [DataMember(Name = "fdk23")]
                public bool? Fdk23 { get; init; }


                [DataMember(Name = "fdk24")]
                public bool? Fdk24 { get; init; }


                [DataMember(Name = "fdk25")]
                public bool? Fdk25 { get; init; }


                [DataMember(Name = "fdk26")]
                public bool? Fdk26 { get; init; }


                [DataMember(Name = "fdk27")]
                public bool? Fdk27 { get; init; }


                [DataMember(Name = "fdk28")]
                public bool? Fdk28 { get; init; }


                [DataMember(Name = "fdk29")]
                public bool? Fdk29 { get; init; }


                [DataMember(Name = "fdk30")]
                public bool? Fdk30 { get; init; }


                [DataMember(Name = "fdk31")]
                public bool? Fdk31 { get; init; }


                [DataMember(Name = "fdk32")]
                public bool? Fdk32 { get; init; }

            }

            /// <summary>
            /// Supporting command keys on the Text Terminal Unit.
            /// </summary>
            [DataMember(Name = "commandKeys")]
            public CommandKeysClass CommandKeys { get; init; }

        }
    }
}
