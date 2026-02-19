/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Import_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = Import
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Biometric.Import")]
    public sealed class ImportCommand : Command<ImportCommand.PayloadData>
    {
        public ImportCommand()
            : base()
        { }

        public ImportCommand(int RequestId, ImportCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<BioDataClass> Templates = null)
                : base()
            {
                this.Templates = Templates;
            }

            /// <summary>
            /// Array of template data to be imported in the device.
            /// </summary>
            [DataMember(Name = "templates")]
            public List<BioDataClass> Templates { get; init; }

        }
    }
}
