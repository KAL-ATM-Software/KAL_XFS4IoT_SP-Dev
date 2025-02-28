/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XFS4IoTFramework.Common.BiometricStatusClass;

namespace XFS4IoTFramework.Common
{
    public sealed class BiometricStatusClass(
        SubjectStatusEnum Subject = SubjectStatusEnum.NotSupported,
        bool Capture = false,
        BiometricCapabilitiesClass.PersistenceModesEnum DataPersistence = BiometricCapabilitiesClass.PersistenceModesEnum.Clear,
        int? RemainingStorage = null) : StatusBase
    {

        public enum SubjectStatusEnum
        {
            NotSupported, NotPresent, Present, Unknown
        }

        /// <summary>
        /// Specifies the state of the subject to be scanned
        /// </summary>
        public SubjectStatusEnum Subject 
        { 
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SubjectStatusEnum subject = Subject;

        /// <summary>
        /// Indicates whether or not scanned biometric data has been captured and is stored locally
        /// </summary>
        public bool Capture 
        { 
            get { return capture; }
            set
            {
                if (capture != value)
                {
                    capture = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool capture = Capture;

        /// <summary>
        /// Specifies the current data persistence mode
        /// </summary>
        public BiometricCapabilitiesClass.PersistenceModesEnum DataPersistence
        {
            get { return dataPersistence; }
            set
            {
                if (dataPersistence != value)
                {
                    dataPersistence = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private BiometricCapabilitiesClass.PersistenceModesEnum dataPersistence = DataPersistence;

        /// <summary>
        /// Specifies how much of the reserved storage specified by the *templateStorage* capability is remaining for the storage of templates in bytes.
        /// Can be null if not supported.
        /// </summary>
        public int? RemainingStorage
        {
            get { return remainingStorage; }
            set
            {
                if (remainingStorage != value)
                {  
                    remainingStorage = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int? remainingStorage = RemainingStorage;
    }
}
