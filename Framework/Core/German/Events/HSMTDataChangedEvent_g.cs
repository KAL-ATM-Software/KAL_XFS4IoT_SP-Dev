/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * HSMTDataChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.German.Events
{

    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Event(Name = "German.HSMTDataChangedEvent")]
    public sealed class HSMTDataChangedEvent : UnsolicitedEvent<HSMTDataChangedEvent.PayloadData>
    {

        public HSMTDataChangedEvent()
            : base()
        { }

        public HSMTDataChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string TerminalId = null, string BankCode = null, string OnlineDateAndTime = null, string ZkaId = null, int? HsmStatus = null, string HsmManufacturerId = null, string HsmSerialNumber = null)
                : base()
            {
                this.TerminalId = TerminalId;
                this.BankCode = BankCode;
                this.OnlineDateAndTime = OnlineDateAndTime;
                this.ZkaId = ZkaId;
                this.HsmStatus = HsmStatus;
                this.HsmManufacturerId = HsmManufacturerId;
                this.HsmSerialNumber = HsmSerialNumber;
            }

            /// <summary>
            /// Terminal ID. ISO 8583 BMP 41 (see [[Ref. german-2](#ref-german-2)]). A data source is the EPP.
            /// This property is null if not applicable.
            /// <example>00054321</example>
            /// </summary>
            [DataMember(Name = "terminalId")]
            [DataTypes(Pattern = @"^[0-9]{8}$")]
            public string TerminalId { get; init; }

            /// <summary>
            /// Bank code. ISO 8583 BMP 42 (rightmost 4 bytes see [[Ref. german-2](#ref-german-2)])
            /// Account data for terminal account. A data source is the EPP.
            /// This property is null if not applicable.
            /// <example>00000414</example>
            /// </summary>
            [DataMember(Name = "bankCode")]
            [DataTypes(Pattern = @"^[0-9]{8}$")]
            public string BankCode { get; init; }

            /// <summary>
            /// Online date and time. ISO 8583 BMP 61 (YYYYMMDDHHMMSS) see [[Ref. german-2](#ref-german-2)].
            /// A data source is the HSM.
            /// This property is null if not applicable.
            /// <example>20240919105500</example>
            /// </summary>
            [DataMember(Name = "onlineDateAndTime")]
            [DataTypes(Pattern = @"^20\\d{2}(0[1-9]|1[0,1,2])(0[1-9]|[12][0-9]|3[01])(0[0-9]|1[0-9]|2[0-3])[0-5][0-9][0-5][0-9]$")]
            public string OnlineDateAndTime { get; init; }

            /// <summary>
            /// ZKA ID (is filled during the pre-initialization of the HSM). A data source is the HSM.
            /// This property is null if not applicable.
            /// <example>K12345P123456789</example>
            /// </summary>
            [DataMember(Name = "zkaId")]
            [DataTypes(Pattern = @"^.{16}$")]
            public string ZkaId { get; init; }

            /// <summary>
            /// HSM status. A data source is the HSM.
            /// * ```1``` - irreversibly out of order.
            /// * ```2``` - out of order, K_UR is not loaded.
            /// * ```3``` - not pre-initialized, K_UR is loaded.
            /// * ```4``` - pre-initialized, K_INIT is loaded.
            /// * ```5``` - initialized/personalized, K_PERS is loaded.
            /// <example>3</example>
            /// </summary>
            [DataMember(Name = "hsmStatus")]
            [DataTypes(Minimum = 1, Maximum = 5)]
            public int? HsmStatus { get; init; }

            /// <summary>
            /// HSM manufacturer ID as needed for ISO BMP 57 of a pre-initialization. A data source is the EPP.
            /// This property is null if not applicable.
            /// <example>H00099</example>
            /// </summary>
            [DataMember(Name = "hsmManufacturerId")]
            [DataTypes(Pattern = @"^.{6,}$")]
            public string HsmManufacturerId { get; init; }

            /// <summary>
            /// HSM serial number as needed for ISO BMP 57 of a pre-initialization. A data source is the EPP.
            /// This property is null if not applicable.
            /// <example>0890001234</example>
            /// </summary>
            [DataMember(Name = "hsmSerialNumber")]
            [DataTypes(Pattern = @"^.{10}$")]
            public string HsmSerialNumber { get; init; }

        }

    }
}
