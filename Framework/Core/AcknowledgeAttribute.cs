﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoT
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AcknowledgeAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
