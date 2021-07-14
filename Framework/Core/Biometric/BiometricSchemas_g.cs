/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * BiometricSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Biometric
{

    [DataContract]
    public sealed class PersistanceModeClass
    {
        public PersistanceModeClass(bool? Persist = null, bool? Clear = null)
        {
            this.Persist = Persist;
            this.Clear = Clear;
        }

        /// <summary>
        /// Biometric data captured using the [Biometric.Read](#biometric.read) command can persist until all 
        /// sessions are closed, the device is power failed or rebooted, or the [Biometric.Read](#biometric.read) command 
        /// is requested again. This captured biometric data can also be explicitly cleared using the 
        /// [Biometric.Clear](#biometric.clear) or [Biometric.Reset](#biometric.reset) commands.
        /// </summary>
        [DataMember(Name = "persist")]
        public bool? Persist { get; init; }

        /// <summary>
        /// Captured biometric data will not persist. Once the data has been either returned in the [Biometric.Read](#biometric.read) 
        /// command or used by the [Biometric.Match](#biometric.match) command, then the data is cleared from the device.
        /// </summary>
        [DataMember(Name = "clear")]
        public bool? Clear { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(SubjectEnum? Subject = null, bool? Capture = null, PersistanceModeClass DataPersistence = null, string RemainingStorage = null)
        {
            this.Subject = Subject;
            this.Capture = Capture;
            this.DataPersistence = DataPersistence;
            this.RemainingStorage = RemainingStorage;
        }

        public enum SubjectEnum
        {
            Present,
            NotPresent,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the subject to be scanned (e.g. finger, palm, retina, etc) as one of the following values:
        /// 
        /// * ```present```\t- The subject to be scanned is on the scanning position. 
        /// * ```notPresent``` - The subject to be scanned is not on the scanning position.
        /// * ```subjectUnknown``` - The subject to be scanned cannot be determined with the device in its 
        ///                          current state (e.g. the value of *device* is noDevice, powerOff, offline, or hwError). 
        /// * ```subjectNotSupported``` - The physical device does not support the ability to report whether or not a subject is on the scanning position.
        /// </summary>
        [DataMember(Name = "subject")]
        public SubjectEnum? Subject { get; init; }

        /// <summary>
        /// Indicates whether or not scanned biometric data has been captured using the [Biometric.Read](#biometric.read) command 
        /// and is currently stored and ready for comparison. true if data has been captured and is stored, false if no scanned data is present.
        /// This will be set to false when scanned data is cleared using the [Biometric.Clear](#biometric.clear) command.
        /// </summary>
        [DataMember(Name = "capture")]
        public bool? Capture { get; init; }

        /// <summary>
        /// Specifies the current data persistence mode. The data persistence mode controls how biometric data that has been captured using the 
        /// [Biometric.Read](#biometric.read) command will be handled.
        /// </summary>
        [DataMember(Name = "dataPersistence")]
        public PersistanceModeClass DataPersistence { get; init; }

        /// <summary>
        /// Specifies how much of the reserved storage specified by the *templateStorage* capability is remaining for the storage of templates in bytes. 
        /// This will be zero if not reported.
        /// </summary>
        [DataMember(Name = "remainingStorage")]
        public string RemainingStorage { get; init; }

    }


    [DataContract]
    public sealed class DataFormatsClass
    {
        public DataFormatsClass(bool? IsoFid = null, bool? IsoFmd = null, bool? AnsiFid = null, bool? AnsiFmd = null, bool? Qso = null, bool? Wso = null, bool? ReservedRaw1 = null, bool? ReservedTemplate1 = null, bool? ReservedRaw2 = null, bool? ReservedTemplate2 = null, bool? ReservedRaw3 = null, bool? ReservedTemplate3 = null)
        {
            this.IsoFid = IsoFid;
            this.IsoFmd = IsoFmd;
            this.AnsiFid = AnsiFid;
            this.AnsiFmd = AnsiFmd;
            this.Qso = Qso;
            this.Wso = Wso;
            this.ReservedRaw1 = ReservedRaw1;
            this.ReservedTemplate1 = ReservedTemplate1;
            this.ReservedRaw2 = ReservedRaw2;
            this.ReservedTemplate2 = ReservedTemplate2;
            this.ReservedRaw3 = ReservedRaw3;
            this.ReservedTemplate3 = ReservedTemplate3;
        }

        /// <summary>
        /// Raw ISO FID format
        /// </summary>
        [DataMember(Name = "isoFid")]
        public bool? IsoFid { get; init; }

        /// <summary>
        /// ISO FMD template format
        /// </summary>
        [DataMember(Name = "isoFmd")]
        public bool? IsoFmd { get; init; }

        /// <summary>
        /// Raw ANSI FID format
        /// </summary>
        [DataMember(Name = "ansiFid")]
        public bool? AnsiFid { get; init; }

        /// <summary>
        /// ANSI FMD template format 
        /// </summary>
        [DataMember(Name = "ansiFmd")]
        public bool? AnsiFmd { get; init; }

        /// <summary>
        /// Raw QSO image format
        /// </summary>
        [DataMember(Name = "qso")]
        public bool? Qso { get; init; }

        /// <summary>
        /// WSQ image format
        /// </summary>
        [DataMember(Name = "wso")]
        public bool? Wso { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined Raw format.
        /// </summary>
        [DataMember(Name = "reservedRaw1")]
        public bool? ReservedRaw1 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined Template format.
        /// </summary>
        [DataMember(Name = "reservedTemplate1")]
        public bool? ReservedTemplate1 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined Raw format.
        /// </summary>
        [DataMember(Name = "reservedRaw2")]
        public bool? ReservedRaw2 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined Template format.
        /// </summary>
        [DataMember(Name = "reservedTemplate2")]
        public bool? ReservedTemplate2 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined Raw format.
        /// </summary>
        [DataMember(Name = "reservedRaw3")]
        public bool? ReservedRaw3 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined Template format.
        /// </summary>
        [DataMember(Name = "reservedTemplate3")]
        public bool? ReservedTemplate3 { get; init; }

    }


    [DataContract]
    public sealed class EncryptionAlgorithmClass
    {
        public EncryptionAlgorithmClass(bool? Ecb = null, bool? Cbc = null, bool? Cfb = null, bool? Rsa = null)
        {
            this.Ecb = Ecb;
            this.Cbc = Cbc;
            this.Cfb = Cfb;
            this.Rsa = Rsa;
        }

        /// <summary>
        /// Triple DES with Electronic Code Book.
        /// </summary>
        [DataMember(Name = "ecb")]
        public bool? Ecb { get; init; }

        /// <summary>
        /// Triple DES with Cipher Block Chaining
        /// </summary>
        [DataMember(Name = "cbc")]
        public bool? Cbc { get; init; }

        /// <summary>
        /// Triple DES with Cipher Feed Back.
        /// </summary>
        [DataMember(Name = "cfb")]
        public bool? Cfb { get; init; }

        /// <summary>
        /// RSA Encryption.
        /// </summary>
        [DataMember(Name = "rsa")]
        public bool? Rsa { get; init; }

    }


    [DataContract]
    public sealed class ScanModesClass
    {
        public ScanModesClass(bool? Scan = null, bool? Match = null)
        {
            this.Scan = Scan;
            this.Match = Match;
        }

        /// <summary>
        /// The [Biometric.Read](#biometric.read) command can be used to scan data only, for example to enroll a 
        /// user or collect data for matching in an external biometric system.
        /// </summary>
        [DataMember(Name = "scan")]
        public bool? Scan { get; init; }

        /// <summary>
        /// The [Biometric.Read](#biometric.read) command can be used to scan data for a match operation using 
        /// the [Biometric.Match](#biometric.match) command.
        /// </summary>
        [DataMember(Name = "match")]
        public bool? Match { get; init; }

    }


    [DataContract]
    public sealed class CompareModeClass
    {
        public CompareModeClass(bool? Verify = null, bool? Identity = null)
        {
            this.Verify = Verify;
            this.Identity = Identity;
        }

        /// <summary>
        /// The biometric data can be compared as a one to one verification operation.
        /// </summary>
        [DataMember(Name = "verify")]
        public bool? Verify { get; init; }

        /// <summary>
        /// The biometric data can be compared as a one to many identification operation
        /// </summary>
        [DataMember(Name = "identity")]
        public bool? Identity { get; init; }

    }


    [DataContract]
    public sealed class ClearDataClass
    {
        public ClearDataClass(bool? ScannedData = null, bool? ImportedData = null, bool? SetMatchedData = null)
        {
            this.ScannedData = ScannedData;
            this.ImportedData = ImportedData;
            this.SetMatchedData = SetMatchedData;
        }

        /// <summary>
        /// Raw image data that has been scanned using the [Biometric.Read](#biometric.read) command can be cleared
        /// </summary>
        [DataMember(Name = "scannedData")]
        public bool? ScannedData { get; init; }

        /// <summary>
        /// Template data that was imported using the [Biometric.Import](#biometric.import) command can be cleared.
        /// </summary>
        [DataMember(Name = "importedData")]
        public bool? ImportedData { get; init; }

        /// <summary>
        /// Match criteria data that was set using the [Biometric.Match](#biometric.match) command can be cleared.
        /// </summary>
        [DataMember(Name = "setMatchedData")]
        public bool? SetMatchedData { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeClass Type = null, bool? Compound = null, int? MaxCapture = null, string TemplateStorage = null, DataFormatsClass DataFormats = null, EncryptionAlgorithmClass EncryptionalAlgorithm = null, StorageClass Storage = null, PersistanceModeClass PersistenceModes = null, MatchSupportedEnum? MatchSupported = null, ScanModesClass ScanModes = null, CompareModeClass CompareModes = null, ClearDataClass ClearData = null)
        {
            this.Type = Type;
            this.Compound = Compound;
            this.MaxCapture = MaxCapture;
            this.TemplateStorage = TemplateStorage;
            this.DataFormats = DataFormats;
            this.EncryptionalAlgorithm = EncryptionalAlgorithm;
            this.Storage = Storage;
            this.PersistenceModes = PersistenceModes;
            this.MatchSupported = MatchSupported;
            this.ScanModes = ScanModes;
            this.CompareModes = CompareModes;
            this.ClearData = ClearData;
        }

        [DataContract]
        public sealed class TypeClass
        {
            public TypeClass(bool? FacialFeatures = null, bool? Voice = null, bool? Fingerprint = null, bool? FingerVein = null, bool? Iris = null, bool? Retina = null, bool? HandGeometry = null, bool? ThermalFace = null, bool? ThermalHand = null, bool? PalmVein = null, bool? Signature = null)
            {
                this.FacialFeatures = FacialFeatures;
                this.Voice = Voice;
                this.Fingerprint = Fingerprint;
                this.FingerVein = FingerVein;
                this.Iris = Iris;
                this.Retina = Retina;
                this.HandGeometry = HandGeometry;
                this.ThermalFace = ThermalFace;
                this.ThermalHand = ThermalHand;
                this.PalmVein = PalmVein;
                this.Signature = Signature;
            }

            /// <summary>
            /// The biometric device supports facial recognition scanning.
            /// </summary>
            [DataMember(Name = "facialFeatures")]
            public bool? FacialFeatures { get; init; }

            /// <summary>
            /// The biometric device supports voice recognition.
            /// </summary>
            [DataMember(Name = "voice")]
            public bool? Voice { get; init; }

            /// <summary>
            /// The biometric device supports fingerprint scanning.
            /// </summary>
            [DataMember(Name = "fingerprint")]
            public bool? Fingerprint { get; init; }

            /// <summary>
            /// The biometric device supports finger vein scanning.
            /// </summary>
            [DataMember(Name = "fingerVein")]
            public bool? FingerVein { get; init; }

            /// <summary>
            /// The biometric device supports iris scanning.
            /// </summary>
            [DataMember(Name = "iris")]
            public bool? Iris { get; init; }

            /// <summary>
            /// The biometric device supports retina scanning.
            /// </summary>
            [DataMember(Name = "retina")]
            public bool? Retina { get; init; }

            /// <summary>
            /// The biometric device supports hand geometry scanning.
            /// </summary>
            [DataMember(Name = "handGeometry")]
            public bool? HandGeometry { get; init; }

            /// <summary>
            /// The biometric device supports thermal face image scanning.
            /// </summary>
            [DataMember(Name = "thermalFace")]
            public bool? ThermalFace { get; init; }

            /// <summary>
            /// The biometric device supports thermal hand image scanning.
            /// </summary>
            [DataMember(Name = "thermalHand")]
            public bool? ThermalHand { get; init; }

            /// <summary>
            /// The biometric device supports palm vein scanning. 
            /// </summary>
            [DataMember(Name = "palmVein")]
            public bool? PalmVein { get; init; }

            /// <summary>
            /// The biometric device supports signature scanning.
            /// </summary>
            [DataMember(Name = "signature")]
            public bool? Signature { get; init; }

        }

        /// <summary>
        /// Specifies the type of biometric device as a combination.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeClass Type { get; init; }

        /// <summary>
        /// Specifies whether the biometric device is part of a compound device
        /// </summary>
        [DataMember(Name = "compound")]
        public bool? Compound { get; init; }

        /// <summary>
        /// Specifies the maximum number of times that the device can attempt to capture biometric data during a 
        /// [Biometric.Read](#biometric.read) command. If this is zero then the device or service provider determines 
        /// how many captures will be attempted.
        /// </summary>
        [DataMember(Name = "maxCapture")]
        public int? MaxCapture { get; init; }

        /// <summary>
        /// Specifies the storage space that is reserved on the device for the storage of templates in bytes. This will be set to zero if not reported or unknown.
        /// </summary>
        [DataMember(Name = "templateStorage")]
        public string TemplateStorage { get; init; }

        /// <summary>
        /// Specifies the supported biometric raw data and template data formats reported 
        /// </summary>
        [DataMember(Name = "dataFormats")]
        public DataFormatsClass DataFormats { get; init; }

        /// <summary>
        /// Supported encryption algorithms or cryptNone if no encryption algorithms
        /// </summary>
        [DataMember(Name = "encryptionalAlgorithm")]
        public EncryptionAlgorithmClass EncryptionalAlgorithm { get; init; }

        [DataContract]
        public sealed class StorageClass
        {
            public StorageClass(bool? Secure = null, bool? Clear = null)
            {
                this.Secure = Secure;
                this.Clear = Clear;
            }

            /// <summary>
            /// Biometric template data is securely stored as encrypted data.
            /// </summary>
            [DataMember(Name = "secure")]
            public bool? Secure { get; init; }

            /// <summary>
            /// Biometric template data is stored unencrypted in the device.
            /// </summary>
            [DataMember(Name = "clear")]
            public bool? Clear { get; init; }

        }

        /// <summary>
        /// Indicates whether or not biometric template data can be stored securely or none if Biometric template data is not stored in the device
        /// </summary>
        [DataMember(Name = "storage")]
        public StorageClass Storage { get; init; }

        /// <summary>
        /// Specifies which data persistence modes can be set using the [Biometric.SetDataPersistence](#biometric.setdatapersistence) command. 
        /// This applies specifically to the biometric data that has been captured using the [Biometric.Read](#biometric.read) command.
        /// A value of none indicates that persistence is entirely under device control and cannot be set.
        /// </summary>
        [DataMember(Name = "persistenceModes")]
        public PersistanceModeClass PersistenceModes { get; init; }

        public enum MatchSupportedEnum
        {
            None,
            StoredMatch,
            CombinedMatch
        }

        /// <summary>
        /// Specifies if matching is supported using the [Biometric.Match](#biometric.match) 
        /// and/or [Biometric.SetMatch](#biometric.setmatch) command. 
        /// This will be one of the following values:
        /// 
        ///   * ```None``` - The device does not support matching. 
        ///   * ```StoredMatch``` -\tThe device scans biometric data using the [Biometric.Read](#biometric.read) command 
        ///                         and stores it, then the scanned data can be compared with imported biometric data 
        ///                         using the [Biometric.Match](#biometric.match) command 
        ///   * ```CombinedMatch``` -\tThe device scans biometric data and performs a match against imported biometric 
        ///                           data as a single operation. The [Biometric.SetMatch](#biometric.setmatch) command 
        ///                           must be called before the [Biometric.Read](#biometric.read) command in order to set
        ///                           the matching criteria. Then the [Biometric.Match](#biometric.match) command can be 
        ///                           called to return the result 
        /// </summary>
        [DataMember(Name = "matchSupported")]
        public MatchSupportedEnum? MatchSupported { get; init; }

        /// <summary>
        /// Specifies the modes that the [Biometric.Read](#biometric.read) command.
        /// </summary>
        [DataMember(Name = "scanModes")]
        public ScanModesClass ScanModes { get; init; }

        /// <summary>
        /// Specifies the type of match operations. A value of none indicates that matching is not supported
        /// </summary>
        [DataMember(Name = "compareModes")]
        public CompareModeClass CompareModes { get; init; }

        /// <summary>
        /// Specifies the type of data that can be cleared from storage using the [Biometric.Clear](#biometric.clear) 
        /// or [Biometric.Reset](#biometric.reset) command as either none. 
        /// </summary>
        [DataMember(Name = "clearData")]
        public ClearDataClass ClearData { get; init; }

    }


    [DataContract]
    public sealed class TypeClass
    {
        public TypeClass(DataFormatsClass Format = null, EncryptionAlgorithmClass Algorithm = null, string KeyName = null)
        {
            this.Format = Format;
            this.Algorithm = Algorithm;
            this.KeyName = KeyName;
        }

        /// <summary>
        /// Specifies the format of the template data.
        /// </summary>
        [DataMember(Name = "format")]
        public DataFormatsClass Format { get; init; }

        /// <summary>
        /// Specifies the encryption algorithm.
        /// </summary>
        [DataMember(Name = "algorithm")]
        public EncryptionAlgorithmClass Algorithm { get; init; }

        /// <summary>
        /// Specifies the name of the key that is used to encrypt the biometric data. This value is omitted if the biometric data is not encrypted.
        /// The detailed key information is available through the [KeyManagement.GetKeyDetail](#keymanagement.getkeydetail).
        /// </summary>
        [DataMember(Name = "keyName")]
        public string KeyName { get; init; }

    }


    [DataContract]
    public sealed class StorageClass
    {
        public StorageClass(int? Identifier = null, TypeClass Type = null)
        {
            this.Identifier = Identifier;
            this.Type = Type;
        }

        /// <summary>
        /// A unique number which identifies the template.
        /// </summary>
        [DataMember(Name = "identifier")]
        public int? Identifier { get; init; }

        /// <summary>
        /// Specifies the biometric data type of the template data.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeClass Type { get; init; }

    }


    [DataContract]
    public sealed class DataListClass
    {
        public DataListClass(TypeClass Type = null, DataClass Data = null)
        {
            this.Type = Type;
            this.Data = Data;
        }

        /// <summary>
        /// This field is used to indicate the biometric data type of the template data contained in Data.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeClass Type { get; init; }

        [DataContract]
        public sealed class DataClass
        {
            public DataClass(int? Length = null, object Data = null)
            {
                this.Length = Length;
                this.Data = Data;
            }

            /// <summary>
            /// Length of the byte stream. 
            /// </summary>
            [DataMember(Name = "length")]
            public int? Length { get; init; }

            /// <summary>
            /// It contains the individual binary data stream
            /// </summary>
            [DataMember(Name = "data")]
            public object Data { get; init; }

        }

        /// <summary>
        /// It contains the binary data stream.
        /// </summary>
        [DataMember(Name = "data")]
        public DataClass Data { get; init; }

    }


}
