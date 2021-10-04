/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "KeyManagement.StartAuthenticate")]
    public sealed class StartAuthenticateCommand : Command<StartAuthenticateCommand.PayloadData>
    {
        public StartAuthenticateCommand(int RequestId, StartAuthenticateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, CommandClass Command = null)
                : base(Timeout)
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
                    /// Specifies the name of key being deleted. if this property is omitted. all keys are deleted.
                    /// </summary>
                    [DataMember(Name = "key")]
                    public string Key { get; init; }

                }

                /// <summary>
                /// This command can be used to delete a key with authentication.
                /// Details of [KeyManagement.DeleteKey](#keymanagement.deletekey) command.
                /// </summary>
                [DataMember(Name = "deleteKey")]
                public DeleteKeyClass DeleteKey { get; init; }

                /// <summary>
                /// This command can be used to initialize encryption module with authentication.
                /// Details of [KeyManagement.Initialization](#keymanagement.initialization) command.
                /// </summary>
                [DataMember(Name = "initialization")]
                public object Initialization { get; init; }

            }

            /// <summary>
            /// The command and the input parameters to which authentication is being applied.
            /// The possible command is one of:
            /// * ```deleteKey``` - Delete a key with authentication.
            /// * ```initialization``` - Initialize encryption module with authentication.
            /// </summary>
            [DataMember(Name = "command")]
            public CommandClass Command { get; init; }

        }
    }
}
