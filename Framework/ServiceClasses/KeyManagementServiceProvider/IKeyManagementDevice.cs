/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;

// KAL specific implementation of keymanagement. 
namespace XFS4IoTFramework.KeyManagement
{
    public interface IKeyManagementDevice : IDevice
    {
        /// <summary>
        /// Importing key components temporarily while in secure key loading process 
        /// </summary>
        Task<ImportKeyResult> ImportKeyPart(ImportKeyPartRequest request,
                                            CancellationToken cancellation);


        /// <summary>
        /// Last part of loading key components stored temporarily in secure key buffer into the EPP
        /// </summary>
        Task<ImportKeyResult>  AssemblyKeyParts(AssemblyKeyPartsRequest request,
                                                CancellationToken cancellation);

        /// <summary>
        /// The encryption key passed by the application is loaded in the encryption module. 
        /// For secret keys, the key must be passed encrypted with an accompanying "key encrypting key" or "key block protection key". For public keys, they key is not required to be encrypted but is required to have verification data in order to be loaded.
        /// </summary>
        Task<ImportKeyResult> ImportKey(ImportKeyRequest request,
                                        CancellationToken cancellation);

        /// <summary>
        /// This command can be used to delete a key without authentication.
        /// Where an authenticated delete is required, the StartAuthenticate command should be used.
        /// </summary>
        Task<DeviceResult> DeleteKey(DeleteKeyRequest request,
                                     CancellationToken cancellation);

        /// <summary>
        /// This command returns the Key Check Value (kcv) for the specified key. 
        /// </summary>
        Task<GenerateKCVResult> GenerateKCV(GenerateKCVRequest request,
                                            CancellationToken cancellation);
        /// <summary>
        /// The encryption module must be initialized before any encryption function can be used.Every call to Initialization destroys all application keys that have been loaded or imported.
        /// it does not affect those keys loaded during manufacturing.
        /// Usually this command is called by an operator task and not by the application program.
        /// Public keys imported under the RSA Signature based remote key loading scheme when public key deletion authentication is required will not be affected. 
        /// However, if this command is requested in authenticated mode, public keys that require authentication for deletion will be deleted. 
        /// This includes public keys imported under either the RSA Signature based remote key loading scheme or the TR34 RSA Certificate based remote key loading scheme. 
        /// Initialization also involves loading 'initial' application keys and local vendor dependent keys. 
        /// These can be supplied, for example, by an operator through a keyboard, a local configuration file, 
        /// remote RSA key management or possibly by means of some secure hardware that can be attached to the device. 
        /// The application 'initial' keys would normally get updated by the application during a Importkey command as soon as possible.
        /// Local vendor dependent static keys (e.g. storage, firmware and offset keys) would normally be transparent to the application and by definition cannot be dynamically changed.
        /// Where initial keys are not available immediately when this command is issued (i.e. when operator intervention is required), 
        /// the Service Provider returns accessDenied and the application must await the initializedevent.
        /// During initialization an optional encrypted ID key can be stored in the HW module. 
        /// The ID key and the corresponding encryption key can be passed as parameters.
        /// if not, they are generated automatically by the encryption module.The encrypted ID is returned to the application and serves as authorization for the key import function.The capabilities indicates whether or not the device will support this feature.
        /// This function also resets the hsm terminal data, except session key index and trace number. 
        /// This function resets all certificate data and authentication public/private keys back to their initial states at the time of production (except for those public keys imported under the RSA Signature based remote key loading scheme when public key deletion authentication is required).
        /// Key-pairs created with [KeyManagement.GenerateRSAKeyPair](#keymanagement.generatersakeypair) are deleted.
        /// Any keys installed during production, which have been permanently replaced, will not be reset.
        /// Any Verification certificates that may have been loaded must be reloaded.The Certificate state will remain the same, 
        /// but the LoadCertificate or ReplaceCertificate commands must be called again.When multiple ZKA HSMs are present, 
        /// this command deletes all keys loaded within all ZKA logical HSMs.
        /// </summary>
        Task<InitializationResult> Initialization(InitializationRequest request, 
                                                  CancellationToken cancellation);

        /// <summary>
        /// The encryption key in the secure key buffer or passed by the application is loaded in the encryption module.
        /// The key can be passed in clear text mode or encrypted with an accompanying 'key encryption key'. 
        /// A key can be loaded in multiple unencrypted parts by combining the construct or secureConstruct value with the final usage flags within the use field. 
        /// If the construct flag is used then the application must provide the key data through the value parameter,
        /// If secureConstruct is used then the encryption key part in the secure key buffer previously populated with the 
        /// keyboard.SecureKeyEntry command is used and value is ignored. 
        /// Key parts loaded with the secureConstruct flag can only be stored once as the encryption key in the secure key buffer is no longer available after this command has been executed. 
        /// The construct and secureConstruct construction flags cannot be used in combination. 
        /// </summary>
        Task<DeriveKeyResult> DeriveKey(DeriveKeyRequest request, 
                                        CancellationToken cancellation);

        /// <summary>
        /// Sends a service reset to the Service Provider. 
        /// </summary>
        Task<DeviceResult> ResetDevice(CancellationToken cancellation);

       
        /// <summary>
        /// This command is used to export data elements from the device, which have been signed by an offline Signature Issuer or
        /// a private key within the EPP. 
        /// This command is used when the default keys and Signature Issuer signatures, installed during manufacture, 
        /// are to be used for remote key loading. This command allows the following data items are to be exported:
        /// - The Security Item which uniquely identifies the device. 
        ///   This value may be used to uniquely identify a device and therefore confer trust upon any key or data obtained from this device.
        /// - The RSA public key component of a public/private key pair that exists within the device.
        ///   These public/private key pairs are installed during manufacture.
        ///   Typically, an exported public key is used by the host to encipher the symmetric key.
        /// </summary>
        Task<RSASignedItemResult> ExportEPPId(ExportEPPIdRequest request,
                                              CancellationToken cancellation);

        Task<RSASignedItemResult> ExportRSAPublicKey(ExportRSAPublicKeyRequest request, 
                                                     CancellationToken cancellation);

 
        /// <summary>
        /// This command will generate a new RSA key pair.
        /// The public key generated as a result of this command can subsequently be obtained by calling 
        /// ExportRSAEPPSignedItem.The newly generated key pair can only be used for the use defined in the use flag.
        /// This flag defines the use of the private key; its public key can only be used for the inverse function.
        /// </summary>
        Task<GenerateRSAKeyPairResult> GenerateRSAKeyPair(GenerateRSAKeyPairRequest request,
                                                          CancellationToken cancellation);

        /// <summary>
        /// This command is used to read out the encryptor's certificate, which has been signed by the trusted Certificate Authority and is sent to the host. 
        /// This command only needs to be called once if no new Certificate Authority has taken over.
        /// The output of this command will specify in the PKCS #7 message the resulting Primary or Secondary certificate.
        /// </summary>
        Task<ExportCertificateResult> ExportCertificate(ExportCertificateRequest request,
                                                        CancellationToken cancellation);

        /// <summary>
        /// This command is used to replace the existing primary or secondary Certificate Authority certificate already loaded into the KeyManagement.
        /// This operation must be done by an Initial Certificate Authority or by a Sub-Certificate Authority.
        /// These operations will replace either the primary or secondary Certificate Authority public verification key inside of the KeyManagement.
        /// After this command is complete, the application should send the LoadCertificate and GetCertificate commands to ensure that the new HOST and the encryptor have all the information required to perform the remote key loading process.
        /// </summary>
        Task<ReplaceCertificateResult> ReplaceCertificate(ReplaceCertificateRequest request,
                                                          CancellationToken cancellation);

        /// <summary>
        /// This command is used to start communication with the host, including transferring the host's Key Transport Key, 
        /// replacing the Host certificate, and requesting initialization remotely. 
        /// This output value is returned to the host and is used in the ImportKey and LoadCertificate to verify that the encryptor is talking to the proper host.
        /// The ImportKey command end the key exchange process.
        /// </summary>
        Task<StartKeyExchangeResult> StartKeyExchange(CancellationToken cancellation);


        /// <summary>
        /// This command is used to load a host certificate to make remote key loading possible. 
        /// This command can be used to load a host certificate when there is not already one present in the encryptor as well as replace the existing host certificate with a new host certificate.
        /// The type of certificate (Primary or Secondary) to be loaded will be embedded within the actual certificate structure.
        /// </summary>
        Task<ImportCertificateResult> ImportCertificate(ImportCertificateRequest request,
                                                        CancellationToken cancellation);

        
        /// <summary>
        /// This command is used to retrieve the data that needs to be signed and hence provided to the Authenticate command in order to perform an authenticated action on the device.
        /// If this command returns data to be signed then the Authenticate command must be used to call the command referenced by the payload.
        /// Any attempt to call the referenced command without using the Authenticate command, if authentication is required, 
        /// shall result in AuthRequired. 
        /// </summary>
        Task<StartAuthenticateResult> StartAuthenticate(StartAuthenticateRequest request, 
                                                        CancellationToken cancellation);

    }
}
