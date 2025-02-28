/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoT;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public sealed class BeepRequest(
        BeepRequest.BeepTypeEnum Type, 
        BeepRequest.BeepActionEnum Action)
    {
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

        /// <summary>
        /// Beep type to enable
        /// </summary>
        public BeepTypeEnum Type { get; init; } = Type;

        /// <summary>
        /// Beep action to turn on, off or Continuous
        /// </summary>
        public BeepActionEnum Action { get; init; } = Action;

    }

    public sealed class SetResolutionRequest(int SizeX, int SizeY)
    {

        /// <summary>
        /// SizeX for the new resolution.
        /// </summary>
        public int SizeX { get; init; } = SizeX;
        /// <summary>
        /// SizeY for the new resolution.
        /// </summary>
        public int SizeY { get; init; } = SizeY;
    }

    public sealed class ClearScreenRequest(
        int PositionX, 
        int PositionY, 
        int Height, 
        int Width)
    {

        /// <summary>
        /// PositionX for the clear rectangle.
        /// </summary>
        public int PositionX { get; init; } = PositionX;

        /// <summary>
        /// PositionY for the clear rectangle.
        /// </summary>
        public int PositionY { get; init; } = PositionY;

        /// <summary>
        /// Height for the clear rectangle.
        /// </summary>
        public int Height { get; init; } = Height;

        /// <summary>
        /// Width for the clear rectangle.
        /// </summary>
        public int Width { get; init; } = Width;
    }

    public sealed class WriteRequest(
        int PosX, 
        int PosY, 
        string Text, 
        WriteRequest.TextAttributesEnum TextAttributes)
    {
        [Flags]
        public enum TextAttributesEnum
        {
            None = 0,
            Underline = 1 << 0,
            Inverted = 1 << 1,
            Flash = 1 << 2
        }

        /// <summary>
        /// PositionX to start writing.
        /// </summary>
        public int PosX { get; init; } = PosX;
        /// <summary>
        /// PositionY to start writing.
        /// </summary>
        public int PosY { get; init; } = PosY;

        /// <summary>
        /// Text to write.
        /// Framework will check for new lines and ensure the text is within the display.
        /// </summary>
        public string Text { get; init; } = Text;

        /// <summary>
        /// Attributes to display the text.
        /// </summary>
        public TextAttributesEnum TextAttributes { get; init; } = TextAttributes;
    }

    public sealed class ReadRequest(
        ReadRequest.EchoModeEnum EchoMode, 
        ReadRequest.TextAttributesEnum EchoAttributes, 
        int PositionX, 
        int PositionY, 
        int NumChars, 
        bool Cursor, 
        bool Flush, 
        bool AutoEnd, 
        List<string> ActiveKeys, 
        List<string> ActiveCommandKeys, 
        List<string> TerminateCommandKeys)
    {
        [Flags]
        public enum TextAttributesEnum
        {
            None = 0,
            Underline = 1 << 0,
            Inverted = 1 << 1,
            Flash = 1 << 2,
        }

        public enum EchoModeEnum
        {
            Text,
            Invisible,
            Password
        }

        /// <summary>
        /// Mode ot output the read keys.
        /// </summary>
        public EchoModeEnum EchoMode { get; init; } = EchoMode;
        /// <summary>
        /// Text attributes when echoing the keys.
        /// </summary>
        public TextAttributesEnum EchoAttributes { get; init; } = EchoAttributes;
        /// <summary>
        /// PositionX to start the read operation.
        /// </summary>
        public int PositionX { get; init; } = PositionX;
        /// <summary>
        /// PositionY to start the read operation.
        /// </summary>
        public int PositionY { get; init; } = PositionY;

        /// <summary>
        /// Number of Characters to read.
        /// </summary>
        public int NumChars { get; init; } = NumChars;
        /// <summary>
        /// To display the cursor or not.
        /// </summary>
        public bool Cursor { get; init; } = Cursor;
        /// <summary>
        /// If <see langword="true"/> any keys already read should be cleared.
        /// </summary>
        public bool Flush { get; init; } = Flush;
        /// <summary>
        /// If <see langword="true"/> Entry should end once NumChars is reached.
        /// </summary>
        public bool AutoEnd { get; init; } = AutoEnd;

        /// <summary>
        /// String containing the ActiveKeys. e.g. "0123456789"
        /// </summary>
        public List<string> ActiveKeys { get; init; } = ActiveKeys;
        /// <summary>
        /// List of active command keys during the read operation
        /// </summary>
        public List<string> ActiveCommandKeys { get; init; } = ActiveCommandKeys;
        /// <summary>
        /// List of keys which can be pressed to terminiate the read operation.
        /// </summary>
        public List<string> TerminateCommandKeys { get; init; } = TerminateCommandKeys;
    }

    public sealed class ReadResult(
        MessageHeader.CompletionCodeEnum CompletionCode, 
        string ErrorDescription = null, 
        string Input = null) 
        : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Text read in during the Read operation.
        /// </summary>
        public string Input { get; init; } = Input;
    }
}
