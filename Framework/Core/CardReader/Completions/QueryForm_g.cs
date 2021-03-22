/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * QueryForm_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.QueryForm")]
    public sealed class QueryFormCompletion : Completion<QueryFormCompletion.PayloadData>
    {
        public QueryFormCompletion(string RequestId, QueryFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid,
            }

            public enum ActionEnum
            {
                Read,
                Write,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string FormName = null, string FieldSeparatorTrack1 = null, string FieldSeparatorTrack2 = null, string FieldSeparatorTrack3 = null, string FieldSeparatorFrontTrack1 = null, string FieldSeparatorJISITrack1 = null, string FieldSeparatorJISITrack3 = null, ActionEnum? Action = null, string ActionDescription = null, bool? Secure = null, List<string> Track1Fields = null, List<string> Track2Fields = null, List<string> Track3Fields = null, List<string> JisITrack1Fields = null, List<string> JisITrack3Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(QueryFormCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.FormName = FormName;
                this.FieldSeparatorTrack1 = FieldSeparatorTrack1;
                this.FieldSeparatorTrack2 = FieldSeparatorTrack2;
                this.FieldSeparatorTrack3 = FieldSeparatorTrack3;
                this.FieldSeparatorFrontTrack1 = FieldSeparatorFrontTrack1;
                this.FieldSeparatorJISITrack1 = FieldSeparatorJISITrack1;
                this.FieldSeparatorJISITrack3 = FieldSeparatorJISITrack3;
                this.Action = Action;
                this.ActionDescription = ActionDescription;
                this.Secure = Secure;
                this.Track1Fields = Track1Fields;
                this.Track2Fields = Track2Fields;
                this.Track3Fields = Track3Fields;
                this.JisITrack1Fields = JisITrack1Fields;
                this.JisITrack3Fields = JisITrack3Fields;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**formNotFound**
            ////The specified form cannot be found.**formInvalid**
            ////The specified form is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///The form name.
            /// </summary>
            [DataMember(Name = "formName")] 
            public string FormName { get; private set; }
            /// <summary>
            ///The value of the field separator of track 1.
            /// </summary>
            [DataMember(Name = "fieldSeparatorTrack1")] 
            public string FieldSeparatorTrack1 { get; private set; }
            /// <summary>
            ///The value of the field separator of track 2.
            /// </summary>
            [DataMember(Name = "fieldSeparatorTrack2")] 
            public string FieldSeparatorTrack2 { get; private set; }
            /// <summary>
            ///The value of the field separator of track 3.
            /// </summary>
            [DataMember(Name = "fieldSeparatorTrack3")] 
            public string FieldSeparatorTrack3 { get; private set; }
            /// <summary>
            ///The value of the field separator of front track 1.
            /// </summary>
            [DataMember(Name = "fieldSeparatorFrontTrack1")] 
            public string FieldSeparatorFrontTrack1 { get; private set; }
            /// <summary>
            ///The value of the field separator of JIS I track 1.
            /// </summary>
            [DataMember(Name = "fieldSeparatorJISITrack1")] 
            public string FieldSeparatorJISITrack1 { get; private set; }
            /// <summary>
            ///The value of the field separator of JIS I track 3.
            /// </summary>
            [DataMember(Name = "fieldSeparatorJISITrack3")] 
            public string FieldSeparatorJISITrack3 { get; private set; }
            /// <summary>
            ///The form action as one of the following:**read**
            ////The form reads the card.**write**
            ////The form writes the card.
            /// </summary>
            [DataMember(Name = "action")] 
            public ActionEnum? Action { get; private set; }
            /// <summary>
            ///The description of the READ or WRITE action.
            /// </summary>
            [DataMember(Name = "actionDescription")] 
            public string ActionDescription { get; private set; }
            /// <summary>
            ///Whether or not to do a security check.
            /// </summary>
            [DataMember(Name = "secure")] 
            public bool? Secure { get; private set; }
            /// <summary>
            ///The field names for track 1.
            /// </summary>
            [DataMember(Name = "track1Fields")] 
            public List<string> Track1Fields{ get; private set; }
            /// <summary>
            ///The field names for track 2.
            /// </summary>
            [DataMember(Name = "track2Fields")] 
            public List<string> Track2Fields{ get; private set; }
            /// <summary>
            ///The field names for track 3.
            /// </summary>
            [DataMember(Name = "track3Fields")] 
            public List<string> Track3Fields{ get; private set; }
            /// <summary>
            ///The field names for JIS I track 1.
            /// </summary>
            [DataMember(Name = "jisITrack1Fields")] 
            public List<string> JisITrack1Fields{ get; private set; }
            /// <summary>
            ///The field names for JIS I track 3.
            /// </summary>
            [DataMember(Name = "jisITrack3Fields")] 
            public List<string> JisITrack3Fields{ get; private set; }

        }
    }
}
