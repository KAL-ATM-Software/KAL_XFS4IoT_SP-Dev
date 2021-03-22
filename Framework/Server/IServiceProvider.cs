﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    public interface IServiceProvider : ICommandDispatcher
    {
        Task RunAsync();
        string Name { get; }

        Uri Uri { get; }

        Uri WSUri { get; }
    }
}