/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    /// <summary>
    /// Indicates that a method is a contract/assert
    /// </summary>
    /// <remarks>
    /// This is useful for code analysis to test which code paths have been 
    /// asserted etc. Anything which is an 'Assert' that checks something and 
    /// triggers an FE if something is false should be attributed as a Contract
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple=false)]
    public sealed class ContractAttribute : Attribute { }
}
