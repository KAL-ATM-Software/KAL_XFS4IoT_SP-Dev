/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * Register_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.VendorMode.Commands
{
    //Original name = Register
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "VendorMode.Register")]
    public sealed class RegisterCommand : Command<RegisterCommand.PayloadData>
    {
        public RegisterCommand(int RequestId, RegisterCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string AppName = null)
                : base()
            {
                this.AppName = AppName;
            }

            /// <summary>
            /// Specifies a logical name for the application that is registering. It should give some indication of
            /// the identity and function of the registering application.
            /// <example>ACME Monitoring app</example>
            /// </summary>
            [DataMember(Name = "appName")]
            public string AppName { get; init; }

        }
    }
}
