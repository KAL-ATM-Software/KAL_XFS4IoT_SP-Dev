/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of pinpad. 
namespace XFS4IoTFramework.PinPad
{
    public interface IPinPadDevice : IDevice
    {
        /// <summary>
        /// This command is used to report information in order to verify the PCI Security Standards Council PIN transaction security (PTS) certification held by the PIN device. 
        /// The command provides detailed information in order to verify the certification level of the device. 
        /// Support of this command by the Service Provider does not imply in anyway the certification level achieved by the device.
        /// Return null if it's not supported
        /// </summary>
        PCIPTSDeviceIdClass GetPCIPTSDeviceId();

        /// <summary>
        /// The PIN, which was entered with the GetPin command, is combined with the requisite data specified by the DES validation algorithm and locally verified for correctness. The result of the verification is returned to the application. This command will clear the PIN unless the application has requested that the PIN be maintained through the [PinPad.MaintinPin](#pinpad.maintainpin) command.
        /// </summary>
        Task<VerifyPINLocalResult> VerifyPINLocalDES(VerifyPINLocalDESRequest request, 
                                                     CancellationToken cancellation);

        /// <summary>
        /// The PIN, which was entered with the GetPin command, is combined with the requisite data specified by the VISA validation algorithm and locally verified for correctness. The result of the verification is returned to the application. This command will clear the PIN unless the application has requested that the PIN be maintained through the [PinPad.MaintinPin](#pinpad.maintainpin) command.
        /// </summary>
        Task<VerifyPINLocalResult> VerifyPINLocalVISA(VerifyPINLocalVISARequest request, 
                                                      CancellationToken cancellation);

        /// <summary>
        /// Sends a service reset to the Service Provider. 
        /// </summary>
        Task<DeviceResult> ResetDevice(CancellationToken cancellation);

        /// <summary>
        /// This command is used to control if the PIN is maintained after a PIN processing command for subsequent use by other PIN processing commands. 
        /// This command is also used to clear the PIN buffer when the PIN is no longer required.
        /// </summary>
        Task<DeviceResult> MaintainPin(bool MaintainPIN, CancellationToken cancellation);

        /// <summary>
        /// This function should be used for devices which need to know the data for the PIN block before the PIN is entered by the user. 
        /// keyboard interface GetPin and GetPinBlock should be called after this command.
        /// For all other devices Unsupported will be returned here. If this command is required and it is not called, the Keyboard.GetPin command will fail with the generic error SequenceError. If the input parameters passed to this commad and [PinPad.GetPinBlock](#pinpad.getpinblock) are not identical, the [PinPad.GetPinBlock](#pinpad.getpinblock) command will fail with the generic error InvalidData. The data associated with this command will be cleared on a [PinPad.GetPinBlock](#pinpad.getpinblock) command.
        /// </summary>
        Task<DeviceResult> SetPinBlockData(PINBlockRequest request, 
                                           CancellationToken cancellation);

        /// <summary>
        /// This function takes the account information and a PIN entered by the user to build a formatted PIN.Encrypting this formatted PIN once or twice returns a PIN block which can be written on a magnetic card or sent to a host. 
        /// The PIN block can be calculated using one of the algorithms specified in the device capabilities.
        /// This command will clear the PIN unless the application has requested that the PIN be maintained through the MaintinPin command enabled.
        /// </summary>
        Task<PINBlockResult> GetPinBlock(IGetPinBlockEvents events, 
                                         PINBlockRequest request, 
                                         CancellationToken cancellation);


        /// <summary>
        /// The PIN, which was entered with the EnterPin command, is combined with the requisite data specified by the IDC presentation algorithm and presented to the smartcard contained in the ID card unit.
        /// The result of the presentation is returned to the application. 
        /// This command will clear the PIN unless the application has requested that the PIN be maintained through the MaintainPin command.
        /// </summary>
        Task<PresentIDCResult> PresentIDC(PresentIDCRequest request, 
                                        CancellationToken cancellation);
    }
}
