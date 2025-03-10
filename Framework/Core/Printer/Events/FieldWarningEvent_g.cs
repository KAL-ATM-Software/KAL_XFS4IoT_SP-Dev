/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * FieldWarningEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Printer.FieldWarningEvent")]
    public sealed class FieldWarningEvent : Event<FieldWarningEvent.PayloadData>
    {

        public FieldWarningEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string FormName = null, string FieldName = null, FailureEnum? Failure = null)
                : base()
            {
                this.FormName = FormName;
                this.FieldName = FieldName;
                this.Failure = Failure;
            }

            /// <summary>
            /// The form name.
            /// <example>Form1</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// The field name.
            /// <example>Field1</example>
            /// </summary>
            [DataMember(Name = "fieldName")]
            public string FieldName { get; init; }

            public enum FailureEnum
            {
                Required,
                StaticOverwrite,
                Overflow,
                NotFound,
                NotRead,
                NotWrite,
                Hwerror,
                Graphic
            }

            /// <summary>
            /// Specifies the type of failure. This property will be null if the
            /// form field type is not supported with the device, otherwsie one of the following:
            /// 
            /// * ```required``` - The specified field must be supplied by the application.
            /// * ```staticOverwrite``` - The specified field is static and thus cannot be overwritten by the application.
            /// * ```overflow``` - The value supplied for the specified fields is too long.
            /// * ```notFound``` - The specified field does not exist.
            /// * ```notRead``` - The specified field is not an input field.
            /// * ```notWrite``` - An attempt was made to write to an input field.
            /// * ```hwerror``` - The specified field uses special hardware (e.g. OCR, Low/High coercivity, etc.) and an error occurred.
            /// * ```graphic``` - The specified graphic image could not be printed.
            /// </summary>
            [DataMember(Name = "failure")]
            public FailureEnum? Failure { get; init; }

        }

    }
}
