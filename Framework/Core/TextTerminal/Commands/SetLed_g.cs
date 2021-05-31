/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetLed_g.cs uses automatically generated parts.
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
        public SetLedCommand(int RequestId, SetLedCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? Led = null, CommandClass Command = null)
                : base(Timeout)
            {
                this.Led = Led;
                this.Command = Command;
            }

            /// <summary>
            /// Specifies the index array as reported in Capabilities of the LED to set as one of the values defined 
            /// within the capabilities section [TextTerminal.Capabilities](#common.capabilities.completion.properties.textterminal.leds)
            /// </summary>
            [DataMember(Name = "led")]
            public int? Led { get; private set; }

            [DataContract]
            public sealed class CommandClass
            {
                public CommandClass(bool? Off = null, bool? SlowFlash = null, bool? MediumFlash = null, bool? QuickFlash = null, bool? Continuous = null, bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
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
                /// The LED is turned off.
                /// Type A
                /// </summary>
                [DataMember(Name = "off")]
                public bool? Off { get; private set; }

                /// <summary>
                /// The LED is set to flash slowly.
                /// Type B
                /// </summary>
                [DataMember(Name = "slowFlash")]
                public bool? SlowFlash { get; private set; }

                /// <summary>
                /// The LED is set to flash medium frequency.
                /// Type B
                /// </summary>
                [DataMember(Name = "mediumFlash")]
                public bool? MediumFlash { get; private set; }

                /// <summary>
                /// The LED is set to flash quickly.
                /// Type B
                /// </summary>
                [DataMember(Name = "quickFlash")]
                public bool? QuickFlash { get; private set; }

                /// <summary>
                /// The LED is turned on continuously(steady).
                /// Type B
                /// </summary>
                [DataMember(Name = "continuous")]
                public bool? Continuous { get; private set; }

                /// <summary>
                /// The LED color is set to red.
                /// Type C
                /// </summary>
                [DataMember(Name = "red")]
                public bool? Red { get; private set; }

                /// <summary>
                /// The LED color is set to green.
                /// Type C
                /// </summary>
                [DataMember(Name = "green")]
                public bool? Green { get; private set; }

                /// <summary>
                /// The LED color is set to yellow.
                /// Type C
                /// </summary>
                [DataMember(Name = "yellow")]
                public bool? Yellow { get; private set; }

                /// <summary>
                /// The LED color is set to blue.
                /// Type C
                /// </summary>
                [DataMember(Name = "blue")]
                public bool? Blue { get; private set; }

                /// <summary>
                /// The LED color is set to cyan.
                /// Type C
                /// </summary>
                [DataMember(Name = "cyan")]
                public bool? Cyan { get; private set; }

                /// <summary>
                /// The LED color is set to magenta.
                /// Type C
                /// </summary>
                [DataMember(Name = "magenta")]
                public bool? Magenta { get; private set; }

                /// <summary>
                /// The LED is set to white.
                /// Type C
                /// </summary>
                [DataMember(Name = "white")]
                public bool? White { get; private set; }

            }

            /// <summary>
            /// Specifies off type A or a combination of the following flags consisting of one type B, and optionally one type C. 
            /// If no value of type C is specified then the default color is used. The Service Provider determines which color is used as the default color.
            /// </summary>
            [DataMember(Name = "command")]
            public CommandClass Command { get; private set; }

        }
    }
}
