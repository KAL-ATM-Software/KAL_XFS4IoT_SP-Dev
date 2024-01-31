/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a biometric service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical biometric device, which only implements the Biometric, KeyManagement and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class BiometricServiceProvider : ServiceProvider, IBiometricService, ICommonService, IKeyManagementService
    {
        public BiometricServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Biometric, XFSConstants.ServiceClass.KeyManagement },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            Biometric = new BiometricServiceClass(this, logger);
            KeyManagementService = new KeyManagementServiceClass(this, logger, persistentData);
        }

        private readonly BiometricServiceClass Biometric;
        private readonly CommonServiceClass CommonService;
        private readonly KeyManagementServiceClass KeyManagementService;

        #region Common unsolicited events
        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);


        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Biometric Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Common Service

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Stores Biometric interface capabilites internally
        /// </summary>
        public BiometricCapabilitiesClass BiometricCapabilities { get => CommonService.BiometricCapabilities; set => CommonService.BiometricCapabilities = value; }

        /// <summary>
        /// Biometric Status
        /// </summary>
        public BiometricStatusClass BiometricStatus { get => CommonService.BiometricStatus; set => CommonService.BiometricStatus = value; }

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set => CommonService.KeyManagementCapabilities = value; }

        /// <summary>
        /// KeyManagement Status
        /// </summary>
        public KeyManagementStatusClass KeyManagementStatus { get => CommonService.KeyManagementStatus; set => CommonService.KeyManagementStatus = value; }

        #endregion

        #region Biometric unsolicited events
        public Task SubjectRemovedEvent()
            => Biometric.SubjectRemovedEvent();

        public Task DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum ClearMode)
            => Biometric.DataClearedEvent(new XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData(ClearMode switch
            {
                BiometricCapabilitiesClass.ClearModesEnum.None => null,
                BiometricCapabilitiesClass.ClearModesEnum.ScannedData => XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData.ClearDataEnum.ScannedData,
                BiometricCapabilitiesClass.ClearModesEnum.ImportedData => XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData.ClearDataEnum.ImportedData,
                BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData => XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData.ClearDataEnum.SetMatchedData,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected ClearMode within {nameof(DataClearedEvent)}")
            }));

        public Task OrientationEvent()
            => Biometric.OrientationEvent();
        #endregion

        #region KeyManagement unsolicited events
        public Task IllegalKeyAccessEvent(string KeyName, KeyAccessErrorCodeEnum ErrorCode)
            => KeyManagementService.IllegalKeyAccessEvent(KeyName, ErrorCode);

        public Task CertificateChangeEvent(CertificateChangeEnum CertificateChange)
            => KeyManagementService.CertificateChangeEvent(CertificateChange);
        #endregion

        #region KeyManagement Service
        public int FindKeySlot(string KeyName)
            => KeyManagementService.FindKeySlot(KeyName);

        public List<KeyDetail> GetKeyTable()
            => KeyManagementService.GetKeyTable();

        public KeyDetail GetKeyDetail(string KeyName)
            => KeyManagementService.GetKeyDetail(KeyName);

        public void AddKey(string KeyName, int KeySlot, string KeyUsage, string Algorithm, string ModeOfUse, int KeyLength, KeyDetail.KeyStatusEnum KeyStatus, bool Preloaded = false, string RestrictedKeyUsage = null, string KeyVersionNumber = null, string Exportability = null, List<byte> OptionalKeyBlockHeader = null, int? Generation = null, DateTime? ActivatingDate = null, DateTime? ExpiryDate = null, int? Version = null)
            => KeyManagementService.AddKey(KeyName, KeySlot, KeyUsage, Algorithm, ModeOfUse, KeyLength, KeyStatus, Preloaded, RestrictedKeyUsage, KeyVersionNumber, Exportability, OptionalKeyBlockHeader, Generation, ActivatingDate, ExpiryDate, Version);

        public void DeleteKey(string KeyName)
            => KeyManagementService.DeleteKey(KeyName);

        public void UpdateKeyStatus(string KeyName, KeyDetail.KeyStatusEnum Status)
            => KeyManagementService.UpdateKeyStatus(KeyName, Status);

        public SecureKeyEntryStatusClass GetSecureKeyEntryStatus()
            => KeyManagementService.GetSecureKeyEntryStatus();
        #endregion

    }
}
