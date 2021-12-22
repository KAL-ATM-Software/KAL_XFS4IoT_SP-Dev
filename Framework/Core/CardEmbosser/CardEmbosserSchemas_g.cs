/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardEmbosser interface.
 * CardEmbosserSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.CardEmbosser
{

    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? CompareMagneticStripe = null, bool? MagneticStripeRead = null, bool? MagneticStripeWrite = null, bool? ChipIO = null, ChipProtocolClass ChipProtocol = null, CharSupportClass CharSupport = null, TypeClass Type = null)
        {
            this.CompareMagneticStripe = CompareMagneticStripe;
            this.MagneticStripeRead = MagneticStripeRead;
            this.MagneticStripeWrite = MagneticStripeWrite;
            this.ChipIO = ChipIO;
            this.ChipProtocol = ChipProtocol;
            this.CharSupport = CharSupport;
            this.Type = Type;
        }

        /// <summary>
        /// Indicates whether the card embosser has capability of comparing magnetic stripe contents as
        /// a prerequisite for an encoding or embossing operation.
        /// </summary>
        [DataMember(Name = "compareMagneticStripe")]
        public bool? CompareMagneticStripe { get; init; }

        /// <summary>
        /// Indicates whether the card embosser has magnetic stripe reading capability.
        /// </summary>
        [DataMember(Name = "magneticStripeRead")]
        public bool? MagneticStripeRead { get; init; }

        /// <summary>
        /// Indicates whether the card embosser has magnetic stripe writing capability.
        /// </summary>
        [DataMember(Name = "magneticStripeWrite")]
        public bool? MagneticStripeWrite { get; init; }

        /// <summary>
        /// Indicates whether the card embosser has smart card updating capability.
        /// </summary>
        [DataMember(Name = "chipIO")]
        public bool? ChipIO { get; init; }

        [DataContract]
        public sealed class ChipProtocolClass
        {
            public ChipProtocolClass(bool? NotSupported = null, bool? ChipT0 = null, bool? ChipT1 = null, bool? ChipProtocolNotRequired = null)
            {
                this.NotSupported = NotSupported;
                this.ChipT0 = ChipT0;
                this.ChipT1 = ChipT1;
                this.ChipProtocolNotRequired = ChipProtocolNotRequired;
            }

            /// <summary>
            /// The card embosser can not handle chip cards.
            /// </summary>
            [DataMember(Name = "notSupported")]
            public bool? NotSupported { get; init; }

            /// <summary>
            /// The card embosser can handle the T=0 protocol.
            /// </summary>
            [DataMember(Name = "chipT0")]
            public bool? ChipT0 { get; init; }

            /// <summary>
            /// The card embosser can handle the T=1 protocol.
            /// </summary>
            [DataMember(Name = "chipT1")]
            public bool? ChipT1 { get; init; }

            /// <summary>
            /// The card embosser is capable of communicating with a chip card without requiring the application to
            /// specify any protocol.
            /// </summary>
            [DataMember(Name = "chipProtocolNotRequired")]
            public bool? ChipProtocolNotRequired { get; init; }

        }

        /// <summary>
        /// Specifies the chip card protocols that are supported by the Service.
        /// </summary>
        [DataMember(Name = "chipProtocol")]
        public ChipProtocolClass ChipProtocol { get; init; }

        [DataContract]
        public sealed class CharSupportClass
        {
            public CharSupportClass(bool? Ascii = null, bool? Unicode = null)
            {
                this.Ascii = Ascii;
                this.Unicode = Unicode;
            }

            /// <summary>
            /// ASCII is supported for XFS forms.
            /// </summary>
            [DataMember(Name = "ascii")]
            public bool? Ascii { get; init; }

            /// <summary>
            /// UNICODE is supported for XFS forms.
            /// </summary>
            [DataMember(Name = "unicode")]
            public bool? Unicode { get; init; }

        }

        /// <summary>
        /// Specifies the character sets, in addition to single byte ASCII, that is supported by the Service.
        /// 
        /// A Service can support ONLY ASCII forms or can support BOTH ASCII and UNICODE forms. A Service
        /// cannot support UNICODE forms without also supporting ASCII forms.
        /// 
        /// This field will be set to a combination of the following:
        /// </summary>
        [DataMember(Name = "charSupport")]
        public CharSupportClass CharSupport { get; init; }

        [DataContract]
        public sealed class TypeClass
        {
            public TypeClass(bool? Emboss = null, bool? Print = null)
            {
                this.Emboss = Emboss;
                this.Print = Print;
            }

            /// <summary>
            /// The card embosser supports embossing data on cards.
            /// </summary>
            [DataMember(Name = "emboss")]
            public bool? Emboss { get; init; }

            /// <summary>
            /// The card embosser supports printing data on cards.
            /// </summary>
            [DataMember(Name = "print")]
            public bool? Print { get; init; }

        }

        /// <summary>
        /// Specifies whether the card embosser has a card embossing capability and/or a card printing capability. This
        /// field will be set to a combination of the following:
        /// </summary>
        [DataMember(Name = "type")]
        public TypeClass Type { get; init; }

    }


}
