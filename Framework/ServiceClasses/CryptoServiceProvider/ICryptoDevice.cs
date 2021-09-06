/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of crypto. 
namespace XFS4IoTFramework.Crypto
{
    public interface ICryptoDevice : IDevice
    {

        /// <summary>
        /// This command is used to generate a random number. 
        /// </summary>
        Task<GenerateRandomNumberResult> GenerateRandomNumber(CancellationToken cancellation);

        /// <summary>
        /// The input data is either encrypted or decrypted using the specified or selected encryption mode.
        /// The input datais padded to the necessary length mandated by the encryption algorithm using the padding parameter. 
        /// This inputdata is padded to necessary length mandated by the signature algorithm using padding parameter.
        /// Applications canuse an alternative padding method by pre-formatting the data passed and combining this with the standard padding method.
        /// The start value (or Initialization Vector) can be provided as input data to this command, or it can beimported via TR-31 prior to requesting this command and referenced by name.
        /// The start value and start value keyare both optional parameters.
        /// </summary>
        Task<CryptoDataResult> Crypto(ICryptoDataEvents events, 
                                      CryptoDataRequest request, 
                                      CancellationToken cancellation);

        /// <summary>
        /// This command can be used for asymmetric signature generation.
        /// This input data is padded to necessary length mandated by the signature algorithm using padding parameter.
        /// Applications can use an alternative padding method by pre-formatting the data passed and combining this withthe standard padding method. 
        /// </summary>
        Task<GenerateAuthenticationDataResult> GenerateSignature(IGenerateAuthenticationEvents events,
                                                                 GenerateSignatureRequest request,
                                                                 CancellationToken cancellation);

        /// This command can be used for Message Authentication Code generation (i.e. macing).
        /// The input data ispadded to the necessary length mandated by the encryption algorithm using the padding parameter.
        Task<GenerateAuthenticationDataResult> GenerateMAC(IGenerateAuthenticationEvents events,
                                                           GenerateMACRequest request,
                                                           CancellationToken cancellation);

        /// <summary>
        /// This command can be used for signature verification.
        /// This input data is padded to necessarylength mandated by the signature algorithm using padding parameter.
        /// Applications can use an alternative paddingmethod by pre-formatting the data passed and combining this with the standard padding method.
        /// The start value (orInitialization Vector) can be provided as input data to this command, or it can be imported via TR-31 prior to requesting this command and referenced by name. 
        /// The start value and start value key are both optional parameters.
        /// </summary>
        Task<VerifyAuthenticationDataResult> VerifySignature(IVerifyAuthenticationEvents events, 
                                                             VerifySignatureRequest request, 
                                                             CancellationToken cancellation);

        /// This command can be used for MAC verification.
        /// The input data is padded to the necessary length mandated by the encryption algorithm using the padding parameter.
        Task<VerifyAuthenticationDataResult> VerifyMAC(IVerifyAuthenticationEvents events,
                                                       VerifyMACRequest request,
                                                       CancellationToken cancellation);

        /// <summary>
        /// This command is used to compute a hash code on a stream of data using the specified hash algorithm. 
        /// This command can be used to verify EMV static and dynamic data.
        /// </summary>
        Task<GenerateDigestResult> GenerateDigest(GenerateDigestRequest request, 
                                                  CancellationToken cancellation);

    }
}
