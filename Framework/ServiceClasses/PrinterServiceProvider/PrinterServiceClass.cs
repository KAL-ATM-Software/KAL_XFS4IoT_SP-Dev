/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
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
            if (ServiceProvider.Device is IStorageDevice)
            {
                StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(PrinterServiceClass)}");
            }

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
        public IStorageService StorageService { get; init; } = null;

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
        /// <param name="FullImage">
        /// False (default) returns a tight crop around the job's tasks with ImageInfo.OffsetX/OffsetY set
        /// for the caller to composite elsewhere. True returns an image covering the entire media instead.
        /// </param>
        public bool PrintToBitmap(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo, bool FullImage = false)
        {
            if (ImageConverter is null)
            {
                ImageConverter = new PrintToBitmapHandler(Device, Logger);
            }

            // The default stack size for a .NET ThreadPool worker thread is 1 MB, which may not be sufficient.
            // Use a larger stack size for the dedicated thread that runs the conversion.
            const int RenderThreadStackSize = 8 * 1024 * 1024;

            bool result = false;
            ImageInfo renderedImageInfo = null;
            Thread renderThread = new(() =>
            {
                try
                {
                    result = ImageConverter.IsNotNull($"Failed to create {nameof(PrintToBitmapHandler)} object.").Convert(job, bitCount, UpsideDown, out renderedImageInfo, FullImage);
                }
                catch (Exception exception)
                {
                    throw new InternalErrorException($"Unexpected exception occurred while rendering print job to bitmap: {exception}");
                }
            }, maxStackSize: RenderThreadStackSize);
            renderThread.Start();
            renderThread.Join();

            imageInfo = renderedImageInfo;
            return result;
        }

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a task object when printed using PrintToImage
        /// </summary>
        /// <param name="task">Task to print data</param>
        /// <param name="width">Width of rectangle needed to contain the task</param>
        /// <param name="height">Height of rectangle needed to contain the task</param>
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
