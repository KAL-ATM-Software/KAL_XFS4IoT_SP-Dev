/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * VendorAppExitedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.VendorApplication.Events
{

    [DataContract]
    [Event(Name = "VendorApplication.VendorAppExitedEvent")]
    public sealed class VendorAppExitedEvent : UnsolicitedEvent<MessagePayloadBase>
    {

        public VendorAppExitedEvent()
            : base()
        { }

    }
}
