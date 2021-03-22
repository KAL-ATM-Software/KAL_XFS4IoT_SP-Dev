using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{
    [DataContract]
    [Event(Name = "Common.ServiceDetailEvent")]
    public sealed class ServiceDetailEvent : Event<ServiceDetailEvent.PayloadData>
    {
        public ServiceDetailEvent(string RequestId, ServiceDetailEvent.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string VendorName, IEnumerable<Service> Services)
            {
                if (string.IsNullOrWhiteSpace(VendorName))
                    this.VendorName = string.Empty;
                else
                    this.VendorName = VendorName;

                Services.IsNotNull($"Expected parameter received in the function{nameof(PayloadData)}. {nameof(Services)}");
                this.Services = Services;
            }

            /// <summary>
            /// Freeform string naming the hardware vendor
            /// </summary>
            [DataMember(Name = "vendorName")]
            public string VendorName { get; private set; }

            [DataContract]
            public sealed class Service
            {
                public Service(string ServiceUri)
                {
                    this.ServiceURI = ServiceUri;
                }

                /// <summary>
                /// The URI which can be used to contact this individual service
                /// </summary>
                /// <example>wss://ATM1:123/xfs4iot/v1.0/CardReader</example>
                [DataMember(Name = "serviceURI", IsRequired = true)]
                public string ServiceURI { get; private set; }
            }

            [DataMember(Name = "services", IsRequired = true)]
            public IEnumerable<Service> Services { get; private set; }
        }
    }
}
