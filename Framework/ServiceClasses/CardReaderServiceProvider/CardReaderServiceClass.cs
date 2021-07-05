/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass
    {
        public CardReaderServiceClass(IServiceProvider ServiceProvider,
                                      ICommonService CommonService,
                                      ILogger logger)
            : this(ServiceProvider, logger)
        {
            this.CommonService = CommonService.IsNotNull($"Unexpected parameter set in the " + nameof(CardReaderServiceClass));
        }

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set => CommonService.CardReaderCapabilities = value; }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }
    }
}
