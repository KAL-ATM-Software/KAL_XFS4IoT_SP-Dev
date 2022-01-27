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
using XFS4IoTFramework.VendorApplication;

namespace XFS4IoTServer
{
    public partial class VendorApplicationServiceClass
    {
        public VendorApplicationServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(VendorApplicationServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IVendorApplicationDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(VendorApplicationServiceClass)}");

            this.CommonService.VendorApplicationStatus = new VendorApplicationStatusClass(VendorApplicationStatusClass.AccessLevelEnum.NotActive);

            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        #region VendorApplication unsolicited events

        /// <summary>
        /// This event is used to indicate that the required interface has changed. 
        /// </summary>
        public Task InterfaceChangedEvent(ActiveInterfaceEnum ActiveInterface)
        {
            return InterfaceChangedEvent(new XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData(
                                            ActiveInterface switch
                                            {
                                                ActiveInterfaceEnum.Consumer => XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData.ActiveInterfaceEnum.Consumer,
                                                _ => XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData.ActiveInterfaceEnum.Operator,
                                            })
                                        );
        }

        #endregion

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "VendorApplicationDev.VendorApplicationCapabilities");
            CommonService.VendorApplicationCapabilities = Device.VendorApplicationCapabilities;
            Logger.Log(Constants.DeviceClass, "VendorApplicationDev.VendorApplicationCapabilities=");

            CommonService.VendorApplicationCapabilities.IsNotNull($"The device class set VendorApplicationCapabilities property to null. The device class must report device capabilities.");
        }
    }
}