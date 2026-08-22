/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CardReader.Events;
using XFS4IoT.Printer.Events;
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Printer;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implementation of a card reader service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical card reader, which only implements the CardReader and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CardReaderServiceProvider : ServiceProvider, ICardReaderService, ICommonService, ILightsService, IStorageService, IPrinterService
    {
        public CardReaderServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger, 
            IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CardReader],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            CardReader = new CardReaderServiceClass(this, logger);

            List<XFSConstants.ServiceClass> services = [.. ServiceClasses];

            if (device as IStorageDevice is not null)
            {
                StorageService = new StorageServiceClass(this, logger, persistentData, StorageTypeEnum.Card);
                services.Add(XFSConstants.ServiceClass.Storage);
            }

            if (device as IPrinterDevice is not null)
            {
                PrinterService = new PrinterServiceClass(this, logger, persistentData);
                services.Add(XFSConstants.ServiceClass.Printer);
            }

            ServiceClasses = services;
        }

        private readonly CardReaderServiceClass CardReader;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService = null;
        private readonly PrinterServiceClass PrinterService = null;

        #region CardReader unsolicited events
        public Task MediaRemovedEvent() => CardReader.MediaRemovedEvent();

        public Task CardActionEvent(MovePosition To, MovePosition From)
        {
            string to = To.Position switch
            {
                MovePosition.MovePositionEnum.Exit => "exit",
                MovePosition.MovePositionEnum.Transport => "transport",
                _ => To.StorageId,
            };
            string from = From.Position switch
            {
                MovePosition.MovePositionEnum.Exit => "exit",
                MovePosition.MovePositionEnum.Transport => "transport",
                _ => From.StorageId,
            };
            return CardReader.CardActionEvent(new CardActionEvent.PayloadData(to, from));
        }

        public Task MediaDetectedEvent(MovePosition Position)
        {
            return CardReader.MediaDetectedEvent(new XFS4IoT.CardReader.Events.MediaDetectedEvent.PayloadData(Position.Position switch
            {
                MovePosition.MovePositionEnum.Exit => "exit",
                MovePosition.MovePositionEnum.Transport => "transport",
                _ => Position.StorageId,
            }));
        }

        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(List<string> CardUnitIds)
        {
            Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service.");
            StorageThresholdEvent.PayloadData paylod = new()
            {
                ExtendedProperties = GetStorages(CardUnitIds)
            };
            return StorageService.StorageThresholdEvent(paylod);
        }

        /// <summary>
        /// Sending status changed event.
        /// </summary>
        public Task StorageChangedEvent(object sender, PropertyChangedEventArgs propertyInfo)
        {
            Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service.");
            return StorageService.StorageChangedEvent(sender, propertyInfo);
        }

        #endregion

        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the CardReader Service.");

        public Task ErrorEvent(
            CommonStatusClass.ErrorEventIdEnum EventId,
            CommonStatusClass.ErrorActionEnum Action,
            string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage Service

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public async Task UpdateCardStorageCount(string storageId, int count)
        {
            if (StorageService is null)
            {
                return;
            }
            await StorageService.UpdateCardStorageCount(storageId, count);
        }

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta) => throw new NotSupportedException($"CardReader service doesn't support cash storage.");

        /// <summary>
        /// Update managed check storage information in the framework.
        /// </summary>
        public Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta = null) => throw new NotSupportedException($"CardReader service class doesn't support check storage.");

        /// <summary>
        /// Update managed printer storage information in the framework.
        /// </summary>
        public Task UpdatePrinterStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CardReader service class doesn't support printer storage.");

        /// <summary>
        /// Update managed deposit storage information in the framework.
        /// </summary>
        public Task UpdateDepositStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CardReader service class doesn't support deposit storage.");


        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.StorageType; } init { } }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); StorageService!.StorePersistent(); }

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.CardUnits; } init { } }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.CashUnits; } init { } }

        /// <summary>
        /// Check storage structure information of this device
        /// </summary>
        public Dictionary<string, CheckUnitStorage> CheckUnits { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.CheckUnits; } init { } }

        /// <summary>
        /// Printer storage structure information of this device
        /// </summary>
        public Dictionary<string, PrinterUnitStorage> PrinterUnits { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.PrinterUnits; } init { } }

        /// <summary>
        /// IBNS storage structure information of this device
        /// </summary>
        public Dictionary<string, IBNSUnitStorage> IBNSUnits { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.IBNSUnits; } init { } }

        /// <summary>
        /// Deposit storage structure information of this device
        /// </summary>
        public Dictionary<string, DepositUnitStorage> DepositUnits { get { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.DepositUnits; } init { } }

        /// <summary>
        /// Return XFS4IoT storage structured object.
        /// </summary>
        public Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> UnitIds)
        {
            return StorageService?.GetStorages(UnitIds) ?? [];
        }

        /// <summary>
        /// This method can use called from the device class when there is a change in the storage outside of the command.
        /// </summary>
        public Task UpdateStorageFromDeviceClass() { Contracts.Assert(StorageService is not null, "Usage error. the device class doesn't support storage service."); return StorageService!.UpdateStorageFromDeviceClass(); }


        #endregion

        #region Common Service

        /// <summary>
        /// Stores Common interface capabilities internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Stores CardReader interface capabilities internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set => CommonService.CardReaderCapabilities = value; }

        /// <summary>
        /// CardReader Status
        /// </summary>
        public CardReaderStatusClass CardReaderStatus { get => CommonService.CardReaderStatus; set => CommonService.CardReaderStatus = value; }


        /// <summary>
        /// Stores Printer interface capabilities internally
        /// </summary>
        public XFS4IoTFramework.Common.PrinterCapabilitiesClass PrinterCapabilities { get => CommonService.PrinterCapabilities; set => CommonService.PrinterCapabilities = value; }

        /// <summary>
        /// Printer Status
        /// </summary>
        public XFS4IoTFramework.Common.PrinterStatusClass PrinterStatus { get => CommonService.PrinterStatus; set => CommonService.PrinterStatus = value; }

        #endregion

        #region Printer Service

        public Task MediaTakenEvent()
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.MediaTakenEvent();
        }
        public Task MediaInsertedUnsolicitedEvent()
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.MediaInsertedUnsolicitedEvent();
        }

        public Task MediaPresentedUnsolicitedEvent(int WadIndex, int TotalWads)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.MediaPresentedUnsolicitedEvent(new MediaPresentedUnsolicitedEvent.PayloadData(WadIndex, TotalWads));
        }

        public Task MediaDetectedEvent(PositionEnum Position, int? RetractBinNumber = null)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");

            (Position == PositionEnum.Retracted && RetractBinNumber is not null).IsTrue($"No retract bin number specified in MediaDetectedEvent.");
            (Position == PositionEnum.Retracted && RetractBinNumber >= 0 && RetractBinNumber <= 9).IsTrue($"Invalid retract bin number specified in MediaDetectedEvent. Must be 0 to 9.");

            return PrinterService.MediaDetectedEvent(
                new XFS4IoT.Printer.Events.MediaDetectedEvent.PayloadData(Position: Position switch
                {
                    PositionEnum.Entering => "entering",
                    PositionEnum.Expelled => "expelled",
                    PositionEnum.Jammed => "jammed",
                    PositionEnum.Present => "present",
                    PositionEnum.Unknown => "unknown",
                    _ => "unit" + RetractBinNumber,
                }));
        }

        public Task TonerThresholdEvent(CommonThresholdStatusEnum Status)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            
            return PrinterService.TonerThresholdEvent(
            new TonerThresholdEvent.PayloadData(Status switch
            {
                CommonThresholdStatusEnum.Full => XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData.StateEnum.Full,
                CommonThresholdStatusEnum.Low => XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData.StateEnum.Low,
                _ => XFS4IoT.Printer.Events.TonerThresholdEvent.PayloadData.StateEnum.Out,
            }));
        }

        public Task LampThresholdEvent(LampThresholdEnum LampThreshold)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");

            return PrinterService.LampThresholdEvent(
                new LampThresholdEvent.PayloadData(LampThreshold switch
                {
                    LampThresholdEnum.Fading => XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData.StateEnum.Fading,
                    LampThresholdEnum.Inop => XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData.StateEnum.Inop,
                    _ => XFS4IoT.Printer.Events.LampThresholdEvent.PayloadData.StateEnum.Ok,
                }));
        }

        public Task InkThresholdEvent(CommonThresholdStatusEnum Status)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");

            return PrinterService.InkThresholdEvent(
            new InkThresholdEvent.PayloadData(Status switch
            {
                CommonThresholdStatusEnum.Full => XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData.StateEnum.Full,
                CommonThresholdStatusEnum.Low => XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData.StateEnum.Low,
                _ => XFS4IoT.Printer.Events.InkThresholdEvent.PayloadData.StateEnum.Out,
            }));
        }

        public Task MediaAutoRetractedEvent(int BinNumber, AutoRetractResultEnum AutoRetractResult)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");

            (AutoRetractResult == AutoRetractResultEnum.Retracted && BinNumber >= 0 && BinNumber <= 9).IsTrue($"Invalid retract bin number specified in MediaAutoRetractedEvent. Must be 0 to 9.");

            string position = AutoRetractResult switch
            {
                AutoRetractResultEnum.Transport => "transport",
                AutoRetractResultEnum.Jammed => "jammed",
                _ => "unit" + BinNumber,
            };
            return PrinterService.MediaAutoRetractedEvent(new MediaAutoRetractedEvent.PayloadData(position));
        }

        public Task PaperThresholdEvent(CommonThresholdStatusEnum Status, PaperSourceEnum? PaperSource, string CustomSource = null)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");

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

            return PrinterService.PaperThresholdEvent(
                new PaperThresholdEvent.PayloadData(
                    paperSource,
                    Status switch
                    {
                        CommonThresholdStatusEnum.Full => XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData.ThresholdEnum.Full,
                        CommonThresholdStatusEnum.Low => XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData.ThresholdEnum.Low,
                        _ => XFS4IoT.Printer.Events.PaperThresholdEvent.PayloadData.ThresholdEnum.Out,
                    }));
        }

        /// <summary>
        /// Set form definition 
        /// </summary>
        public bool SetForm(string definition, Form form)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.SetForm(definition, form);
        }

        /// <summary>
        /// Set media definition
        /// </summary>
        public bool SetMedia(string definition, Media media)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.SetMedia(definition, media);
        }

        /// <summary>
        /// Return forms loaded
        /// </summary>
        public Dictionary<string, Form> GetForms()
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.GetForms();
        }

        /// <summary>
        /// Return a list of medias loaded
        /// </summary>
        public Dictionary<string, Media> GetMedias()
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.GetMedias();
        }

        /// <summary>
        /// Job containing print tasks that have not been flushed.
        /// The printer service stores all printing in this KXPrintJob until the application requests a flush.
        /// </summary>
        public PrintJobClass PrintJob
        {
            get
            {
                Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
                return PrinterService.PrintJob;
            }
        }

        /// <summary>
        /// The method can be called in the device class to convert print job to a single image that can be sent to the printer.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="bitCount">Bits per pixel in returned data</param>
        /// <param name="UpsideDown"></param>
        /// <param name="imageInfo">Information bitmap created</param>
        /// <param name="FullImage">
        /// False (default) returns a tight crop around the job's tasks with ImageInfo.OffsetX/OffsetY set
        /// for the caller to composite elsewhere. True returns an image covering the entire media instead -
        /// see PrintToBitmapHandler.Convert for details.
        /// </param>
        public bool PrintToBitmap(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo, bool FullImage = false)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.PrintToBitmap(job, bitCount, UpsideDown, out imageInfo, FullImage);
        }

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a task object when printed using PrintToImage
        /// </summary>
        /// <param name="task">Task to print data</param>
        /// <param name="width">Width of rectangle needed to contain the task</param>
        /// <param name="height">Height of rectangle needed to contain the task</param>
        public bool GetBitmapPrintDimensions(PrintTask task, out int width, out int height)
        {
            Contracts.Assert(PrinterService is not null, "Usage error. the device class doesn't support printer service.");
            return PrinterService.GetBitmapPrintDimensions(task, out width, out height);
        }

        #endregion
    }
}
