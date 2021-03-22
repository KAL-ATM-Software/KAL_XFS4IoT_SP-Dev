/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using XFS4IoT.UnitTestCore;

namespace XFS4IoT
{
    // Don't open Assert - too easy to overlap with the error handling methods, 
    // that we're testing. 
    //using static Assert;

    [TestClass]
    public partial class ErrorHandlingTests
    {
        [TestMethod]
        public void ErrorHandleDelegateCantSet()
        {
            // note the delegate is already set on Assembly Initialise
            // Can't change it. 
            Assert.ThrowsException<TestFatalErrorException>(() =>
            {
                ErrorHandling.ErrorHandler = (message) => throw new TestFatalErrorException(message);
            });
        }

        [TestMethod]
        public void TestError()
        {
            Assert.ThrowsException<TestFatalErrorException>(() => ErrorHandling.Error("Error Message"));
        }

        [TestMethod]
        public void TestAssert()
        {
            Contracts.Assert(true);
            // Assert true should do nothing, so nothing to test. 
        }

        [TestMethod]
        public void TestAssertFalse()
        {
            // Skip file names and paths in stack trace so that we don't have to change this each time the 
            // test is recompiled on a different machine. 
            Contracts.IncludeFileInfoInStacktrace = false;

            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.Assert(false));
        }

        [TestMethod]
        public void TestStackTrace()
        {
            // Skip file names and paths in stack trace so that we don't have to change this each time the 
            // test is recompiled on a different machine. 
            Contracts.IncludeFileInfoInStacktrace = false;

            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.Fail("Fail stack test"));

            var expectedHead = ExpectedStackTrace.Split('\n').Take(5).Aggregate( (a,b) => a + '\n' + b );
            var actualHead = TestLogger.LastTrace.Split('\n').Take(5).Aggregate( (a,b) => a + '\n' + b );

            Assert.AreEqual(expectedHead, actualHead);
        }

        private const string ExpectedStackTrace =
@"Fail stack test" + "\n" +
@"Failed assertion   at XFS4IoT.ErrorHandlingTests.<>c.<TestStackTrace>b__4_0()
   at Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException[T](Action action, String message, Object[] parameters)
   at Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException[T](Action action)
   at XFS4IoT.ErrorHandlingTests.TestStackTrace()
   at System.RuntimeMethodHandle.InvokeMethod(Object target, Object[] arguments, Signature sig, Boolean constructor)
   at System.Reflection.RuntimeMethodInfo.UnsafeInvokeInternal(Object obj, Object[] parameters, Object[] arguments)
   at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
   at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)
";

        [TestMethod]
        public void TestFail()
        {
            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.Fail());
            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.Fail("Fail with message"));
            Assert.ThrowsException<TestFatalErrorException>(() => false ? "" : Contracts.Fail<object>("Expression fail"));
            Assert.ThrowsException<TestFatalErrorException>(() => false ? "" : Contracts.Fail<object>("Expression fail with message"));
        }
        [TestMethod]
        public void TestAssertTrueAndFalse()
        {
            Contracts.Assert(true);
            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.Assert(false));
        }

        [TestMethod]
        public void TestIs()
        {
            Assert.IsTrue(true.Is(true));
            Assert.IsFalse( false.Is( true ) );
            Assert.ThrowsException<TestFatalErrorException>(() => true.Is(false));
            Assert.ThrowsException<TestFatalErrorException>(() => false.Is(false));
            Assert.IsTrue(true.Is(true));
            Assert.IsFalse( false.Is( true ) );
            Assert.ThrowsException<TestFatalErrorException>(() => true.Is(false));
            Assert.ThrowsException<TestFatalErrorException>(() => false.Is(false));
        }

        [TestMethod]
        public void TestIsTrue()
        {
            // Remember that pass-through asserts return the value,
            // and nothing to do with the assertion. 
            Assert.IsTrue(true.IsTrue());
            Assert.IsTrue(true.IsTrue(), "Message" );
            Assert.ThrowsException<TestFatalErrorException>(() => false.IsTrue() );
            Assert.ThrowsException<TestFatalErrorException>(() => false.IsTrue("Failed") );
        }

        [TestMethod]
        public void TestIsNullExtension()
        {
            object v = null;
            Assert.IsTrue(v.IsNull() == null);

            Assert.ThrowsException<TestFatalErrorException>(() => "Not null value".IsNull() != null);
        }

        [TestMethod]
        public void TestIsNull()
        {
            Contracts.IsNull(null);

            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.IsNull("Not null value") );
        }

        [TestMethod]
        public void TestIsNullOrWhatspaceString()
        {
            Contracts.IsNullOrWhitespace("");
            Contracts.IsNullOrWhitespace(null);

            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.IsNullOrWhitespace("Not Empty"));
        }

        [TestMethod]
        public void TestIsNotNullOrWhitespaceString()
        {
            Contracts.IsNotNullOrWhitespace("Not empty");

            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.IsNotNullOrWhitespace(null));
            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.IsNotNullOrWhitespace(""));
            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.IsNotNullOrWhitespace("    \t"));
        }

        [TestMethod]
        public void TestIsNullOrWhatspaceStringExtension()
        {
            "".IsNullOrWhitespace();
            "    \t".IsNullOrWhitespace();
            string nullString = null;
            nullString.IsNullOrWhitespace(null);

            Assert.ThrowsException<TestFatalErrorException>(() => "NotEmpty".IsNullOrWhitespace());
        }

        [TestMethod]
        public void TestIsNotNullOrWhitespaceStringExtension()
        {
            "Not empty".IsNotNullOrWhitespace();

            string nullString = null;
            Assert.ThrowsException<TestFatalErrorException>(() => nullString.IsNotNullOrWhitespace());
            Assert.ThrowsException<TestFatalErrorException>(() => "".IsNotNullOrWhitespace());
            Assert.ThrowsException<TestFatalErrorException>(() => "    \t".IsNotNullOrWhitespace());
        }

        [TestMethod]
        public void TestIsNotNullExtension()
        {
            object v = "Object value";
            Assert.IsTrue(v.IsNotNull() == v);

            object n = null;
            Assert.ThrowsException<TestFatalErrorException>(() => n.IsNotNull() == null);
        }

        [TestMethod]
        public void TestIsNotNull()
        {
            object v = "Object value";
            Contracts.IsNotNull(v);

            object n = null;
            Assert.ThrowsException<TestFatalErrorException>(() => Contracts.IsNotNull(n) );
        }

        [TestMethod]
        public void TestIsFalse()
        {
            Assert.IsFalse(false.IsFalse());
            Assert.IsFalse(false.IsFalse(), "Message");
            Assert.ThrowsException<TestFatalErrorException>(() => true.IsFalse());
            Assert.ThrowsException<TestFatalErrorException>(() => true.IsFalse(), "Message");
        }

        [TestMethod]
        public void TestIsIgnore()
        {
            string value = "SomeValue";
            var newValue = value.Ignore();
            Assert.AreEqual(value, newValue);
        }

        [TestMethod]
        public void TestIsIgnoreNull()
        {
            string value = null;
            var newValue = value.Ignore();
            Assert.AreEqual(value, newValue);
        }
    }
}