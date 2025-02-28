/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public sealed class CreateSignatureCommandEvents(ICreateSignatureEvents events) : CashInCommonCommandEvents(events)
    { }
}
