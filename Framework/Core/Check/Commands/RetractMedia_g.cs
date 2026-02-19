/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * RetractMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = RetractMedia
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.RetractMedia")]
    public sealed class RetractMediaCommand : Command<RetractMediaCommand.PayloadData>
    {
        public RetractMediaCommand()
            : base()
        { }

        public RetractMediaCommand(int RequestId, RetractMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string RetractLocation = null)
                : base()
            {
                this.RetractLocation = RetractLocation;
            }

            /// <summary>
            /// Specifies the location for the retracted media, on input where it is to be retracted to, on output where it
            /// was retracted to. See [retractLocation](#common.capabilities.completion.description.check.retractlocation) to
            /// determine the supported locations. This can take one of the following values:
            /// 
            /// * ```stacker``` - The device stacker.
            /// * ```transport``` - The device transport.
            /// * ```rebuncher``` - The device re-buncher.
            /// * ```[storage unit identifier]``` - A storage unit as specified by
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// <example>rebuncher</example>
            /// </summary>
            [DataMember(Name = "retractLocation")]
            [DataTypes(Pattern = @"^stacker$|^transport$|^rebuncher$|^unit[0-9A-Za-z]+$")]
            public string RetractLocation { get; init; }

        }
    }
}
