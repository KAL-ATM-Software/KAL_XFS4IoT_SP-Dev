/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Diagnostics;

namespace XFS4IoT
{
    /// <summary>
    /// Fatal error handling. 
    /// </summary>
    public static class ErrorHandling
    {
        /// <summary>
        /// Default fatal error handling if not replaced. 
        /// </summary>
        /// <param name="message"></param>
        private static void DefaultError(string message)
        {
            message.Ignore(/*can't fail asserts inside error handling*/);
            Debug.WriteLine(message);
            if (Debugger.IsAttached) Debugger.Break();
            Environment.Exit(256);
        }

        /// <summary>
        /// Trigger a fatal error
        /// </summary>
        /// <remarks> 
        /// This will cause a fatal error, exit the process or similar. 
        /// It's safe to assume that this method will never return. 
        /// </remarks>
        public static void Error( string Message ) => ErrorHandler(Message);

        /// <summary>
        /// Implementation of a FatalError handling method. 
        /// </summary>
        /// <remarks>
        /// Implementations of this method must handle an FE. 
        /// As a minimum, they must never return.
        /// </remarks>
        public delegate void ErrorDelegate(string message);

        /// <summary>
        /// Invoke the registered error handler from an expression. 
        /// </summary>
        /// <remarks> 
        /// This version can be used in an throw statement inline with an expression. For example
        /// <code><![CDATA[
        /// var x = y!=null ? "Some value" : throw InvokeErrorHandler<string>( "Something failed" ); 
        /// ]]></code>
        /// though this is normally wrapped in a contract assertion rather than being called directly. 
        /// </remarks>
        /// <seealso cref="Contracts.Fail{T}(string)"/>
        /// <param name="Message">Error message to report</param>
        /// <returns>never returns</returns>
        public static Exception InvokeErrorHandler(string Message)
        {
            Message.Ignore(/*can't fail asserts inside error handling*/);
            ErrorHandler(Message);
            return default(Exception).Ignore();
        }

        /// <summary>
        /// The registered fatal error handler. 
        /// </summary>
        /// <remarks>
        /// This should be set during program setup with the required fatal error handling 
        /// mechanism. 
        /// This could be a real fatal error handler, or it could be something like 
        /// special version for unit testing. 
        /// The fatal error handler can only be set once. 
        /// </remarks>
        public static ErrorDelegate ErrorHandler
        {
            get => _ErrorHandler;
            set
            {
                // Only allow setting to a new value once. 
                // However, allow setting to the same value multiple times - this is useful for 
                // unit tests where TestInit is called before each test. 
                Contracts.Assert(_ErrorHandler == DefaultError || _ErrorHandler == value );
                _ErrorHandler = value; 
            }
        }

        private static ErrorDelegate _ErrorHandler = DefaultError;

    }
}
