/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
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
}