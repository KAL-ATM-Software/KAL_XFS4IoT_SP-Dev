/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT
{
    /// <summary>
    /// A message object representing header
    /// </summary>
    [DataContract]
    public sealed class MessageHeader
    {
        /// <summary>
        /// The original message name, for example "CardReader.Status"
        /// </summary>
        [DataMember(IsRequired = true, Name = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Unique request identifier supplied by the client used to correlate the command with responses, events and
        /// completions.For Unsolicited Events the field will be empty.
        /// </summary>
        [DataMember(IsRequired = true, Name = "requestId")]
        public int? RequestId { get; private set; } = default;

        /// <summary>
        /// Possible type of message 
        /// </summary>
        [DataContract]
        public enum TypeEnum
        {
            Command,
            Acknowledge,
            Event,
            Completion,
            Unsolicited,
        }

        /// <summary>
        /// The original message name, for example "CardReader.Status"
        /// </summary>
        [DataMember(IsRequired = true, Name = "type")]
        public TypeEnum Type { get; private set; }

        /// <summary>
        /// The version of the message. 1.0, 2.0, etc
        /// </summary>
        [DataMember(IsRequired = true, Name = "version")]
        public string Version { get; private set; }

        /// <summary>
        /// Timeout in milliseconds for the command to complete. If set to 0, 
        /// the command will not timeout but can be canceled.
        /// </summary>
        [DataMember(IsRequired = false, Name = "timeout")]
        public int? Timeout { get; private set; } = default;

        public enum StatusEnum
        {
            InvalidMessage,
            InvalidRequestID,
            TooManyRequests
        }

        /// <summary>
        /// If null the command has been accepted for execution. The command will complete with a completion
        /// message.Otherwise there is an error that stops the command from being queued and there will be no
        /// further messages.
        /// 
        /// * ```invalidMessage``` - The JSON in the message is invalid and can't be parsed.
        /// * ```invalidRequestID``` - The request ID on the command is invalid.This could be because the value
        /// was not an integer, had a zero value, or because a command with the same request ID was already
        /// queued or is executing.
        /// * ```tooManyRequests``` - The service has currently received and queued more requests than it can
        /// process.
        /// </summary>
        public StatusEnum? Status { get; private set; } = default;

        /// <summary>
        /// success if the command was successful otherwise error
        /// </summary>
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

        /// <summary>
        /// The completion code of the message.
        /// </summary>
        [DataMember(IsRequired = true, Name = "completionCode")]
        public CompletionCodeEnum? CompletionCode { get; private set; } = default;

        /// <summary>
        ///  If not success, then this is optional vendor dependent information to provide additional information
        /// </summary>
        [DataMember(Name = "errorDescription")]
        public string ErrorDescription { get; private set; } = default;

        /// <summary>
        /// MessageHeader class representing XFS4 message header
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="RequestId"></param>
        /// <param name="Version"></param>
        /// <param name="Type"></param>
        /// <param name="Timeout">This property is only applicable to command messages.</param>
        /// <param name="Status">This property is only applicable to acknowledge messages.</param>
        /// <param name="CompletionCode">This property is only applicable to completion messages.</param>
        /// <param name="ErrorDescription">This property is only applicable to completion messages.</param>
        public MessageHeader(string Name, int? RequestId, string Version, TypeEnum Type, int? Timeout = null, StatusEnum? Status = null, CompletionCodeEnum? CompletionCode = null, string ErrorDescription = null)
        {
            Contracts.IsNotNullOrWhitespace(Name, $"Null or an empty value for {nameof(Name)} in the header.");
            // RequestId may be null for unsolicited events

            this.Name = Name;
            this.RequestId = RequestId;
            this.Type = Type;
            this.Version = Version;
            this.Timeout = Timeout;
            this.Status = Status;
            this.CompletionCode = CompletionCode;
            this.ErrorDescription = ErrorDescription;
        }
    }
}
