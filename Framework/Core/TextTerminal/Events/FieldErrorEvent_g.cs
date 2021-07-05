/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * FieldErrorEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.TextTerminal.Events
{

    [DataContract]
    [Event(Name = "TextTerminal.FieldErrorEvent")]
    public sealed class FieldErrorEvent : UnsolicitedEvent<FieldErrorEvent.PayloadData>
    {

        public FieldErrorEvent(PayloadData Payload)
            : base(Payload)
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
            /// Specifies the form name.
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; private set; }

            /// <summary>
            /// Specifies the field name.
            /// </summary>
            [DataMember(Name = "fieldName")]
            public string FieldName { get; private set; }

            public enum FailureEnum
            {
                Required,
                StaticOvwr,
                Overflow,
                NotFound,
                NotRead,
                NotWrite,
                TypeNotSupported,
                CharSetForm
            }

            /// <summary>
            /// Specifies the type of failure and can be one of the following:
            /// * ```required``` - The specified field must be supplied by the application.
            /// * ```staticOvwr``` - The specified field is static and thus cannot be overwritten by the application.
            /// * ```overflow``` - The value supplied for the specified fields is too long.
            /// * ```notFound``` - The specified field does not exist.
            /// * ```notRead``` - The specified field is not an input field.
            /// * ```notWrite``` - An attempt was made to write to an input field.
            /// * ```typeNotSupported``` - The form field type is not supported with device.
            /// * ```charSetForm``` - Service Provider does not support character set specified in form.
            /// </summary>
            [DataMember(Name = "failure")]
            public FailureEnum? Failure { get; private set; }

        }

    }
}
