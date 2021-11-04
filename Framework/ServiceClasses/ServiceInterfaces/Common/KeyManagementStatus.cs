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
    public sealed class KeyManagementStatusClass
    {
        public enum EncryptionStateEnum
        {
            Ready,
            NotReady,
            NotInitialized,
            Busy,
            Undefined,
            Initialized
        }

        public enum CertificateStateEnum
        {
            NotSupported,
            Unknown,
            Primary,
            Secondary,
            NotReady
        }

        public KeyManagementStatusClass(EncryptionStateEnum EncryptionState,
                                        CertificateStateEnum CertificateState)
        {
            this.EncryptionState = EncryptionState;
            this.CertificateState = CertificateState;
        }

        /// <summary>
        /// Specifies the state of the encryption module.
        /// </summary>
        public EncryptionStateEnum EncryptionState { get; set; }

        /// <summary>
        /// Specifies the state of the public verification or encryption key in the PIN certificate modules.
        /// </summary>
        public CertificateStateEnum CertificateState { get; set; }
    }
}
