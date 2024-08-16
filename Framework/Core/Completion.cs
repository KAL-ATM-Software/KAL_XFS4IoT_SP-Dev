/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;
using static XFS4IoT.MessageHeader;

namespace XFS4IoT.Completions
{
    [DataContract]
    public abstract class Completion<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Initialise any response object
        /// </summary>
        /// <param name="RequestId">request id</param>
        /// <param name="Payload">payload contents</param>
        /// <param name="CompletionCode">completion code</param>
        /// <param name="ErrorDescription">error description</param>
        public Completion(int RequestId, T Payload, CompletionCodeEnum CompletionCode, string ErrorDescription) :
            base(RequestId, MessageHeader.TypeEnum.Completion, Payload, CompletionCode, ErrorDescription)
        { }
    }
}
