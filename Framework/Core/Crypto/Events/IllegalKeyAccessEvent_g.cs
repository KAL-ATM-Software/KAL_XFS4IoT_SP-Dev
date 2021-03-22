/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * IllegalKeyAccessEvent_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Crypto.Events
{

    [DataContract]
    [Event(Name = "Crypto.IllegalKeyAccessEvent")]
    public sealed class IllegalKeyAccessEvent : Event<IllegalKeyAccessEvent.PayloadData>
    {

        public IllegalKeyAccessEvent(string RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public enum ErrorCodeEnum
            {
                Keynotfound,
                Keynovalue,
                Useviolation,
                Algorithmnotsupp,
            }


            public PayloadData(string KeyName = null, ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.KeyName = KeyName;
                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            ///Specifies the name of the key that caused the error. 
            /// </summary>
            [DataMember(Name = "keyName")] 
            public string KeyName { get; private set; }
            /// <summary>
            ///Specifies the type of illegal key access that occurred
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
        }

    }
}
