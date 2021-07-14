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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string Keys = null, List<CommandKeysClass> CommandKeys = null)
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
                public CommandKeysClass(bool? CkEnter = null, bool? CkCancel = null, bool? CkClear = null, bool? CkBackspace = null, bool? CkHelp = null, bool? Ck00 = null, bool? Ck000 = null, bool? CkArrowUp = null, bool? CkArrowDown = null, bool? CkArrowLeft = null, bool? CkArrowRight = null, bool? CkOEM1 = null, bool? CkOEM2 = null, bool? CkOEM3 = null, bool? CkOEM4 = null, bool? CkOEM5 = null, bool? CkOEM6 = null, bool? CkOEM7 = null, bool? CkOEM8 = null, bool? CkOEM9 = null, bool? CkOEM10 = null, bool? CkOEM11 = null, bool? CkOEM12 = null, bool? CkFDK01 = null, bool? CkFDK02 = null, bool? CkFDK03 = null, bool? CkFDK04 = null, bool? CkFDK05 = null, bool? CkFDK06 = null, bool? CkFDK07 = null, bool? CkFDK08 = null, bool? CkFDK09 = null, bool? CkFDK10 = null, bool? CkFDK11 = null, bool? CkFDK12 = null, bool? CkFDK13 = null, bool? CkFDK14 = null, bool? CkFDK15 = null, bool? CkFDK16 = null, bool? CkFDK17 = null, bool? CkFDK18 = null, bool? CkFDK19 = null, bool? CkFDK20 = null, bool? CkFDK21 = null, bool? CkFDK22 = null, bool? CkFDK23 = null, bool? CkFDK24 = null, bool? CkFDK25 = null, bool? CkFDK26 = null, bool? CkFDK27 = null, bool? CkFDK28 = null, bool? CkFDK29 = null, bool? CkFDK30 = null, bool? CkFDK31 = null, bool? CkFDK32 = null)
                {
                    this.CkEnter = CkEnter;
                    this.CkCancel = CkCancel;
                    this.CkClear = CkClear;
                    this.CkBackspace = CkBackspace;
                    this.CkHelp = CkHelp;
                    this.Ck00 = Ck00;
                    this.Ck000 = Ck000;
                    this.CkArrowUp = CkArrowUp;
                    this.CkArrowDown = CkArrowDown;
                    this.CkArrowLeft = CkArrowLeft;
                    this.CkArrowRight = CkArrowRight;
                    this.CkOEM1 = CkOEM1;
                    this.CkOEM2 = CkOEM2;
                    this.CkOEM3 = CkOEM3;
                    this.CkOEM4 = CkOEM4;
                    this.CkOEM5 = CkOEM5;
                    this.CkOEM6 = CkOEM6;
                    this.CkOEM7 = CkOEM7;
                    this.CkOEM8 = CkOEM8;
                    this.CkOEM9 = CkOEM9;
                    this.CkOEM10 = CkOEM10;
                    this.CkOEM11 = CkOEM11;
                    this.CkOEM12 = CkOEM12;
                    this.CkFDK01 = CkFDK01;
                    this.CkFDK02 = CkFDK02;
                    this.CkFDK03 = CkFDK03;
                    this.CkFDK04 = CkFDK04;
                    this.CkFDK05 = CkFDK05;
                    this.CkFDK06 = CkFDK06;
                    this.CkFDK07 = CkFDK07;
                    this.CkFDK08 = CkFDK08;
                    this.CkFDK09 = CkFDK09;
                    this.CkFDK10 = CkFDK10;
                    this.CkFDK11 = CkFDK11;
                    this.CkFDK12 = CkFDK12;
                    this.CkFDK13 = CkFDK13;
                    this.CkFDK14 = CkFDK14;
                    this.CkFDK15 = CkFDK15;
                    this.CkFDK16 = CkFDK16;
                    this.CkFDK17 = CkFDK17;
                    this.CkFDK18 = CkFDK18;
                    this.CkFDK19 = CkFDK19;
                    this.CkFDK20 = CkFDK20;
                    this.CkFDK21 = CkFDK21;
                    this.CkFDK22 = CkFDK22;
                    this.CkFDK23 = CkFDK23;
                    this.CkFDK24 = CkFDK24;
                    this.CkFDK25 = CkFDK25;
                    this.CkFDK26 = CkFDK26;
                    this.CkFDK27 = CkFDK27;
                    this.CkFDK28 = CkFDK28;
                    this.CkFDK29 = CkFDK29;
                    this.CkFDK30 = CkFDK30;
                    this.CkFDK31 = CkFDK31;
                    this.CkFDK32 = CkFDK32;
                }


                [DataMember(Name = "ckEnter")]
                public bool? CkEnter { get; init; }


                [DataMember(Name = "ckCancel")]
                public bool? CkCancel { get; init; }


                [DataMember(Name = "ckClear")]
                public bool? CkClear { get; init; }


                [DataMember(Name = "ckBackspace")]
                public bool? CkBackspace { get; init; }


                [DataMember(Name = "ckHelp")]
                public bool? CkHelp { get; init; }


                [DataMember(Name = "ck00")]
                public bool? Ck00 { get; init; }


                [DataMember(Name = "ck000")]
                public bool? Ck000 { get; init; }


                [DataMember(Name = "ckArrowUp")]
                public bool? CkArrowUp { get; init; }


                [DataMember(Name = "ckArrowDown")]
                public bool? CkArrowDown { get; init; }


                [DataMember(Name = "ckArrowLeft")]
                public bool? CkArrowLeft { get; init; }


                [DataMember(Name = "ckArrowRight")]
                public bool? CkArrowRight { get; init; }


                [DataMember(Name = "ckOEM1")]
                public bool? CkOEM1 { get; init; }


                [DataMember(Name = "ckOEM2")]
                public bool? CkOEM2 { get; init; }


                [DataMember(Name = "ckOEM3")]
                public bool? CkOEM3 { get; init; }


                [DataMember(Name = "ckOEM4")]
                public bool? CkOEM4 { get; init; }


                [DataMember(Name = "ckOEM5")]
                public bool? CkOEM5 { get; init; }


                [DataMember(Name = "ckOEM6")]
                public bool? CkOEM6 { get; init; }


                [DataMember(Name = "ckOEM7")]
                public bool? CkOEM7 { get; init; }


                [DataMember(Name = "ckOEM8")]
                public bool? CkOEM8 { get; init; }


                [DataMember(Name = "ckOEM9")]
                public bool? CkOEM9 { get; init; }


                [DataMember(Name = "ckOEM10")]
                public bool? CkOEM10 { get; init; }


                [DataMember(Name = "ckOEM11")]
                public bool? CkOEM11 { get; init; }


                [DataMember(Name = "ckOEM12")]
                public bool? CkOEM12 { get; init; }


                [DataMember(Name = "ckFDK01")]
                public bool? CkFDK01 { get; init; }


                [DataMember(Name = "ckFDK02")]
                public bool? CkFDK02 { get; init; }


                [DataMember(Name = "ckFDK03")]
                public bool? CkFDK03 { get; init; }


                [DataMember(Name = "ckFDK04")]
                public bool? CkFDK04 { get; init; }


                [DataMember(Name = "ckFDK05")]
                public bool? CkFDK05 { get; init; }


                [DataMember(Name = "ckFDK06")]
                public bool? CkFDK06 { get; init; }


                [DataMember(Name = "ckFDK07")]
                public bool? CkFDK07 { get; init; }


                [DataMember(Name = "ckFDK08")]
                public bool? CkFDK08 { get; init; }


                [DataMember(Name = "ckFDK09")]
                public bool? CkFDK09 { get; init; }


                [DataMember(Name = "ckFDK10")]
                public bool? CkFDK10 { get; init; }


                [DataMember(Name = "ckFDK11")]
                public bool? CkFDK11 { get; init; }


                [DataMember(Name = "ckFDK12")]
                public bool? CkFDK12 { get; init; }


                [DataMember(Name = "ckFDK13")]
                public bool? CkFDK13 { get; init; }


                [DataMember(Name = "ckFDK14")]
                public bool? CkFDK14 { get; init; }


                [DataMember(Name = "ckFDK15")]
                public bool? CkFDK15 { get; init; }


                [DataMember(Name = "ckFDK16")]
                public bool? CkFDK16 { get; init; }


                [DataMember(Name = "ckFDK17")]
                public bool? CkFDK17 { get; init; }


                [DataMember(Name = "ckFDK18")]
                public bool? CkFDK18 { get; init; }


                [DataMember(Name = "ckFDK19")]
                public bool? CkFDK19 { get; init; }


                [DataMember(Name = "ckFDK20")]
                public bool? CkFDK20 { get; init; }


                [DataMember(Name = "ckFDK21")]
                public bool? CkFDK21 { get; init; }


                [DataMember(Name = "ckFDK22")]
                public bool? CkFDK22 { get; init; }


                [DataMember(Name = "ckFDK23")]
                public bool? CkFDK23 { get; init; }


                [DataMember(Name = "ckFDK24")]
                public bool? CkFDK24 { get; init; }


                [DataMember(Name = "ckFDK25")]
                public bool? CkFDK25 { get; init; }


                [DataMember(Name = "ckFDK26")]
                public bool? CkFDK26 { get; init; }


                [DataMember(Name = "ckFDK27")]
                public bool? CkFDK27 { get; init; }


                [DataMember(Name = "ckFDK28")]
                public bool? CkFDK28 { get; init; }


                [DataMember(Name = "ckFDK29")]
                public bool? CkFDK29 { get; init; }


                [DataMember(Name = "ckFDK30")]
                public bool? CkFDK30 { get; init; }


                [DataMember(Name = "ckFDK31")]
                public bool? CkFDK31 { get; init; }


                [DataMember(Name = "ckFDK32")]
                public bool? CkFDK32 { get; init; }

            }

            /// <summary>
            /// Array of command keys on the Text Terminal Unit.
            /// </summary>
            [DataMember(Name = "commandKeys")]
            public List<CommandKeysClass> CommandKeys { get; init; }

        }
    }
}
