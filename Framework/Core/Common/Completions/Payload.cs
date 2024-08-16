/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Runtime.Serialization;

namespace XFS4IoT.Completions
{
    /// <summary>
    /// A message object representing payload on responses
    /// </summary>
    [DataContract]
    public class MessagePayload : MessagePayloadBase
    {
        /// <summary>
        /// Constructor of the common payload for response
        /// </summary>
        public MessagePayload()
        { }

        /// <summary>
        /// success if the command was successful otherwise error
        /// </summary>
        [Obsolete("This property is obsolete, use MessageHeader.CompletionCodeEnum. " +
            "This enumlation will not be supported from package version 3.0. " +
            "Please migrate changes in the device class before applying 3.0 package.", false)]
        public enum CompletionCodeEnum
        {
            Success,
            CommandErrorCode,
            Canceled,
            DeviceNotReady,
            HardwareError,
            InternalError,
            InvalidCommand,
            InvalidRequestID,
            TimeOut,
            UnsupportedCommand,
            InvalidData,
            UserError,
            UnsupportedData,
            FraudAttempt,
            SequenceError,
            AuthorisationRequired,
            NoCommandNonce,
            InvalidToken,
            InvalidTokenNonce,
            InvalidTokenHMAC,
            InvalidTokenFormat,
            InvalidTokenKeyNoValue,
            NotEnoughSpace
        }
    }
}
