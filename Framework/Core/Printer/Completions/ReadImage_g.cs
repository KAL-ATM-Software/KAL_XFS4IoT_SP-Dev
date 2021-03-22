/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadImage_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.ReadImage")]
    public sealed class ReadImageCompletion : Completion<ReadImageCompletion.PayloadData>
    {
        public ReadImageCompletion(string RequestId, ReadImageCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                ShutterFail,
                MediaJammed,
                FileIOError,
                LampInoperative,
                MediaSize,
                MediaRejected,
            }

            /// <summary>
            ///The status and data for each of the requested images.
            /// </summary>
            public class ImagesClass
            {
                
                /// <summary>
                ///The front image status and data.
                /// </summary>
                public class FrontClass 
                {
                    public enum StatusEnum
                    {
                        Ok,
                        NotSupp,
                        Missing,
                    }
                    [DataMember(Name = "status")] 
                    public StatusEnum? Status { get; private set; }
                    [DataMember(Name = "data")] 
                    public string Data { get; private set; }

                    public FrontClass (StatusEnum? Status, string Data)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }
                }
                [DataMember(Name = "front")] 
                public FrontClass Front { get; private set; }
                
                /// <summary>
                ///The back image status and data.
                /// </summary>
                public class BackClass 
                {
                    public enum StatusEnum
                    {
                        Ok,
                        NotSupp,
                        Missing,
                    }
                    [DataMember(Name = "status")] 
                    public StatusEnum? Status { get; private set; }
                    [DataMember(Name = "data")] 
                    public string Data { get; private set; }

                    public BackClass (StatusEnum? Status, string Data)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }
                }
                [DataMember(Name = "back")] 
                public BackClass Back { get; private set; }
                
                /// <summary>
                ///The codeline status and data.
                /// </summary>
                public class CodelineClass 
                {
                    public enum StatusEnum
                    {
                        Ok,
                        NotSupp,
                        Missing,
                    }
                    [DataMember(Name = "status")] 
                    public StatusEnum? Status { get; private set; }
                    [DataMember(Name = "data")] 
                    public string Data { get; private set; }

                    public CodelineClass (StatusEnum? Status, string Data)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }
                }
                [DataMember(Name = "codeline")] 
                public CodelineClass Codeline { get; private set; }

                public ImagesClass (FrontClass Front, BackClass Back, CodelineClass Codeline)
                {
                    this.Front = Front;
                    this.Back = Back;
                    this.Codeline = Codeline;
                }


            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, ImagesClass Images = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ReadImageCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.Images = Images;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**shutterFail**
            ////  Open or close of the shutter failed due to manipulation or hardware error.**mediaJammed**
            ////  The media is jammed; operator intervention is required.**fileIOError**
            ////  Directory does not exist or a File IO error occurred while storing the image to the hard disk.**lampInoperative**
            ////  Imaging lamp is inoperative.**mediaSize**
            ////  The media entered has an incorrect size and the media remains inside the device.**mediaRejected**
            ////  The media was rejected during the insertion phase. The  [Printer.MediaRejectedEvent](#message-Printer.MediaRejectedEvent) event is posted with the details.  The device is still operational.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///The status and data for each of the requested images.
            /// </summary>
            [DataMember(Name = "images")] 
            public ImagesClass Images { get; private set; }

        }
    }
}
