/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoT.TextTerminal.Completions;
using XFS4IoTServer;

// KAL specific implementation of textterminal. 
namespace XFS4IoTFramework.TextTerminal
{
    public interface ITextTerminalDevice : IDevice
    {
        /// <summary>
        /// This command returns information about the Keys (buttons) supported by the device. This command should be issued to determine which Keys are available.
        /// </summary>
        GetKeyDetailCompletion.PayloadData GetKeyDetail();

        /// <summary>
        /// This command is used to beep at the text terminal unit.
        /// </summary>
        Task<DeviceResult> BeepAsync(BeepRequest beepInfo, CancellationToken cancellation);

        /// <summary>
        /// This command clears the specified area of the text terminal unit screen.
        /// The cursor is positioned to the upper left corner of the cleared area.
        /// </summary>
        Task<DeviceResult> ClearScreenAsync(ClearScreenRequest clearInfo, CancellationToken cancellation);

        /// <summary>
        /// This command is used to switch the lighting of the text terminal unit on
        /// </summary>
        Task<DeviceResult> DispLightOnAsync(CancellationToken cancellation);

        /// <summary>
        /// This command is used to switch the lighting of the text terminal unit off
        /// </summary>
        Task<DeviceResult> DispLightOffAsync(CancellationToken cancellation);

        /// <summary>
        /// This command is used to turn on an LED.
        /// </summary>
        Task<DeviceResult> LEDOnAsync(LEDOnRequest LEDOnInfo, CancellationToken cancellation);

        /// <summary>
        /// This command is used to turn off an LED.
        /// </summary>
        Task<DeviceResult> LEDOffAsync(int LED, CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the resolution of the display.
        /// ClearScreenAsync will be called first to clear the screen and set the cursor to the top left of the display.
        /// </summary>
        Task<DeviceResult> SetResolutionAsync(SetResolutionRequest resolutionInfo, CancellationToken cancellation);

        /// <summary>
        /// This command writes the specified line of text to the display.
        /// The Framework will handle new lines and ensure the text fits within the resolution.
        /// If required the Framework will ask the device to scroll the displayed text using ScrollAsync.
        /// </summary>
        Task<DeviceResult> WriteAsync(WriteRequest writeInfo, CancellationToken cancellation);

        /// <summary>
        /// Called by the Framework to scroll the text on the display.
        /// Will be called during a write operation if the text lines exceed the CurrentHeight.
        /// </summary>
        Task<DeviceResult> ScrollAsync(CancellationToken cancellation);

        /// <summary>
        /// Sends a service reset to the Service Provider. 
        /// This command clears the screen, clears the keyboard buffer, sets the default resolution and sets the cursor position to the upper left.
        /// </summary>
        Task<DeviceResult> ResetDeviceAsync(IResetEvents events, CancellationToken cancellation);

        /// <summary>
        /// This command is used to start a read operation on the device.
        /// A KeyPress event should be triggered for each valid key.
        /// The method will return once a termination key is pressed or if the numChars is reached when AutoEnd is true.
        /// </summary>
        Task<ReadResult> ReadAsync(ReadRequest readInfo, CancellationToken cancellation);


        /// <summary>
        /// Current Width for the display.
        /// </summary>
        int CurrentWidth { get; }
        /// <summary>
        /// Current Height for the display.
        /// </summary>
        int CurrentHeight { get; }

        /// <summary>
        /// Current Cursor X Position from the left.
        /// </summary>
        int CurrentX { get; }
        /// <summary>
        /// Current Cursor Y Position from the top of the display.
        /// </summary>
        int CurrentY { get; }

        /// <summary>
        /// Controls if the Framework should call ScrollAsync during a Write operation.
        /// If set to false and the text overwrites the display area then the Write operation will end.
        /// If set to true the Framework will make space for the text by calling ScrollAsync.
        /// </summary>
        bool ScrollingSupported { get; }
    }
}
