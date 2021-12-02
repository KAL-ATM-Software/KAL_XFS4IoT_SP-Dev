/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoT.Printer.Events;
using XFS4IoTFramework.Printer;
using XFS4IoTFramework.Common;

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
            PrinterService = new PrinterServiceClass(this, CommonService, logger, persistentData);
        }

        private readonly PrinterServiceClass PrinterService;
        private readonly CommonServiceClass CommonService;

        #region Printer unsolicited events
        public Task MediaTakenEvent() => PrinterService.MediaTakenEvent();

        public Task MediaInsertedUnsolicitedEvent() => PrinterService.MediaInsertedUnsolicitedEvent();

        public Task MediaPresentedUnsolicitedEvent(int WadIndex, int TotalWads) => PrinterService.MediaPresentedUnsolicitedEvent(new MediaPresentedUnsolicitedEvent.PayloadData(WadIndex, TotalWads));

        public Task MediaDetectedEvent(PositionEnum Position, int? RetractBinNumber = null) => PrinterService.MediaDetectedEvent(
            new MediaDetectedEvent.PayloadData(Position switch
                                               {
                                                   PositionEnum.Entering => XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData.PositionEnum.Entering,
                                                   PositionEnum.Expelled => XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData.PositionEnum.Expelled,
                                                   PositionEnum.Jammed => XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData.PositionEnum.Jammed,
                                                   PositionEnum.Present => XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData.PositionEnum.Present,
                                                   PositionEnum.Retracted => XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData.PositionEnum.Retracted,
                                                   _ => XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData.PositionEnum.Unknown,
                                               }, 
                                               RetractBinNumber));

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

        public Task MediaAutoRetractedEvent(int BinNumber, AutoRetractResultEnum AutoRetractResult) => PrinterService.MediaAutoRetractedEvent(
            new MediaAutoRetractedEvent.PayloadData(AutoRetractResult switch
                                                    {
                                                        AutoRetractResultEnum.Ok => XFS4IoT.Printer.Events.MediaAutoRetractedEvent.PayloadData.RetractResultEnum.Ok,
                                                        _ => XFS4IoT.Printer.Events.MediaAutoRetractedEvent.PayloadData.RetractResultEnum.Jammed,
                                                    }, 
                                                    BinNumber)
            );

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
        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public Task PowerSaveChangeEvent(int PowerSaveRecoveryTime) => CommonService.PowerSaveChangeEvent(new PowerSaveChangeEvent.PayloadData(PowerSaveRecoveryTime));

        public Task DevicePositionEvent(CommonStatusClass.PositionStatusEnum Position) => CommonService.DevicePositionEvent(
                                                                                                        new DevicePositionEvent.PayloadData(Position switch
                                                                                                        {
                                                                                                            CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                                                                                                            CommonStatusClass.PositionStatusEnum.NotInPosition => XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                                                                                                            _ => XFS4IoT.Common.PositionStatusEnum.Unknown,
                                                                                                        }
                                                                                                    ));

        public Task NonceClearedEvent(string ReasonDescription) => CommonService.NonceClearedEvent(new NonceClearedEvent.PayloadData(ReasonDescription));

        public Task ExchangeStateChangedEvent(CommonStatusClass.ExchangeEnum Exchange) => CommonService.ExchangeStateChangedEvent(
                                                                                                        new ExchangeStateChangedEvent.PayloadData(Exchange switch
                                                                                                        {
                                                                                                            CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                                                                                                            CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                                                                                                            _ => XFS4IoT.Common.ExchangeEnum.NotSupported,
                                                                                                        }
                                                                                                    ));
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
    }
}
