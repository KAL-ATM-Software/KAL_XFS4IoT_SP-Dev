/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

// KAL specific implementation of common. 
namespace XFS4IoTFramework.Common
{
    public interface ICommonDevice : IDevice
    {

        /// <summary>
        /// This command is used to obtain the overall status of any XFS4IoT service. The status includes common status information and can include zero or more interface specific status objects, depending on the implemented interfaces of the service. It may also return vendor-specific status information.
        /// </summary>
        StatusCompletion.PayloadData Status();

        /// <summary>
        /// This command retrieves the capabilities of the device. It may also return vendor specific capability information.
        /// </summary>
        CapabilitiesCompletion.PayloadData Capabilities();


        /// <summary>
        /// This command activates or deactivates the power-saving mode.If the Service Provider receives another execute command while in power saving mode, the Service Provider automatically exits the power saving mode, and executes the requested command. If the Service Provider receives an information command while in power saving mode, the Service Provider will not exit the power saving mode.
        /// </summary>
        Task<PowerSaveControlCompletion.PayloadData> PowerSaveControl(PowerSaveControlCommand.PayloadData payload);

        /// <summary>
        /// {}
        /// </summary>
        Task<SynchronizeCommandCompletion.PayloadData> SynchronizeCommand(SynchronizeCommandCommand.PayloadData payload);

        /// <summary>
        /// This command allows the application to specify the transaction state, which the Service Provider can then utilize in order to optimize performance. After receiving this command, this Service Provider can perform the necessary processing to start or end the customer transaction. This command should be called for every Service Provider that could be used in a customer transaction. The transaction state applies to every session.
        /// </summary>
        Task<SetTransactionStateCompletion.PayloadData> SetTransactionState(SetTransactionStateCommand.PayloadData payload);

        /// <summary>
        /// This command can be used to get the transaction state.
        /// </summary>
        GetTransactionStateCompletion.PayloadData GetTransactionState();

        /// <summary>
        /// Get a nonce to be included in an Authorisation Token for a command that will be used to ensure end to end security.The hardware will overwrite any existing stored Command nonce with this new value. The value will be stored for future authentication. Any Authorisation Token received will be compared with this stored nonce and if the Token doesn't contain the same nonce it will be considered invalid and rejected, causing the command that contains that Authentication Token to fail.The nonce must match the algorithm used. For example, HMAC means the nonce must be 128 bit/16 bytes.
        /// </summary>
        Task<GetCommandRandomNumberResult> GetCommandRandomNumber();

    }
}
