/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoT.PinPad.Events;
using XFS4IoT.KeyManagement.Events;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a pinpad service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical pinpad, which implements the PinPad, KeyManagement, Keyboard, Crypto and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class PinPadServiceProvider : ServiceProvider, IPinPadServiceClass, IKeyManagementServiceClass, IKeyboardServiceClass, ICryptoServiceClass, ICommonServiceClass
    {
        public PinPadServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Crypto, XFSConstants.ServiceClass.Keyboard, XFSConstants.ServiceClass.KeyManagement, XFSConstants.ServiceClass.PinPad },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            CryptoService = new CryptoServiceClass(this, logger);
            KeyboardService = new KeyboardServiceClass(this, logger);
            KeyManagementService = new KeyManagementServiceClass(this, logger);
            PinPadService = new PinPadServiceClass(this, logger);
        }

        private readonly PinPadServiceClass PinPadService;
        private readonly KeyManagementServiceClass KeyManagementService;
        private readonly KeyboardServiceClass KeyboardService;
        private readonly CryptoServiceClass CryptoService;
        private readonly CommonServiceClass CommonService;


        #region PinPad unsolicited events
        public Task IllegalKeyAccessEvent(XFS4IoT.PinPad.Events.IllegalKeyAccessEvent.PayloadData Payload) => PinPadService.IllegalKeyAccessEvent(Payload);
        #endregion

        #region KeyManagement unsolicited events
        public Task InitializedEvent(XFS4IoT.KeyManagement.Events.InitializedEvent.PayloadData Payload) => KeyManagementService.InitializedEvent(Payload);

        public Task IllegalKeyAccessEvent(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData Payload) => KeyManagementService.IllegalKeyAccessEvent(Payload);

        public Task CertificateChangeEvent(XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData Payload) => KeyManagementService.CertificateChangeEvent(Payload);
        #endregion

        #region Crypto unsolicited events
        public Task IllegalKeyAccessEvent(XFS4IoT.Crypto.Events.IllegalKeyAccessEvent.PayloadData Payload) => CryptoService.IllegalKeyAccessEvent(Payload);
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => CommonService.NonceClearedEvent(Payload);
        #endregion

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set => CommonService.KeyManagementCapabilities = value; }

        /// <summary>
        /// Find keyslot available or being used
        /// </summary>
        public int FindKeySlot(string KeyName) => KeyManagementService.FindKeySlot(KeyName);

        /// <summary>
        /// Stored key information of this device
        /// </summary>
        public List<KeyDetail> GetKeyTable() => KeyManagementService.GetKeyTable();

        /// <summary>
        /// Return detailed stored key information
        /// </summary>
        public KeyDetail GetKeyDetail(string KeyName) => KeyManagementService.GetKeyDetail(KeyName);

        /// <summary>
        /// Add new key into the collection and return key slot
        /// </summary>
        public void AddKey(string KeyName,
                           int KeySlot,
                           string KeyUsage,
                           string Algorithm,
                           string ModeOfUse,
                           int KeyLength,
                           KeyDetail.KeyStatusEnum KeyStatus,
                           bool Preloaded,
                           string RestrictedKeyUsage,
                           string KeyVersionNumber,
                           string Exportability,
                           List<byte> OptionalKeyBlockHeader,
                           int? Generation,
                           DateTime? ActivatingDate,
                           DateTime? ExpiryDate,
                           int? Version) => throw new NotSupportedException("The AddKey method is not supported in the PinPad ServiceProvider.");

        /// <summary>
        /// Delete specified key from the collection and return key slot
        /// </summary>
        public void DeleteKey(string KeyName) => throw new NotSupportedException("The DeleteKey method is not supported in the PinPad ServiceProvider.");

        /// <summary>
        /// Update key status
        /// </summary>
        public void UpdateKeyStatus(string KeyName, KeyDetail.KeyStatusEnum Status) => throw new NotSupportedException("The UpdateKeyStatus method is not supported in the PinPad ServiceProvider.");

        /// <summary>
        /// Return secure key entry component status
        /// The device specified class reset current status if the stored key components are claered except successful Initialization command.
        /// </summary>
        /// <returns></returns>
        public SecureKeyEntryStatusClass GetSecureKeyEntryStatus() => KeyManagementService.GetSecureKeyEntryStatus();
    }
}
