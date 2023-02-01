/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BarcodeReader interface.
 * Read_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.BarcodeReader.Commands
{
    //Original name = Read
    [DataContract]
    [Command(Name = "BarcodeReader.Read")]
    public sealed class ReadCommand : Command<ReadCommand.PayloadData>
    {
        public ReadCommand(int RequestId, ReadCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, SymbologiesPropertiesClass Symbologies = null)
                : base(Timeout)
            {
                this.Symbologies = Symbologies;
            }

            /// <summary>
            /// Specifies the sub-set of bar code symbologies that the application wants to be accepted for this
            /// command. In some cases the Service can discriminate between barcode symbologies and return
            /// the data only if the presented symbology matches with one of the desired symbologies. See the
            /// [canFilterSymbologies](#common.capabilities.completion.properties.barcodereader.canfiltersymbologies)
            /// capability to determine if the Service supports this feature. If the Service does
            /// not support this feature then this property is ignored. If all symbologies should be accepted then
            /// the *symbologies* property should be omitted.
            /// </summary>
            [DataMember(Name = "symbologies")]
            public SymbologiesPropertiesClass Symbologies { get; init; }

        }
    }
}
