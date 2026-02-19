/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DataEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Keyboard.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Keyboard.DataEntry")]
    public sealed class DataEntryCompletion : Completion<DataEntryCompletion.PayloadData>
    {
        public DataEntryCompletion()
            : base()
        { }

        public DataEntryCompletion(int RequestId, DataEntryCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? Keys = null, List<KeyPressedClass> PinKeys = null, EntryCompletionEnum? Completion = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Keys = Keys;
                this.PinKeys = PinKeys;
                this.Completion = Completion;
            }

            public enum ErrorCodeEnum
            {
                KeyInvalid,
                KeyNotSupported,
                NoActivekeys
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```keyInvalid``` - At least one of the specified function keys or FDKs is invalid.
            /// * ```keyNotSupported``` - At least one of the specified function keys or FDKs is not supported by the
            ///   Service Provider.
            /// * ```noActivekeys``` - There are no active keys specified, or there is no defined layout definition.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Number of keys entered by the user
            /// </summary>
            [DataMember(Name = "keys")]
            [DataTypes(Minimum = 0)]
            public int? Keys { get; init; }

            /// <summary>
            /// Array contains the keys entered by the user
            /// </summary>
            [DataMember(Name = "pinKeys")]
            public List<KeyPressedClass> PinKeys { get; init; }

            [DataMember(Name = "completion")]
            public EntryCompletionEnum? Completion { get; init; }

        }
    }
}
