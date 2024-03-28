/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTServer;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// Midex media common defintion
    /// </summary>
    public static class MixedMedia
    {
        /// <summary>
        /// Cash - Accept cash.
        /// Check - Accept cheque.
        /// </summary>
        public enum ModeTypeEnum
        {
            None = 0,
            Cash = 1 << 0,
            Check = 1 << 1,
        }
    }

    /// <summary>
    /// Mixed media status class
    /// </summary>
    public sealed class MixedMediaCapabilitiesClass(
        MixedMedia.ModeTypeEnum Modes,
        bool DynamicMode)
    {
        /// <summary>
        /// Specifies the transaction modes supported by the Service.
        /// </summary>
        public MixedMedia.ModeTypeEnum Modes { get; init; } = Modes;

        /// <summary>
        /// Specifies whether the mode can be modified during a transaction.
        /// </summary>
        public bool DynamicMode { get; init; } = DynamicMode;
    }
}