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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, PositionEnum? Position = null)
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
            /// "unsupportedPosition": The position specified is not supported or is not a valid position for this command.
            /// 
            /// "positionNotEmpty": The input or output position is not empty.
            /// 
            /// "noItems": There were no items to present at the specified position.
            /// 
            /// "cashUnitError": A cash unit caused a problem. A CashManagement.CashUnitErrorEvent will be posted with the details.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            public enum PositionEnum
            {
                Null,
                OutLeft,
                OutRight,
                OutCenter,
                OutTop,
                OutBottom,
                OutFront,
                OutRear
            }

            /// <summary>
            /// Describes the position where the items are to be moved. Following values are possible:
            /// 
            /// "null": The default configuration information should be used.
            /// 
            /// "outLeft": Move items to the left output position.
            /// 
            /// "outRight": Move items to the right output position.
            /// 
            /// "outCenter": Move items to the center output position.
            /// 
            /// "outTop": Move items to the top output position.
            /// 
            /// "outBottom": Move items to the bottom output position.
            /// 
            /// "outFront": Move items to the front output position.
            /// 
            /// "outRear": Move items to the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }
    }
}
