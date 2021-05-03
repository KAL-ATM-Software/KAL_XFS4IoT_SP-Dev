/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ITextTerminalDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of textterminal. 
namespace XFS4IoTFramework.TextTerminal
{
    public interface ITextTerminalDevice : IDevice
    {

        /// <summary>
        /// |-  This command is used to retrieve the  list of forms available on the device.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.GetFormListCompletion.PayloadData> GetFormList(IGetFormListEvents events, 
                                                                                             XFS4IoT.TextTerminal.Commands.GetFormListCommand.PayloadData payload, 
                                                                                             CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve details of the definition of a specified form.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.GetQueryFormCompletion.PayloadData> GetQueryForm(IGetQueryFormEvents events, 
                                                                                               XFS4IoT.TextTerminal.Commands.GetQueryFormCommand.PayloadData payload, 
                                                                                               CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve details of the definition of a single or all fields on a specified form.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.GetQueryFieldCompletion.PayloadData> GetQueryField(IGetQueryFieldEvents events, 
                                                                                                 XFS4IoT.TextTerminal.Commands.GetQueryFieldCommand.PayloadData payload, 
                                                                                                 CancellationToken cancellation);

        /// <summary>
        /// This command returns information about the Keys (buttons) supported by the device. This command should be issued to determine which Keys are available.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.GetKeyDetailCompletion.PayloadData> GetKeyDetail(IGetKeyDetailEvents events, 
                                                                                               XFS4IoT.TextTerminal.Commands.GetKeyDetailCommand.PayloadData payload, 
                                                                                               CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to beep at the text terminal unit.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.BeepCompletion.PayloadData> Beep(IBeepEvents events, 
                                                                               XFS4IoT.TextTerminal.Commands.BeepCommand.PayloadData payload, 
                                                                               CancellationToken cancellation);

        /// <summary>
        /// |-  This command clears the specified area of the text terminal unit screen.The cursor is positioned to the upper left corner of the cleared area.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.ClearScreenCompletion.PayloadData> ClearScreen(IClearScreenEvents events, 
                                                                                             XFS4IoT.TextTerminal.Commands.ClearScreenCommand.PayloadData payload, 
                                                                                             CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to switch the lighting of the text terminal unit on or off.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.DispLightCompletion.PayloadData> DispLight(IDispLightEvents events, 
                                                                                         XFS4IoT.TextTerminal.Commands.DispLightCommand.PayloadData payload, 
                                                                                         CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to set the status of the LEDs.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.SetLedCompletion.PayloadData> SetLed(ISetLedEvents events, 
                                                                                   XFS4IoT.TextTerminal.Commands.SetLedCommand.PayloadData payload, 
                                                                                   CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the resolution of the display. The screen is cleared and the cursor is positioned at the upper left position.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.SetResolutionCompletion.PayloadData> SetResolution(ISetResolutionEvents events, 
                                                                                                 XFS4IoT.TextTerminal.Commands.SetResolutionCommand.PayloadData payload, 
                                                                                                 CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to display a form by merging the supplied variable field data with the defined form and field data specified in the form.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.WriteFormCompletion.PayloadData> WriteForm(IWriteFormEvents events, 
                                                                                         XFS4IoT.TextTerminal.Commands.WriteFormCommand.PayloadData payload, 
                                                                                         CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to read data from input fields on the specified form.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.ReadFormCompletion.PayloadData> ReadForm(IReadFormEvents events, 
                                                                                       XFS4IoT.TextTerminal.Commands.ReadFormCommand.PayloadData payload, 
                                                                                       CancellationToken cancellation);

        /// <summary>
        /// This command displays the specified text on the display of the text terminal unit. The specified text may include the control characters CR (Carriage Return) and LF (Line Feed). The control characters can be included in the text as CR, or LF, or CR LF, or LF CR and all combinations will perform the function of relocating the cursor position to the left hand side of the display on the next line down. If the text will overwrite the display area then the display will scroll.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.WriteCompletion.PayloadData> Write(IWriteEvents events, 
                                                                                 XFS4IoT.TextTerminal.Commands.WriteCommand.PayloadData payload, 
                                                                                 CancellationToken cancellation);

        /// <summary>
        /// This command activates the keyboard of the text terminal unit for input of the specified number of characters. Depending on the specified flush mode the input buffer is cleared. During this command, pressing an active key results in a [TextTerminal.KeyEvent](#textterminal.keyevent) event containing the key details. On completion of the command (when the maximum number of keys have been pressed or a terminator key is pressed), the entered string, as interpreted by the Service Provider, is returned. The Service Provider takes command keys into account when interpreting the data.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.ReadCompletion.PayloadData> Read(IReadEvents events, 
                                                                               XFS4IoT.TextTerminal.Commands.ReadCommand.PayloadData payload, 
                                                                               CancellationToken cancellation);

        /// <summary>
        /// Sends a service reset to the Service Provider. This command clears the screen, clears the keyboard buffer, sets the default resolution and sets the cursor position to the upper left.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.ResetCompletion.PayloadData> Reset(IResetEvents events, 
                                                                                 XFS4IoT.TextTerminal.Commands.ResetCommand.PayloadData payload, 
                                                                                 CancellationToken cancellation);

        /// <summary>
        /// This command defines the keys that will be active during the next [TextTerminal.ReadForm](#textterminal.readform) command.The configured set will be active until the next [TextTerminal.ReadForm](#textterminal.readform) command ends, at which point the default values are restored.
        /// </summary>
        Task<XFS4IoT.TextTerminal.Completions.DefineKeysCompletion.PayloadData> DefineKeys(IDefineKeysEvents events, 
                                                                                           XFS4IoT.TextTerminal.Commands.DefineKeysCommand.PayloadData payload, 
                                                                                           CancellationToken cancellation);

    }
}
