/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using XFS4IoT;
using XFS4IoTFramework.Printer;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;
using System.ComponentModel;

namespace XFS4IoTServer
{
    public partial class PrinterServiceClass
    {
        public PrinterServiceClass(IServiceProvider ServiceProvider,
                                   ILogger logger,
                                   IPersistentData PersistentData)
        {

            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(PrinterServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IPrinterDevice>();

            RegisterFactory(ServiceProvider);

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(PrinterServiceClass)}");
            StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(PrinterServiceClass)}");

            this.PersistentData = PersistentData;
            
            // Load forms and medias stored in persistent data
            Forms = PersistentData.Load<Dictionary<string, Form>>(typeof(Form).FullName) ?? [];
            Medias = PersistentData.Load<Dictionary<string, Media>>(typeof(Media).FullName) ?? [];

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Storage service interface
        /// </summary>
        public IStorageService StorageService { get; init; }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterCapabilities");
            CommonService.PrinterCapabilities = Device.PrinterCapabilities;
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterCapabilities=");

            CommonService.PrinterCapabilities.IsNotNull($"The device class set PrinterCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterStatus");
            CommonService.PrinterStatus = Device.PrinterStatus;
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterStatus=");

            CommonService.PrinterStatus.IsNotNull($"The device class set PrinterStatus property to null. The device class must report device status.");
            CommonService.PrinterStatus.PropertyChanged += StatusChangedEventFowarder;
            if (CommonService.PrinterStatus.Paper is not null)
            {
                foreach (var paper in CommonService.PrinterStatus.Paper)
                {
                    paper.Value.PropertyChanged += StatusChangedEventFowarder;
                }
            }
            if (CommonService.PrinterStatus.CustomPaper is not null)
            {
                foreach (var papercustom in CommonService.PrinterStatus.CustomPaper)
                {
                    papercustom.Value.PropertyChanged += StatusChangedEventFowarder;
                }
            }
        }

        /// <summary>
        /// Load form  definition 
        /// </summary>
        public bool SetForm(string definition, Form from)
        {
            from.IsNotNull($"Unexpected form object set. {nameof(SetForm)}");

            if (!Forms.ContainsKey(from.Name))
            {
                Forms.Add(from.Name, from);
            }
            else
            {
                Forms[from.Name] = from;
            }

            return true;
        }

        /// <summary>
        /// Load media definition 
        /// </summary>
        public bool SetMedia(string definition, Media media)
        {
            media.IsNotNull($"Unexpected media object set. {nameof(SetMedia)}");

            if (!Medias.ContainsKey(media.Name))
            {
                Medias.Add(media.Name, media);
            }
            else
            {
                Medias[media.Name] = media;
            }

            return true;
        }

        /// <summary>
        /// Return forms loaded
        /// </summary>
        public Dictionary<string, Form> GetForms() => Forms;

        /// <summary>
        /// Return a list of medias loaded
        /// </summary>
        public Dictionary<string, Media> GetMedias() => Medias;

        /// <summary>
        /// The method can convert print job to a single image that can be sent to the printer.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="bitCount">Bits per pixel in returned data</param>
        /// <param name="UpsideDown"></param>
        /// <param name="imageInfo">Information image created</param>
        [SupportedOSPlatform("windows")]
        public bool PrintToBitmap(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo)
        {
            if (ImageConverter is null)
            {
                ImageConverter = new PrintToBitmapHandler(Device, Logger);
            }
            return ImageConverter.IsNotNull($"Failed to create {nameof(PrintToBitmapHandler)} object.").Convert(job, bitCount, UpsideDown, out imageInfo);
        }

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a task object when printed using PrintToImage
        /// </summary>
        /// <param name="task">Task to print data</param>
        /// <param name="width">Width of rectangle needed to contain the task</param>
        /// <param name="height">Height of rectangle needed to contain the task</param>
        [SupportedOSPlatform("windows")]
        public bool GetBitmapPrintDimensions(PrintTask task, out int width, out int height)
        {
            if (ImageConverter is null)
            {
                ImageConverter = new PrintToBitmapHandler(Device, Logger);
            }
            return ImageConverter.IsNotNull($"Failed to create {nameof(PrintToBitmapHandler)} object.").GetTaskDimensions(task, out width, out height);
        }

        /// <summary>
        /// Job containing print tasks that have not been flushed.
        /// The printer service stores all printing in this KXPrintJob until the application requests a flush.
        /// </summary>
        public PrintJobClass PrintJob { get; } = new();

        /// <summary>
        /// The key value pair of form name and form class representing XFS form 
        /// </summary>
        private Dictionary<string, Form> Forms { get; init; }

        /// <summary>
        /// The key value pair of media name and media class representing XFS media 
        /// </summary>
        private Dictionary<string, Media> Medias { get; init; }

        /// <summary>
        /// This class used to convert from XFS form into image
        /// </summary>
        private PrintToBitmapHandler ImageConverter { get; set; }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
