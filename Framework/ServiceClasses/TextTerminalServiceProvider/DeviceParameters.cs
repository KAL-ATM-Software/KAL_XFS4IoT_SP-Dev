/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoT;

namespace XFS4IoTFramework.TextTerminal
{
    public sealed class BeepRequest
    {
        [Flags]
        public enum BeepTypeEnum
        {
            None,
            KeyPress,
            Exclamation,
            Warning,
            Error,
            Critical,
        }

        public enum BeepActionEnum
        {
            Off,
            On,
            Continuous,
        }

        public BeepRequest(BeepTypeEnum Type, BeepActionEnum Action)
        {
            this.Type = Type;
            this.Action = Action;
        }

        /// <summary>
        /// Beep type to enable
        /// </summary>
        BeepTypeEnum Type { get; init; }

        /// <summary>
        /// Beep action to turn on, off or Continuous
        /// </summary>
        BeepActionEnum Action { get; init; }

    }

    public sealed class SetResolutionRequest
    {
        public SetResolutionRequest(int SizeX, int SizeY)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }

        /// <summary>
        /// SizeX for the new resolution.
        /// </summary>
        public int SizeX { get; init; }
        /// <summary>
        /// SizeY for the new resolution.
        /// </summary>
        public int SizeY { get; init; }
    }

    public sealed class LEDOnRequest
    {
        public enum LEDCommandEnum
        {
            SlowFlash, MediumFlash, QuickFlash, Continuous
        }

        public enum LEDColorEnum
        {
            Default = 0, Red, Green, Yellow, Blue, Cyan, Magenta, White
        }

        public LEDOnRequest(int LEDNumber, LEDCommandEnum Command, LEDColorEnum Colour)
        {
            this.LEDNumber = LEDNumber;
            this.Command = Command;
            this.Colour = Colour;
        }

        /// <summary>
        /// LED Number to turn on.
        /// </summary>
        public int LEDNumber { get; init; }
        /// <summary>
        /// Action to set the LED.
        /// </summary>
        public LEDCommandEnum Command { get; init; }
        /// <summary>
        /// Colour to set the light.
        /// </summary>
        public LEDColorEnum Colour { get; init; }
    }

    public sealed class ClearScreenRequest
    {

        public ClearScreenRequest(int PositionX, int PositionY, int Height, int Width)
        {
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            this.Height = Height;
            this.Width = Width;
        }

        /// <summary>
        /// PositionX for the clear rectangle.
        /// </summary>
        public int PositionX { get; init; }
        
        /// <summary>
        /// PositionY for the clear rectangle.
        /// </summary>
        public int PositionY { get; init; }

        /// <summary>
        /// Height for the clear rectangle.
        /// </summary>
        public int  Height { get; init; }

        /// <summary>
        /// Width for the clear rectangle.
        /// </summary>
        public int  Width { get; init; }
    }

    public sealed class WriteRequest
    {
        [Flags]
        public enum TextAttributesEnum
        {
            None = 0,
            Underline,
            Inverted,
            Flash
        }

        public WriteRequest(int PosX, int PosY, string Text, TextAttributesEnum TextAttributes)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.Text = Text;
            this.TextAttributes = TextAttributes;
        }
        /// <summary>
        /// PositionX to start writing.
        /// </summary>
        public int PosX { get; init; }
        /// <summary>
        /// PositionY to start writing.
        /// </summary>
        public int PosY { get; init; }

        /// <summary>
        /// Text to write.
        /// Framework will check for new lines and ensure the text is within the display.
        /// </summary>
        public string Text { get; init; }

        /// <summary>
        /// Attributes to display the text.
        /// </summary>
        public TextAttributesEnum TextAttributes { get; init; }
    }

    public sealed class ReadRequest
    {
        [Flags]
        public enum TextAttributesEnum
        {
            None = 0,
            Underline,
            Inverted,
            Flash
        }

        public enum EchoModeEnum
        {
            Text,
            Invisible,
            Password
        }

        public ReadRequest(EchoModeEnum EchoMode, TextAttributesEnum EchoAttributes, int PositionX, int PositionY, int NumChars, bool Cursor, bool Flush, bool AutoEnd,string  ActiveKeys, List<string> ActiveCommandKeys, List<string> TerminateCommandKeys)
        {
            this.EchoMode = EchoMode;
            this.EchoAttributes = EchoAttributes;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            this.NumChars = NumChars;
            this.Cursor = Cursor;
            this.Flush = Flush;
            this.AutoEnd = AutoEnd;
            this.ActiveKeys = ActiveKeys;
            this.ActiveCommandKeys = ActiveCommandKeys;
            this.TerminateCommandKeys = TerminateCommandKeys;
        }

        /// <summary>
        /// Mode ot output the read keys.
        /// </summary>
        public EchoModeEnum EchoMode { get; init; }
        /// <summary>
        /// Text attributes when echoing the keys.
        /// </summary>
        public TextAttributesEnum EchoAttributes { get; init; }
        /// <summary>
        /// PositionX to start the read operation.
        /// </summary>
        public int PositionX { get; init; }
        /// <summary>
        /// PositionY to start the read operation.
        /// </summary>
        public int PositionY { get; init; }

        /// <summary>
        /// Number of Characters to read.
        /// </summary>
        public int NumChars { get; init; }
        /// <summary>
        /// To display the cursor or not.
        /// </summary>
        public bool Cursor { get; init; }
        /// <summary>
        /// If <see langword="true"/> any keys already read should be cleared.
        /// </summary>
        public bool Flush { get; init; }
        /// <summary>
        /// If <see langword="true"/> Entry should end once NumChars is reached.
        /// </summary>
        public bool AutoEnd { get; init; }

        /// <summary>
        /// String containing the ActiveKeys. e.g. "0123456789"
        /// </summary>
        public string ActiveKeys { get; init; }
        /// <summary>
        /// List of active command keys during the read operation
        /// </summary>
        public List<string> ActiveCommandKeys { get; init; }
        /// <summary>
        /// List of keys which can be pressed to terminiate the read operation.
        /// </summary>
        public List<string> TerminateCommandKeys { get; init; }
    }

    public sealed class ReadResult : DeviceResult
    {
        public ReadResult(MessagePayload.CompletionCodeEnum CompletionCode, string ErrorDescription = null, string Input = null) 
            : base(CompletionCode, ErrorDescription)
        {
            this.Input = Input;
        }
        /// <summary>
        /// Text read in during the Read operation.
        /// </summary>
        public string Input { get; init; }
    }
}
