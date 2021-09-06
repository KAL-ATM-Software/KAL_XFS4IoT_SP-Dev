/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * SecureKeyEntryHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;

namespace XFS4IoTFramework.Keyboard
{
    public partial class SecureKeyEntryHandler
    {

        private Task<SecureKeyEntryCompletion.PayloadData> HandleSecureKeyEntry(ISecureKeyEntryEvents events, SecureKeyEntryCommand secureKeyEntry, CancellationToken cancel)
        {
            //ToDo: Implement HandleSecureKeyEntry for Keyboard.
            
            #if DEBUG
                throw new NotImplementedException("HandleSecureKeyEntry for Keyboard is not implemented in SecureKeyEntryHandler.cs");
            #else
                #error HandleSecureKeyEntry for Keyboard is not implemented in SecureKeyEntryHandler.cs
            #endif
        }

    }
}
