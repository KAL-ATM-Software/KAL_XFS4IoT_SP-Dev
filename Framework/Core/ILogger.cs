/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

namespace XFS4IoT
{
    public interface ILogger
    {
        /// <summary>
        /// Add a message to the trace. 
        /// </summary>
        /// <remarks> 
        /// Note that there are limitations in the fields of many implementations of this 
        /// interface. For example, the Kalignite Trace only supports eight characters for 
        /// SubSystem and Operation fields. Longer strings will be truncated. 
        /// The Message field allows much longer strings. 
        /// </remarks>
        /// <param name="SubSystem">Sub-system field</param>
        /// <param name="Operation">Operation field</param>
        /// <param name="Message">Unlimited message flield</param>
        void Trace(string SubSystem, string Operation, string Message);
        /// <summary>
        /// The passed in string Message parameter may contains sensitive data to not be traced with triangle brackets. process string in the triangle brackets to not be traced.
        /// </summary>
        /// <param name="SubSystem"></param>
        /// <param name="Operation"></param>
        /// <param name="Message"></param>
        void TraceSensitive(string SubSystem, string Operation, string Message);

        /// <summary>
        /// Logging warning operation to the trace.
        /// </summary>
        /// <param name="SubSystem"></param>
        /// <param name="Message"></param>
        void Warning(string SubSystem, string Message);

        /// <summary>
        /// The passed in string Message parameter may contains sensitive data to not be traced with triangle brackets. process string in the triangle brackets to not be traced.
        /// </summary>
        /// <param name="SubSystem"></param>
        /// <param name="Message"></param>
        void WarningSensitive(string SubSystem, string Message);

        /// <summary>
        /// Logging information to the trace.
        /// </summary>
        /// <param name="SubSystem"></param>
        /// <param name="Message"></param>
        void Log(string SubSystem, string Message);

        /// <summary>
        /// The passed in string Message parameter may contains sensitive data to not be traced with triangle brackets. process string in the triangle brackets to not be traced.
        /// </summary>
        /// <param name="SubSystem"></param>
        /// <param name="Message"></param>
        void LogSensitive(string SubSystem, string Message);
    }
}