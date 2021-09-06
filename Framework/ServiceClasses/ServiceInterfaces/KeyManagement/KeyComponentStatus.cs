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

namespace XFS4IoTFramework.KeyManagement
{
    /// <summary>
    /// The class keeps current stored secure key components
    /// </summary>
    public sealed class SecureKeyEntryStatusClass
    {
        public enum KeyPartEnum
        {
            First = 1,
            Second = 2,
        }

        public SecureKeyEntryStatusClass()
        {
            this.KeyBuffered = false;
        }

        public void KeyPartLoaded(KeyPartEnum KeyPart, string KeyName)
        {
            keyPartsState[KeyPart].KeyName = KeyName;
            keyPartsState[KeyPart].Stored = true;
        }

        /// <summary>
        /// The device specific class can reset the key component status
        /// </summary>
        public void ResetKeyPart(KeyPartEnum KeyPart)
        {
            keyPartsState[KeyPart].KeyName = string.Empty;
            keyPartsState[KeyPart].Stored = false;
        }

        public Dictionary<KeyPartEnum, KeyComponentStatus> GetKeyStatus() => keyPartsState;

        /// <summary>
        /// Reset secure key component status, the framework clear this state when
        /// 1. ImportKey on assemblying key components completed successfully
        /// 2. Initialization command completed successfully
        /// Otherwise device specific class should clear status whenever the stored key cleared
        /// </summary>
        public void Reset()
        {
            keyPartsState = new() { { KeyPartEnum.First, new() }, { KeyPartEnum.Second, new() } };
            KeyBuffered = false;
        }

        public void ResetSecureKeyBuffered()
        {
            KeyBuffered = false;
        }

        public void SecureKeyBuffered()
        {
            KeyBuffered = true;
        }

        /// <summary>
        /// Secure key entry is completed and key stored in the buffer
        /// </summary>
        public bool KeyBuffered { get; private set; }


        private Dictionary<KeyPartEnum, KeyComponentStatus> keyPartsState = new() { { KeyPartEnum.First, new() }, { KeyPartEnum.Second, new() } };
    }

    /// <summary>
    /// KeyComponentStatus keeps each key component information
    /// </summary>
    public sealed class KeyComponentStatus
    {
        /// <summary>
        /// Keep secure key entry information
        /// </summary>
        public KeyComponentStatus()
        {
            KeyName = string.Empty;
            Stored = false;
        }

        /// <summary>
        /// Temporarily key component data stored
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Secure key entry is in progress or not
        /// </summary>
        public bool Stored { get; set; }
    }
}
