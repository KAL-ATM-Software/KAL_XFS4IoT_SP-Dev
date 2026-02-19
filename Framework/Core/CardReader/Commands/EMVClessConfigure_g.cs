/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessConfigure_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = EMVClessConfigure
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "CardReader.EMVClessConfigure")]
    public sealed class EMVClessConfigureCommand : Command<EMVClessConfigureCommand.PayloadData>
    {
        public EMVClessConfigureCommand()
            : base()
        { }

        public EMVClessConfigureCommand(int RequestId, EMVClessConfigureCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<byte> TerminalData = null, List<AidDataClass> AidData = null, List<KeyDataClass> KeyData = null)
                : base()
            {
                this.TerminalData = TerminalData;
                this.AidData = AidData;
                this.KeyData = KeyData;
            }

            /// <summary>
            /// Base64 encoded representation of the BER-TLV formatted data for the terminal, e.g., Terminal Type,
            /// Transaction Category Code, Merchant Name &amp; Location etc. Any terminal based data elements referenced
            /// in the Payment Systems Specifications or EMVCo Contactless Payment Systems Specifications Books may be
            /// included (see [[Ref. cardreader-1](#ref-cardreader-1)], [[Ref. cardreader-2](#ref-cardreader-2)] and
            /// [[Ref. cardreader-3](#ref-cardreader-3)] for more details).
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "terminalData")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> TerminalData { get; init; }

            [DataContract]
            public sealed class AidDataClass
            {
                public AidDataClass(List<byte> Aid = null, bool? PartialSelection = null, int? TransactionType = null, List<byte> KernelIdentifier = null, List<byte> ConfigData = null)
                {
                    this.Aid = Aid;
                    this.PartialSelection = PartialSelection;
                    this.TransactionType = TransactionType;
                    this.KernelIdentifier = KernelIdentifier;
                    this.ConfigData = ConfigData;
                }

                /// <summary>
                /// The application identifier to be accepted by the contactless chip card reader. The
                /// [CardReader.EMVClessQueryApplications](#cardreader.emvclessqueryapplications) command will
                /// return the list of supported application identifiers.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "aid")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Aid { get; init; }

                /// <summary>
                /// If *partialSelection* is *true*, partial name selection of the specified AID is enabled. If
                /// *partialSelection* is *false*, partial name selection is disabled. A detailed explanation for
                /// partial name selection is given in [[Ref. cardreader-2](#ref-cardreader-2)], Section 11.3.5.
                /// </summary>
                [DataMember(Name = "partialSelection")]
                public bool? PartialSelection { get; init; }

                /// <summary>
                /// The transaction type supported by the AID. This indicates the type of financial transaction
                /// represented by the first two digits of the ISO 8583:1987 Processing Code [[Ref. cardreader-4](#ref-cardreader-4)].
                /// </summary>
                [DataMember(Name = "transactionType")]
                [DataTypes(Minimum = 0)]
                public int? TransactionType { get; init; }

                /// <summary>
                /// Base64 encoded representation of the EMVCo defined kernel identifier associated with the *aid*.
                /// This will be ignored if the reader does not support kernel identifiers. This property is null if not applicable.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "kernelIdentifier")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> KernelIdentifier { get; init; }

                /// <summary>
                /// Base64 encoded representation of the list of BER-TLV formatted configuration data, applicable to
                /// the specific AID-Kernel ID-Transaction Type combination. The appropriate payment systems
                /// specifications define the BER-TLV tags to be configured.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "configData")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> ConfigData { get; init; }

            }

            /// <summary>
            /// Specifies the list of acceptable payment system applications. For EMVCo approved contactless card
            /// readers each AID is associated with a Kernel Identifier and a Transaction Type. Legacy approved
            /// contactless readers may use only the AID.
            /// 
            /// Each AID-Transaction Type or each AID-Kernel-Transaction Type combination will have its own unique set
            /// of configuration data. See [[Ref. cardreader-2](#ref-cardreader-2)] and
            /// [[Ref. cardreader-3](#ref-cardreader-3)] for more details.
            /// </summary>
            [DataMember(Name = "aidData")]
            public List<AidDataClass> AidData { get; init; }

            [DataContract]
            public sealed class KeyDataClass
            {
                public KeyDataClass(List<byte> Rid = null, CaPublicKeyClass CaPublicKey = null)
                {
                    this.Rid = Rid;
                    this.CaPublicKey = CaPublicKey;
                }

                /// <summary>
                /// Specifies the payment system's Registered Identifier (RID). RID is the first 5 bytes of the AID
                /// and identifies the payments system.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "rid")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Rid { get; init; }

                [DataContract]
                public sealed class CaPublicKeyClass
                {
                    public CaPublicKeyClass(int? Index = null, int? AlgorithmIndicator = null, List<byte> Exponent = null, List<byte> Modulus = null, List<byte> Checksum = null)
                    {
                        this.Index = Index;
                        this.AlgorithmIndicator = AlgorithmIndicator;
                        this.Exponent = Exponent;
                        this.Modulus = Modulus;
                        this.Checksum = Checksum;
                    }

                    /// <summary>
                    /// Specifies the CA Public Key Index for the specific *rid*.
                    /// </summary>
                    [DataMember(Name = "index")]
                    [DataTypes(Minimum = 0)]
                    public int? Index { get; init; }

                    /// <summary>
                    /// Specifies the algorithm used in the calculation of the CA Public Key checksum. A detailed
                    /// description of secure hash algorithm values is given in EMV Book 2, Annex B3; see
                    /// [[Ref. cardreader-2](#ref-cardreader-2)]. For example, if the EMV specification indicates
                    /// the algorithm is '01', the value of the algorithm is coded as 1.
                    /// </summary>
                    [DataMember(Name = "algorithmIndicator")]
                    [DataTypes(Minimum = 0)]
                    public int? AlgorithmIndicator { get; init; }

                    /// <summary>
                    /// Base64 encoded representation of the CA Public Key Exponent for the specific RID. This value
                    /// is represented by the minimum number of bytes required. A detailed description of public key
                    /// exponent values is given in EMV Book 2, Annex B2; see
                    /// [[Ref. cardreader-2](#ref-cardreader-2)]. For example, representing value ‘2[sup]16[/sup] +
                    /// 1’ requires 3 bytes in hexadecimal (0x01, 0x00, 0x01), while value ‘3’ is coded as 0x03.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "exponent")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> Exponent { get; init; }

                    /// <summary>
                    /// Base64 encoded representation of the CA Public Key Modulus for the specific RID.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "modulus")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> Modulus { get; init; }

                    /// <summary>
                    /// Base64 encoded representation of the 20 byte checksum value for the CA Public Key.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "checksum")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> Checksum { get; init; }

                }

                /// <summary>
                /// CA Public Key information for the specified *rid*.
                /// </summary>
                [DataMember(Name = "caPublicKey")]
                public CaPublicKeyClass CaPublicKey { get; init; }

            }

            /// <summary>
            /// Specifies the encryption key information required by an intelligent contactless chip card reader for
            /// offline data authentication. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "keyData")]
            public List<KeyDataClass> KeyData { get; init; }

        }
    }
}
