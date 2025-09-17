/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    /// <summary>
    /// Exception class on detecting an invalid data being received or sent, currently being used on receiving command has unexpected data
    /// and then automatically send a response with the invalid data error code to the command.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    [Serializable]
    
    public class InvalidDataException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting an internal error on handling command in device class or framework.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class InternalErrorException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting unsupported data on execute command if the device doesn't support specified capability.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class UnsupportedDataException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting a sequential command error.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class SequenceErrorException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting an operation requires authorisation data to perform sensitive operations.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class AuthorisationRequiredException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on facing hardware error while executing an operation.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class HardwareErrorException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting vandalism
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class UserErrorException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting an fraund attempt and not possible to process other execute commands until it's being cleared.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class FraudAttemptException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting on handling command in device class when device is not ready to respond.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class DeviceNotReadyException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on detecting an invalid command on handling command in device class or framework.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class InvalidCommandException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on there is not enough storage on the device for the requested action.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class NotEnoughSpaceException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Exception class on there is not enough storage on the device for the requested action.
    /// </summary>
    /// <param name="message">This string parameter will be set to the error description common payload on response.
    /// it would be good to set meaningful description for an application developer.
    /// </param>
    public class UnsupportedCommandException(string message) : Exception(message)
    {
    }
}
