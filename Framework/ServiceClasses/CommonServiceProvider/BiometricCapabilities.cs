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
    /// <summary>
    /// BiometricCapabilitiesClass
    /// Store device capabilites for the biometric device
    /// </summary>
    public sealed class BiometricCapabilitiesClass(
        BiometricCapabilitiesClass.DeviceTypeEnum DeviceType,
        int MaxCaptures = 0,
        int TemplateStorageSpace = 0,
        BiometricCapabilitiesClass.FormatEnum DataFormats = BiometricCapabilitiesClass.FormatEnum.None,
        BiometricCapabilitiesClass.AlgorithmEnum EncryptionAlgorithms = BiometricCapabilitiesClass.AlgorithmEnum.None,
        BiometricCapabilitiesClass.StorageEnum Storage = BiometricCapabilitiesClass.StorageEnum.None,
        BiometricCapabilitiesClass.PersistenceModesEnum PersistenceModes = BiometricCapabilitiesClass.PersistenceModesEnum.None,
        BiometricCapabilitiesClass.MatchModesEnum MatchSupported = BiometricCapabilitiesClass.MatchModesEnum.None,
        BiometricCapabilitiesClass.ScanModesEnum ScanModes = BiometricCapabilitiesClass.ScanModesEnum.None,
        BiometricCapabilitiesClass.CompareModesEnum CompareModes = BiometricCapabilitiesClass.CompareModesEnum.None,
        BiometricCapabilitiesClass.ClearModesEnum ClearData = BiometricCapabilitiesClass.ClearModesEnum.None)
    {

        [Flags]
        public enum DeviceTypeEnum
        {
            FacialFeatures = 1 << 0,
            Voice = 1 << 1,
            FingerPrint = 1 << 2,
            FingerVein = 1 << 3,
            Iris = 1 << 4,
            Retina = 1 << 5,
            HandGeometry = 1 << 6,
            ThermalFace = 1 << 7,
            ThermalHand = 1 << 8,
            PalmVein = 1 << 9,
            Signature = 1 << 10,
        }

        [Flags]
        public enum FormatEnum
        {
            None = 0,
            IsoFid = 1 << 0,
            IsoFmd = 1 << 1,
            AnsiFid = 1 << 2,
            AnsiFmd = 1 << 3,
            Qso = 1 << 4,
            Wso = 1 << 5,
            ReservedRaw1 = 1 << 6,
            ReservedTemplate1 = 1 << 7,
            ReservedRaw2 = 1 << 8,
            ReservedTemplate2 = 1 << 9,
            ReservedRaw3 = 1 << 10,
            ReservedTemplate3 = 1 << 11,
        }

        [Flags]
        public enum AlgorithmEnum
        {
            None = 0,
            Ecb = 1 << 0,
            Cbc = 1 << 1,
            Cfb = 1 << 2,
            Rsa = 1 << 3,
        }

        [Flags]
        public enum StorageEnum
        {
            None = 0,
            Secure = 1 << 0,
            Clear = 1 << 1,
        }

        [Flags]
        public enum PersistenceModesEnum
        {
            None = 0,
            Persist = 1 << 0,
            Clear = 1 << 1,
        }

        public enum MatchModesEnum
        {
            None,
            StoredMatch,
            CombinedMatch,
        }

        [Flags]
        public enum ScanModesEnum
        {
            None = 0,
            Scan = 1 << 0,
            Match = 1 << 1,
        }

        [Flags]
        public enum CompareModesEnum
        {
            None = 0,
            Verify = 1 << 0,
            Identity = 1 << 1,
        }

        [Flags]
        public enum ClearModesEnum
        {
            None = 0,
            ScannedData = 1 << 0,
            ImportedData = 1 << 1,
            SetMatchedData = 1 << 2,
        }

        /// <summary>
        /// Specifies the type of biometric device.
        /// </summary>
        public DeviceTypeEnum Type { get; init; } = DeviceType;

        /// <summary>
        /// Specifies the maximum number of times that the device can attempt to capture biometric data during a 
        /// [Biometric.Read](#biometric.read). If this is zero then the device or the Service determines 
        /// how many captures will be attempted.
        /// </summary>
        public int MaxCaptures { get; init; } = MaxCaptures;

        /// <summary>
        /// Specifies the storage space that is reserved on the device for the storage of templates in bytes.
        /// This will be set to zero if not reported or unknown.
        /// </summary>
        public int TemplateStorageSpace { get; init; } = TemplateStorageSpace;

        /// <summary>
        /// Specifies the supported biometric raw data and template data formats reported.
        /// </summary>
        public FormatEnum DataFormats { get; init; } = DataFormats;

        /// <summary>
        /// Supported encryption algorithms. Omitted if no encryption algorithms.
        /// </summary>
        public AlgorithmEnum EncryptionAlgorithms { get; init; } = EncryptionAlgorithms;

        /// <summary>
        /// Indicates whether or not biometric template data can be stored securely or 
        /// none if Biometric template data is not stored in the device.
        /// </summary>
        public StorageEnum Storage { get; init; } = Storage;

        /// <summary>
        /// Specifies which data persistence modes can be set using the [Biometric.SetDataPersistence](#biometric.setdatapersistence). 
        /// This applies specifically to the biometric data that has been captured using the [Biometric.Read](#biometric.read).
        /// A value of none indicates that persistence is entirely under device control and cannot be set.
        /// </summary>
        public PersistenceModesEnum PersistenceModes { get; init; } = PersistenceModes;

        /// <summary>
        /// Specifies if matching is supported using the [Biometric.Match](#biometric.match) 
        /// and/or [Biometric.SetMatch](#biometric.setmatch) command. Omitted if the device does not support matching.
        /// </summary>
        public MatchModesEnum MatchSupported { get; init; } = MatchSupported;

        /// <summary>
        /// Specifies the scan modes that can be used through the [Biometric.Read](#biometric.read).
        /// </summary>
        public ScanModesEnum ScanModes { get; init; } = ScanModes;

        /// <summary>
        /// Specifies the type of match operations. A value of none indicates that matching is not supported.
        /// </summary>
        public CompareModesEnum CompareModes { get; init; } = CompareModes;

        /// <summary>
        /// Specifies the type of data that can be cleared from storage using the [Biometric.Clear](#biometric.clear) 
        /// or [Biometric.Reset](#biometric.reset) command.
        /// </summary>
        public ClearModesEnum ClearData { get; init; } = ClearData;
    }
}
