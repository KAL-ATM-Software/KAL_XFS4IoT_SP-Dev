/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * RetainCard_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.RetainCard")]
    public sealed class RetainCardCompletion : Completion<RetainCardCompletion.PayloadData>
    {
        public RetainCardCompletion(string RequestId, RetainCardCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                MediaJam,
                NoMedia,
                RetainBinFull,
                ShutterFail,
            }

            public enum PositionEnum
            {
                Unknown,
                Present,
                Entering,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? Count = null, PositionEnum? Position = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(RetainCardCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.Count = Count;
                this.Position = Position;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**mediaJam**
            ////The card is jammed. Operator intervention is required.**noMedia**
            ////No card has been inserted. The *position* parameter has the value mediaUnknown.**retainBinFull**
            ////The retain bin is full; no more cards can be retained. The current card is still in the device.**shutterFail**
            ////The open of the shutter failed due to manipulation or hardware error. Operator intervention is required.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///Total number of ID cards retained up to and including this operation, since the last ResetCount command was executed.
            /// </summary>
            [DataMember(Name = "count")] 
            public int? Count { get; private set; }
            /// <summary>
            ///Position of card; only relevant if card could not be retained. Possible positions:**unknown**
            ////The position of the card cannot be determined with the device in its current state.**present**
            ////The card is present in the reader.**entering**
            ////The card is in the entering position (shutter).
            /// </summary>
            [DataMember(Name = "position")] 
            public PositionEnum? Position { get; private set; }

        }
    }
}
