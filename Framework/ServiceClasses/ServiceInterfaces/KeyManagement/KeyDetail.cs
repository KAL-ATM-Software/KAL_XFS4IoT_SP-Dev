/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoT;
using System.Collections;

namespace XFS4IoTFramework.KeyManagement
{
    /// <summary>
    /// KeyDetail contains detailed stored key information
    /// </summary>
    [Serializable()]
    public sealed class KeyDetail
    {
        public enum KeyStatusEnum
        {
            Loaded,
            Construct,
            NotLoaded,
            Unknown,
        }

        public KeyDetail(string KeyName,
                         int KeySlot,
                         string KeyUsage,
                         string Algorithm,
                         string ModeOfUse,
                         int KeyLength,
                         KeyStatusEnum KeyStatus,
                         bool Preloaded,
                         string RestrictedKeyUsage,
                         string KeyVersionNumber,
                         string Exportability,
                         List<byte> OptionalKeyBlockHeader,
                         int? Generation = null,
                         DateTime? ActivatingDate = null,
                         DateTime? ExpiryDate = null,
                         int? Version = null)
        {
            this.KeyName = KeyName;
            this.KeySlot = KeySlot;

            string.IsNullOrEmpty(KeyUsage).IsFalse($"Key usage must be specified.");

            Regex.IsMatch(KeyUsage, regxKeyUsage).IsTrue($"Invalid key usage specified. {KeyUsage}");
            this.KeyUsage = KeyUsage;

            string.IsNullOrEmpty(Algorithm).IsFalse($"Algorithm must be specified.");
            Regex.IsMatch(Algorithm, regxAlgorithm).IsTrue($"Invalid key usage specified. {Algorithm}");
            this.Algorithm = Algorithm;

            string.IsNullOrEmpty(ModeOfUse).IsFalse($"ModeOfUse must be specified.");
            Regex.IsMatch(ModeOfUse, regxModeOfUse).IsTrue($"Invalid key usage specified. {ModeOfUse}");
            this.ModeOfUse = ModeOfUse;

            if (!string.IsNullOrEmpty(RestrictedKeyUsage))
                Regex.IsMatch(RestrictedKeyUsage, regxKeyUsage).IsTrue($"Invalid restricted key usage specified. {RestrictedKeyUsage}");
            this.RestrictedKeyUsage = RestrictedKeyUsage;

            if (!string.IsNullOrEmpty(KeyVersionNumber))
                Regex.IsMatch(KeyVersionNumber, regxKeyVersionNumber).IsTrue($"Invalid version number specified. {KeyVersionNumber}");
            this.KeyVersionNumber = KeyVersionNumber;

            this.KeyStatus = KeyStatus;
            this.Preloaded = Preloaded;

            if (!string.IsNullOrEmpty(Exportability))
                Regex.IsMatch(Exportability, regxExportability).IsTrue($"Invalid key usage specified. {Exportability}");
            this.Exportability = Exportability;

            this.KeyLength = KeyLength;
            this.OptionalKeyBlockHeader = OptionalKeyBlockHeader;

            this.Generation = Generation;
            this.ActivatingDate = ActivatingDate;
            this.ExpiryDate = ExpiryDate;
            this.Version = Version;
        }

        /// <summary>
        /// Key name
        /// </summary>
        public string KeyName { get; init; }

        /// <summary>
        /// Key slot number to store
        /// </summary>
        public int KeySlot { get; init; }
        
        /// <summary>
        /// Specifies the intended function of the key.
        /// </summary>
        public string KeyUsage { get; init; }

        /// <summary>
        /// Specifies supported algorithm
        /// </summary>
        public string Algorithm { get; init; }

        /// <summary>
        /// Specifies supported mode of use
        /// </summary>
        public string ModeOfUse { get; init; }

        /// <summary>
        /// Specifies restricted key usage of the key associated with the keyUsage property specified.
        /// </summary>
        public string RestrictedKeyUsage { get; init; }

        /// <summary>
        /// Specifies a two-digit ASCII character version number, which is optionally used to indicate that contents 
        /// of the key block are a component, or to prevent re-injection of old keys.
        /// See [Reference 35. ANS X9 TR-31 2018] for all possible values.
        /// </summary>
        public string KeyVersionNumber { get; init; }

        /// <summary>
        /// Specifies whether the key may be transferred outside of the cryptographic domain in which the key is found.
        /// See[Reference 35.ANS X9 TR - 31 2018] for all possible values.
        /// </summary>
        public string Exportability { get; init; }

        /// <summary>
        /// Status of the key
        /// </summary>
        public KeyStatusEnum KeyStatus { get; init; }

        /// <summary>
        /// The key is preloaded or not
        /// </summary>
        public bool Preloaded { get; set; }

        /// <summary>
        /// Specifies the length, in bits, of the key. 0 if the key length is unknown.
        /// </summary>
        public int KeyLength { get; init; }

        /// <summary>
        /// Contains any optional header blocks, as defined in [Reference 35. ANS X9 TR-31 2018].
        /// This value can be omitted if there are no optional block headers.
        /// </summary>
        public List<byte> OptionalKeyBlockHeader { get; init; }

        /// <summary>
        /// Specifies the generation of the key.
        /// Different generations might correspond to different environments(e.g.test or production environment).
        /// The content is vendor specific.
        /// </summary>
        public int? Generation { get; init; }

        /// <summary>
        /// Specifies the date when the key is activated in the format YYYYMMDD.
        /// </summary>
        public DateTime? ActivatingDate { get; init; }

        /// <summary>
        /// Specifies the date when the key expires in the format YYYYMMDD.
        /// </summary>
        public DateTime? ExpiryDate { get; init; }

        /// <summary>
        /// Specifies the version of the key (the year in which the key is valid, e.g. 1 for 2001).
        /// This value can be omitted if no such information is available for the key.
        /// </summary>
        public int? Version { get; init; }


        /// <summary>
        /// Constants
        /// </summary>
        public static readonly string regxKeyUsage = "^B[0-2]$|^C0$|^D[0-2]$|^E[0-6]$|^I0$|^K[0-3]$|^M[0-8]$|^P0$|^S[0-2]$|^V[0-4]$|^[0-9][0-9]$";
        public static readonly string regxAlgorithm = "^[0-9ADRT]$";
        public static readonly string regxModeOfUse = "^[0-9BCDEGSTVXY]$";
        public static readonly string regxExportability = "^[0-9ESN]$";
        public static readonly string regxKeyVersionNumber = "^[0-9a-zA-Z][0-9a-zA-Z]$";

        public static readonly string regxVerifyKeyUsage = "^M[0-8]$|^S[0-2]$|^[0-9][0-9]$";
        public static readonly string regxVerifyModeOfUse = "^[0-9SV]$";
    }
}
