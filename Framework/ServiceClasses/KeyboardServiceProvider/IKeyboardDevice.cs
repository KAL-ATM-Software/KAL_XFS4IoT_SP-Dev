/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * IKeyboardDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of keyboard. 
namespace XFS4IoTFramework.Keyboard
{
    public interface IKeyboardDevice : IDevice
    {
        /// <summary>
        /// This command allows an application to retrieve layout information for any device. Either one layout or all defined layouts can be retrieved with a single request of this command. There can be a layout for each of the different types of keyboard entry modes, if the vendor and the hardware support these different methods. The types of keyboard entry modes are: (1) Data Entry mode which corresponds to the [Keyboard.DataEntry](#keyboard.dataentry) command,(2) PIN Entry mode which corresponds to the [Keyboard.PinEntry](#keyboard.pinentry) command, (3) Secure Key Entry mode which corresponds to the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command. The layouts can be preloaded into the device, if the device supports this, or a single layout can be loaded into the device immediately prior to the keyboard command being requested.
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.GetLayoutCompletion.PayloadData> GetLayout(IGetLayoutEvents events, 
                                                                                     XFS4IoT.Keyboard.Commands.GetLayoutCommand.PayloadData payload, 
                                                                                     CancellationToken cancellation);

        /// <summary>
        /// This function stores the pin entry via the Keyboard device. From the point this function is invoked, pin digit entries are not passed to the application. For each pin digit, or any other active key entered, an execute notification event [Keyboard.KeyEvent](#keyboard.keyevent) is sent in order to allow an application to perform the appropriate display action (i.e. when the pin pad has no integrated display). The application is not informed of the value entered. The execute notification only informs that a key has been depressed. The [Keyboard.EnterDataEvent](#keyboard.enterdataevent) will be generated when the Keyboard is ready for the user to start entering data. Some Keyboard devices do not inform the application as each PIN digit is entered, but locally process the PIN entry based upon minimum pin length and maximum PIN length input parameters. When the maximum number of pin digits is entered and the flag autoEnd is true, or a terminating key is pressed after the minimum number of pin digits is entered, the command completes.If the &lt;Cancel&gt; key is a terminator key and is pressed, then the command will complete successfully even if the minimum number of pin digits has not been entered. Terminating FDKs can have the functionality of &lt;Enter&gt; (terminates only if minimum length has been reached) or &lt;Cancel&gt; (can terminate before minimum length is reached). The configuration of this functionality is vendor specific.If maxLen is zero, the Service Provider does not terminate the command unless the application sets terminateKeys or terminateFDKs. In the event that terminateKeys or terminateFDKs are not set and maxLen is zero, the command will not terminate and the application must issue a Cancel command. If active the fkCancel and fkClear keys will cause the PIN buffer to be cleared. The fkBackspace key will cause the last key in the PIN buffer to be removed. Terminating keys have to be active keys to operate. If this command is canceled by a CancelAsyncRequest the PIN buffer is not cleared. If maxLen has been met and autoEnd is set to False, then all numeric keys will automatically be disabled. If the clear or backspace key is pressed to reduce the number of entered keys, the numeric keys will be re-enabled. If the enter key (or FDK representing the enter key - note that the association of an FDK to enter functionality is vendor specific) is pressed prior to minLen being met, then the enter key or FDK is ignored. In some cases the Keyboard device cannot ignore the enter key then the command will complete normally. To handle these types of devices the application should use the output parameter digits property to check that sufficient digits have been entered.  The application should then get the user to re-enter their PIN with the correct number of digits. If the application makes a call to [PinPad.GetPinblock](#pinpad.getpinblock) or a local verification command without the minimum PIN digits having been entered, either the command will fail or the PIN verification will fail. It is the responsibility of the application to identify the mapping between the FDK code and the physical location of the FDK.
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.PinEntryCompletion.PayloadData> PinEntry(IPinEntryEvents events, 
                                                                                   XFS4IoT.Keyboard.Commands.PinEntryCommand.PayloadData payload, 
                                                                                   CancellationToken cancellation);

        /// <summary>
        /// This function enables keyboard insercure mode and report entered key in clear text with solicited events. For Keyboard device, this command will clear the pin unless the application has requested that the pin be maintained through the [PinPad.MaintainPin](#pinpad.maintainpin) command.
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.DataEntryCompletion.PayloadData> DataEntry(IDataEntryEvents events, 
                                                                                     XFS4IoT.Keyboard.Commands.DataEntryCommand.PayloadData payload, 
                                                                                     CancellationToken cancellation);

        /// <summary>
        /// Sends a service reset to the Service Provider. 
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.ResetCompletion.PayloadData> Reset(IResetEvents events, 
                                                                             XFS4IoT.Keyboard.Commands.ResetCommand.PayloadData payload, 
                                                                             CancellationToken cancellation);

        /// <summary>
        /// This command allows a full length symmetric encryption key part to be entered directly into the device without being exposed outside of the device. From the point this function is invoked, encryption key digits (fk0 to fk9 and fkA to fkF) are not passed to the application. For each encryption key digit, or any other active key entered (except for shift), an execute notification event [Keyboard.KeyEvent](#keyboard.keyevent) is sent in order to allow an application to perform the appropriate display action (i.e. when the device has no integrated display). When an encryption key digit is entered the application is not informed of the value entered, instead zero is returned. The [Keyboard.EnterDataEvent](#keyboard.enterdataevent) will be generated when the device is ready for the user to start entering data. The keys that can be enabled by this command are defined by the FuncKeyDetail parameter of the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command. Function keys which are not associated with an encryption key digit may be enabled but will not contribute to the secure entry buffer (unless they are Cancel, Clear or Backspace) and will not count towards the length of the key entry. The Cancel and Clear keys will cause the encryption key buffer to be cleared. The Backspace key will cause the last encryption key digit in the encryption key buffer to be removed.If autoEnd is TRUE the command will automatically complete when the required number of encryption key digits have been added to the buffer. If autoEnd is FALSE then the command will not automatically complete and Enter, Cancel or any terminating key must be pressed. When keyLen hex encryption key digits have been entered then all encryption key digits keys are disabled. If the Clear or Backspace key is pressed to reduce the number of entered encryption key digits below usKeyLen, the same keys will be reenabled. Terminating keys have to be active keys to operate.If an FDK is associated with Enter, Cancel, Clear or Backspace then the FDK must be activated to operate. The Enter and Cancel FDKs must also be marked as a terminator if they are to terminate entry. These FDKs are reported as normal FDKs within the KeyEvent, applications must be aware of those FDKs associated with Cancel, Clear, Backspace and Enter and handle any user interaction as required. For example, if the fdk01 is associated with Clear, then the application must include the fk_fdk01 FDK code in the activeFDKs parameter (if the clear functionality is required). In addition when this FDK is pressed the [Keyboard.KeyEvent](#keyboard.keyevent) will contain the fk_fdk01 mask value in the digit property. The application must update the user interface to reflect the effect of the clear on the encryption key digits entered so far. On some devices that are configured as either regularUnique or irregularUnique all the function keys on the device will be associated with hex digits and there may be no FDKs available either. On these devices there may be no way to correct mistakes or cancel the key encryption entry before all the encryption key digits are entered, so the application must set the autoEnd flag to TRUE and wait for the command to auto-complete. Applications should check the KCV to avoid storing an incorrect key component. Encryption key parts entered with this command are stored through either the [KeyManagement.ImportKey](#keymanagement.importkey). Each key part can only be stored once after which the secure key buffer will be cleared automatically.
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.SecureKeyEntryCompletion.PayloadData> SecureKeyEntry(ISecureKeyEntryEvents events, 
                                                                                               XFS4IoT.Keyboard.Commands.SecureKeyEntryCommand.PayloadData payload, 
                                                                                               CancellationToken cancellation);

        /// <summary>
        /// This command is used to enable or disable the device from emitting a beep tone on subsequent key presses of active or in-active keys. This command is valid only on devices which have the capability to support application control of automatic beeping. See [Capabilities](#common.capabilities.completion.properties.keyboard) structure for information.
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.KeypressBeepCompletion.PayloadData> KeypressBeep(IKeypressBeepEvents events, 
                                                                                           XFS4IoT.Keyboard.Commands.KeypressBeepCommand.PayloadData payload, 
                                                                                           CancellationToken cancellation);

        /// <summary>
        /// This command allows an application to configure a layout for any device. One or more layouts can be defined with a single request of this command. There can be a layout for each of the different types of keyboard entry modes, if the vendor and the hardware supports these different methods.The types of keyboard entry modes are (1) Mouse mode,(2) Data mode which corresponds to the [Keyboard.DataEntry](#keyboard.dataentry) command,(3) PIN mode which corresponds to the [Keyboard.PinEntry](#keyboard.pinentry) command,(4) Secure mode which corresponds to the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command. One or more layouts can be preloaded into the device, if the device supports this, or a single layout can be loaded into the device immediately prior to the keyboard command being requested. If a [Keyboard.DataEntry](#keyboard.dataentry), [Keyboard.PinEntry](#keyboard.pinentry), or [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command is already in progress (or queued), then this command is rejected with a command result of SequenceError. Layouts defined with this command are persistent.
        /// </summary>
        Task<XFS4IoT.Keyboard.Completions.DefineLayoutCompletion.PayloadData> DefineLayout(IDefineLayoutEvents events, 
                                                                                           XFS4IoT.Keyboard.Commands.DefineLayoutCommand.PayloadData payload, 
                                                                                           CancellationToken cancellation);

    }
}
