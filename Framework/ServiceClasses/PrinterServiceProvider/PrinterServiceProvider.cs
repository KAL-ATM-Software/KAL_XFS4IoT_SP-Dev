/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoT.Printer.Events;
using XFS4IoTFramework.Printer;
using XFS4IoTFramework.Common;
using System.ComponentModel;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a printer service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a service provider for the printer device. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class PrinterServiceProvider : ServiceProvider, IPrinterService, ICommonService
    {
        public PrinterServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData) 
            : 
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Printer },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            PrinterService = new PrinterServiceClass(this, logger, persistentData);
        }

        private readonly PrinterServiceClass PrinterService;
        private readonly CommonServiceClass CommonService;

        #region Printer unsolicited events
        public Task MediaTakenEvent() => PrinterService.MediaTakenEvent();

        public Task MediaInsertedUnsolicitedEvent() => PrinterService.MediaInsertedUnsolicitedEvent();

        public Task MediaPresentedUnsolicitedEvent(int WadIndex, int TotalWads) => PrinterService.MediaPresentedUnsolicitedEvent(new MediaPresentedUnsolicitedEvent.PayloadData(WadIndex, TotalWads));

        public Task MediaDetectedEvent(PositionEnum Position, int? RetractBinNumber = null)
        {
            (Position == PositionEnum.Retracted && RetractBinNumber is not null).IsTrue($"No retract bin number specified in MediaDetectedEvent.");
            (Position == PositionEnum.Retracted && RetractBinNumber >= 0 && RetractBinNumber <= 9).IsTrue($"Invalid retract bin number specified in MediaDetectedEvent. Must be 0 to 9.");

            return PrinterService.MediaDetectedEvent(
                new MediaDetectedEvent.PayloadData(Position: Position switch
                {
                    PositionEnum.Entering => "entering",
                    PositionEnum.Expelled => "expelled",
                    PositionEnum.Jammed => "jammed",
                    PositionEnum.Present => "present",
                    PositionEnum.Unknown => "unknown",
                    _ => "unit" + RetractBinNumber,
                }));
        }

        public Task TonerThresholdEvent(CommonThresholdStatusEnum Status) => PrinterService.TonerThresholdEvent(
            new TonerThresholdEvent.PayloadData(Status switch
            {
                CommonThresholdStatusEnum.Full => XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData.StateEnum.Full,
                CommonThresholdStatusEnum.Low => XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData.StateEnum.Low,
                _ => XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData.StateEnum.Out,
            }));

        public Task LampThresholdEvent(LampThresholdEnum LampThreshold) => PrinterService.LampThresholdEvent(
            new LampThresholdEvent.PayloadData(LampThreshold switch
            {
                LampThresholdEnum.Fading => XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData.StateEnum.Fading,
                LampThresholdEnum.Inop => XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData.StateEnum.Inop,
                _ => XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData.StateEnum.Ok,
            }));

        public Task InkThresholdEvent(CommonThresholdStatusEnum Status) => PrinterService.InkThresholdEvent(
            new InkThresholdEvent.PayloadData(Status switch
            {
                CommonThresholdStatusEnum.Full => XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData.StateEnum.Full,
                CommonThresholdStatusEnum.Low => XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData.StateEnum.Low,
                _ => XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData.StateEnum.Out,
            }));

        public Task RetractBinThresholdEvent(int BinNumber, RetractThresholdStatusEnum Status) => PrinterService.RetractBinThresholdEvent(
            new RetractBinThresholdEvent.PayloadData(BinNumber, 
                                                     Status switch
                                                     {
                                                         RetractThresholdStatusEnum.Full => XFS4IoT.Printer.Events.RetractBinThresholdEvent.PayloadData.StateEnum.Full,
                                                         RetractThresholdStatusEnum.High => XFS4IoT.Printer.Events.RetractBinThresholdEvent.PayloadData.StateEnum.High,
                                                         _ => XFS4IoT.Printer.Events.RetractBinThresholdEvent.PayloadData.StateEnum.Ok,
                                                     }));

        public Task MediaAutoRetractedEvent(int BinNumber, AutoRetractResultEnum AutoRetractResult)
        {
            (AutoRetractResult == AutoRetractResultEnum.Retracted && BinNumber >= 0 && BinNumber <= 9).IsTrue($"Invalid retract bin number specified in MediaAutoRetractedEvent. Must be 0 to 9.");

            string position = AutoRetractResult switch
            {
                AutoRetractResultEnum.Transport => "transport",
                AutoRetractResultEnum.Jammed => "jammed",
                _ => "unit" + BinNumber,
            };
            return PrinterService.MediaAutoRetractedEvent(new MediaAutoRetractedEvent.PayloadData(position));
        }

        public Task RetractBinStatusEvent(int BinNumber, PhysicalBinStatusEnum State) => PrinterService.RetractBinStatusEvent(
            new RetractBinStatusEvent.PayloadData(BinNumber, 
                                                  State switch
                                                  {
                                                      PhysicalBinStatusEnum.Inserted => XFS4IoT.Printer.Events.RetractBinStatusEvent.PayloadData.StateEnum.Inserted,
                                                      _ => XFS4IoT.Printer.Events.RetractBinStatusEvent.PayloadData.StateEnum.Removed,
                                                  })
            );

        public Task PaperThresholdEvent(CommonThresholdStatusEnum Status, PaperSourceEnum? PaperSource, string CustomSource = null)
        {
            string paperSource = PaperSource switch
            {
                PaperSourceEnum.AUX => "aux",
                PaperSourceEnum.AUX2 => "aux2",
                PaperSourceEnum.External => "external",
                PaperSourceEnum.Lower => "lower",
                PaperSourceEnum.Park => "park",
                PaperSourceEnum.Upper => "upper",
                _ => null
            };
            if (!string.IsNullOrEmpty(CustomSource))
            {
                paperSource = CustomSource;
            }

            return PrinterService.PaperThresholdEvent(new PaperThresholdEvent.PayloadData(paperSource,
                                                                                          Status switch
                                                                                          {
                                                                                              CommonThresholdStatusEnum.Full => XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData.ThresholdEnum.Full,
                                                                                              CommonThresholdStatusEnum.Low => XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData.ThresholdEnum.Low,
                                                                                              _ => XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData.ThresholdEnum.Out,
                                                                                          }));
        }
        #endregion


        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Crypto Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Common Service
        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

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
        /// Load form or media definition 
        /// </summary>
        public bool LoadDefinition(string definition, bool overwrite) => PrinterService.LoadDefinition(definition, overwrite);

        /// <summary>
        /// Load a single form or media definition 
        /// </summary>
        public bool LoadSingleDefinition(string definition, bool overwrite, out XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum? type, out string name, out string errorMsg)
            => PrinterService.LoadSingleDefinition(definition, overwrite, out type, out name, out errorMsg);

        /// <summary>
        /// Return forms loaded
        /// </summary>
        public Dictionary<string, Form> GetForms() => PrinterService.GetForms();

        /// <summary>
        /// Return a list of medias loaded
        /// </summary>
        public Dictionary<string, Media> GetMedias() => PrinterService.GetMedias();

        /// <summary>
        /// Job containing print tasks that have not been flushed.
        /// The printer service stores all printing in this KXPrintJob until the application requests a flush.
        /// </summary>
        public PrintJobClass PrintJob => PrinterService.PrintJob;

        /// <summary>
        /// The method can be called in the device class to convert print job to a single image that can be sent to the printer.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="bitCount">Bits per pixel in returned data</param>
        /// <param name="UpsideDown"></param>
        /// <param name="imageInfo">Information bitmap created</param>
        [SupportedOSPlatform("windows")]
        public bool PrintToBitmap(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo) => PrinterService.PrintToBitmap(job, bitCount, UpsideDown, out imageInfo);

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a task object when printed using PrintToImage
        /// </summary>
        /// <param name="task">Task to print data</param>
        /// <param name="width">Width of rectangle needed to contain the task</param>
        /// <param name="height">Height of rectangle needed to contain the task</param>
        [SupportedOSPlatform("windows")]
        public bool GetBitmapPrintDimensions(PrintTask task, out int width, out int height) => PrinterService.GetBitmapPrintDimensions(task, out width, out height);
    }
}
