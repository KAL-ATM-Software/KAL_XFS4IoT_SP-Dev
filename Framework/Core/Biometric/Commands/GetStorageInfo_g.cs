/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * GetStorageInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = GetStorageInfo
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Biometric.GetStorageInfo")]
    public sealed class GetStorageInfoCommand : Command<MessagePayload>
    {
        public GetStorageInfoCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}
