/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PreparePresent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.PreparePresent")]
    public sealed class PreparePresentCompletion : Completion<PreparePresentCompletion.PayloadData>
    {
        public PreparePresentCompletion(int RequestId, PreparePresentCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, CashManagement.OutputPositionEnum? Position = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Position = Position;
            }

            public enum ErrorCodeEnum
            {
                UnsupportedPosition,
                PositionNotEmpty,
                NoItems,
                CashUnitError
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```unsupportedPosition``` - The position specified is not supported or is not a valid position for this command.
            /// * ```positionNotEmpty``` - The input or output position is not empty.
            /// * ```noItems``` - There were no items to present at the specified position.
            /// * ```cashUnitError``` - A cash unit caused a problem. A 
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

        }
    }
}
