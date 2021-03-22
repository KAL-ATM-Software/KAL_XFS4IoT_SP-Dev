/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Printer;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;

// KAL specific implementation of printer. 
namespace XFS4IoTFramework.Printer
{
    public interface IPrinterDevice : ICommonDevice
    {

        /// <summary>
        /// This command is used to retrieve the list of forms available on the device.
        /// </summary>
        Task<GetFormListCompletion.PayloadData> GetFormList(IPrinterConnection connection, GetFormListCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the list of media definitions available on the device.
        /// </summary>
        Task<GetMediaListCompletion.PayloadData> GetMediaList(IPrinterConnection connection, GetMediaListCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve details of the definition of a specified form.
        /// </summary>
        Task<GetQueryFormCompletion.PayloadData> GetQueryForm(IPrinterConnection connection, GetQueryFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve details of the definition of a specified media.
        /// </summary>
        Task<GetQueryMediaCompletion.PayloadData> GetQueryMedia(IPrinterConnection connection, GetQueryMediaCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve details of the definition of a single or all fields on a specified form.
        /// </summary>
        Task<GetQueryFieldCompletion.PayloadData> GetQueryField(IPrinterConnection connection, GetQueryFieldCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the byte code mapping for the special banking symbols defined for image processing (e.g. check processing). This mapping must be reported as there is no standard for the fonts defined below.
        /// </summary>
        Task<GetCodelineMappingCompletion.PayloadData> GetCodelineMapping(IPrinterConnection connection, GetCodelineMappingCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to control media.If an eject operation is specified, it completes when the media is moved to the exit slot. A service event is generated when the media has been taken by the user (only if the [mediaTaken](#printer-capability-mediaTaken) capability is true.
        /// </summary>
        Task<ControlMediaCompletion.PayloadData> ControlMedia(IPrinterConnection connection, ControlMediaCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to print a form by merging the supplied variable field data with the defined form and field data specified in the form. If no media is present, the device waits, for the period of time specified by the *timeout* parameter, for media to be inserted from the external paper source.
        /// </summary>
        Task<PrintFormCompletion.PayloadData> PrintForm(IPrinterConnection connection, PrintFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to read data from input fields on the specified form. These input fields may consist of MICR, OCR, MSF, BARCODE, or PAGEMARK input fields. These input fields may also consist of TEXT fields for purposes of detecting available passbook print lines with passbook printers supporting such capability. If no media is present, the device waits, for the timeout specified, for media to be inserted.
        /// </summary>
        Task<ReadFormCompletion.PayloadData> ReadForm(IPrinterConnection connection, ReadFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to send raw data (a byte string of device dependent data) to the physical device.
        /// </summary>
        Task<RawDataCompletion.PayloadData> RawData(IPrinterConnection connection, RawDataCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to get the extents of the media inserted in the physical device. The input parameter specifies the base unit and fractions in which the media extent values will be returned. If no media is present, the device waits, for the timeout specified, for media to be inserted.
        /// </summary>
        Task<MediaExtentsCompletion.PayloadData> MediaExtents(IPrinterConnection connection, MediaExtentsCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This function resets the present value for number of media items retracted to zero. The function is possible only for printers with retract capability.The number of media items retracted is controlled by the service and can be requested before resetting using the info Printer.Status command.
        /// </summary>
        Task<ResetCountCompletion.PayloadData> ResetCount(IPrinterConnection connection, ResetCountCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This function returns image data from the current media. If no media is present, the device waits for the timeout specified, for media to be inserted.
        /// </summary>
        Task<ReadImageCompletion.PayloadData> ReadImage(IPrinterConnection connection, ReadImageCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used by the application to perform a hardware reset which will attempt to return the device to a known good state.The device will attempt to retract or eject any items found anywhere within the device. This may not always be possible because of hardware problems. The [Printer.MediaDetectedEvent](#message-Printer.MediaDetectedEvent) event will inform the application where items were actually moved to.
        /// </summary>
        Task<ResetCompletion.PayloadData> Reset(IPrinterConnection connection, ResetCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// The media is removed from its present position (media inserted into device, media entering, unknown position) and stored in one of the retract bins. An event is sent if the storage capacity of the specified retract bin is reached. If the bin is already full and the command cannot be executed, an error is returned and the media remains in its present position.
        /// </summary>
        Task<RetractMediaCompletion.PayloadData> RetractMedia(IPrinterConnection connection, RetractMediaCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to move paper (which can also be a new passbook) from a paper source into the print position.
        /// </summary>
        Task<DispensePaperCompletion.PayloadData> DispensePaper(IPrinterConnection connection, DispensePaperCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to print a file that contains a complete print job in the native printer language. The creation and content of this file are both Operating System and printer specific and outwith the scope of this specification.If no media is present, the device waits, for the timeout specified, for media to be inserted from the external paper source.This command must not complete until all pages have been presented to the customer.
        /// </summary>
        Task<PrintRawFileCompletion.PayloadData> PrintRawFile(IPrinterConnection connection, PrintRawFileCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to load a form (including sub-forms and frames) or media definition into the list of available forms. Once a form or media definition has been loaded through this command it can be used by any of the other form/media definition processing commands. Forms and media definitions loaded through this command are persistent. When a form or media definition is loaded a [Printer.DefinitionLoadedEvent](#message-Printer.DefinitionLoadedEvent) event is generated to inform applications that a form or media definition has been added or replaced.
        /// </summary>
        Task<LoadDefinitionCompletion.PayloadData> LoadDefinition(IPrinterConnection connection, LoadDefinitionCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// After the supplies have been replenished, this command is used to indicate that one or more supplies have been replenished and are expected to be in a healthy state.Hardware that cannot detect the level of a supply and reports on the supply's status using metrics (or some other means), must assume the supply has been fully replenished after this command is issued. The appropriate threshold event must be broadcast.Hardware that can detect the level of a supply must update its status based on its sensors, generate a threshold event if appropriate, and succeed the command even if the supply has not been replenished. If it has already detected the level and reported the threshold before this command was issued, the command must succeed and no threshold event is required.
        /// </summary>
        Task<SupplyReplenishCompletion.PayloadData> SupplyReplenish(IPrinterConnection connection, SupplyReplenishCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command can turn the pages of a passbook inserted in the printer by a specified number of pages in a specified direction and it can close the passbook. The [controlPassbook](#printer-capability-controlpassbook) field returned by [Common.Capabilities](#command-Common.Capabilities) specifies which functionality is supported. This command flushes the data before the pages are turned or the passbook is closed.
        /// </summary>
        Task<ControlPassbookCompletion.PayloadData> ControlPassbook(IPrinterConnection connection, ControlPassbookCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command switches the black mark detection mode and associated functionality on or off. The black mark detection mode is persistent. If the selected mode is already active this command will complete with success.
        /// </summary>
        Task<SetBlackMarkModeCompletion.PayloadData> SetBlackMarkMode(IPrinterConnection connection, SetBlackMarkModeCommand.PayloadData payload, CancellationToken cancellation);
    }
}
