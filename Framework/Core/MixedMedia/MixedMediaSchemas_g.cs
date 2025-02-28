/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT MixedMedia interface.
 * MixedMediaSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.MixedMedia
{

    [DataContract]
    public sealed class ModesClass
    {
        public ModesClass(bool? CashAccept = null, bool? CheckAccept = null)
        {
            this.CashAccept = CashAccept;
            this.CheckAccept = CheckAccept;
        }

        /// <summary>
        /// Specifies whether transactions can accept cash. This property may be null if no change is required, or if its state has not changed in
        /// [Common.StatusChangedEvent](#common.statuschangedevent).
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "cashAccept")]
        public bool? CashAccept { get; init; }

        /// <summary>
        /// Specifies whether transactions can accept checks. This property may be null if no change is required, or if its state has not changed in
        /// [Common.StatusChangedEvent](#common.statuschangedevent).
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "checkAccept")]
        public bool? CheckAccept { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(ModesClass Modes = null)
        {
            this.Modes = Modes;
        }

        /// <summary>
        /// Specifies the state of the transaction modes supported by the Service.
        /// </summary>
        [DataMember(Name = "modes")]
        public ModesClass Modes { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(ModesClass Modes = null, bool? Dynamic = null)
        {
            this.Modes = Modes;
            this.Dynamic = Dynamic;
        }

        /// <summary>
        /// Specifies the transaction modes supported by the Service.
        /// </summary>
        [DataMember(Name = "modes")]
        public ModesClass Modes { get; init; }

        /// <summary>
        /// Specifies whether the mode can be modified during a transaction.
        /// <example>false</example>
        /// </summary>
        [DataMember(Name = "dynamic")]
        public bool? Dynamic { get; init; }

    }


}
