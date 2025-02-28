/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartAuthenticate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = StartAuthenticate
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.StartAuthenticate")]
    public sealed class StartAuthenticateCommand : Command<StartAuthenticateCommand.PayloadData>
    {
        public StartAuthenticateCommand(int RequestId, StartAuthenticateCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CommandClass Command = null)
                : base()
            {
                this.Command = Command;
            }

            [DataContract]
            public sealed class CommandClass
            {
                public CommandClass(DeleteKeyClass DeleteKey = null, object Initialization = null)
                {
                    this.DeleteKey = DeleteKey;
                    this.Initialization = Initialization;
                }

                [DataContract]
                public sealed class DeleteKeyClass
                {
                    public DeleteKeyClass(string Key = null)
                    {
                        this.Key = Key;
                    }

                    /// <summary>
                    /// The name of key being deleted.
                    /// <example>Key01</example>
                    /// </summary>
                    [DataMember(Name = "key")]
                    public string Key { get; init; }

                }

                /// <summary>
                /// See [KeyManagement.DeleteKey](#keymanagement.deletekey) description.
                /// </summary>
                [DataMember(Name = "deleteKey")]
                public DeleteKeyClass DeleteKey { get; init; }

                /// <summary>
                /// See [KeyManagement.Initialization](#keymanagement.initialization) description.
                /// </summary>
                [DataMember(Name = "initialization")]
                public object Initialization { get; init; }

            }

            /// <summary>
            /// The command and associated command specific input properties for which data to sign is requested.
            /// This must be one of:
            /// 
            /// * ```deleteKey``` - The [KeyManagement.DeleteKey](#keymanagement.deletekey) command.
            /// * ```initialization``` - [KeyManagement.Initialization](#keymanagement.initialization) command.
            /// </summary>
            [DataMember(Name = "command")]
            public CommandClass Command { get; init; }

        }
    }
}
