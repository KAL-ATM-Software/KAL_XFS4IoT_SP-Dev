/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
    public sealed class PinPadCapabilitiesClass(
        PinPadCapabilitiesClass.PINFormatEnum PINFormat,
        PinPadCapabilitiesClass.PresentationAlgorithmEnum PresentationAlgorithm,
        PinPadCapabilitiesClass.DisplayTypeEnum DisplayType,
        bool IDConnect,
        PinPadCapabilitiesClass.ValidationAlgorithmEnum ValidationAlgorithm,
        bool PinCanPersistAfterUse,
        bool TypeCombined,
        bool SetPinblockDataRequired,
        Dictionary<string, Dictionary<string, Dictionary<string, PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm>>> PinBlockAttributes)
    {
        [Flags]
        public enum PINFormatEnum
        {
            NotSupported = 0,
            IBM3624 = 1 << 0,
            ANSI = 1 << 1,
            ISO0 = 1 << 2,
            ISO1 = 1 << 3,
            ECI2 = 1 << 4,
            ECI3 = 1 << 5,
            VISA = 1 << 6,
            DIEBOLD = 1 << 7,
            DIEBOLDCO = 1 << 8,
            VISA3 = 1 << 9,
            BANKSYS = 1 << 10,
            EMV = 1 << 11,
            ISO3 = 1 << 12,
            AP = 1 << 13,
            ISO4 = 1 << 14,
        }

        [Flags]
        public enum PresentationAlgorithmEnum
        {
            NotSupported = 0,
            PresentClear = 1 << 0,
        }

        [Flags]
        public enum DisplayTypeEnum
        {
            NotSupported = 0,
            LEDThrough = 1 << 0,
            Display = 1 << 1,
        }

        [Flags]
        public enum ValidationAlgorithmEnum
        {
            NotSupported = 0,
            DES = 1 << 0,
            VISA = 1 << 1,
        }

        /// <summary>
        /// Supported PIN block format
        /// </summary>
        public PINFormatEnum PINFormat { get; init; } = PINFormat;

        /// <summary>
        /// Supported presentation algorithms
        /// </summary>
        public PresentationAlgorithmEnum PresentationAlgorithm { get; init; } = PresentationAlgorithm;

        /// <summary>
        /// Specifies the type of the display used in the PIN pad module 
        /// </summary>
        public DisplayTypeEnum DisplayType { get; init; } = DisplayType;

        /// <summary>
        /// Specifies whether the PIN pad is directly physically connected to the ID card unit
        /// </summary>
        public bool IDConnect { get; init; } = IDConnect;

        /// <summary>
        /// Specifies the algorithms for PIN validation supported by the service
        /// </summary>
        public ValidationAlgorithmEnum ValidationAlgorithm { get; init; } = ValidationAlgorithm;

        /// <summary>
        /// Specifies whether the device can retain the PIN after a PIN processing command.
        /// </summary>
        public bool PinCanPersistAfterUse { get; init; } = PinCanPersistAfterUse;

        /// <summary>
        /// Specifies whether the keypad used in the secure PIN pad module is integrated within a generic Win32 keyboard.
        /// </summary>
        public bool TypeCombined { get; init; } = TypeCombined;

        /// <summary>
        /// Specifies whether the command SetPinblockData must be 
        /// called before the PIN is entered via Keyboard.PinEntry and retrieved via 
        /// GetPinblock
        /// </summary>
        public bool SetPinblockDataRequired { get; init; } = SetPinblockDataRequired;

        public sealed class PinBlockEncryptionAlgorithm
        {
            [Flags]
            public enum EncryptionAlgorithmEnum
            {
                NotSupported = 0,
                ECB = 1 << 0,
                CBC = 1 << 1,
                CFB = 1 << 2,
                OFB = 1 << 3,
                CTR = 1 << 4,
                XTS = 1 << 5,
                RSAES_PKCS1_V1_5 = 1 << 6,
                RSAES_OAEP = 1 << 7,
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
        public Dictionary<string, Dictionary<string, Dictionary<string, PinBlockEncryptionAlgorithm>>> PinBlockAttributes { get; init; } = PinBlockAttributes;

    }
}
