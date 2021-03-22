/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using XFS4IoT.UnitTestCore; 

namespace XFS4IoT
{
    [TestClass]
    public class Global
    {
        [AssemblyInitialize]
        public static void Initialise( TestContext _ )
        {
            UnitTesting.SetupUnitTests();
        }

        public static ILogger TheLogger { get; } = new TestLogger();
    }
}
