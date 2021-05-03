/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * IPrinterDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of printer. 
namespace XFS4IoTFramework.Printer
{
    public interface IPrinterDevice : IDevice
    {

        /// <summary>
        /// |-  This command is used to retrieve the list of forms available on the device.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.GetFormListCompletion.PayloadData> GetFormList(IGetFormListEvents events, 
                                                                                        XFS4IoT.Printer.Commands.GetFormListCommand.PayloadData payload, 
                                                                                        CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve the list of media definitions available on the device.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.GetMediaListCompletion.PayloadData> GetMediaList(IGetMediaListEvents events, 
                                                                                          XFS4IoT.Printer.Commands.GetMediaListCommand.PayloadData payload, 
                                                                                          CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve details of the definition of a specified form.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.GetQueryFormCompletion.PayloadData> GetQueryForm(IGetQueryFormEvents events, 
                                                                                          XFS4IoT.Printer.Commands.GetQueryFormCommand.PayloadData payload, 
                                                                                          CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve details of the definition of a specified media.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.GetQueryMediaCompletion.PayloadData> GetQueryMedia(IGetQueryMediaEvents events, 
                                                                                            XFS4IoT.Printer.Commands.GetQueryMediaCommand.PayloadData payload, 
                                                                                            CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve details of the definition of a single or all fields on a specified form.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.GetQueryFieldCompletion.PayloadData> GetQueryField(IGetQueryFieldEvents events, 
                                                                                            XFS4IoT.Printer.Commands.GetQueryFieldCommand.PayloadData payload, 
                                                                                            CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the byte code mapping for the special banking symbols defined for imageprocessing (e.g. check processing). This mapping must be reported as there is no standard for the fonts definedbelow.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.GetCodelineMappingCompletion.PayloadData> GetCodelineMapping(IGetCodelineMappingEvents events, 
                                                                                                      XFS4IoT.Printer.Commands.GetCodelineMappingCommand.PayloadData payload, 
                                                                                                      CancellationToken cancellation);

        /// <summary>
        /// This command is used to control media.If an eject operation is specified, it completes when the media is moved to the exit slot. An unsolicited event isgenerated when the media has been taken by the user (only if the[mediaTaken](#common.capabilities.completion.properties.printer.mediataken) capability is true.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.ControlMediaCompletion.PayloadData> ControlMedia(IControlMediaEvents events, 
                                                                                          XFS4IoT.Printer.Commands.ControlMediaCommand.PayloadData payload, 
                                                                                          CancellationToken cancellation);

        /// <summary>
        /// This command is used to print a form by merging the supplied variable field data with the defined form and fielddata specified in the form. If no media is present, the device waits, for the period of time specified by the[timeout](#printer.printform.command.properties.timeout) parameter, for media to be inserted from the externalpaper source.All error codes (except noMediaPresent) and events listed under the[Printer.ControlMedia](#printer.controlmedia) command description can also occur on this command.An invalid field name is treated as a [Printer.FieldWarningEvent](#printer.fieldwarningevent) event with[failure](#printer.fieldwarningevent.event.properties.failure) *notfound*. A *Printer.FieldWarningEvent* event isreturned with fieldOverflow status if the data overflows the field, and the field definition OVERFLOW value isTRUNCATE, BESTFIT, OVERWRITE or WORDWRAP. Other field-related problems generate a field error return and event.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.PrintFormCompletion.PayloadData> PrintForm(IPrintFormEvents events, 
                                                                                    XFS4IoT.Printer.Commands.PrintFormCommand.PayloadData payload, 
                                                                                    CancellationToken cancellation);

        /// <summary>
        /// This command is used to read data from input fields on the specified form. These input fields may consist of MICR,OCR, MSF, BARCODE, or PAGEMARK input fields. These input fields may also consist of TEXT fields for purposes ofdetecting available passbook print lines with passbook printers supporting such capability. If no media ispresent, the device waits, for the [timeout](#printer.readform.command.properties.timeout) specified, for media tobe inserted.All error codes (except *noMediaPresent*) and events listed under the[Printer.ControlMedia](#printer.controlmedia) command description can also occur on this command.The following applies to the usage of [fields](#printer.readform.completion.properties.fields) for passbook:If the media type is PASSBOOK, and the field(s) type is TEXT, and the Service Provider and the underlying passbookprinter are capable of detecting available passbook print lines, then the field(s) will be returned without avalue, in the format \"<FieldName>\" or *fieldname[index]*, if the field is available for passbook printing.Field(s) unavailable for passbook printing will not be returned. The Service Provider will examine the passbooktext field(s) supplied in the *fieldNames* field, and with the form/fields definition and the underlying passbookprinter capability determine which fields should be available for passbook printing.To illustrate when media type is PASSBOOK, if a form named PSBKTST1 contains 24 fields, one field per line, andthe field names are LINE1 through LINE24 (same order as printing), and after execution of this command *fields*contains fields LINE13 through LINE24, then the first print line available for passbook printing is line 13.To illustrate another example when media type is PASSBOOK, if a form named PSBKTST2 contains 24 fields, one fieldper line, and the field names are LINE1 through LINE24 (same order as printing), and after execution of thiscommand *fields* contains fields LINE13, and LINE20 through LINE24 then the first print line available forpassbook printing is line 13, however lines 14-19 are not also available, so if the application is attempting todetermine the first available print line after which all subsequent print lines are also available then line 20 isa better choice.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.ReadFormCompletion.PayloadData> ReadForm(IReadFormEvents events, 
                                                                                  XFS4IoT.Printer.Commands.ReadFormCommand.PayloadData payload, 
                                                                                  CancellationToken cancellation);

        /// <summary>
        /// This command is used to send raw data (a byte string of device dependent data) to the physical device.Applications which send raw data to a device will typically not be device or vendor independent. Problems with theuse of this command include:1. The data sent to the device can include commands that change the state of the device in unpredictable ways (in   particular, in ways that the Service Provider may not be aware of).2. Usage of this command will not be portable.3. This command violates the XFS forms model that is the basis of XFS printer access.Thus usage of this command should be avoided whenever possible.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.RawDataCompletion.PayloadData> RawData(IRawDataEvents events, 
                                                                                XFS4IoT.Printer.Commands.RawDataCommand.PayloadData payload, 
                                                                                CancellationToken cancellation);

        /// <summary>
        /// This command is used to get the extents of the media inserted in the physical device. The input parameterspecifies the base unit and fractions in which the media extent values will be returned. If no media is present,the device waits, for the [timeout](#printer.mediaextents.command.properties.timeout) specified, for media to beinserted.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.MediaExtentsCompletion.PayloadData> MediaExtents(IMediaExtentsEvents events, 
                                                                                          XFS4IoT.Printer.Commands.MediaExtentsCommand.PayloadData payload, 
                                                                                          CancellationToken cancellation);

        /// <summary>
        /// This function resets the present value for number of media items retracted to zero. The function is possible onlyfor printers with retract capability.The number of media items retracted is controlled by the service and can be requested before resetting using theinfo [Common.Status](#common.status) command.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.ResetCountCompletion.PayloadData> ResetCount(IResetCountEvents events, 
                                                                                      XFS4IoT.Printer.Commands.ResetCountCommand.PayloadData payload, 
                                                                                      CancellationToken cancellation);

        /// <summary>
        /// This function returns image data from the current media. If no media is present, the device waits for the timeoutspecified, for media to be inserted.If the returned image data is in Windows bitmap format (BMP) and a file path for storing the image is notsupplied, then the first byte of data will be the start of the Bitmap Info Header (this bitmap format is known asDIB, Device Independent Bitmap). The Bitmap File Info Header, which is only present in file versions of bitmaps,will NOT be returned. If the returned image data is in bitmap format (BMP) and a file path for storing the imageis supplied, then the first byte of data in the stored file will be the Bitmap File Info Header.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.ReadImageCompletion.PayloadData> ReadImage(IReadImageEvents events, 
                                                                                    XFS4IoT.Printer.Commands.ReadImageCommand.PayloadData payload, 
                                                                                    CancellationToken cancellation);

        /// <summary>
        /// This command is used by the application to perform a hardware reset which will attempt to return the device to aknown good state.The device will attempt to retract or eject any items found anywhere within the device. This may not always bepossible because of hardware problems. The [Printer.MediaDetectedEvent](#printer.mediadetectedevent) event willinform the application where items were actually moved to.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.ResetCompletion.PayloadData> Reset(IResetEvents events, 
                                                                            XFS4IoT.Printer.Commands.ResetCommand.PayloadData payload, 
                                                                            CancellationToken cancellation);

        /// <summary>
        /// The media is removed from its present position (media inserted into device, media entering, unknown position) andstored in one of the retract bins. An event is sent if the storage capacity of the specified retract bin isreached. If the bin is already full and the command cannot be executed, an error is returned and the media remainsin its present position.If a retract request is received for a device with no retract capability, the unsupportedCommand error isreturned.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.RetractMediaCompletion.PayloadData> RetractMedia(IRetractMediaEvents events, 
                                                                                          XFS4IoT.Printer.Commands.RetractMediaCommand.PayloadData payload, 
                                                                                          CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to move paper (which can also be a new passbook) from a paper source into the print position.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.DispensePaperCompletion.PayloadData> DispensePaper(IDispensePaperEvents events, 
                                                                                            XFS4IoT.Printer.Commands.DispensePaperCommand.PayloadData payload, 
                                                                                            CancellationToken cancellation);

        /// <summary>
        /// This command is used to print a file that contains a complete print job in the native printer language. Thecreation and content of this file are both Operating System and printer specific and outwith the scope of thisspecification.If no media is present, the device waits, for the [timeout](#printer.printrawfile.command.properties.timeout)specified, for media to be inserted from the external paper source.This command must not complete until all pages have been presented to the customer.Printing of multiple pages is handled as described in[Command and Event Flows during Single and Multi-Page / Wad Printing](#printer.generalinformation.commandeventflows).
        /// </summary>
        Task<XFS4IoT.Printer.Completions.PrintRawFileCompletion.PayloadData> PrintRawFile(IPrintRawFileEvents events, 
                                                                                          XFS4IoT.Printer.Commands.PrintRawFileCommand.PayloadData payload, 
                                                                                          CancellationToken cancellation);

        /// <summary>
        /// This command is used to load a form (including sub-forms and frames) or media definition into the list ofavailable forms. Once a form or media definition has been loaded through this command it can be used by any of theother form/media definition processing commands. Forms and media definitions loaded through this command arepersistent. When a form or media definition is loaded a[Printer.DefinitionLoadedEvent](#printer.definitionloadedevent) event is generated to inform applicationsthat a form or media definition has been added or replaced.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.LoadDefinitionCompletion.PayloadData> LoadDefinition(ILoadDefinitionEvents events, 
                                                                                              XFS4IoT.Printer.Commands.LoadDefinitionCommand.PayloadData payload, 
                                                                                              CancellationToken cancellation);

        /// <summary>
        /// After the supplies have been replenished, this command is used to indicate that one or more supplies have beenreplenished and are expected to be in a healthy state.Hardware that cannot detect the level of a supply and reports on the supply's status using metrics (or some othermeans), must assume the supply has been fully replenished after this command is issued. The appropriate thresholdevent must be broadcast.Hardware that can detect the level of a supply must update its status based on its sensors, generate a thresholdevent if appropriate, and succeed the command even if the supply has not been replenished. If it has alreadydetected the level and reported the threshold before this command was issued, the command must succeed and nothreshold event is required.If any one of the specified supplies is not supported by a Service Provider, unsupportedData should be returned,and no replenishment actions will be taken by the Service Provider.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.SupplyReplenishCompletion.PayloadData> SupplyReplenish(ISupplyReplenishEvents events, 
                                                                                                XFS4IoT.Printer.Commands.SupplyReplenishCommand.PayloadData payload, 
                                                                                                CancellationToken cancellation);

        /// <summary>
        /// This command can turn the pages of a passbook inserted in the printer by a specified number of pages in aspecified direction and it can close the passbook. The[controlPassbook](#common.capabilities.completion.properties.printer.controlpassbook) field returned by[Common.Capabilities](#common.capabilities) specifies which functionality is supported. This command flushes thedata before the pages are turned or the passbook is closed.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.ControlPassbookCompletion.PayloadData> ControlPassbook(IControlPassbookEvents events, 
                                                                                                XFS4IoT.Printer.Commands.ControlPassbookCommand.PayloadData payload, 
                                                                                                CancellationToken cancellation);

        /// <summary>
        /// This command switches the black mark detection mode and associated functionality on or off. The black markdetection mode is persistent. If the selected mode is already active this command will complete with success.
        /// </summary>
        Task<XFS4IoT.Printer.Completions.SetBlackMarkModeCompletion.PayloadData> SetBlackMarkMode(ISetBlackMarkModeEvents events, 
                                                                                                  XFS4IoT.Printer.Commands.SetBlackMarkModeCommand.PayloadData payload, 
                                                                                                  CancellationToken cancellation);

    }
}
