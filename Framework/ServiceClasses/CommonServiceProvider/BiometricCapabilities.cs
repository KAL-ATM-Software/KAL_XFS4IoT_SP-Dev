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
    /// <summary>
    /// BiometricCapabilitiesClass
    /// Store device capabilites for the biometric device
    /// </summary>
    public sealed class BiometricCapabilitiesClass
    {

        [Flags]
        public enum DeviceTypeEnum
        {
            FacialFeatures = 1,
            Voice = 2,
            FingerPrint = 4, 
            FingerVein = 8,
            Iris = 16,
            Retina = 32,
            HandGeometry = 64,
            ThermalFace = 128,
            ThermalHand = 256,
            PalmVein = 512,
            Signature = 1024,
        }

        [Flags]
        public enum FormatEnum
        {
            None = 0,
            IsoFid = 1,
            IsoFmd = 2,
            AnsiFid = 4,
            AnsiFmd = 8,
            Qso = 16,
            Wso = 32,
            ReservedRaw1 = 64,
            ReservedTemplate1 = 128,
            ReservedRaw2 = 256,
            ReservedTemplate2 = 512,
            ReservedRaw3 = 1024,
            ReservedTemplate3 = 2048,
        }

        [Flags]
        public enum AlgorithmEnum
        {
            None = 0,
            Ecb = 1,
            Cbc = 2,
            Cfb = 4,
            Rsa = 8,
        }

        [Flags]
        public enum StorageEnum
        {
            None = 0,
            Secure = 1, 
            Clear = 2,
        }

        [Flags]
        public enum PersistenceModesEnum
        {
            None = 0,
            Persist = 1, 
            Clear = 2,
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
            Scan = 1,
            Match = 2,
        }

        [Flags]
        public enum CompareModesEnum
        {
            None = 0,
            Verify = 1,
            Identity = 2,
        }

        [Flags]
        public enum ClearModesEnum
        {
            None = 0,
            ScannedData = 1,
            ImportedData = 2,
            SetMatchedData = 4,
        }

        public BiometricCapabilitiesClass(DeviceTypeEnum DeviceType,
                                          int MaxCaptures = 0,
                                          int TemplateStorageSpace = 0,
                                          FormatEnum DataFormats = FormatEnum.None,
                                          AlgorithmEnum EncryptionAlgorithms = AlgorithmEnum.None,
                                          StorageEnum Storage = StorageEnum.None,
                                          PersistenceModesEnum PersistenceModes = PersistenceModesEnum.None,
                                          MatchModesEnum MatchSupported = MatchModesEnum.None,
                                          ScanModesEnum ScanModes = ScanModesEnum.None,
                                          CompareModesEnum CompareModes = CompareModesEnum.None,
                                          ClearModesEnum ClearData = ClearModesEnum.None)
        {
            this.Type = DeviceType;
            this.MaxCaptures = MaxCaptures;
            this.TemplateStorageSpace = TemplateStorageSpace;
            this.DataFormats = DataFormats;
            this.EncryptionAlgorithms = EncryptionAlgorithms;
            this.Storage = Storage;
            this.PersistenceModes = PersistenceModes;
            this.MatchSupported = MatchSupported;
            this.ScanModes = ScanModes;
            this.CompareModes = CompareModes;
            this.ClearData = ClearData;
        }

        /// <summary>
        /// Specifies the type of biometric device.
        /// </summary>
        public DeviceTypeEnum Type { get; init; }

        /// <summary>
        /// Specifies the maximum number of times that the device can attempt to capture biometric data during a 
        /// [Biometric.Read](#biometric.read). If this is zero then the device or the Service determines 
        /// how many captures will be attempted.
        /// </summary>
        public int MaxCaptures { get; init; }

        /// <summary>
        /// Specifies the storage space that is reserved on the device for the storage of templates in bytes.
        /// This will be set to zero if not reported or unknown.
        /// </summary>
        public int TemplateStorageSpace { get; init; }

        /// <summary>
        /// Specifies the supported biometric raw data and template data formats reported.
        /// </summary>
        public FormatEnum DataFormats { get; init; }

        /// <summary>
        /// Supported encryption algorithms. Omitted if no encryption algorithms.
        /// </summary>
        public AlgorithmEnum EncryptionAlgorithms { get; init; }

        /// <summary>
        /// Indicates whether or not biometric template data can be stored securely or 
        /// none if Biometric template data is not stored in the device.
        /// </summary>
        public StorageEnum Storage { get; init; }

        /// <summary>
        /// Specifies which data persistence modes can be set using the [Biometric.SetDataPersistence](#biometric.setdatapersistence). 
        /// This applies specifically to the biometric data that has been captured using the [Biometric.Read](#biometric.read).
        /// A value of none indicates that persistence is entirely under device control and cannot be set.
        /// </summary>
        public PersistenceModesEnum PersistenceModes { get; init; }

        /// <summary>
        /// Specifies if matching is supported using the [Biometric.Match](#biometric.match) 
        /// and/or [Biometric.SetMatch](#biometric.setmatch) command. Omitted if the device does not support matching.
        /// </summary>
        public MatchModesEnum MatchSupported { get; init; }

        /// <summary>
        /// Specifies the scan modes that can be used through the [Biometric.Read](#biometric.read).
        /// </summary>
        public ScanModesEnum ScanModes { get; init; }

        /// <summary>
        /// Specifies the type of match operations. A value of none indicates that matching is not supported.
        /// </summary>
        public CompareModesEnum CompareModes { get; init; }

        /// <summary>
        /// Specifies the type of data that can be cleared from storage using the [Biometric.Clear](#biometric.clear) 
        /// or [Biometric.Reset](#biometric.reset) command.
        /// </summary>
        public ClearModesEnum ClearData { get; init; }
    }
}
