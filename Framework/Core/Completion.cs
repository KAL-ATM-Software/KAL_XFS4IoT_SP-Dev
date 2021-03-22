/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT.Completions
{
    [DataContract]
    public abstract class Completion<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Initialise any response object
        /// </summary>
        /// <param name="ID">Unique ID each message class for serialize/deserialize object</param>
        /// <param name="Name">Name of the response of the command required</param>
        /// <param name="RequestId">request id</param>
        /// <param name="Payload">payload contents</param>
        public Completion(string RequestId, T Payload) :
            base(RequestId, MessageHeader.TypeEnum.Completion, Payload)
        { }
    }
}
