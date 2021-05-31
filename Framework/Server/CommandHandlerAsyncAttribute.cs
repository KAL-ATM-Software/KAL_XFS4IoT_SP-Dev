/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoTServer
{
    /// <summary>
    /// Indicate that a command handler should be run asynchronously and 
    /// commands should not be queued.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandHandlerAsyncAttribute : Attribute
    {
    }
}
