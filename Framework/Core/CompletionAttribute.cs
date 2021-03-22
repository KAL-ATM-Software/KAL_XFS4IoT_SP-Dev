/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    /// <summary>
    /// Indicate that a class can be used as a response in a message dispatcher. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class CompletionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}