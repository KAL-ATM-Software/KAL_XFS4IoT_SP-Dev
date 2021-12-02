/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// GetCommandRandomNumberResult
    /// Return authorisation token for a command
    /// </summary>
    public sealed class GetCommandNonceResult : DeviceResult
    {
        public GetCommandNonceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                            string ErrorDescription = null,
                                            string Nonce = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.Nonce = Nonce;
        }

        public GetCommandNonceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                     string Nonce = null)
            : base(CompletionCode, null)
        {
            this.Nonce = Nonce;
        }

        /// <summary>
        /// A nonce that should be included in the authorisation token in a command used to provide 
        /// end to end protection.
        /// 
        /// The nonce will be given as HEX (upper case.)
        /// </summary>
        public string Nonce { get; private set; }
    }

    /// <summary>
    /// GetCommandRandomNumberResult
    /// Return transaction state
    /// </summary>
    public sealed class GetTransactionStateResult : DeviceResult
    {
        public GetTransactionStateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                         string ErrorDescription)
            : base(CompletionCode, ErrorDescription)
        {
            this.State = StateEnum.None;
            this.TransactionID = string.Empty;
            this.Extra = null;
        }
        public GetTransactionStateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                         StateEnum State,
                                         string TransactionID,
                                         List<string> Extra)
            : base(CompletionCode, string.Empty)
        {
            this.State = State;
            this.TransactionID = TransactionID;
            this.Extra = Extra;
        }

        public enum StateEnum
        {
            None,
            Active,
            Inactive,
        }

        /// <summary>
        /// Specifies the transaction state. Following values are possible:
        /// 
        /// "active": A customer transaction is in progress.
        /// 
        /// "inactive": No customer transaction is in progress.
        /// </summary>
        public StateEnum State { get; init; }

        /// <summary>
        /// Specifies a string which identifies the transaction ID. The value returned in this 
        /// parameter is an application defined customer transaction identifier, which was previously set in the Common.SetTransactionState command
        /// </summary>
        public string TransactionID { get; init; }

        /// <summary>
        /// A list of vendor-specific, or any other extended, transaction information. The information is set as a series 
        /// of "key=value" strings. Each string is null-terminated, with the final string terminating with two null characters. 
        /// An empty list may be indicated by either a NULL pointer or a pointer to two consecutive null characters
        /// </summary>
        public List<string> Extra { get; init; }
    }

    /// <summary>
    /// SetTransactionStateRequest
    /// Set transaction information to the device
    /// </summary>
    public sealed class SetTransactionStateRequest
    {
        public SetTransactionStateRequest(StateEnum State,
                                         string TransactionID,
                                         List<string> Extra)
        {
            this.State = State;
            this.TransactionID = TransactionID;
            this.Extra = Extra;
        }

        public enum StateEnum
        {
            Active,
            Inactive,
        }

        /// <summary>
        /// Specifies the transaction state. Following values are possible:
        /// 
        /// "active": A customer transaction is in progress.
        /// 
        /// "inactive": No customer transaction is in progress.
        /// </summary>
        public StateEnum State { get; init; }

        /// <summary>
        /// Specifies a string which identifies the transaction ID. The value returned in this 
        /// parameter is an application defined customer transaction identifier, which was previously set in the Common.SetTransactionState command
        /// </summary>
        public string TransactionID { get; init; }

        /// <summary>
        /// A list of vendor-specific, or any other extended, transaction information. The information is set as a series 
        /// of "key=value" strings. Each string is null-terminated, with the final string terminating with two null characters. 
        /// An empty list may be indicated by either a NULL pointer or a pointer to two consecutive null characters
        /// </summary>
        public List<string> Extra { get; init; }
    }

    /// <summary>
    /// SynchronizeCommandRequest
    /// Request to synchronize command with other services
    /// </summary>
    public sealed class SynchronizeCommandRequest
    {

        public SynchronizeCommandRequest(CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum? AuxiliariesCommand = null,
                                         CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum? CardReaderCommand = null,
                                         CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum? CashDispenserCommand = null,
                                         CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum? CashManagementCommand = null,
                                         CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum? CommonCommand = null,
                                         CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum? CryptoCommand = null,
                                         CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum? KeyboardCommand = null,
                                         CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum? KeyManagementCommand = null,
                                         CommonCapabilitiesClass.LightsInterfaceClass.CommandEnum? LightsCommand = null,
                                         CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum? PinPadCommand = null,
                                         CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum? PrinterCommand = null,
                                         CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum? StorageCommand = null,
                                         CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum? TextTerminalCommand = null,
                                         object CmdData = null)
        {
            this.AuxiliariesCommand = AuxiliariesCommand;
            this.CardReaderCommand = CardReaderCommand;
            this.CashDispenserCommand = CashDispenserCommand;
            this.CashManagementCommand = CashManagementCommand;
            this.CommonCommand = CommonCommand;
            this.CryptoCommand = CryptoCommand;
            this.KeyboardCommand = KeyboardCommand;
            this.KeyManagementCommand = KeyManagementCommand;
            this.LightsCommand = LightsCommand;
            this.PinPadCommand = PinPadCommand;
            this.PrinterCommand = PrinterCommand;
            this.StorageCommand = StorageCommand;
            this.TextTerminalCommand = TextTerminalCommand;
            this.CmdData = CmdData;
        }

        /// <summary>
        /// The command name to be synchronized and executed next. 
        /// </summary>
        public CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum? AuxiliariesCommand { get; init; }
        public CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum? CardReaderCommand { get; init; }
        public CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum? CashDispenserCommand { get; init; }
        public CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum? CashManagementCommand { get; init; }
        public CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum? CommonCommand { get; init; }
        public CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum? CryptoCommand { get; init; }
        public CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum? KeyboardCommand { get; init; }
        public CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum? KeyManagementCommand { get; init; }
        public CommonCapabilitiesClass.LightsInterfaceClass.CommandEnum? LightsCommand { get; init; }
        public CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum? PinPadCommand { get; init; }
        public CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum? PrinterCommand { get; init; }
        public CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum? StorageCommand { get; init; }
        public CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum? TextTerminalCommand { get; init; }
        /// <summary>
        /// A payload that represents the parameter that is normally associated with the command.
        /// </summary>
        public object CmdData { get; init; }
    }
}