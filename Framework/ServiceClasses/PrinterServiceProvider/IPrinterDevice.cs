/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using System;

namespace XFS4IoTFramework.Printer
{
    public interface IPrinterDevice : IDevice
    {
        /// <summary>
        /// This method is used to control media.
        /// If an eject operation is specified, it completes when the media is moved to the exit slot.An unsolicited event is
        /// generated when the media has been taken by the device capability is true.
        /// </summary>
        Task<ControlMediaResult> ControlMediaAsync(ControlMediaEvent controlMediaEvent,
                                                   ControlMediaRequest request,
                                                   CancellationToken cancellation);

        /// <summary>
        /// This method can turn the pages of a passbook inserted in the printer by a specified number of pages in a
        /// specified direction and it can close the passbook.
        /// </summary>
        Task<ControlPassbookResult> ControlPassbookAsync(ControlPassbookRequest request,
                                                         CancellationToken cancellation);


        /// <summary>
        /// This method is used to move paper (which can also be a new passbook) from a paper source into the print position.
        /// </summary>
        Task<DispensePaperResult> DispensePaperAsync(DispensePaperCommandEvents events,
                                                     DispensePaperRequest request,
                                                     CancellationToken cancellation);

        /// <summary>
        /// This method returns image data from the current media. If no media is present, the device waits for the timeout
        /// specified, for media to be inserted.
        /// If the returned image data is in Windows bitmap format (BMP) and a file path for storing the image is not
        /// supplied, then the first byte of data will be the start of the Bitmap Info Header (this bitmap format is known as
        /// DIB, Device Independent Bitmap). The Bitmap File Info Header, which is only present in file versions of bitmaps,
        /// will NOT be returned.If the returned image data is in bitmap format(BMP) and a file path for storing the image
        /// is supplied, then the first byte of data in the stored file will be the Bitmap File Info Header.
        /// </summary>
        Task<AcceptAndReadImageResult> AcceptAndReadImageAsync(ReadImageCommandEvents events,
                                                               AcceptAndReadImageRequest request,
                                                               CancellationToken cancellation);

        /// <summary>
        /// This method resets the present value for number of media items retracted to zero. The function is possible only
        /// for printers with retract capability.
        /// if the binNumber is -1, all retract bin counter to be reset
        /// </summary>
        [Obsolete("This method is no longer used by the common interface. Migrate reset bin counter in the storage interface. this interface will be removed after version 4.")]
        Task<DeviceResult> ResetBinCounterAsync(int binNumber,
                                                CancellationToken cancellation);

        /// <summary>
        /// This method is used by the application to perform a hardware reset which will attempt to return the device to a
        /// known good state.
        /// </summary>
        Task<ResetDeviceResult> ResetDeviceAsync(ResetDeviceRequest request,
                                                 CancellationToken cancellation);

        /// <summary>
        /// The media is removed from its present position (media inserted into device, media entering, unknown position) and
        /// stored in one of the retract bins.An event is sent if the storage capacity of the specified retract bin is
        /// reached. If the bin is already full and the command cannot be executed, an error is returned and the media remains
        /// in its present position.
        /// if the binNumber is -1, move media to transport.
        /// </summary>
        Task<RetractResult> RetractAsync(int binNumber,
                                         CancellationToken cancellation);


        /// <summary>
        /// This method switches the black mark detection mode and associated functionality on or off. The black mark
        /// detection mode is persistent. If the selected mode is already active this command will complete with success.
        /// </summary>
        Task<DeviceResult> SetBlackMarkModeAsync(BlackMarkModeEnum mode,
                                                 CancellationToken cancellation);

        /// <summary>
        /// After the supplies have been replenished, this method is used to indicate that one or more supplies have been
        /// replenished and are expected to be in a healthy state.
        /// Hardware that cannot detect the level of a supply and reports on the supply's status using metrics (or some other
        /// means), must assume the supply has been fully replenished after this command is issued.The appropriate threshold
        /// event must be broadcast.
        /// Hardware that can detect the level of a supply must update its status based on its sensors, generate a threshold
        /// event if appropriate, and succeed the command even if the supply has not been replenished. If it has already
        /// detected the level and reported the threshold before this command was issued, the command must succeed and no
        /// threshold event is required.
        /// </summary>
        Task<DeviceResult> SupplyReplenishedAsync(SupplyReplenishedRequest request,
                                                  CancellationToken cancellation);

        /// <summary>
        /// This method is the main method for printing forms.
        /// It is passed a KXPrintJob containing one or more atomic PrintTask to print.
        /// The passed tasks are printed and flushed to the printer.
        /// Measurements in all tasks are in printer dots.
        /// This method must be implemented if your device is capable or printing.
        /// </summary>
        Task<PrintTaskResult> ExecutePrintTasksAsync(PrintTaskRequest request,
                                                     CancellationToken cancellation);

        /// <summary>
        /// This method is used to send raw data (a byte string of device dependent data) to the physical device.
        /// </summary>
        Task<RawPrintResult> RawPrintAsync(RawPrintCommandEvents events,
                                           RawPrintRequest request,
                                           CancellationToken cancellation);

        /// <summary>
        /// Returns width and height of passed task in printer dots.
        /// i.e. width and height of rectangle needed to contain the task when executed.
        /// Normally expected to return true since no hardware action is requested.
        /// </summary>
        bool GetTaskDimensions(PrintTask task, out int width, out int height);

        /// <summary>
        /// This method sets the page size in dots.  The requested size
        /// may be given as zero - since that's a valid media length.
        /// In XFS the value zero means an infinite roll which is never cut
        /// so if page size is set to zero and a cut instruction sent
        /// the SP can decide what to do: either cut at current position
        /// or go into black mark mode for instance.
        /// </summary>
        bool SetPageSize(int pageSize);

        /// <summary>
        /// Return binary data for mapping codeline format
        /// </summary>
        List<byte> GetCodelineMapping(CodelineFormatEnum codelineFormat);

        /// <summary>
        /// This method is to print a loaded form and media in the firmware where all fields prefixed positions are recognized.
        /// </summary>
        Task<PrintFormResult> DirectFormPrintAsync(DirectFormPrintRequest request,
                                                   CancellationToken cancellation);


        /// <summary>
        /// Printer Status
        /// </summary>
        PrinterStatusClass PrinterStatus { get; set; }

        /// <summary>
        /// Printer Capabilities
        /// </summary>
        PrinterCapabilitiesClass PrinterCapabilities { get; set; }

        /// <summary>
        /// This property must added MediaSpec structures to reflect the media supported by the specific device.
        /// At least one element must be added. If the printer has more than one paper supply, more than one structure may be returned.
        /// </summary>
        List<MediaSpec> MediaSpecs { get; set; }

        /// <summary>
        /// This property must return a FormRules structure which reflects
        /// the print capabilities of the specific device being supported.
        /// </summary>
        FormRules FormRules { get; set; }

        /// <summary>
        /// Values for unit conversions
        /// All print tasks supplied to the printer are in printer dots.
        /// The printer must therefore export information about these
        /// units to the printer framework ie. how many dots-per-mm, dots-per-inch 
        /// and dots-per-row/column.  Conversion factors are given as a fraction -
        /// So there are DotsPerInchTop/DotsPerInchBottom dots-per-inch
        /// and similarly for mm and row/column.  Interpretation of Row/Column
        /// should be average size of a character in the default font.
        /// </summary>
        int DotsPerInchTopX { get; set; }
        int DotsPerInchBottomX { get; set; }
        int DotsPerInchTopY { get; set; }
        int DotsPerInchBottomY { get; set; }
        int DotsPerMMTopX { get; set; }
        int DotsPerMMBottomX { get; set; }
        int DotsPerMMTopY { get; set; }
        int DotsPerMMBottomY { get; set; }
        int DotsPerRowTop { get; set; }
        int DotsPerRowBottom { get; set; }
        int DotsPerColumnTop { get; set; }
        int DotsPerColumnBottom { get; set; }
    }
}
