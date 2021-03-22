/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;

namespace XFS4IoT.UnitTestCore
{
    public class TestFatalErrorException : Exception
    {
        public TestFatalErrorException(string message) : base(message) { }
    };
}
