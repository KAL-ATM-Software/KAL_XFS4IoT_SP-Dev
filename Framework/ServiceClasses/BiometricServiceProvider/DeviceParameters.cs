/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.Biometric.Completions;

namespace XFS4IoTFramework.Biometric
{
    // The classes used by the device interface for an Input/Output parameters

    /// <summary>
    /// Biometric Data Type
    /// </summary>
    /// <param name="Format">Specifies the format of the template data. </param>
    /// <param name="Algorithm">Specifies the encryption algorithm. This can be ommited if the data is not encrypted.</param>
    /// <param name="KeyName">Specifies the name of the key that is used to encrypt the biometric data. This can be ommited if the data is not encrypted.</param>
    public sealed record BiometricDataType(
        BiometricCapabilitiesClass.FormatEnum Format, 
        BiometricCapabilitiesClass.AlgorithmEnum? 
        Algorithm = null, 
        string KeyName = null);

    /// <summary>
    /// Biometric Data to import
    /// </summary>
    /// <param name="DataType">Information on the type of data template supplied.</param>
    /// <param name="Data">The byte data representing the template.</param>
    public sealed record BiometricData(
        BiometricDataType DataType,
        List<byte> Data);

    /// <summary>
    /// Parameters to clear the data through Reset or Clear commands.
    /// </summary>
    /// <param name="ClearData">The ClearMode to be used. Can be multiple flags in case no ClearMode was supplied by the client.</param>
    public sealed record ClearDataRequest(BiometricCapabilitiesClass.ClearModesEnum ClearData);

    /// <summary>
    /// Import Data Request
    /// </summary>
    /// <param name="Data">Biometric data to import</param>
    public sealed record ImportRequest(List<BiometricData> Data);

    public sealed class ImportResult : DeviceResult
    {
        public ImportResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            string ErrorDescription = null, 
            ImportCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) 
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Templates = null;
        }

        public ImportResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            Dictionary<string, 
                BiometricDataType> Templates) 
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Templates = Templates;
        }

        public ImportCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, BiometricDataType> Templates { get; init; }
    }

    /// <summary>
    /// ReadRequest
    /// </summary>
    /// <param name="Timeout">The timeout provided by the client, in-case the device requires a timeout to be set when starting a read operation.</param>
    /// <param name="DataTypes">The data types in which the data should be returned upon completion. This can be ommited if no data should be returned.</param>
    /// <param name="NumCaptures">The number of times to attempt capture, if this is ommited or 0 then the device will decide the number of captures.</param>
    /// <param name="ScanMode">The reason for the Read request provided by the client to allow for optimisations within the device. If ommited then this will be <see cref="BiometricCapabilitiesClass.ScanModesEnum.None"/></param>
    public sealed record ReadRequest(
        int Timeout, 
        List<BiometricDataType> DataTypes = null, 
        int NumCaptures = 0, 
        BiometricCapabilitiesClass.ScanModesEnum ScanMode = BiometricCapabilitiesClass.ScanModesEnum.None);

    public sealed class ReadResult : DeviceResult
    {   
        public ReadResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            string ErrorDescription = null, 
            ReadCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) 
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Data = null;
        }

        public ReadResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            List<BiometricData> Templates) 
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Data = Templates;
        }

        public ReadCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }


        public List<BiometricData> Data { get; init; }
    }

    /// <summary>
    /// Parameters for Match or SetMatch commands.
    /// </summary>
    /// <param name="Mode">The mode for the Match operation.</param>
    /// <param name="Threshold">The threshold to match with, between 0 and 100.</param>
    /// <param name="Identifier">The identifier for the template to use. Will be <see langword="null"/> if <paramref name="Mode"/> is <see cref="BiometricCapabilitiesClass.CompareModesEnum.Identity"/></param>
    /// <param name="Maximum">The maximum number of matches to return in the result. Will be <see langword="null"/> if <paramref name="Mode"/> is <see cref="BiometricCapabilitiesClass.CompareModesEnum.Verify"/></param>
    public sealed record MatchRequest(
        BiometricCapabilitiesClass.CompareModesEnum Mode, 
        int Threshold, 
        string Identifier = null, 
        int? Maximum = null);

    public sealed class SetMatchResult : DeviceResult
    {   
        public SetMatchResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            string ErrorDescription = null, 
            SetMatchCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) 
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public SetMatchResult(MessageHeader.CompletionCodeEnum CompletionCode) 
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
        }

        public SetMatchCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    /// <summary>
    /// A candidate returned by a Match operation.
    /// </summary>
    /// <param name="ConfidenceLevel">
    /// The confidence level for the match between 0-100. Where 0 is no match and 100 is an exact match.
    /// This should be no less than the <see cref="MatchRequest.Threshold"/> property.</param>
    /// <param name="Data">Contains the biometric template data which was matched. This can be omitted if no addional comparison data is returned.</param>
    public sealed record MatchCandidate(int ConfidenceLevel, List<byte> Data);

    public sealed class MatchResult : DeviceResult
    {   
        public MatchResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            string ErrorDescription = null, 
            MatchCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) 
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Candidates = null;
        }

        public MatchResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            Dictionary<string, MatchCandidate> Candidates) 
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Candidates = Candidates;
        }

        public MatchCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, MatchCandidate> Candidates { get; init; }
    }
}
