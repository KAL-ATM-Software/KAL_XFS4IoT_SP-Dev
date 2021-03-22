/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

namespace XFS4IoT.UnitTestCore
{
    public static class UnitTesting
    {
        /// <summary>
        /// Setup generic environment for unit testing with XFS code. 
        /// </summary>
        public static void SetupUnitTests()
        {
            ErrorHandling.ErrorHandler = (message) =>
            {
                TestTrace.Trace("Testing", "Error", message);
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw new TestFatalErrorException(message);
            };
        }

        private static readonly TestLogger TestTrace = new TestLogger();
    }
}
