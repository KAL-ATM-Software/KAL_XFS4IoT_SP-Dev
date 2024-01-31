/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashIn_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.CashIn")]
    public sealed class CashInCompletion : Completion<CashInCompletion.PayloadData>
    {
        public CashInCompletion(int RequestId, CashInCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, CashManagement.StorageCashCountsClass Items = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Items = Items;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                TooManyItems,
                NoItems,
                ExchangeActive,
                ShutterNotClosed,
                NoCashInActive,
                PositionNotEmpty,
                SafeDoorOpen,
                ForeignItemsDetected,
                ShutterNotOpen
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```cashUnitError``` - A problem occurred with a storage unit. A
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```tooManyItems``` - There were too many items inserted previously. The cash-in stacker is full at
            /// the beginning of this command. This may also be reported where a limit specified by
            /// [CashAcceptor.CashInStart](#cashacceptor.cashinstart) has already been reached at the beginning
            /// of this command.
            /// * ```noItems``` - There were no items to cash-in.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```shutterNotClosed``` - Shutter failed to close. In the case of explicit shutter control the
            /// application should close the shutter first.
            /// * ```noCashInActive``` - There is no cash-in transaction active.
            /// * ```positionNotEmpty``` - The output position is not empty so a cash-in is not possible.
            /// * ```safeDoorOpen``` - The safe door is open. This device requires the safe door to be closed in order
            /// to perform this command. (See [Common.Status](#common.status.completion.properties.auxiliaries.safedoor)) property.
            /// * ```foreignItemsDetected``` - Foreign items have been detected inside the input position.
            /// * ```shutterNotOpen``` - Shutter failed to open.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Items detected during the command. May be null if no items were detected. This information is not
            /// cumulative over multiple *CashIn* commands.
            /// </summary>
            [DataMember(Name = "items")]
            public CashManagement.StorageCashCountsClass Items { get; init; }

        }
    }
}
