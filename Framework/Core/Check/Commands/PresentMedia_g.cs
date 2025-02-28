/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * PresentMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = PresentMedia
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.PresentMedia")]
    public sealed class PresentMediaCommand : Command<PresentMediaCommand.PayloadData>
    {
        public PresentMediaCommand(int RequestId, PresentMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(SourceClass Source = null)
                : base()
            {
                this.Source = Source;
            }

            [DataContract]
            public sealed class SourceClass
            {
                public SourceClass(PresentMediaPositionEnum? Position = null)
                {
                    this.Position = Position;
                }

                [DataMember(Name = "position")]
                public PresentMediaPositionEnum? Position { get; init; }

            }

            /// <summary>
            /// Specifies the position where items are to be returned from. If null, all items are returned from all
            /// positions in a sequence determined by the Service, otherwise an individual *position* must be specified.
            /// </summary>
            [DataMember(Name = "source")]
            public SourceClass Source { get; init; }

        }
    }
}
