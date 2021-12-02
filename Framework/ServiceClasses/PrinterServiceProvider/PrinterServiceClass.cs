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
using XFS4IoT;
using XFS4IoTFramework.Printer;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class PrinterServiceClass
    {
        public PrinterServiceClass(IServiceProvider ServiceProvider,
                                   ICommonService CommonService,
                                   ILogger logger,
                                   IPersistentData PersistentData)
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(PrinterServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(PrinterServiceClass));

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

        #region Common Service
        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        #endregion

        #region Printer Service
        /// <summary>
        /// Stores Printer interface capabilites internally
        /// </summary>
        public PrinterCapabilitiesClass PrinterCapabilities { get => CommonService.PrinterCapabilities; set => CommonService.PrinterCapabilities = value; }

        /// <summary>
        /// Stores Printer interface status internally
        /// </summary>
        public PrinterStatusClass PrinterStatus { get => CommonService.PrinterStatus; set => CommonService.PrinterStatus = value; }

        #endregion

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "PrinterrDev.PrinterCapabilities");
            PrinterCapabilities = Device.PrinterCapabilities;
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterCapabilities=");

            PrinterCapabilities.IsNotNull($"The device class set PrinterCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterStatus");
            PrinterStatus = Device.PrinterStatus;
            Logger.Log(Constants.DeviceClass, "PrinterDev.PrinterStatus=");

            PrinterStatus.IsNotNull($"The device class set PrinterStatus property to null. The device class must report device status.");
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
        public Dictionary<string, Media> Medias { get; init; }
    }
}
