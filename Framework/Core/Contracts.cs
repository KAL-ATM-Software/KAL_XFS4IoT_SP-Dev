/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    /// <summary>
    /// Support for development by contract 
    /// </summary>
    /// <remarks>
    /// This class is intentionally static so that it can be opened. 
    /// <code><![CDATA[
    ///     using static Contracts;
    ///     
    ///     Assert( something == true ); 
    ///     Assert( otherthing != null, "Forgot the thing" );
    ///     myVal.IsNotNull();
    /// ]]></code>
    /// 
    /// This also includes generic extension methods.
    /// </remarks>
    public static class Contracts
    {
        /// <summary>
        /// If set to false file information will be excluded from stack frames. Defaults to true. 
        /// </summary>
        /// <remarks>
        /// Defaults to true. Normally we want to include file information (including file paths) 
        /// in the stack trace to make debugging easier, but that makes unit testing more difficult. 
        /// So for unit tests, where the stack needs to be predicable, set this to false. 
        /// </remarks>
        public static bool IncludeFileInfoInStacktrace { private get; set; } = true;

        /// <summary>
        /// Assert that the given expression is true. 
        /// </summary>
        /// <param name="Predicate">Value that must be true</param>
        /// <param name="Message">Message to report in error reporting</param>
        [Contract]
        public static void Assert(bool Predicate, string Message = null)
        {
            if (!Predicate)
            {
                ErrorHandling.ErrorHandler(GetFailedAssertionMessage(Message));
                // This is mainly here so that code analysers can know that this method will never 
                // 'return' in this case. 
                throw new Exception("Invalid error handling in XFS4IoT Core - Error handler must never return");
            }
        }
        /// <summary>
        /// Fail immediately
        /// </summary>
        /// <remarks>
        /// This is useful in combination with more complex error checking. For example, the default
        /// case in a switch statement may be a Fail()
        /// </remarks>
        /// <param name="Message">Trace message</param>
        [Contract]
        public static void Fail(string Message = null) => ErrorHandling.ErrorHandler(GetFailedAssertionMessage(Message));


        /// <summary>
        /// Fail immediately
        /// </summary>
        /// <remarks>
        /// This version gives Fail() a type, which is useful when using inline. For example, 
        /// <code><![CDATA[
        /// var x = y!=null ? "Some value" : Fail<string>( "Something failed" ); 
        /// ]]></code>
        /// 
        /// It can also be used to indicate to the compiler that Fail() will not return, but 'throwing' 
        /// an exception. The exception will never actually be thrown because Fail() will never return, 
        /// but the compiler will assume that anything after Fail() can't be reached - which is correct. 
        /// <code><![CDATA[
        /// int fn() 
        /// {
        ///     throw Fail<Exception>( "Failed will never return" ); 
        ///     // unreachable code. 
        /// }
        /// ]]></code>
        /// Just calling Fail() will cause the compiler to complain that fn() doesn't return a value, but
        /// throwing a Fail lets the compiler know that the following code can't be called. 
        /// </remarks>
        [Contract]
        public static T Fail<T>(string Message = null) => throw ErrorHandling.InvokeErrorHandler(GetFailedAssertionMessage(Message));

        /// <summary>
        /// Format the default failed assertion message.
        /// </summary>
        /// <param name="Message">Message text. If not given, will use a default message</param>
        /// <returns>Formated message for failed assertions</returns>
        private static string GetFailedAssertionMessage(string Message) => $"{(Message != null ? Message + "\n" : "")}Failed assertion{StackTrace}";

        private static System.Diagnostics.StackTrace StackTrace => new System.Diagnostics.StackTrace(3, fNeedFileInfo: IncludeFileInfoInStacktrace);

        /// <summary>
        /// Check that the predicate is true for the given value. 
        /// </summary>
        /// <remarks>
        /// This is useful for fluent programming, to check assertions inline. 
        /// For example, 
        /// <code><![CDATA[
        ///     var x = MyValue.Is( x => x!=null )
        ///                    .MyProperty; 
        /// ]]></code>
        /// This is most often used in other specialisations rather than directly. 
        /// </remarks>
        /// <typeparam name="T">Type applied to. Usually implicit.</typeparam>
        /// <param name="v">Value being checked.</param>
        /// <param name="Predicate">function to check V. Should return true in all valid cases. False is an assertion failure. </param>
        /// <param name="Message">Message to display if Predicate fails.</param>
        /// <returns></returns>
        [Contract]
        public static T Is<T>(this T v, bool Predicate, string Message = "Failed assert")
        {
            if (!Predicate) Fail(Message);
            return v;
        }

        [Contract]
        private static T Is<T>(this T v, Predicate<T> Predicate, string message)
        {
            if (!Predicate(v)) Fail(message);
            return v;
        }

        /// <summary>
        /// Assert value must be null
        /// </summary>
        [Contract]
        public static T IsNull<T>(this T v, string message = "Value should be null") where T:class => v.Is(x => x == null, message);

        /// <summary>
        /// Assert value must be null
        /// </summary>
        [Contract]
        public static void IsNull( object v, string message = "Value should be null") => v.Is(x => x == null, message);

        /// <summary>
        /// Assert value must be not null 
        /// <code><![CDATA[
        ///     var x = MyValue.IsNotNull().MyProperty; 
        /// ]]></code>

        /// </summary>
        [Contract]
        public static T IsNotNull<T>(this T v, string message = "Value should not be null") where T:class => v.Is(x => x != null, message);

        /// <summary>
        /// Assert value must be not null 
        /// </summary>
        [Contract]
        public static void IsNotNull(object v, string Message = "Value should not be null") => v.Is(x => x != null, Message);

        /// <summary>
        /// Assert value must be true
        /// </summary>
        [Contract]
        public static bool IsTrue(this bool v, string message = "Value should be true") => v.Is(x => x == true, message);

        /// <summary>
        /// Assert value must be false
        /// </summary>
        [Contract]
        public static bool IsFalse(this bool v, string message = "Value should be false") => v.Is(x => x == false, message);

        /// <summary>
        /// Assert string should be null or empty.
        /// </summary>
        [Contract]
        public static void IsNullOrWhitespace(this string v, string message = "Value should be a null or empty string" ) => v.Is( x => string.IsNullOrWhiteSpace(x), message );

        /// <summary>
        /// Assert string should not be null or empty.
        /// </summary>
        [Contract]
        public static string IsNotNullOrWhitespace(this string v, string message = "Value should be a non-empty string" ) => v.Is( x => !string.IsNullOrWhiteSpace(x), message );

        /// <summary>
        /// Assert that any value is acceptable
        /// </summary>
        /// <remarks>
        /// This implies that the author has intentially ignored assertions, rather than just forgetting to assert something. 
        /// It will keep code checkers happy when there's no other assertion. 
        /// </remarks>
        [Contract]
        public static T Ignore<T>(this T v) => v;

    }
}
