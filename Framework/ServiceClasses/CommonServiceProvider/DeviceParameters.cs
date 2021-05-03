/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetCommandRandomNumberHandler.cs uses automatically generated parts. 
 * created at 4/20/2021 12:28:05 PM
\***********************************************************************************************/


using System;
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
    public sealed class GetCommandRandomNumberResult : DeviceResult
    {
        public GetCommandRandomNumberResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                            string ErrorDescription = null,
                                            string CommandRandomNumber = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.CommandRandomNumber = CommandRandomNumber;
        }

        public GetCommandRandomNumberResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                            string CommandRandomNumber = null)
            : base(CompletionCode, null)
        {
            this.CommandRandomNumber = CommandRandomNumber;
        }

        /// <summary>
        /// A nonce that should be included in the authorisation token in a command used to provide 
        /// end to end protection.
        /// 
        /// The nonce will be given as HEX (upper case.)
        /// </summary>
        public string CommandRandomNumber { get; private set; }
    }
}