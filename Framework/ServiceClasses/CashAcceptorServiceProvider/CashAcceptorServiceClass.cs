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
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{

    public partial class CashAcceptorServiceClass
    {

        public CashAcceptorServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashAcceptorServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashAcceptorDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CashAcceptorServiceClass)}");
            CashManagement = ServiceProvider.IsA<ICashManagementService>($"Invalid interface parameter specified for cash management service. {nameof(CashAcceptorServiceClass)}");

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// CashManagement service interface
        /// </summary>
        public ICashManagementService CashManagement { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorCapabilities");
            CommonService.CashAcceptorCapabilities = Device.CashAcceptorCapabilities;
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorCapabilities=");

            CommonService.CashAcceptorCapabilities.IsNotNull($"The device class set CashAcceptorCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorStatus");
            CommonService.CashAcceptorStatus = Device.CashAcceptorStatus;
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorStatus=");

            CommonService.CashAcceptorStatus.IsNotNull($"The device class set CashAcceptorStatus property to null. The device class must report device status.");
        }
    }
}