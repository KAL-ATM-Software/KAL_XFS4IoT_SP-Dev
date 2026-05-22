/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2026
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using XFS4IoT;

namespace XFS4IoTServer
{
    /// <summary>
    /// Loads an X.509 certificate from a PKCS#12 (.pfx) file.
    /// </summary>
    internal static class CertificateLoader
    {
        /// <summary>
        /// Loads the certificate at <paramref name="path"/> with the given <paramref name="password"/>.
        /// Returns null and logs a warning on any failure.
        /// </summary>
        internal static X509Certificate2 Load(string path, string password, ILogger logger)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (!File.Exists(path))
            {
                logger.Warning(Constants.Component, $"TLS certificate file not found: {path}");
                return null;
            }

            try
            {
                // EphemeralKeySet keeps the private key only in process memory.
                var cert = new X509Certificate2(path, password ?? string.Empty,
                    X509KeyStorageFlags.EphemeralKeySet);

                logger.Log(Constants.Component,
                    $"Loaded TLS certificate '{cert.Subject}' with thumbprint '{cert.Thumbprint}'");

                return cert;
            }
            catch (Exception ex)
            {
                logger.Warning(Constants.Component, $"Failed to load TLS certificate from '{path}'. Error message: {ex.Message}");
                return null;
            }
        }
    }
}
