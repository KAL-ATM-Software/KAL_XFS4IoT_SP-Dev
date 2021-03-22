/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetLed_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = SetLed
    [DataContract]
    [Command(Name = "TextTerminal.SetLed")]
    public sealed class SetLedCommand : Command<SetLedCommand.PayloadData>
    {
        public SetLedCommand(string RequestId, SetLedCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            /// <summary>
            ///Specifies off type A or a combination of the following flags consisting of one type B, and optionally one type C. If no value of type C is specified then the default color is used. The Service Provider determines which color is used as the default color.
            /// </summary>
            public class CommandClass
            {
                [DataMember(Name = "off")] 
                public bool? Off { get; private set; }
                [DataMember(Name = "slowFlash")] 
                public bool? SlowFlash { get; private set; }
                [DataMember(Name = "mediumFlash")] 
                public bool? MediumFlash { get; private set; }
                [DataMember(Name = "quickFlash")] 
                public bool? QuickFlash { get; private set; }
                [DataMember(Name = "continuous")] 
                public bool? Continuous { get; private set; }
                [DataMember(Name = "red")] 
                public bool? Red { get; private set; }
                [DataMember(Name = "green")] 
                public bool? Green { get; private set; }
                [DataMember(Name = "yellow")] 
                public bool? Yellow { get; private set; }
                [DataMember(Name = "blue")] 
                public bool? Blue { get; private set; }
                [DataMember(Name = "cyan")] 
                public bool? Cyan { get; private set; }
                [DataMember(Name = "magenta")] 
                public bool? Magenta { get; private set; }
                [DataMember(Name = "white")] 
                public bool? White { get; private set; }

                public CommandClass (bool? Off, bool? SlowFlash, bool? MediumFlash, bool? QuickFlash, bool? Continuous, bool? Red, bool? Green, bool? Yellow, bool? Blue, bool? Cyan, bool? Magenta, bool? White)
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


            }


            public PayloadData(int Timeout, int? Led = null, object Command = null)
                : base(Timeout)
            {
                this.Led = Led;
                this.Command = Command;
            }

            /// <summary>
            ///Specifies the index array as reported in Capabilities) of the LED to set as one of the values defined within the capabilities section.
            /// </summary>
            [DataMember(Name = "led")] 
            public int? Led { get; private set; }
            /// <summary>
            ///Specifies off type A or a combination of the following flags consisting of one type B, and optionally one type C. If no value of type C is specified then the default color is used. The Service Provider determines which color is used as the default color.
            /// </summary>
            [DataMember(Name = "command")] 
            public object Command { get; private set; }

        }
    }
}
