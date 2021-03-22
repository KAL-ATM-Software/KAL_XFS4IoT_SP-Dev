/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ParseData_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.ParseData")]
    public sealed class ParseDataCompletion : Completion<ParseDataCompletion.PayloadData>
    {
        public ParseDataCompletion(string RequestId, ParseDataCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                InvalidData,
                FormNotFound,
                FormInvalid,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string TrackData = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ParseDataCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.TrackData = TrackData;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**invalidData**
            ////The read operation specified by the forms definition could not be completed successfully due to invalid or incomplete track data being passed in. This is returned if none of the tracks in an ‘or’ (|) operation is contained in the *data* array or if any track in an ‘and’ (&) operation is not found in the input. One [CardReader.InvalidTrackDataEvent](#message-CardReader.InvalidTrackDataEvent) event is generated for each specified track which could not be parsed successfully. See the form description for the rules defining how tracks are specified.**formNotFound**
            ////The specified form cannot be found.**formInvalid**
            ////The specified form definition is invalid (e.g. syntax error).
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///The data read successfully from the selected tracks (and value of security module if available).
            /// </summary>
            [DataMember(Name = "trackData")] 
            public string TrackData { get; private set; }

        }
    }
}
