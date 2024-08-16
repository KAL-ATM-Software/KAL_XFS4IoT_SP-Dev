/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using XFS4IoTServer;
using XFS4IoT.MixedMedia.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.MixedMedia
{
    /// <summary>
    /// SetModeRequest
    /// Request device to change mixed media mode.
    /// </summary>
    public sealed class SetModeRequest(
        Common.MixedMedia.ModeTypeEnum Modes)
    {
        /// <summary>
        /// Specifies the required mixed media modes.
        /// </summary>
        public Common.MixedMedia.ModeTypeEnum Modes { get; init; } = Modes;
    }

    /// <summary>
    /// SetModeResult
    /// Return result of mode changes
    /// </summary>
    public sealed class SetModeResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        SetModeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset operation
        /// </summary>
        public SetModeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }
}
