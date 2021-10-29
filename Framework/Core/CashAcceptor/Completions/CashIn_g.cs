/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "CashAcceptor.CashIn")]
    public sealed class CashInCompletion : Completion<CashInCompletion.PayloadData>
    {
        public CashInCompletion(int RequestId, CashInCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? Unrecognized = null, Dictionary<string, CashManagement.StorageCashCountClass> Cash = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Unrecognized = Unrecognized;
                this.Cash = Cash;
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
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A problem occurred with a cash unit. A 
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
            /// to perform this command.
            /// * ```foreignItemsDetected``` - Foreign items have been detected inside the input position.
            /// * ```shutterNotOpen``` - Shutter failed to open.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Count of unrecognized items handled by the cash interface
            /// </summary>
            [DataMember(Name = "unrecognized")]
            public int? Unrecognized { get; init; }

            /// <summary>
            /// Counts of cash items broken down by cash item type and classification
            /// </summary>
            [DataMember(Name = "cash")]
            public Dictionary<string, CashManagement.StorageCashCountClass> Cash { get; init; }

        }
    }
}
