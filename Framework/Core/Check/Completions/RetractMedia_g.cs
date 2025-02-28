/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * RetractMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Check.RetractMedia")]
    public sealed class RetractMediaCompletion : Completion<RetractMediaCompletion.PayloadData>
    {
        public RetractMediaCompletion(int RequestId, RetractMediaCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, MediaClass Media = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Media = Media;
            }

            public enum ErrorCodeEnum
            {
                NoMediaPresent,
                MediaJammed,
                StackerFull,
                InvalidBin,
                NoBin,
                MediaBinError,
                ShutterFail,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```noMediaPresent``` - No media present on retract. Either there was no media present (in a position to be retracted) when the command was called or the media was removed during the retract.
            /// * ```mediaJammed``` - The media is jammed. Operator intervention is required.
            /// * ```stackerFull``` - The stacker or re-buncher is full.
            /// * ```invalidBin``` - The specified storage unit cannot accept retracted items.
            /// * ```noBin``` - The specified storage unit does not exist.
            /// * ```mediaBinError``` - A storage unit caused a problem. A [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```foreignItemsDetected``` - Foreign items have been detected in the input position.
            /// <example>shutterFail</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class MediaClass
            {
                public MediaClass(string Count = null, string RetractLocation = null)
                {
                    this.Count = Count;
                    this.RetractLocation = RetractLocation;
                }

                /// <summary>
                /// Contains the number of media items retracted as a result of this command. The following values
                /// are possible:
                /// 
                ///   * ```[number]``` - The number of items retracted.
                ///   * ```unknown``` - The number of items is unknown.
                /// <example>1</example>
                /// </summary>
                [DataMember(Name = "count")]
                [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
                public string Count { get; init; }

                /// <summary>
                /// Specifies the location for the retracted media, on input where it is to be retracted to, on output where it
                /// was retracted to. See [retractLocation](#common.capabilities.completion.description.check.retractlocation) to
                /// determine the supported locations. This can take one of the following values:
                /// 
                /// * ```stacker``` - The device stacker.
                /// * ```transport``` - The device transport.
                /// * ```rebuncher``` - The device re-buncher.
                /// * ```[storage unit identifier]``` - A storage unit as specified by
                ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
                /// <example>rebuncher</example>
                /// </summary>
                [DataMember(Name = "retractLocation")]
                [DataTypes(Pattern = @"^stacker$|^transport$|^rebuncher$|^unit[0-9A-Za-z]+$")]
                public string RetractLocation { get; init; }

            }

            /// <summary>
            /// The details of the media retracted. May be null if no media was retracted.
            /// </summary>
            [DataMember(Name = "media")]
            public MediaClass Media { get; init; }

        }
    }
}
