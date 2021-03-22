/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ReadTrack_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.ReadTrack")]
    public sealed class ReadTrackCompletion : Completion<ReadTrackCompletion.PayloadData>
    {
        public ReadTrackCompletion(string RequestId, ReadTrackCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                MediaJam,
                ShutterFail,
                InvalidData,
                NoMedia,
                InvalidMedia,
                FormNotFound,
                FormInvalid,
                SecurityFail,
                CardTooShort,
                CardTooLong,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string TrackData = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ReadTrackCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.TrackData = TrackData;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**mediaJam**
            ////The card is jammed. Operator intervention is required.**shutterFail**
            ////The open of the shutter failed due to manipulation or hardware error. Operator intervention is  required.**invalidData**
            ////The read operation specified by the forms definition could not be completed successfully due to invalid track data. This is returned if all tracks in an ‘or’ (|) operation cannot be read or if any track in an ‘and’ (&) operation cannot be read. *trackData* contains to data from the successfully read tracks (if any). One [CardReader.InvalidTrackDataEvent](#message-CardReader.InvalidTrackDataEvent) event is generated for each specified track which could not be read successfully. See the form description for the rules defining how tracks are specified.**noMedia**
            ////The card was removed before completion of the read action (the event [CardReader.MediaInsertedEvent](#message-CardReader.MediaInsertedEvent) has been generated). For motor driven devices, the read is disabled; i.e. another command has to be issued to enable the reader for card entry.**invalidMedia**
            ////No track found; card may have been inserted or pulled through the wrong way.**formNotFound**
            ////The specified form cannot be found.**formInvalid**
            ////The specified form definition is invalid (e.g. syntax error).**securityFail**
            ////The security module failed reading the cards security sign.**cardTooShort**
            ////The card that was inserted is too short. When this error occurs the card remains at the exit slot.**cardTooLong**
            ////The card that was inserted is too long. When this error occurs the card remains at the exit slot.
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
