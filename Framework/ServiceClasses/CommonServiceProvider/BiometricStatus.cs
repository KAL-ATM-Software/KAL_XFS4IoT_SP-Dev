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
    public sealed class BiometricStatusClass
    {

        public enum SubjectStatusEnum
        {
            NotSupported, NotPresent, Present, Unknown
        }

        /// <summary>
        /// Biometric Status
        /// </summary>
        /// <param name="Subject">Specifies the state of the subject to be scanned</param>
        /// <param name="Capture">Indicates whether or not scanned biometric data has been captured and is stored locally</param>
        /// <param name="DataPersistence">Specifies the current data persistence mode</param>
        /// <param name="RemainingStorage">Specifies how much of the reserved storage specified by the *templateStorage* capability is remaining for the storage of templates in bytes. Can be null if not supported.</param>
        public BiometricStatusClass(SubjectStatusEnum Subject = SubjectStatusEnum.NotSupported,
                                    bool Capture = false,
                                    BiometricCapabilitiesClass.PersistenceModesEnum DataPersistence = BiometricCapabilitiesClass.PersistenceModesEnum.Clear,
                                    int? RemainingStorage = null)
        {
            this.Subject = Subject;
            this.Capture = Capture;
            this.DataPersistence = DataPersistence;
            this.RemainingStorage = RemainingStorage;
        }

        /// <summary>
        /// Specifies the state of the subject to be scanned
        /// </summary>
        public SubjectStatusEnum Subject { get; set; }

        /// <summary>
        /// Indicates whether or not scanned biometric data has been captured and is stored locally
        /// </summary>
        public bool Capture { get; set; }

        /// <summary>
        /// Specifies the current data persistence mode
        /// </summary>
        public BiometricCapabilitiesClass.PersistenceModesEnum DataPersistence { get; set; }

        /// <summary>
        /// Specifies how much of the reserved storage specified by the *templateStorage* capability is remaining for the storage of templates in bytes.
        /// Can be null if not supported.
        /// </summary>
        public int? RemainingStorage { get; set; }
    }
}
