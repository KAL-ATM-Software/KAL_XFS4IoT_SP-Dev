/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * IBiometricDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

// KAL specific implementation of biometric. 
namespace XFS4IoTFramework.Biometric
{
    public interface IBiometricDevice : IDevice
    {


        /// <summary>
        /// This command enables the device for biometric scanning, then captures and optionally returns biometric data. A [Biometric.PresentSubjectEvent](#biometric.presentsubjectevent) will be sent to notify the client when it is ready to begin scanning and a [Biometric.SubjectDetectedEvent](#biometric.subjectdetectedevent) sent for each scanning attempt. The *numCaptures* input parameter specifies how many captures should be attempted, unless it is zero in which case the device itself will determine this. Once this command has successfully captured biometric raw data it will complete with Success.The [Biometric.Read](#biometric.read) command has two purposes:**Scanning**: The biometric data that is captured into the device can be processed into biometric template data and returned as an output parameter for enrollment or storage elsewhere, e.g. on a server or smart card.**Matching**: The biometric data that is captured into the device can be used for subsequent matching. Once data has been scanned into the device it can be compared to existing biometric templates that have been imported using the [Biometric.Import](#biometric.import) in order to allow verification or identification of an individual. The [matchSupported](#common.capabilities.completion.properties.biometric.matchsupported) capability indicates if the [Biometric.Match](#biometric.match) can be used for matching, otherwise the matching must be done externally, e.g. on a server or smart card. In either case the data that has been scanned into the device will be persistent according to the current persistence mode as reported by the *dataPersistence* status property.
        /// </summary>
        Task<ReadResult> ReadAsync(ReadCommandEvents events, ReadRequest request, CancellationToken cancellation);

        /// <summary>
        /// This command imports a list of biometric template data structures into the device for later comparison with biometric datascanned using the [Biometric.Read](#biometric.read). Normally this data is read from the chip on a customer’s card or provided by the host system. Data that has been imported is available until a [Biometric.Clear](#biometric.clear) is called. If template data has been previously imported using a call to [Biometric.Import](#biometric.import), then it is overwritten. This data is not persistent across power fails.
        /// </summary>
        Task<ImportResult> ImportAsync(ImportRequest request, CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/MatchDescription.md
        /// </summary>
        Task<MatchResult> MatchAsync(MatchRequest request, CancellationToken cancellation);

        /// <summary>
        /// This command is used for devices which need to know the match criteria data for the [Biometric.Match](#biometric.match) before any biometric scanning is performed by the [Biometric.Read](#biometric.read). The [Biometric.Read](#biometric.read) and [Biometric.Match](#biometric.match) should be called after this command. For all other devices [unsupportedCommand](#api.messagetypes.completionmessages.completioncodes) will be returned here.If the capability *[matchSupported](#common.capabilities.completion.properties.biometric.matchsupported)* == combinedMatch then this command is mandatory. If it is not called first, the [Biometric.Match](#biometric.match) will fail with the generic error [sequenceError](#api.messagetypes.completionmessages.completioncodes). The data set using this command is not persistent across power failures.
        /// </summary>
        Task<SetMatchResult> SetMatchAsync(MatchRequest request, CancellationToken cancellation);

        /// <summary>
        /// |-  This command can be used to clear stored data. In the case where there is no stored data to clear this command completes with Success.
        /// </summary>
        Task<DeviceResult> ClearAsync(ClearDataRequest ClearMode, CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used by the client to perform a hardware reset which will attempt to return the biometric device to a known good state.
        /// </summary>
        Task<DeviceResult> ResetDeviceAsync(ClearDataRequest ClearMode, CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the persistence mode. This controls how the biometric data is persisted after a [Biometric.Read](#biometric.read). The data can be persisted for use by subsequent commands, or it can be automatically cleared.
        /// </summary>
        Task<DeviceResult> SetDataPersistenceAsync(BiometricCapabilitiesClass.PersistenceModesEnum Mode, CancellationToken cancellation);

        /// <summary>
        /// BiometricDataTypes stored by the device
        /// </summary>
        public Dictionary<string, BiometricDataType> StorageInfo { get; }

        /// <summary>
        /// Biometric Status
        /// </summary>
        BiometricStatusClass BiometricStatus { get; set; }

        /// <summary>
        /// Biometric Capabilities
        /// </summary>
        BiometricCapabilitiesClass BiometricCapabilities { get; set; }

    }
}
