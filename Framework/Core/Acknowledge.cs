/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoT
{
    [DataContract]
    [Acknowledge(Name = "Common.Acknowledge")]
    public class Acknowledge : Message<AcknowledgePayload>
    {
        /// <summary>
        /// Initialise any response object
        /// </summary>
        /// <param name="RequestId">request id</param>
        /// <param name="Payload">payload contents</param>
        public Acknowledge(int RequestId, AcknowledgePayload Payload) :
            base(RequestId, MessageHeader.TypeEnum.Acknowledgement, Payload)
        { }

    }

    [DataContract]
    public class AcknowledgePayload : MessagePayloadBase
    {
        public AcknowledgePayload(StatusEnum? Status = null, string ErrorDescription = null)
        {
            this.Status = Status;
            this.ErrorDescription = ErrorDescription;
        }

        public enum StatusEnum
        {
            Ok,
            InvalidMessage,
            InvalidRequestID,
            TooManyRequests
        }

        /// <summary>
        /// "ok" if the command was successful and has been queued. The command 
        /// will complete with a completion message.Otherwise there is an error that
        /// stops the command from being queued and there will be no further messages. 
        /// 
        /// * ````ok```` - The command has been accepted for execution
        /// * ````invalidMessage```` - The JSON in the message is invalid and can't be parsed. 
        /// * ````invalidRequestID```` - The request ID on the command is invalid.This could be because the value was
        /// not an integer, had a zero value, or because a command with the same request ID was already queued or
        ///      is executing.
        /// * ````tooManyRequests```` - The service has currently received and queued more requests than it can
        /// process.
        /// </summary>
        public StatusEnum? Status { get; init; }

        /// <summary>
        /// If the status is not ok this will give a human readable description of what caused the error. This may include
        /// details which help diagnose the cause.The format of this string should not be relied on. 
        /// </summary>
        public string ErrorDescription { get; init; }

    }
}
