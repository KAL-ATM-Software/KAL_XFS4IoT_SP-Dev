/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(PrinterServiceClass)}");

            this.PersistentData = PersistentData;
            
            // Load forms and medias stored in persistent data
            Forms = PersistentData.Load<Dictionary<string, Form>>(typeof(Form).FullName);
            if (Forms is null)
            {
                Forms = new();
            }
            Medias = PersistentData.Load<Dictionary<string, Media>>(typeof(Media).FullName);
            if (Medias is null)
            {
                Medias = new();
            }

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "PrinterrDev.PrinterCapabilities");
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
        }

        /// <summary>
        /// Load form or media definition 
        /// </summary>
        public bool LoadDefinition(string definition, bool overwrite)
        {
            try
            {
                XFSFormReader formReader = new(definition);
                PrinterDefinitionReader definitionReader = new(Logger, Device);

                while(formReader.ReadNextToken() is not XFSFormReader.TokenType.EOF)
                {
                    switch (formReader.CurrentTokenType)
                    {
                        case XFSFormReader.TokenType.XFSMEDIA:
                            Media media = definitionReader.ReadPrinterMedia(formReader);
                            if (!Medias.ContainsKey(media.Name))
                                Medias.Add(media.Name, media);
                            else if (overwrite)
                                Medias[media.Name] = media;
                            else
                                Logger.Log(Constants.DeviceName, $"Media {media.Name} already exists and overwrite is false. Skipping media..");
                            break;

                        case XFSFormReader.TokenType.XFSFORM:
                            Form form = definitionReader.ReadPrinterForm(formReader);
                            if (!Forms.ContainsKey(form.Name))
                                Forms.Add(form.Name, form);
                            else if (overwrite)
                                Forms[form.Name] = form;
                            else
                                Logger.Log(Constants.DeviceName, $"Form {form.Name} already exists and overwrite is false. Skipping form..");
                            break;

                        default:
                            formReader.FormReaderAssert(false, "Unexpected CurrentTokenType when loading definitions.");
                            break;
                    }
                }
            }
            catch (XFSFormReader.FormParseException parseException)
            {
                Logger.Log(Constants.DeviceName, parseException.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load a single form or media definition 
        /// </summary>
        public bool LoadSingleDefinition(string definition, bool overwrite, out XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum? type, out string name, out string errorMsg)
        {
            type = null;
            name = null;
            errorMsg = null;
            try
            {
                XFSFormReader formReader = new(definition);
                PrinterDefinitionReader definitionReader = new(Logger, Device);

                formReader.FormReaderAssert(formReader.ReadNextToken() is XFSFormReader.TokenType.XFSMEDIA or XFSFormReader.TokenType.XFSFORM, "Expected XFSMEDIA or XFSFORM as first token.");
                
                if (formReader.CurrentTokenType is XFSFormReader.TokenType.XFSMEDIA)
                {
                    type = XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum.Media;
                    Media media = definitionReader.ReadPrinterMedia(formReader);
                    name = media.Name;

                    if (Medias.ContainsKey(media.Name))
                    {
                        if (overwrite)
                            Medias[media.Name] = media;
                        else
                        {
                            type = null;
                            return false;
                        }
                    }
                    else
                        Medias.Add(media.Name, media);
                    PersistentData.Store<Dictionary<string, Media>>(typeof(Media).FullName, Medias);
                }
                else if (formReader.CurrentTokenType is XFSFormReader.TokenType.XFSFORM)
                {
                    type = XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum.Form;
                    Form form = definitionReader.ReadPrinterForm(formReader);
                    name = form.Name;

                    if (Forms.ContainsKey(form.Name))
                    {
                        if (overwrite)
                            Forms[form.Name] = form;
                        else
                        {
                            type = null;
                            return false;
                        }
                    }
                    else
                        Forms.Add(form.Name, form);
                    PersistentData.Store<Dictionary<string, Form>>(typeof(Form).FullName, Forms);
                }
            }
            catch (XFSFormReader.FormParseException parseException)
            {
                Logger.Log(Constants.DeviceClass, parseException.Message);
                errorMsg = parseException.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Return forms loaded
        /// </summary>
        public Dictionary<string, Form> GetForms() => Forms.ToDictionary(x => x.Key, x => x.Value);

        /// <summary>
        /// Return a list of medias loaded
        /// </summary>
        public Dictionary<string, Media> GetMedias() => Medias.ToDictionary(x => x.Key, x => x.Value);

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
    }
}
