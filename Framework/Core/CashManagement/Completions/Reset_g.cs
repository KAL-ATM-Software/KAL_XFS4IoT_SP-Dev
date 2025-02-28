/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashManagement.Reset")]
    public sealed class ResetCompletion : Completion<ResetCompletion.PayloadData>
    {
        public ResetCompletion(int RequestId, ResetCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, StorageCashInClass> Storage = null, StorageCashCountsClass Transport = null, StorageCashCountsClass Stacker = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Storage = Storage;
                this.Transport = Transport;
                this.Stacker = Stacker;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                UnsupportedPosition,
                InvalidCashUnit,
                InvalidRetractPosition,
                NotRetractArea,
                PositionNotEmpty,
                ForeignItemsDetected,
                IncompleteRetract
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```cashUnitError``` - There is a problem with a storage unit. A
            ///   [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// * ```unsupportedPosition``` - The output position specified is not supported.
            /// * ```invalidCashUnit``` - The storage unit number specified is not valid.
            /// * ```invalidRetractPosition``` - The *index* is not supported.
            /// * ```notRetractArea``` - The retract area specified in *retractArea* is not supported.
            /// * ```positionNotEmpty``` - The retract area specified in *retractArea* is not empty so the moving of
            ///   items was not possible.
            /// * ```foreignItemsDetected``` - Foreign items have been detected in the input position.
            /// * ```incompleteRetract``` - Some or all of the items were not retracted for a reason not covered by
            ///   other error codes. The detail will be reported with a
            ///   [CashManagement.IncompleteRetractEvent](#cashmanagement.incompleteretractevent).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Object containing the storage units which have had items inserted during the associated operation or
            /// transaction. Only storage units whose contents have been modified are included.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, StorageCashInClass> Storage { get; init; }

            /// <summary>
            /// List of items moved to transport by this transaction or command.
            /// </summary>
            [DataMember(Name = "transport")]
            public StorageCashCountsClass Transport { get; init; }

            /// <summary>
            /// List of items moved to stacker by this transaction or command.
            /// </summary>
            [DataMember(Name = "stacker")]
            public StorageCashCountsClass Stacker { get; init; }

        }
    }
}
