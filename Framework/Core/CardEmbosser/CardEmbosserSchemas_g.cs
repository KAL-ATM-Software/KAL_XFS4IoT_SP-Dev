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
    public sealed class StatusClass
    {
        public StatusClass(MediaEnum? Media = null, RetainBinEnum? RetainBin = null, OutputBinEnum? OutputBin = null, InputBinEnum? InputBin = null, int? TotalCards = null, int? OutputCards = null, int? RetainCards = null, TonerEnum? Toner = null)
        {
            this.Media = Media;
            this.RetainBin = RetainBin;
            this.OutputBin = OutputBin;
            this.InputBin = InputBin;
            this.TotalCards = TotalCards;
            this.OutputCards = OutputCards;
            this.RetainCards = RetainCards;
            this.Toner = Toner;
        }

        public enum MediaEnum
        {
            Present,
            NotPresent,
            Jammed,
            NotSupported,
            Unknown,
            Entering,
            Topper,
            InHopper,
            OutHopper,
            Msre,
            Retained
        }

        /// <summary>
        /// Specifies the state of the card embosser media as one of the following:
        /// 
        /// * ```present``` - Media is present in the device, not in the entering position and not jammed.
        /// * ```notPresent``` - Media is not present in the device and not at the entering position.
        /// * ```jammed``` - Media is jammed in the device; operator intervention is required.
        /// * ```notSupported``` - Capability to report media position is not supported by the device.
        /// * ```unknown``` - The media state cannot be determined with the device in its current state (e.g. the value
        ///   of [device](#common.status.completion.properties.common.device) is *noDevice*, *powerOff*, *offline*, or
        ///   *hardwareError*).
        /// * ```entering``` - Media is at the entry/exit slot.
        /// * ```topper``` - Topper failure.
        /// * ```inHopper``` - Card is positioned in input bin.
        /// * ```outHopper``` - Card is positioned in output bin.
        /// * ```msre``` - Encoding failure.
        /// * ```retained``` - Card is positioned in retain bin.
        /// </summary>
        [DataMember(Name = "media")]
        public MediaEnum? Media { get; init; }

        public enum RetainBinEnum
        {
            Ok,
            Full,
            High,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the card embosser retain bin as one of the following:
        /// 
        /// * ```ok``` - The retain bin is in a good state.
        /// * ```full``` - The retain bin is full.
        /// * ```high``` - The retain bin is nearly full.
        /// * ```notSupported``` - The retain bin state can not be reported.
        /// </summary>
        [DataMember(Name = "retainBin")]
        public RetainBinEnum? RetainBin { get; init; }

        public enum OutputBinEnum
        {
            Ok,
            Full,
            High,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the card embosser output bin as one of the following:
        /// 
        /// * ```ok``` - The output bin is in a good state.
        /// * ```full``` - The output bin is full.
        /// * ```high``` - The output bin is nearly full.
        /// * ```notSupported``` - The output bin state can not be reported.
        /// </summary>
        [DataMember(Name = "outputBin")]
        public OutputBinEnum? OutputBin { get; init; }

        public enum InputBinEnum
        {
            Ok,
            Empty,
            Low,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the card embosser input bin as one of the following:
        /// 
        /// * ```ok``` - The input bin is in a good state.
        /// * ```empty``` - The input bin is empty.
        /// * ```low``` - The input bin is nearly empty.
        /// * ```notSupported``` - The input bin state can not be reported.
        /// </summary>
        [DataMember(Name = "inputBin")]
        public InputBinEnum? InputBin { get; init; }

        /// <summary>
        /// The total number of cards, including those in input bin, output bin, and retain bin.
        /// </summary>
        [DataMember(Name = "totalCards")]
        public int? TotalCards { get; init; }

        /// <summary>
        /// The total number of output bin cards.
        /// </summary>
        [DataMember(Name = "outputCards")]
        public int? OutputCards { get; init; }

        /// <summary>
        /// The total number of retain bin cards.
        /// </summary>
        [DataMember(Name = "retainCards")]
        public int? RetainCards { get; init; }

        public enum TonerEnum
        {
            Full,
            Low,
            Out,
            NotSupported,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the toner or ink supply or the state of the ribbon as one of the following:
        /// 
        /// * ```full``` - The toner or ink supply is full or the ribbon is OK.
        /// * ```low``` - The toner or ink supply is low or the print contrast with a ribbon is weak.
        /// * ```out``` - The toner or ink supply is empty or the print contrast with a ribbon is not sufficient any
        ///   more.
        /// * ```notSupported``` - The toner or ink supply is not supported by the device.
        /// * ```unknown``` - Status of toner or ink supply or the ribbon cannot be determined with device in its
        /// current state.
        /// </summary>
        [DataMember(Name = "toner")]
        public TonerEnum? Toner { get; init; }

    }


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
        /// Indicates whether the card embosser has capability of comparing magnetic stripe contents (* ```true* ```) as
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
        /// Specifies the chip card protocols that are supported by the Service Provider as a combination of the
        /// following:
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
        /// Specifies the character sets, in addition to single byte ASCII, that is supported by the Service Provider
        /// 
        /// A Service Provider can support ONLY ASCII forms or can support BOTH ASCII and UNICODE forms. A Service
        /// Provider cannot support UNICODE forms without also supporting ASCII forms.
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
