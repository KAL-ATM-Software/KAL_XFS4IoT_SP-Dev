/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Diagnostics;
using XFS4IoT;

namespace XFS4IoT.UnitTestCore
{
    /// <summary>
    /// Simple test trace. Also sets Tracing.WriteTraceRecord.
    /// </summary>
    public class TestLogger : ILogger
    {

        /// <summary>
        /// Setup test tracing when TestTrace is used. 
        /// </summary>
        private static void WriteTraceRecord(string SubSystem, string Operation, string Message)
        {
            LastSubSystem = SubSystem;
            LastOperation = Operation; 
            LastTrace = Message;
            Debug.WriteLine($"{SubSystem}::{Operation} - {Message}");
        }
        public void WriteLine(string Message) => WriteTraceRecord(Message, null, null);

        public void Trace(string SubSystem, string Operation, string Message) => WriteTraceRecord(SubSystem, Operation, Message);

        public void Warning(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

        public void Log(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

        public void TraceSensitive(string SubSystem, string Operation, string Message) => Trace(SubSystem, Operation, Message);

        public void WarningSensitive(string SubSystem, string Message) => Warning(SubSystem, Message);

        public void LogSensitive(string SubSystem, string Message) => Log(SubSystem, Message);


        public static string LastSubSystem { get; private set; }
        public static string LastOperation { get; private set; }
        public static string LastTrace { get; private set; }
    }
}