/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class PinPadCapabilitiesClass
    {
        [Flags]
        public enum PINFormatEnum
        {
            NotSupported = 0,
            IBM3624 = 0x0001,
            ANSI = 0x0002,
            ISO0 = 0x0004,
            ISO1 = 0x0008,
            ECI2 = 0x0010,
            ECI3 = 0x0020,
            VISA = 0x0040,
            DIEBOLD = 0x0080,
            DIEBOLDCO = 0x0100,
            VISA3 = 0x0200,
            BANKSYS = 0x0400,
            EMV = 0x0800,
            ISO3 = 0x1000,
            AP = 0x2000,
        }

        [Flags]
        public enum PresentationAlgorithmEnum
        {
            NotSupported = 0,
            PresentClear = 0x0001,
        }

        [Flags]
        public enum DisplayTypeEnum
        {
            NotSupported = 0,
            LEDThrough = 0x0001,
            Display = 0x0002,
        }

        [Flags]
        public enum ValidationAlgorithmEnum
        {
            NotSupported = 0,
            DES = 0x0001,
            VISA = 0x0002,
        }

        public PinPadCapabilitiesClass(PINFormatEnum PINFormat,
                                       PresentationAlgorithmEnum PresentationAlgorithm,
                                       DisplayTypeEnum DisplayType,
                                       bool IDConnect,
                                       ValidationAlgorithmEnum ValidationAlgorithm,
                                       bool PinCanPersistAfterUse,
                                       bool TypeCombined,
                                       bool SetPinblockDataRequired,
                                       Dictionary<string, Dictionary<string, Dictionary<string, PinBlockEncryptionAlgorithm>>> PinBlockAttributes)
        {
            this.PINFormat = PINFormat;
            this.PresentationAlgorithm = PresentationAlgorithm;
            this.DisplayType = DisplayType;
            this.IDConnect = IDConnect;
            this.ValidationAlgorithm = ValidationAlgorithm;
            this.PinCanPersistAfterUse = PinCanPersistAfterUse;
            this.TypeCombined = TypeCombined;
            this.SetPinblockDataRequired = SetPinblockDataRequired;
            this.PinBlockAttributes = PinBlockAttributes;
        }

        /// <summary>
        /// Supported PIN block format
        /// </summary>
        public PINFormatEnum PINFormat { get; init; }

        /// <summary>
        /// Supported presentation algorithms
        /// </summary>
        public PresentationAlgorithmEnum PresentationAlgorithm { get; init; }

        /// <summary>
        /// Specifies the type of the display used in the PIN pad module 
        /// </summary>
        public DisplayTypeEnum DisplayType { get; init; }

        /// <summary>
        /// Specifies whether the PIN pad is directly physically connected to the ID card unit
        /// </summary>
        public bool IDConnect { get; init; }

        /// <summary>
        /// Specifies the algorithms for PIN validation supported by the service
        /// </summary>
        public ValidationAlgorithmEnum ValidationAlgorithm { get; init; }

        /// <summary>
        /// Specifies whether the device can retain the PIN after a PIN processing command.
        /// </summary>
        public bool PinCanPersistAfterUse { get; init; }

        /// <summary>
        /// Specifies whether the keypad used in the secure PIN pad module is integrated within a generic Win32 keyboard.
        /// </summary>
        public bool TypeCombined { get; init; }

        /// <summary>
        /// Specifies whether the command SetPinblockData must be 
        /// called before the PIN is entered via Keyboard.PinEntry and retrieved via 
        /// GetPinblock
        /// </summary>
        public bool SetPinblockDataRequired { get; init; }

        public sealed class PinBlockEncryptionAlgorithm
        {
            [Flags]
            public enum EncryptionAlgorithmEnum
            {
                NotSupported = 0,
                ECB = 0x0001,
                CBC = 0x0002,
                CFB = 0x0004,
                OFB = 0x0008,
                CTR = 0x0010,
                XTS = 0x0020,
                RSAES_PKCS1_V1_5 = 0x0040,
                RSAES_OAEP = 0x0080,
            }

            public PinBlockEncryptionAlgorithm(EncryptionAlgorithmEnum EncryptionAlgorithm)
            {
                this.EncryptionAlgorithm = EncryptionAlgorithm;
            }


            /// <summary>
            /// Specifies the cryptographic method supported. 
            /// If the algorithm is 'A', 'D', or 'T', then the following properties can be true." 
            /// 
            /// * ```ECB``` - The ECB encryption method. 
            /// * ```CBC``` - The CBC encryption method.  
            /// * ```CFB``` - The CFB encryption method.  
            /// * ```OFB``` - The OFB encryption method. 
            /// * ```CTR``` - The CTR method defined in NIST SP800-38A.  
            /// * ```XTS``` - The XTS method defined in NIST SP800-38E. 
            /// 
            /// If the algorithm is 'R', then following properties can be true.  
            /// 
            /// * ```RSAES_PKCS1_V1_5``` - Use the RSAES_PKCS1-v1.5 algorithm. 
            /// * ```RSAES_OAEP``` - Use the RSAES OAEP algorithm.
            /// </summary>
            public EncryptionAlgorithmEnum EncryptionAlgorithm { get; init; }
        }

        /// <summary>
        /// Key-value pair of attributes supported by the GetPinBlock
        /// </summary>
        public Dictionary<string, Dictionary<string, Dictionary<string, PinBlockEncryptionAlgorithm>>> PinBlockAttributes { get; init; }

    }
}
