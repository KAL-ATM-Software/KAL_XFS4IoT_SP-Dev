/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using TextTerminal;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

// KAL specific implementation of textterminal. 
namespace XFS4IoTFramework.TextTerminal
{
    public interface ITextTerminalDevice : ICommonDevice
    {

        /// <summary>
        /// This command is used to retrieve the  list of forms available on the device.
        /// </summary>
        Task<GetFormListCompletion.PayloadData> GetFormList(ITextTerminalConnection connection, GetFormListCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve details of the definition of a specified form.
        /// </summary>
        Task<GetQueryFormCompletion.PayloadData> GetQueryForm(ITextTerminalConnection connection, GetQueryFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve details of the definition of a single or all fields on a specified form.
        /// </summary>
        Task<GetQueryFieldCompletion.PayloadData> GetQueryField(ITextTerminalConnection connection, GetQueryFieldCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command returns information about the Keys (buttons) supported by the device.This command should be issued to determine which Keys are available.
        /// </summary>
        Task<GetKeyDetailCompletion.PayloadData> GetKeyDetail(ITextTerminalConnection connection, GetKeyDetailCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to beep at the text terminal unit.
        /// </summary>
        Task<BeepCompletion.PayloadData> Beep(ITextTerminalConnection connection, BeepCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command clears the specified area of the text terminal unit screen.The cursor is positioned to the upper left corner of the cleared area.
        /// </summary>
        Task<ClearScreenCompletion.PayloadData> ClearScreen(ITextTerminalConnection connection, ClearScreenCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to switch the lighting of the text terminal unit on or off.
        /// </summary>
        Task<DispLightCompletion.PayloadData> DispLight(ITextTerminalConnection connection, DispLightCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the status of the LEDs.
        /// </summary>
        Task<SetLedCompletion.PayloadData> SetLed(ITextTerminalConnection connection, SetLedCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the resolution of the display.The screen is cleared and the cursor is positioned at the upper left position.
        /// </summary>
        Task<SetResolutionCompletion.PayloadData> SetResolution(ITextTerminalConnection connection, SetResolutionCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to display a form by merging the supplied variable field data with the defined form and field data specified in the form.
        /// </summary>
        Task<WriteFormCompletion.PayloadData> WriteForm(ITextTerminalConnection connection, WriteFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to read data from input fields on the specified form.
        /// </summary>
        Task<ReadFormCompletion.PayloadData> ReadForm(ITextTerminalConnection connection, ReadFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command displays the specified text on the display of the text terminal unit. The specified text may include the control characters CR (Carriage Return) and LF (Line Feed). The control characters can be included in the text as CR, or LF, or CR LF, or LF CR and all combinations will perform the function of relocating the cursor position to the left hand side of the display on the next line down. If the text will overwrite the display area then the display will scroll.
        /// </summary>
        Task<WriteCompletion.PayloadData> Write(ITextTerminalConnection connection, WriteCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command activates the keyboard of the text terminal unit for input of the specified number of characters. Depending on the specified flush mode the input buffer is cleared.During this command, pressing an active key results in a Key event containing the key details. On completion of the command (when the maximum number of keys have been pressed or a terminator key is pressed), the entered string, as interpreted by the Service Provider, is returned. The Service Provider takes command keys into account when interpreting the data.
        /// </summary>
        Task<ReadCompletion.PayloadData> Read(ITextTerminalConnection connection, ReadCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// Sends a service reset to the Service Provider. This command clears the screen, clears the keyboard buffer, sets the default resolution and sets the cursor position to the upper left.
        /// </summary>
        Task<ResetCompletion.PayloadData> Reset(ITextTerminalConnection connection, ResetCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command defines the keys that will be active during the next ReadForm command.The configured set will be active until the next ReadForm command ends, at which point the default values are restored.
        /// </summary>
        Task<DefineKeysCompletion.PayloadData> DefineKeys(ITextTerminalConnection connection, DefineKeysCommand.PayloadData payload, CancellationToken cancellation);

    }
}
