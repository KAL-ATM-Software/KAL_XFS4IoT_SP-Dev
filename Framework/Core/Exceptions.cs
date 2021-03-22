/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    [Serializable]
    
    public class InvalidDataException : Exception
    {
        /// <summary>
        /// Exception class on detecting an invalid data being received or sent, currently being used on receiving command has unexpected data
        /// and then automatically send a response with the invalid data error code to the command.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public InvalidDataException(string message)
            : base(message)
        { }
    }
}
