/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "CardReader.EMVClessConfigure")]
    public sealed class EMVClessConfigureCommand : Command<EMVClessConfigureCommand.PayloadData>
    {
        public EMVClessConfigureCommand(int RequestId, EMVClessConfigureCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string TerminalData = null, List<AidDataClass> AidData = null, List<KeyDataClass> KeyData = null)
                : base(Timeout)
            {
                this.TerminalData = TerminalData;
                this.AidData = AidData;
                this.KeyData = KeyData;
            }

            /// <summary>
            /// Base64 encoded representation of the BER-TLV formatted data for the terminal e.g. Terminal Type,
            /// Transaction Category Code, Merchant Name & Location etc. Any terminal based data elements referenced
            /// in the Payment Systems Specifications or EMVCo Contactless Payment Systems Specifications Books may be
            /// included (see References [2] to [14] section for more details).
            /// </summary>
            [DataMember(Name = "terminalData")]
            public string TerminalData { get; private set; }

            [DataContract]
            public sealed class AidDataClass
            {
                public AidDataClass(string Aid = null, bool? PartialSelection = null, int? TransactionType = null, string KernelIdentifier = null, string ConfigData = null)
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
                /// </summary>
                [DataMember(Name = "aid")]
                public string Aid { get; private set; }

                /// <summary>
                /// If *partialSelection* is *true*, partial name selection of the specified AID is enabled. If
                /// *partialSelection* is *false*, partial name selection is disabled. A detailed explanation for
                /// partial name selection is given in EMV 4.3 Book 1, Section 11.3.5.
                /// </summary>
                [DataMember(Name = "partialSelection")]
                public bool? PartialSelection { get; private set; }

                /// <summary>
                /// The transaction type supported by the AID. This indicates the type of financial transaction
                /// represented by the first two digits of the ISO 8583:1987 Processing Code.
                /// </summary>
                [DataMember(Name = "transactionType")]
                public int? TransactionType { get; private set; }

                /// <summary>
                /// Base64 encoded representation of the EMVCo defined kernel identifier associated with the *aid*.
                /// This field will be ignored if the reader does not support kernel identifiers.
                /// </summary>
                [DataMember(Name = "kernelIdentifier")]
                public string KernelIdentifier { get; private set; }

                /// <summary>
                /// Base64 encoded representation of the list of BER-TLV formatted configuration data, applicable to
                /// the specific AID-Kernel ID-Transaction Type combination. The appropriate payment systems
                /// specifications define the BER-TLV tags to be configured.
                /// </summary>
                [DataMember(Name = "configData")]
                public string ConfigData { get; private set; }

            }

            /// <summary>
            /// Specifies the list of acceptable payment system applications. For EMVCo approved contactless card
            /// readers each AID is associated with a Kernel Identifier and a Transaction Type. Legacy approved
            /// contactless readers may use only the AID.
            /// 
            /// Each AID-Transaction Type or each AID-Kernel-Transaction Type combination will have its own unique set
            /// of configuration data. See References [2] and [3] for more details.
            /// </summary>
            [DataMember(Name = "aidData")]
            public List<AidDataClass> AidData { get; private set; }

            [DataContract]
            public sealed class KeyDataClass
            {
                public KeyDataClass(string Rid = null, CaPublicKeyClass CaPublicKey = null)
                {
                    this.Rid = Rid;
                    this.CaPublicKey = CaPublicKey;
                }

                /// <summary>
                /// Specifies the payment system's Registered Identifier (RID). RID is the first 5 bytes of the AID
                /// and identifies the payments system.
                /// </summary>
                [DataMember(Name = "rid")]
                public string Rid { get; private set; }

                [DataContract]
                public sealed class CaPublicKeyClass
                {
                    public CaPublicKeyClass(int? Index = null, int? AlgorithmIndicator = null, string Exponent = null, string Modulus = null, string Checksum = null)
                    {
                        this.Index = Index;
                        this.AlgorithmIndicator = AlgorithmIndicator;
                        this.Exponent = Exponent;
                        this.Modulus = Modulus;
                        this.Checksum = Checksum;
                    }

                    /// <summary>
                    /// Specifies the CA Public Key Index for the specific RID.
                    /// </summary>
                    [DataMember(Name = "index")]
                    public int? Index { get; private set; }

                    /// <summary>
                    /// Specifies the algorithm used in the calculation of the CA Public Key checksum. A detailed
                    /// description of secure hash algorithm values is given in EMV Book 2, Annex B3; see reference
                    /// [2]. For example, if the EMV specification indicates the algorithm is ‘01’, the value of the
                    /// algorithm is coded as 0x01.
                    /// </summary>
                    [DataMember(Name = "algorithmIndicator")]
                    public int? AlgorithmIndicator { get; private set; }

                    /// <summary>
                    /// Base64 encoded representation of the CA Public Key Exponent for the specific RID. This value
                    /// is represented by the minimum number of bytes required. A detailed description of public key
                    /// exponent values is given in EMV Book 2, Annex B2; see reference [2]. For example,
                    /// representing value ‘216 + 1’ requires 3 bytes in hexadecimal (0x01, 0x00, 0x01), while value
                    /// ‘3’ is coded as 0x03.
                    /// </summary>
                    [DataMember(Name = "exponent")]
                    public string Exponent { get; private set; }

                    /// <summary>
                    /// Base64 encoded representation of the CA Public Key Modulus for the specific RID.
                    /// </summary>
                    [DataMember(Name = "modulus")]
                    public string Modulus { get; private set; }

                    /// <summary>
                    /// Base64 encoded representation of the 20 byte checksum value for the CA Public Key.
                    /// </summary>
                    [DataMember(Name = "checksum")]
                    public string Checksum { get; private set; }

                }

                /// <summary>
                /// CA Public Key information for the specified *rid*
                /// </summary>
                [DataMember(Name = "caPublicKey")]
                public CaPublicKeyClass CaPublicKey { get; private set; }

            }

            /// <summary>
            /// Specifies the encryption key information required by an intelligent contactless chip card reader for
            /// offline data authentication.
            /// </summary>
            [DataMember(Name = "keyData")]
            public List<KeyDataClass> KeyData { get; private set; }

        }
    }
}
