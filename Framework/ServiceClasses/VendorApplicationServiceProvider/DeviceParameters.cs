/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.VendorApplication
{
    public enum ActiveInterfaceEnum
    {
        Consumer, //The consumer interface.
        Operator  //The operator interface.
    }

    public sealed class StartLocalApplicationRequest(
        string ApplicationName,
        StartLocalApplicationRequest.AccessLevelEnum? AccessLevel)
    {
        public enum AccessLevelEnum
        {
            Basic,        //The application is active for the basic access level.
            Intermediate, //The application is active for the intermediate access level.
            Full,         //The application is active for the full access level.
        }

        public string ApplicationName { get; init; } = ApplicationName;

        public AccessLevelEnum? AccessLevel { get; init; } = AccessLevel;
    }

    public sealed class SetActiveInterfaceRequest(ActiveInterfaceEnum ActiveInterface)
    {
        public ActiveInterfaceEnum ActiveInterface { get; init; } = ActiveInterface;
    }

    public sealed class GetActiveInterfaceResult : DeviceResult
    {
        /// <summary>
        /// GetActiveInterfaceResult
        /// Return result of active interface
        /// </summary>
        public GetActiveInterfaceResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription)
            : base(CompletionCode, ErrorDescription)
        {
            this.ActiveInterface = null;
        }

        /// <summary>
        /// GetActiveInterfaceResult
        /// Return result of active interface
        /// </summary>
        public GetActiveInterfaceResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            ActiveInterfaceEnum ActiveInterface)
            : base(CompletionCode, null)
        {
            this.ActiveInterface = ActiveInterface;
        }

        public ActiveInterfaceEnum? ActiveInterface { get; init; }
    }
}
