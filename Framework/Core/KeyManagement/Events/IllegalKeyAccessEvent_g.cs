/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * IllegalKeyAccessEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.KeyManagement.Events
{

    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Event(Name = "KeyManagement.IllegalKeyAccessEvent")]
    public sealed class IllegalKeyAccessEvent : UnsolicitedEvent<IllegalKeyAccessEvent.PayloadData>
    {

        public IllegalKeyAccessEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string KeyName = null, ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.KeyName = KeyName;
                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            /// Specifies the name of the key that caused the error.
            /// <example>Key02</example>
            /// </summary>
            [DataMember(Name = "keyName")]
            public string KeyName { get; init; }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                KeyNoValue,
                UseViolation,
                AlgorithmNotSupported,
                DukptOverflow
            }

            /// <summary>
            /// Specifies the type of illegal key access that occurred.
            /// The following values are possible:
            /// 
            /// * ```keyNotFound``` - The specified key was not loaded or attempting to delete a non-existent key.
            /// * ```keyNoValue``` - The specified key is not loaded.
            /// * ```useViolation``` - The specified use is not supported by this key.
            /// * ```algorithmNotSupported``` - The specified algorithm is not supported by this key.
            /// * ```dukptOverflow``` - The DUKPT KSN encryption counter has overflowed to zero. A new IPEK must be
            ///   loaded.
            /// 
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }

    }
}
