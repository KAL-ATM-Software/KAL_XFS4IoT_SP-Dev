/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

    public class InternalErrorException : Exception
    {
        /// <summary>
        /// Exception class on detecting an internal error on handling command in device class or framework.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public InternalErrorException(string message)
            : base(message)
        { }
    }

    public class UnsupportedDataException : Exception
    {
        /// <summary>
        /// Exception class on detecting unsupported data on execute command if the device doesn't support specified capability.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public UnsupportedDataException(string message)
            : base(message)
        { }
    }

    public class SequenceErrorException : Exception
    {
        /// <summary>
        /// Exception class on detecting a sequential command error.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public SequenceErrorException(string message)
            : base(message)
        { }
    }

    public class AuthorisationRequiredException : Exception
    {
        /// <summary>
        /// Exception class on detecting an operation requires authorisation data to perform sensitive operations.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public AuthorisationRequiredException(string message)
            : base(message)
        { }
    }

    public class HardwareErrorException : Exception
    {
        /// <summary>
        /// Exception class on facing hardware error while executing an operation.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public HardwareErrorException(string message)
            : base(message)
        { }
    }

    public class UserErrorException : Exception
    {
        /// <summary>
        /// Exception class on detecting vandalism
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public UserErrorException(string message)
            : base(message)
        { }
    }

    public class FraudAttemptException : Exception
    {
        /// <summary>
        /// Exception class on detecting an fraund attempt and not possible to process other execute commands until it's being cleared.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public FraudAttemptException(string message)
            : base(message)
        { }
    }

    public class DeviceNotReadyException : Exception
    {
        /// <summary>
        /// Exception class on detecting on handling command in device class when device is not ready to respond.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public DeviceNotReadyException(string message)
            : base(message)
        { }
    }

    public class InvalidCommandException : Exception
    {
        /// <summary>
        /// Exception class on detecting an invalid command on handling command in device class or framework.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public InvalidCommandException(string message)
            : base(message)
        { }
    }

    public class NotEnoughSpaceException : Exception
    {
        /// <summary>
        /// Exception class on there is not enough storage on the device for the requested action.
        /// </summary>
        /// <param name="message">This string parameter will be set to the error description common payload on response.
        /// it would be good to set meaningful description for an application developer.
        /// </param>
        public NotEnoughSpaceException(string message)
            : base(message)
        { }
    }

}
