using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoT
{
    /// <summary>
    /// Represents the result of a command execution.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    public class CommandResult<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{T}"/> class with the specified payload, completion code, and error description.
        /// </summary>
        /// <param name="Payload">The payload of the command result.</param>
        /// <param name="CompletionCode">The completion code of the command result.</param>
        /// <param name="ErrorDescription">The error description of the command result.</param>
        public CommandResult(T Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription = null)
        {
            this.Payload = Payload;
            this.CompletionCode = CompletionCode;
            this.ErrorDescription = ErrorDescription;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{T}"/> class with the specified completion code and error description.
        /// </summary>
        /// <param name="CompletionCode">The completion code of the command result.</param>
        /// <param name="ErrorDescription">The error description of the command result.</param>
        public CommandResult(MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription = null)
        {
            this.Payload = null;
            this.CompletionCode = CompletionCode;
            this.ErrorDescription = ErrorDescription;
        }

        /// <summary>
        /// The payload of the command result.
        /// </summary>
        public T Payload { get; init; }

        /// <summary>
        /// The completion code of the command result.
        /// </summary>
        public MessageHeader.CompletionCodeEnum CompletionCode { get; init; }

        /// <summary>
        /// The error description of the command result.
        /// </summary>
        public string ErrorDescription { get; init; }
    }
}
