/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Printer.Events;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class PrintFormHandler
    {
        private async Task<PrintFormCompletion.PayloadData> HandlePrintForm(IPrintFormEvents events, PrintFormCommand printForm, CancellationToken cancel)
        {
            PrintFormEvents = events;

            Dictionary<string, Form> forms = Printer.GetForms();
            // Locate the Form and Media and check they are valid for this device
            // First check specified form loaded or not
            if (!forms.ContainsKey(printForm.Payload.FormName))
            {
                return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Requested form not found. {printForm.Payload.FormName}",
                                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FormNotFound);
            }

            // Check form device supported
            Form form = forms[printForm.Payload.FormName]; 
            FormRules rules = Device.FormRules;
            if (rules.RowColumnOnly && 
                form.Base != Form.BaseEnum.ROWCOLUMN)
            {
                return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"UNIT is not valid for this printer - only ROWCOLUMN supported. {form.Base}, {printForm.Payload.FormName}",
                                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FormNotFound);
            }

            Contracts.IsTrue(rules.MaxSkew >= rules.MinSkew, $"Unexpected form. the MinSkew is greater than MaxSkew. {printForm.Payload.FormName}");

            if (!rules.ValidOrientation.HasFlag(form.Orientation))
            {
                return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Invalid orientation. {form.Orientation}, {printForm.Payload.FormName}",
                                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FormNotFound);
            }

            // Check all fields
            foreach (var field in form.Fields)
            {
                var result = CheckFieldValid(field.Value, form);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               $"{result.ErrorDescription} Field:{field.Key}",
                                                               result.ErrorCode);
                }
            }

            // Check media specified by the client
            Dictionary<string, Media> medias = Printer.GetMedias();
            if (!medias.ContainsKey(printForm.Payload.MediaName))
            {
                return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Requested media not found. {printForm.Payload.MediaName}",
                                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaNotFound);
            }

            // Check media
            Media media = medias[printForm.Payload.MediaName];
            {
                var result = CheckMediaValid(media);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               $"Requested media is invalid. {result.ErrorDescription} {printForm.Payload.MediaName}",
                                                               result.ErrorCode);
                }
            }

            PaperSourceEnum? paperSource = null;
            string customSource = null;

            // Capability check
            if (printForm.Payload.PaperSource is null)
            {
                paperSource = PaperSourceEnum.Default;
            }
            else
            {
                if (printForm.Payload.PaperSource == "aux" ||
                    printForm.Payload.PaperSource == "aux2" ||
                    printForm.Payload.PaperSource == "external" ||
                    printForm.Payload.PaperSource == "lower" ||
                    printForm.Payload.PaperSource == "upper" ||
                    printForm.Payload.PaperSource == "park")
                {
                    if ((printForm.Payload.PaperSource == "aux" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.AUX)) ||
                        (printForm.Payload.PaperSource == "aux2" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.AUX2)) ||
                        (printForm.Payload.PaperSource == "external" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.External)) ||
                        (printForm.Payload.PaperSource == "lower" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Lower)) ||
                        (printForm.Payload.PaperSource == "upper" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Upper)) ||
                        (printForm.Payload.PaperSource == "park" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Park)))
                    {
                        return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified paper source is not supported by the device. {printForm.Payload.PaperSource}");
                    }

                    paperSource = printForm.Payload.PaperSource switch
                    {
                        "aux" => PaperSourceEnum.AUX,
                        "aux2" => PaperSourceEnum.AUX2,
                        "external" => PaperSourceEnum.External,
                        "lower" => PaperSourceEnum.Lower,
                        "upper" => PaperSourceEnum.Upper,
                        _ => PaperSourceEnum.Park,
                    };
                }
                else
                {
                    if (!Common.PrinterCapabilities.CustomPaperSources.ContainsKey(printForm.Payload.PaperSource))
                    {
                        return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified paper source is not supported by the device. {printForm.Payload.PaperSource}");
                    }
                    customSource = printForm.Payload.PaperSource;
                }
            }

            // Trying to print direct form printing if the firmware keeps
            // loaded form information or XFS3 wrapper.
            try
            {
                Logger.Log(Constants.DeviceClass, $"PrinterDev.DirectFormPrintAsync()");
                var result = await Device.DirectFormPrintAsync(new DirectFormPrintRequest(printForm.Payload.FormName,
                                                                                          printForm.Payload.MediaName,
                                                                                          printForm.Payload.Fields,
                                                                                          paperSource,
                                                                                          customSource),
                                                               cancel);
                Logger.Log(Constants.DeviceClass, $"PrinterDev.DirectFormPrintAsync() -> {result.CompletionCode}, {result.ErrorCode}");

                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.UnsupportedCommand)
                {
                    if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success && printForm.Payload.MediaControl is not null)
                    {
                        // Now do any other media control requested
                        var mediaControlResult = await ExecuteControlMedia(events, printForm.Payload, cancel);
                        if (mediaControlResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                        {
                            return mediaControlResult;
                        }
                    }

                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
                }
            }
            catch (NotImplementedException)
            {
                Logger.Log(Constants.DeviceClass, $"PrinterDev.DirectFormPrint() -> Unsupported.");
            }
            catch (NotSupportedException)
            {
                Logger.Log(Constants.DeviceClass, $"PrinterDev.DirectFormPrint() -> Unsupported.");
            }
            catch (Exception)
            { throw; }

            // This step assign field and field check
            List<FieldAssignment> fieldAssignments = new();
            int elementNumber;
            foreach (var fieldName in printForm.Payload.Fields)
            {
                elementNumber = -1;
                string fname = fieldName.Key;

                int separator = fieldName.Key.IndexOf('[');
                if (separator != -1 &&
                    fieldName.Key[separator] == '[')
                {
                    fname = fieldName.Key.Substring(0, separator);
                    elementNumber = 0;
                    int N = separator + 1;
                    while (fieldName.Key[N] >= '0' && fieldName.Key[N] <= '9')
                    {
                        elementNumber = elementNumber * 10 + fieldName.Key[N] - '0';
                        N++;
                    }
                    if (N - separator < 2 || fieldName.Key[N] != ']')
                    {
                        return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                   $"Invalid field assignment: expected <name>[nnn]=<value>. {fieldName.Key} {printForm.Payload.FormName}",
                                                                   PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldSpecFailure);
                    }
                }

                if (!form.Fields.ContainsKey(fname))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Requested field is invalid. {fieldName.Key} {printForm.Payload.FormName}",
                                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }

                // Check the field is not read - only.
                if (form.Fields[fname].Access == FieldAccessEnum.READ)
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Value supplied for printing in READ only field. {fieldName.Key} {printForm.Payload.FormName}",
                                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }

                // Check field is not static.
                if (form.Fields[fname].Class == FormField.ClassEnum.STATIC)
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Value supplied for printing in STATIC field. {fieldName.Key} {printForm.Payload.FormName}",
                                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }

                // Check for index given on non-indexed field or vice versa.
                if (form.Fields[fname].Repeat == 0 && elementNumber != -1)
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Indexed value supplied for printing in non-indexed field. {fieldName.Key} {printForm.Payload.FormName}",
                                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }
                else if (form.Fields[fname].Repeat != 0)
                {
                    if (elementNumber == -1)
                    {
                        return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                   $"Non-indexed value supplied for printing in indexed field. {fieldName.Key} {printForm.Payload.FormName}",
                                                                   PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                    }

                    if (elementNumber >= form.Fields[fname].Repeat)
                    {
                        return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                   $"Index value supplied larger than field INDEX repeat count. {fieldName.Key} {printForm.Payload.FormName}",
                                                                   PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                    }
                }

                FormField thisField = form.Fields[fname];
                // Check if this field was already assigned
                bool duplicatedValue = false;
                foreach (var field in fieldAssignments)
                {
                    if (field.Field == thisField &&
                        field.ElementIndex == elementNumber)
                    {
                        // Passed in field value is a key-value pair and should not reach here
                        Logger.Warning(Constants.Framework, $"Field supplied duplicated values. Form:{printForm.Payload.FormName} field:{field.Field.Name}");
                        duplicatedValue = true;
                        break;
                    }
                }

                if (!duplicatedValue)
                {
                    // Add a new field assignment
                    FieldAssignment fieldAssignment = new(thisField, elementNumber, fieldName.Value);
                    fieldAssignments.Add(fieldAssignment);
                }
            }

            // This step involves checking all
            // REQUIRED fields have assignments, creating assignments for
            // STATIC fields and filling in OPTIONAL field assignments.
            foreach (var field in form.Fields)
            {
                // If STATIC field, just add a new assignment for the INITIALVALUE
                if (field.Value.Class == FormField.ClassEnum.STATIC)
                {
                    if (field.Value.Repeat > 0)
                    {
                        for (int i = 0; i < field.Value.Repeat; i++)
                        {
                            fieldAssignments.Add(new(field.Value, i, field.Value.InitialValue));
                        }
                    }
                    else
                    {
                        fieldAssignments.Add(new(field.Value, field.Value.InitialValue));
                    }
                    continue;
                }

                // If REQUIRED, just check there is already an assignment for the field.
                if (field.Value.Class == FormField.ClassEnum.REQUIRED)
                {
                    // For index fields, all elements must be assigned: count how many there are in the list.
                    int NoExpectedAssignments = (field.Value.Repeat > 0) ? field.Value.Repeat : 1;
                    foreach (var fieldAssignment in fieldAssignments)
                    {
                        if (fieldAssignment.Field == field.Value)
                            NoExpectedAssignments--;
                    }

                    if (NoExpectedAssignments != 0)
                    {
                        return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                   $"A value or values were not supplied for a REQUIRED field. {printForm.Payload.FormName}",
                                                                   PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                    }
                    continue;
                }

                // Must be an OPTIONAL field: fill in any missing assignments with the
                // INITIALVALUE if there is one.
                (field.Value.Class == FormField.ClassEnum.OPTIONAL).IsTrue($"Unexpected class type received. {field.Value.Class}");

                if (string.IsNullOrEmpty(field.Value.InitialValue))
                {
                    continue;
                }

                for (int elemIndex = (field.Value.Repeat > 0) ? 0 : -1; elemIndex < field.Value.Repeat; elemIndex++)
                {
                    FieldAssignment assignment = new(field.Value, elemIndex, field.Value.InitialValue);
                    // If this assignment isn't already in there, add it.
                    int i = 0;
                    for (; i < fieldAssignments.Count; i++)
                    {
                        if (fieldAssignments[i].Field == assignment.Field &&
                            fieldAssignments[i].ElementIndex == elemIndex)
                        {
                            break;
                        }
                    }
                    if (i == fieldAssignments.Count)
                    {
                        fieldAssignments.Add(assignment);
                    }
                }
            }

            int FormLeftOffset, FormTopOffset;
            // Calculate position of form on media.
            // This depends on the alignment and offsets sent in the command and those in the form.
            {
                var result = CalculateFormOffsets(printForm.Payload, form, media, out FormLeftOffset, out FormTopOffset);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
                }
            }

            // Check length of paper to print this form & store in PrintJob if greater than length needed to print any buffer stuff.
            int PrintLength = 0;
            if (form.Orientation == FormOrientationEnum.PORTRAIT)
            {
                PrintLength = FormTopOffset + form.DotHeight;
            }
            else
            {
                PrintLength = FormLeftOffset + form.DotWidth;
            }

            if (PrintLength > Printer.PrintJob.PrintLength)
            {
                Printer.PrintJob.PrintLength = PrintLength;
            }

            // Calculate assignment print locations
            // Work out the position and dimensions of the rectangle each FieldAssignment
            // must fit within.
            {
                var result = CalculateAssignmentPrintLocations(form, fieldAssignments, FormLeftOffset, FormTopOffset);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
                }
            }

            // About to convert field assignments to Task list to be added to the print job.
            // First check the form orientation is the same as for any buffered
            // tasks. If not, flush buffered tasks.
            if (Printer.PrintJob.Orientation != form.Orientation &&
                Printer.PrintJob.Tasks.Count > 0)
            {
                Logger.Log(Constants.DeviceClass, "PrinterDev.SetPageSize()");
                bool pageSizeResult = Device.SetPageSize(Printer.PrintJob.PrintLength);
                Logger.Log(Constants.DeviceClass, $"PrinterDev.SetPageSize() -> {pageSizeResult}");

                PrintTaskResult result = null;
                if (pageSizeResult)
                {
                    Printer.PrintJob.SortTasks();
                    Logger.Log(Constants.DeviceClass, "PrinterDev.ExecutePrintTasksAsync()");
                    result = await Device.ExecutePrintTasksAsync(new PrintTaskRequest(Printer.PrintJob,
                                                                                      paperSource,
                                                                                      customSource), cancel);
                    Logger.Log(Constants.DeviceClass, $"PrinterDev.ExecutePrintTasksAsync() -> {result.CompletionCode}, {result.ErrorCode}");
                }

                // Clear flushed tasks
                Printer.PrintJob.Tasks.Clear();

                if (result is not null &&
                    result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
                }
                if (!pageSizeResult)
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                               $"Failed to set page size. {Printer.PrintJob.PrintLength}");
                }
            }

            Printer.PrintJob.Orientation = form.Orientation;

            // Convert FieldAssignments to Tasks and add them to the current print job
            // For ROWCOLUMN Forms, all chars must be aligned on ROWCOLUMN boundaries.
            // For other forms don't care.  
            // Calculate RowUnit and ColumnUnit which are minimum char spacing in order to 
            // check & ensure this alignment.
            if (form.Base == Form.BaseEnum.ROWCOLUMN)
            {
                (Device.DotsPerRowTop % Device.DotsPerRowBottom == 0).IsTrue($"Unexpected top and bottom (Row) in dots reported by the device class. DotsPerRowTop:{Device.DotsPerRowTop}, DotsPerRowBottom:{Device.DotsPerRowBottom}");
                (Device.DotsPerColumnTop % Device.DotsPerColumnBottom == 0).IsTrue($"Unexpected top and bottom (Column) in dots reported by the device class. DotsPerColumnTop:{Device.DotsPerColumnTop}, DotsPerColumnBottom:{Device.DotsPerColumnBottom}");
                RowUnit = Device.DotsPerRowTop / Device.DotsPerRowBottom;
                ColumnUnit = Device.DotsPerColumnTop / Device.DotsPerColumnBottom;
            }
            else
            {
                RowUnit = 1;
                ColumnUnit = 1;
            }

            // Loop through assignments processing according to field type.
            foreach (var fieldAssignment in fieldAssignments)
            {
                PrintFormResult result;
                switch (fieldAssignment.Field.Type)
                {
                    case FieldTypeEnum.TEXT:
                        {
                            result = ConvertTextFieldAssignment(fieldAssignment, form);
                        }
                        break;

                    case FieldTypeEnum.GRAPHIC:
                        {
                            // Check format of the image is valid or not
                            result = ConvertGraphicFieldAssignment(fieldAssignment);
                        }
                        break;

                    case FieldTypeEnum.BARCODE:
                        {
                            // Check format of the barcode is valid or not
                            result = ConvertBarcodeFieldAssignment(fieldAssignment);
                        }
                        break;
                    default:
                        {
                            return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                       $"Unsupported field type. {fieldAssignment.Field.Name}",
                                                                       PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                        }
                }

                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    Printer.PrintJob.Tasks.Clear();
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
                }
            }

            // Check the MediaControl.  If non-zero, flush the print job
            if (printForm.Payload.MediaControl?.Flush is not null &&
                !(bool)printForm.Payload.MediaControl?.Flush)
            {
                // No Flush
                return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                           string.Empty);
            }

            if (Printer.PrintJob.Tasks.Count > 0)
            {
                Logger.Log(Constants.DeviceClass, $"PrinterDev.SetPageSize({Printer.PrintJob.PrintLength})");
                bool pageSizeResult = Device.SetPageSize(Printer.PrintJob.PrintLength);
                Logger.Log(Constants.DeviceClass, $"PrinterDev.SetPageSize() -> {pageSizeResult}");

                PrintTaskResult result = null;
                if (pageSizeResult)
                {
                    Printer.PrintJob.SortTasks();
                    Logger.Log(Constants.DeviceClass, $"PrinterDev.ExecutePrintTasksAsync({Printer.PrintJob.PrintLength})");
                    result = await Device.ExecutePrintTasksAsync(new PrintTaskRequest(Printer.PrintJob,
                                                                                      paperSource,
                                                                                      customSource), cancel);
                    Logger.Log(Constants.DeviceClass, $"PrinterDev.ExecutePrintTasksAsync() -> {result.CompletionCode}, {result.ErrorCode}");
                }

                // Clear flushed tasks
                Printer.PrintJob.Tasks.Clear();

                if (result is not null &&
                    result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
                }
                if (!pageSizeResult)
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                               $"Failed to set page size. {Printer.PrintJob.PrintLength}");
                }
            }

            if (printForm.Payload.MediaControl is not null)
            {
                // Now do any other media control requested
                var result = await ExecuteControlMedia(events, printForm.Payload, cancel);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return result;
                }
            }

            return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty);
        }

        /// <summary>
        /// Execute control media after form prining completed successfully
        /// </summary>
        private async Task<PrintFormCompletion.PayloadData> ExecuteControlMedia(IPrintFormEvents events, 
                                                                                PrintFormCommand.PayloadData payload, 
                                                                                CancellationToken cancel)
        {
            PrinterCapabilitiesClass.ControlEnum controls = PrinterCapabilitiesClass.ControlEnum.NotSupported;

            // Capability check
            if (payload.MediaControl.Alarm is not null &&
                (bool)payload.MediaControl.Alarm)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Alarm))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Alarm}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Alarm;
            }
            if (payload.MediaControl.Backward is not null &&
                (bool)payload.MediaControl.Backward)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Backward))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Backward}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Backward;
            }
            if (payload.MediaControl.ClearBuffer is not null &&
                (bool)payload.MediaControl.ClearBuffer)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.ClearBuffer))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.ClearBuffer}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.ClearBuffer;
            }
            if (payload.MediaControl.Cut is not null &&
                (bool)payload.MediaControl.Cut)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Cut))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Cut}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Cut;
            }
            if (payload.MediaControl.Eject is not null &&
                (bool)payload.MediaControl.Eject)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Eject))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Eject}");
                }

                if ((payload.MediaControl.EjectToTransport is not null &&
                    (bool)payload.MediaControl.EjectToTransport) ||
                    (payload.MediaControl.Retract is not null &&
                    (bool)payload.MediaControl.Retract) ||
                    (payload.MediaControl.Park is not null &&
                    (bool)payload.MediaControl.Park) ||
                    (payload.MediaControl.Expel is not null &&
                    (bool)payload.MediaControl.Expel))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Eject control can't combined with other actions, EjectToTransport, Retract, Park or Expel. {payload.MediaControl.Eject}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Eject;
            }
            if (payload.MediaControl.EjectToTransport is not null &&
                (bool)payload.MediaControl.EjectToTransport)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.EjectToTransport))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.EjectToTransport}");
                }

                if ((payload.MediaControl.Eject is not null &&
                    (bool)payload.MediaControl.Eject) ||
                    (payload.MediaControl.Retract is not null &&
                    (bool)payload.MediaControl.Retract) ||
                    (payload.MediaControl.Park is not null &&
                    (bool)payload.MediaControl.Park) ||
                    (payload.MediaControl.Expel is not null &&
                    (bool)payload.MediaControl.Expel))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"EjectToTransport control can't combined with other actions, Eject, Retract, Park or Expel. {payload.MediaControl.EjectToTransport}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.EjectToTransport;
            }
            if (payload.MediaControl.Flush is not null &&
                (bool)payload.MediaControl.Flush)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Flush))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Flush}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Flush;
            }
            if (payload.MediaControl.Forward is not null &&
                (bool)payload.MediaControl.Forward)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Forward))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Forward}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Forward;
            }
            if (payload.MediaControl.Park is not null &&
                (bool)payload.MediaControl.Park)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Park))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Park}");
                }

                if ((payload.MediaControl.Eject is not null &&
                    (bool)payload.MediaControl.Eject) ||
                    (payload.MediaControl.Retract is not null &&
                    (bool)payload.MediaControl.Retract) ||
                    (payload.MediaControl.EjectToTransport is not null &&
                    (bool)payload.MediaControl.EjectToTransport) ||
                    (payload.MediaControl.Expel is not null &&
                    (bool)payload.MediaControl.Expel))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Park control can't combined with other actions, Eject, Retract, EjectToTransport or Expel. {payload.MediaControl.Park}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Park;
            }
            if (payload.MediaControl.PartialCut is not null &&
                (bool)payload.MediaControl.PartialCut)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.PartialCut))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.PartialCut}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.PartialCut;
            }
            if (payload.MediaControl.Perforate is not null &&
                (bool)payload.MediaControl.Perforate)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Perforate))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Perforate}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Perforate;
            }
            if (payload.MediaControl.Retract is not null &&
                (bool)payload.MediaControl.Retract)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Retract}");
                }

                if ((payload.MediaControl.Eject is not null &&
                    (bool)payload.MediaControl.Eject) ||
                    (payload.MediaControl.Park is not null &&
                    (bool)payload.MediaControl.Park) ||
                    (payload.MediaControl.EjectToTransport is not null &&
                    (bool)payload.MediaControl.EjectToTransport) ||
                    (payload.MediaControl.Expel is not null &&
                    (bool)payload.MediaControl.Expel))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Retract control can't combined with other actions, Eject, Park, EjectToTransport or Expel. {payload.MediaControl.Retract}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Retract;
            }
            if (payload.MediaControl.Rotate180 is not null &&
                (bool)payload.MediaControl.Rotate180)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Rotate180))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Rotate180}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Rotate180;
            }
            if (payload.MediaControl.Skip is not null &&
                (bool)payload.MediaControl.Skip)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Skip))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Skip}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Skip;
            }
            if (payload.MediaControl.Stack is not null &&
                (bool)payload.MediaControl.Stack)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stack))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Stack}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Stack;
            }
            if (payload.MediaControl.Stamp is not null &&
                (bool)payload.MediaControl.Stamp)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stamp))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Stamp}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Stamp;

            }
            if (payload.MediaControl.TurnMedia is not null &&
                (bool)payload.MediaControl.TurnMedia)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.TurnMedia))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.TurnMedia}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.TurnMedia;
            }

            if (payload.MediaControl.Expel is not null &&
                (bool)payload.MediaControl.Expel)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Expel))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Unsupported control media specified. {payload.MediaControl.Expel}");
                }

                if ((payload.MediaControl.EjectToTransport is not null &&
                    (bool)payload.MediaControl.EjectToTransport) ||
                    (payload.MediaControl.Retract is not null &&
                    (bool)payload.MediaControl.Retract) ||
                    (payload.MediaControl.Park is not null &&
                    (bool)payload.MediaControl.Park) ||
                    (payload.MediaControl.Retract is not null &&
                    (bool)payload.MediaControl.Retract))
                {
                    return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Expel control can't combined with other actions, EjectToTransport, Retract, Park or Retract. {payload.MediaControl.Expel}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Expel;
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlMediaAsync()");
            var result = await Device.ControlMediaAsync(new ControlMediaEvent(events),
                                                        new ControlMediaRequest(controls),
                                                        cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlMediaAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return new PrintFormCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode switch
                                                           {
                                                               ControlMediaResult.ErrorCodeEnum.BlackMark => PrintFormCompletion.PayloadData.ErrorCodeEnum.BlackMark,
                                                               ControlMediaResult.ErrorCodeEnum.FlushFail => PrintFormCompletion.PayloadData.ErrorCodeEnum.FlushFail,
                                                               ControlMediaResult.ErrorCodeEnum.InkOut => PrintFormCompletion.PayloadData.ErrorCodeEnum.InkOut,
                                                               ControlMediaResult.ErrorCodeEnum.MediaJammed => PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaJammed,
                                                               ControlMediaResult.ErrorCodeEnum.MediaRetained => PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaRetained,
                                                               ControlMediaResult.ErrorCodeEnum.MediaRetracted => PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaRetracted,
                                                               ControlMediaResult.ErrorCodeEnum.MediaTurnFail => PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaTurnFail,
                                                               ControlMediaResult.ErrorCodeEnum.PageTurnFail => PrintFormCompletion.PayloadData.ErrorCodeEnum.PageTurnFail,
                                                               ControlMediaResult.ErrorCodeEnum.PaperJammed => PrintFormCompletion.PayloadData.ErrorCodeEnum.PaperJammed,
                                                               ControlMediaResult.ErrorCodeEnum.PaperOut => PrintFormCompletion.PayloadData.ErrorCodeEnum.PaperOut,
                                                               ControlMediaResult.ErrorCodeEnum.RetractBinFull => PrintFormCompletion.PayloadData.ErrorCodeEnum.RetractBinFull,
                                                               ControlMediaResult.ErrorCodeEnum.SequenceInvalid => PrintFormCompletion.PayloadData.ErrorCodeEnum.SequenceInvalid,
                                                               ControlMediaResult.ErrorCodeEnum.ShutterFail => PrintFormCompletion.PayloadData.ErrorCodeEnum.ShutterFail,
                                                               ControlMediaResult.ErrorCodeEnum.StackerFull => PrintFormCompletion.PayloadData.ErrorCodeEnum.StackerFull,
                                                               ControlMediaResult.ErrorCodeEnum.TonerOut => PrintFormCompletion.PayloadData.ErrorCodeEnum.TonerOut,
                                                               _ => null,
                                                           });
            }

            return new PrintFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty);
        }

        /// <summary>
        /// Check the passed Form/Field is valid for this device.
        /// </summary>
        private PrintFormResult CheckFieldValid(FormField field, Form form)
        {
            FormRules rules = Device.FormRules;
           
            // Check the field is entirely inside the form
            // Do this using printer units not dots: otherwise rounding may cause
            // us to reject valid forms.
            int max_x, max_y;

            if (field.Repeat == 0)
            {
                max_x = field.X + field.Width;
                max_y = field.Y + field.Height;
            }
            else
            {
                max_x = field.X + (field.Repeat - 1) * field.XOffset +
                        field.Width;
                max_y = field.Y + (field.Repeat - 1) * field.YOffset +
                        field.Height;
            }

            if (max_x > form.Width || max_y > form.Height)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"Extent of field is larger than form.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check SIDE
            if (!rules.ValidSide.HasFlag(field.Side))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"SIDE is not valid.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check TYPE
            if (!rules.ValidType.HasFlag(field.Type))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"TYPE is not valid.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check SCALING for GRAPHIC fields
            if (field.Type == FieldTypeEnum.GRAPHIC)
            {
                if (!rules.ValidScaling.HasFlag(field.Scaling))
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"SCALING is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }

                if (string.IsNullOrEmpty(field.Format))
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"FORMAT is not set for graphic field.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }

                if (field.Format == "JPG" &&
                    field.Format == "BMP")
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"FORMAT is not valid for graphic field. {field.Format}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }
            }

            // Check BARCODE for BARCODE fields
            if (field.Type == FieldTypeEnum.BARCODE)
            {
                if (!rules.ValidBarcode.HasFlag(field.Barcode))
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"BARCODE is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                }
            }

            // Check ACCESS
            if (!rules.ValidAccess.HasFlag(field.Access))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"SIDE is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check OVERWRITE
            if (field.Overflow == FormField.OverflowEnum.OVERWRITE)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"OVERFLOW is not supported.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check STYLE
            if (!rules.ValidStyle.HasFlag(field.Style))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"STYLE is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check COLOR
            if (!rules.ValidColor.HasFlag(field.Color))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"COLOR is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check FONT

            if (field.Type == FieldTypeEnum.TEXT)
            {
                if (rules.ValidFonts != "ALL")
                {
                    if (!rules.ValidFonts.Contains(field.Font, StringComparison.OrdinalIgnoreCase))
                    {
                        return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                   $"FONT is not valid.",
                                                   PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                    }
                }
            }

            // Check point size
            if (field.PointSize != -1 &&
                (field.PointSize > rules.MaxPointSize ||
                 field.PointSize < rules.MinPointSize))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"POINTSIZE is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check CPI
            if (field.CPI != -1 &&
                (field.CPI > rules.MaxCPI ||
                 field.CPI < rules.MinCPI))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"CPI is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            // Check LPI
            if (field.LPI != -1 &&
                (field.LPI > rules.MaxLPI ||
                 field.LPI < rules.MinLPI))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"LPI is not valid.",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
            }

            if (field.Class == FormField.ClassEnum.REQUIRED &&
                !string.IsNullOrEmpty(field.InitialValue))
            {
                Logger.Warning(Constants.Framework, $"INITIALVALUE for REQUIRED field ignored. {field.Name}");
            }

            if (field.Class == FormField.ClassEnum.STATIC && 
                string.IsNullOrEmpty(field.InitialValue))
            {
                Logger.Warning(Constants.Framework, $"INITIALVALUE for STATIC field. {field.Name}");
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Check the passed media is valid for this device.
        /// </summary>
        private PrintFormResult CheckMediaValid(Media media)
        {
            List<MediaSpec> mediaSpecs = Device.MediaSpecs;

            int i = 0;
            for (; i < mediaSpecs.Count; i++)
            {
                if (mediaSpecs[i].Width >= media.DotWidth &&
                    (mediaSpecs[i].Height == 0 ||
                     mediaSpecs[i].Height >= media.DotHeight))
                {
                    break;
                }
            }

            // Check width
            if (i == mediaSpecs.Count)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"Size of media is greater than any media supported by the device.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            if (media.Fold != Media.FoldEnum.NONE)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"FOLD not supported for this printer type.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            if (media.Staggering != 0)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"STAGGERING not supported for this printer type.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            if (media.Pages != 0)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"PAGE not supported for this printer type.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            if (media.Lines != 0)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"LINES not supported for this printer type.",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            if (media.PrintAreaX < 0 ||
                media.PrintAreaX + media.PrintAreaWidth > media.Width ||
                media.PrintAreaY < 0 ||
                media.PrintAreaY + media.PrintAreaHeight > media.Height)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"PRINTAREA not within extent of media. PrintAreaX:{media.PrintAreaX}, PrintAreaY:{media.PrintAreaY}",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            if (media.RestrictedAreaX < 0 ||
                media.RestrictedAreaX + media.RestrictedAreaWidth > media.Width ||
                media.RestrictedAreaY < 0 ||
                media.RestrictedAreaY + media.RestrictedAreaHeight > media.Height)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"RESTRICTED area not within extent of media. RestrictedAreaX:{media.RestrictedAreaX}, RestrictedAreaY:{media.RestrictedAreaY}",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaInvalid);
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Calculate where the form should be positioned on the media.
        /// This depends on the offsets and alignment given in the command and those in the form.
        /// Calculated offsets are returned in output parameters.
        /// </summary>
        private PrintFormResult CalculateFormOffsets(PrintFormCommand.PayloadData command, Form form, Media media, out int FormLeftOffset, out int FormTopOffset)
        {
            FormLeftOffset = 0;
            FormTopOffset = 0;

            // Get offsets from command or form, command overriding form.
            // We get them now because they are about to be used.
            // If from command, the units must be converted to dots.
            int XOff, YOff;

            if (command.Alignment != PrintFormCommand.PayloadData.AlignmentEnum.FormDefinition)
            {
                XOff = form.XConvertToDots((int)command.OffsetX);
            }
            else
            {
                XOff = form.DotXOffset;
            }

            if (command.Alignment != PrintFormCommand.PayloadData.AlignmentEnum.FormDefinition)
            {
                YOff = form.YConvertToDots((int)command.OffsetY);
            }
            else
            {
                YOff = form.DotYOffset;
            }

            // Get alignment to use.  Again, command overrides form
            Form.AlignmentEnum alignment = command.Alignment switch
            {
                PrintFormCommand.PayloadData.AlignmentEnum.FormDefinition => form.Alignment,
                PrintFormCommand.PayloadData.AlignmentEnum.TopLeft => Form.AlignmentEnum.TOPLEFT,
                PrintFormCommand.PayloadData.AlignmentEnum.TopRight => Form.AlignmentEnum.TOPRIGHT,
                PrintFormCommand.PayloadData.AlignmentEnum.BottomLeft => Form.AlignmentEnum.BOTTOMLEFT,
                _ => Form.AlignmentEnum.BOTTOMRIGHT,
            };

            // Now calculate x and y position of top left corner of form.
            // The form is offset a given amount from a given corner of the print area
            // In landscape, x for the form is measured from the top
            // of the paper and y from the right hand edge - the form is rotated 90 deg clockwise.
            switch (alignment)
            {
                case Form.AlignmentEnum.TOPLEFT:
                    if (form.Orientation == FormOrientationEnum.PORTRAIT)
                    {
                        FormLeftOffset = media.DotPrintAreaX + XOff;
                        FormTopOffset = media.DotPrintAreaY + YOff;
                    }
                    else
                    {
                        // Left of form aligned relative to top edge of print area
                        // Top of form aligned relative to right hand edge of print area
                        // Right edge of print area is MediaWidth - PrintAreaWidth - PrintAreaX from right hand edge of media
                        FormLeftOffset = media.DotPrintAreaY + XOff;
                        FormTopOffset = (media.DotWidth - media.DotPrintAreaWidth - media.DotPrintAreaX) + YOff;
                    }
                    break;

                case Form.AlignmentEnum.TOPRIGHT:
                    if (form.Orientation == FormOrientationEnum.PORTRAIT)
                    {
                        FormLeftOffset = media.DotPrintAreaX + media.DotPrintAreaWidth - form.DotWidth - XOff;
                        FormTopOffset = media.DotPrintAreaY + YOff;
                    }
                    else
                    {
                        // Right of form aligned relative to bottom edge of print area
                        // Top of form aligned relative to right hand edge of print area
                        // Right edge of print area is MediaWidth - PrintAreaWidth - PrintAreaX from right hand edge of media
                        FormLeftOffset = media.DotPrintAreaY + media.DotPrintAreaHeight - form.DotWidth - XOff;
                        FormTopOffset = (media.DotWidth - media.DotPrintAreaWidth - media.DotPrintAreaX) + YOff;
                    }
                    break;

                case Form.AlignmentEnum.BOTTOMLEFT:
                    if (form.Orientation == FormOrientationEnum.PORTRAIT)
                    {
                        FormLeftOffset = media.DotPrintAreaX + XOff;
                        FormTopOffset = media.DotPrintAreaY + media.DotPrintAreaHeight - form.DotHeight - YOff;
                    }
                    else
                    {
                        // Left of form aligned relative to top edge of print area
                        // Bottom of form aligned relative to left edge of print area
                        FormLeftOffset = media.DotPrintAreaY + XOff;
                        FormTopOffset = media.DotWidth - media.DotPrintAreaX - form.DotHeight - YOff;
                    }
                    break;

                case Form.AlignmentEnum.BOTTOMRIGHT:
                    if (form.Orientation == FormOrientationEnum.PORTRAIT)
                    {
                        FormLeftOffset = media.DotPrintAreaX + media.DotPrintAreaWidth - form.DotWidth - XOff;
                        FormTopOffset = media.DotPrintAreaY + media.DotPrintAreaHeight - form.DotHeight - YOff;
                    }
                    else
                    {
                        // Right of form aligned relative to bottom edge of print area
                        // Bottom of form aligned relative to left edge of print area
                        FormLeftOffset = media.DotPrintAreaY + media.DotPrintAreaHeight - form.DotWidth - XOff;
                        FormTopOffset = media.DotWidth - media.DotPrintAreaX - form.DotHeight - YOff;
                    }
                    break;

                default:
                    {
                        Contracts.Assert(false, $"Unexpected alignment. {alignment}");
                    }
                    break;
            }

            // Check for media overflow: topleft & bottomright of form must be within print area
            if (form.Orientation == FormOrientationEnum.PORTRAIT)
            {
                // To be OK, the following must be >= 0
                int left_ok = FormLeftOffset - media.DotPrintAreaX;
                int top_ok = FormTopOffset - media.DotPrintAreaY;
                int right_ok = media.DotPrintAreaX + media.DotPrintAreaWidth - FormLeftOffset - form.DotWidth;
                int bottom_ok = (media.DotHeight == 0) ?
                                            1 :
                                            media.DotPrintAreaY + media.DotPrintAreaHeight - FormTopOffset - form.DotHeight;

                if (left_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Left edge of form overflows print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }

                if (right_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Right edge of form overflows print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }

                if (top_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Top edge of form overflows print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }

                if (bottom_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Bottom edge of form overflows print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }
            }
            else
            {
                // To be OK, the following must be >= 0
                int left_ok = FormLeftOffset - media.DotPrintAreaY;
                int right_ok = (media.DotHeight == 0) ?
                                1 :
                                media.DotPrintAreaY + media.DotPrintAreaHeight - FormLeftOffset - form.DotWidth;
                int top_ok = FormTopOffset -
                                (media.DotWidth - media.DotPrintAreaWidth - media.DotPrintAreaX);
                int bottom_ok = media.DotWidth - media.DotPrintAreaX - FormTopOffset - form.DotHeight;

                if (left_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Left edge of form overflows top of print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }

                if (right_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Right edge of form overflows bottom of print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }

                if (top_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Top edge of form overflows right of print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }

                if (bottom_ok < 0)
                {
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                               $"Bottom edge of form overflows left of print area. {media.Name}",
                                               PrintFormCompletion.PayloadData.ErrorCodeEnum.MediaOverflow);
                }
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// For each field assignment, calculate the position and size of the rectangular area on the media it must fit within.
        /// This depends on several considerations:
        /// - the form offsets: position of form relative to media
        /// - field position: the position of the field within the form.T
        ///   This position may not be trivial to calculate if the field FOLLOWs some other field.
        /// - the element index of the assignment.
        ///   If the assignment is for one element of an index field, the INDEX offsets must be accounted for.
        /// </summary>
        private PrintFormResult CalculateAssignmentPrintLocations(Form form, List<FieldAssignment> fieldAssignments, int FormLeftOffset, int FormTopOffset)
        {
            foreach (var fieldAssignment in fieldAssignments)
            {
                // Width and Height come straight from Field
                fieldAssignment.Width = fieldAssignment.Field.DotWidth;
                fieldAssignment.Height = fieldAssignment.Field.DotHeight;

                // Get position of element rel to field
                int ElemX = 0;
                int ElemY = 0;
                if (fieldAssignment.ElementIndex > 0)
                {
                    ElemX = fieldAssignment.Field.DotXOffset * fieldAssignment.ElementIndex;
                    ElemY = fieldAssignment.Field.DotYOffset * fieldAssignment.ElementIndex;
                }

                // Get position of field rel to form
                int FieldX = fieldAssignment.Field.DotX;
                int FieldY = fieldAssignment.Field.DotY;
                if (!string.IsNullOrEmpty(fieldAssignment.Field.Follows))
                {
                    var result = CalculateFollowsPosition(form, fieldAssignments, fieldAssignment.Field.Follows, 0, out FieldX, out FieldY);
                    if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                    {
                        return result;
                    }
                }

                // Assignment position rel to media is combination of:
                // - Form pos rel to Media
                // - Field pos rel to Form
                // - Element pos rel to Field
                fieldAssignment.X = FormLeftOffset + FieldX + ElemX;
                fieldAssignment.Y = FormTopOffset + FieldY + ElemY;
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Calculate the X and Y position immediately following a specified field.
        /// If the specified field FOLLOWS some other field, make a recursive call to find the location of that field first.
        /// </summary>
        private PrintFormResult CalculateFollowsPosition(Form form, List<FieldAssignment> fieldAssignments, string followedField, int Depth, out int FieldX, out int FieldY)
        {
            FieldX = 0;
            FieldY = 0;
            // Check recursion depth to detect circular references.
            if (Depth > form.Fields.Count)
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                           $"Circular FOLLOWS references detected in form. {form.Name}",
                                           PrintFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid);
            }

            // Find the FollowedField to get its position
            foreach (var field in form.Fields)
            {
                if (field.Value.Name == followedField)
                {
                    // Get position of the FollowedField - may require recursive call if the 
                    // FollowedField follows some other field.
                    int FFieldX = field.Value.DotX;
                    int FFieldY = field.Value.DotY;

                    if (!string.IsNullOrEmpty(field.Value.Follows))
                    {
                        var result = CalculateFollowsPosition(form, fieldAssignments, field.Value.Follows, Depth + 1, out FFieldX, out FFieldY);
                        if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                        {
                            return result;
                        }
                    }

                    // Get coords after this field.  If its an index field, get coords after
                    // last element for which a value is supplied.
                    FieldX = FFieldX;
                    FieldY = FFieldY + field.Value.DotHeight;

                    if (field.Value.Repeat > 0)
                    {
                        foreach (var fieldAssignment in fieldAssignments)
                        {
                            if (fieldAssignment.Field == field.Value)
                            {
                                int Y = FFieldY + field.Value.DotHeight * fieldAssignment.ElementIndex;
                                if (Y > FieldY)
                                {
                                    FieldY = Y;
                                }
                            }
                        }
                    }
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
                }
            }

            // Followed field not found
            return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                       $"Field in FOLLOWS reference not found. {followedField}",
                                       PrintFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid);
        }

        /// <summary>
        /// Assign dynamic field for TEXT type
        /// </summary>
        private PrintFormResult ConvertTextFieldAssignment(FieldAssignment fieldAssignment, Form form)
        {
            // else another method should have been called
            (fieldAssignment.Field.Type == FieldTypeEnum.TEXT).IsTrue($"Unexpected field type specified. {fieldAssignment.Field.Type} " + nameof(ConvertTextFieldAssignment));

            // If empty text, succeed
            if (string.IsNullOrEmpty(fieldAssignment.Value))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
            }

            // First stage is to split the text into segments that fit into
            // the field box width-wise.  How text can be split (whether only 
            // at word ends or anywhere) depends on the OVERFLOW attribute.
            // See comments at top of file for OVERFLOW interpretation.
            // Calculate text height and width if printed as single line
            var result = GetSingleLineDimensions(fieldAssignment, form, out int SingleWidth, out int SingleHeight);
            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return result;
            }

            // Calculate max number of text lines that fit in the field height
            // Space between lines depends on LPI if specified: if distance
            // from top of one line to top of next is lpi_height, then for
            // N lines of text, we use up (N-1)*lpi_height + SingleHeight dots
            int MaxLines, LPIHeight;

            if (fieldAssignment.Field.LPI == -1)
                LPIHeight = SingleHeight;
            else
            {
                LPIHeight = Device.DotsPerInchTopY / (fieldAssignment.Field.LPI * Device.DotsPerInchBottomY);
                OrientateY(form, ref LPIHeight);
            }

            if (fieldAssignment.Field.DotHeight < SingleHeight)
                MaxLines = 0;
            else
                MaxLines = 1 + (fieldAssignment.Field.DotHeight - SingleHeight) / LPIHeight;

            // Now break up text
            // If OVERFLOW is TRUNCATE, then we can chop the text anywhere
            // for all othervalues, we must try to wrap it first
            bool Chop = (fieldAssignment.Field.Overflow == FormField.OverflowEnum.TRUNCATE);
            List<TextTask> tasks = new();

            result = BreakTextIntoLines(Chop, fieldAssignment, form, MaxLines, out bool Overflow, out int RequiredLines, tasks);
            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return result;
            }

            // If there was an overflow, the action depends on the overflow attribute
            if (Overflow)
            {
                switch (fieldAssignment.Field.Overflow)
                {
                    case FormField.OverflowEnum.TERMINATE:
                        // Try chopping text: if another overflow, return error
                        tasks.Clear();
                        result = BreakTextIntoLines(true, fieldAssignment, form, MaxLines, out Overflow, out RequiredLines, tasks);
                        if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                        {
                            return result;
                        }

                        if (Overflow)
                        {
                            Logger.Warning(Constants.Framework, $"Value too big for field. Value: {fieldAssignment.Value} Field: {fieldAssignment.Field.Name}");
                            PrintFormEvents.FieldErrorEvent(new FieldErrorEvent.PayloadData(form.Name, fieldAssignment.Field.Name, FieldErrorEvent.PayloadData.FailureEnum.Overflow));
                            return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                       $"Value too big for field. Value: {fieldAssignment.Value} Field: {fieldAssignment.Field.Name}",
                                                       PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);
                        }
                        break;

                    case FormField.OverflowEnum.TRUNCATE:
                        // Send a warning if text has been truncated to 0 lines
                        if (tasks.Count == 0)
                        {
                            Logger.Warning(Constants.Framework, $"Value too big for field. Value: {fieldAssignment.Value} Field: {fieldAssignment.Field.Name}");
                        }

                        // Ignore the overflow, but send an error event
                        PrintFormEvents.FieldErrorEvent(new FieldErrorEvent.PayloadData(form.Name, fieldAssignment.Field.Name, FieldErrorEvent.PayloadData.FailureEnum.Overflow));
                        break;

                    case FormField.OverflowEnum.BESTFIT:
                        // Try chop, continue on second overflow
                        tasks.Clear();
                        result = BreakTextIntoLines(true, fieldAssignment, form, MaxLines, out Overflow, out RequiredLines, tasks);
                        if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                        {
                            return result;
                        }

                        if (Overflow)
                        {
                            Logger.Warning(Constants.Framework, $"Value too big for field. Value: {fieldAssignment.Value} Field: {fieldAssignment.Field.Name}");
                            PrintFormEvents.FieldErrorEvent(new FieldErrorEvent.PayloadData(form.Name, fieldAssignment.Field.Name, FieldErrorEvent.PayloadData.FailureEnum.Overflow));
                        }
                        break;

                    case FormField.OverflowEnum.OVERWRITE:
                        // Overwrite not supported
                        Logger.Warning(Constants.Framework, $"OVERFLOW OVERWRITE not fully supported. Field: {fieldAssignment.Field.Name}");
                        return new PrintFormResult(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                   $"OVERFLOW OVERWRITE not fully supported. Field: {fieldAssignment.Field.Name}",
                                                   PrintFormCompletion.PayloadData.ErrorCodeEnum.FieldError);

                    case FormField.OverflowEnum.WORDWRAP:
                        // Just report the error and carry on
                        Logger.Warning(Constants.Framework, $"Value too big for field. Value: {fieldAssignment.Value} Field: {fieldAssignment.Field.Name}");
                        PrintFormEvents.FieldErrorEvent(new FieldErrorEvent.PayloadData(form.Name, fieldAssignment.Field.Name, FieldErrorEvent.PayloadData.FailureEnum.Overflow));
                        break;

                    default:
                        {
                            Contracts.Assert(false, $"Unexpected overflow type received. {fieldAssignment.Field.Overflow}");
                        }
                        break;
                }
            }

            // Second stage is to apply vertical justification
            // Extract the tasks from the temporary manager and mess with the
            // y coordinates
            if (tasks.Count == 0)
            {
                // nothing to print
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
            }

            // Calculate block height
            int block_height = LPIHeight * (tasks.Count - 1) + SingleHeight;

            // Check height of field is sufficient to hold the text: should be assured by earlier processing
            (fieldAssignment.Field.DotHeight >= block_height).IsTrue($"Height of field is not sufficient.");

            // calc amount to shift
            int shift = 0;

            switch (fieldAssignment.Field.Vertical)
            {
                case FormField.VerticalEnum.BOTTOM:
                    // Shift lines so bottom line is aligned with bottom edge of field box.
                    shift = fieldAssignment.Field.DotHeight - block_height;
                    break;

                case FormField.VerticalEnum.CENTER:
                    // Shift lines down to half-way
                    shift = (fieldAssignment.Field.DotHeight - block_height) / 2;
                    // round down to nearest unit
                    shift -= (shift % RowUnit);
                    break;

                case FormField.VerticalEnum.TOP:
                    // already at top so no shift
                    shift = 0;
                    break;

                default:
                    {
                        Contracts.Assert(false, $"Unexpected vertical type received. {fieldAssignment.Field.Vertical}");
                    }
                    break;
            }

            if (shift != 0)
            {
                foreach (var task in tasks)
                {
                    task.y += shift;
                }
            }

            // Third and last stage is to work out horizontal justification
            // For LEFT, RIGHT & CENTER this is quite easy. For JUSTIFY, each
            // line of text must be split into a separate task for each word.

            // Justify all tasks horizontally
            switch (fieldAssignment.Field.Horizontal)
            {
                case FormField.HorizontalEnum.LEFT:
                    // Already left justified so do nothing
                    break;

                case FormField.HorizontalEnum.RIGHT:
                    // Shift each line so its right aligned
                    foreach (var task in tasks)
                    {
                        result = GetTaskDimensions(task, out int width, out int height);
                        if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                        {
                            return result;
                        }

                        task.x = fieldAssignment.X + fieldAssignment.Width - width;
                    }
                    break;

                case FormField.HorizontalEnum.CENTER:
                    // Shift each line so its centred
                    foreach (var task in tasks)
                    {
                        result = GetTaskDimensions(task, out int width, out int height);
                        if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                        {
                            return result;
                        }

                        task.x = fieldAssignment.X + (fieldAssignment.Width - width) / 2;
                        // round down to nearest unit
                        task.x -= (task.x % ColumnUnit);
                    }
                    break;

                case FormField.HorizontalEnum.JUSTIFY:
                    {
                        //////////////////////////////////////////////////
                        // Split each task into a task for each word

                        // Copy tasks to be justified to new vector
                        List<TextTask> unjustifiedTasks = new();

                        foreach (var task in tasks)
                        {
                            unjustifiedTasks.Add(new TextTask(task));
                        }
                        TextTask T = new (tasks[^1]);
                        tasks.Clear();
                        tasks.Add(T);

                        foreach (var unjustifiedTask in unjustifiedTasks)
                        {
                            // last line is not justified
                            result = JustifyTextTask(unjustifiedTask, fieldAssignment, tasks);
                            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                            {
                                return result;
                            }
                        }
                    }
                    break;

                default:
                    {
                        Contracts.Assert(false, $"Unexpected horizontal type received. {fieldAssignment.Field.Horizontal}");
                    }
                    break;
            }

            // Copy Tasks to PrintJob
            foreach (var task in tasks)
                Printer.PrintJob.Tasks.Add(new TextTask(task));
            tasks.Clear();

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// This method justifies one line of text.  The text is passed in
        /// a single task and the field in which the text must fit is also passed.
        /// The single task is split into multiple text tasks, one for each
        /// word and the word tasks are distributed so that they appear
        /// evenly spaced across the whole field width.
        /// </summary>
        private PrintFormResult JustifyTextTask(TextTask task, FieldAssignment fieldAssignment, List<TextTask> tasks)
        {
            // Split task text into separate words
            // and calculate total space taken up by words and
            // number of words

            // Save complete string
            string AllText = task.Text.TrimEnd();
            // working string
            string Text = task.Text.TrimEnd();
            // sum printed width of words
            int TotalWordWidth = 0;
            // number of words to justify
            int NWords = 0;

            while (Text.Length > 0)
            {
                int NextSpace = Text.IndexOf(' ');
                if (NextSpace == -1 || Text == " ")
                {
                    task.Text = Text;
                    Text = "";
                }
                else
                {
                    task.Text = Text.Substring(0, NextSpace);
                    Text = Text[NextSpace..];
                    Text = Text.TrimStart();
                }

                NWords++;
                var result = GetTaskDimensions(task, out int width, out int height);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return result;
                }

                TotalWordWidth += width;
            }

            // Now spread out the words so the space is distributed
            // evenly in the available gaps

            // If not more than one word, can't justify it so just do whole text
            // in one task.
            if (NWords < 2)
            {
                TextTask NewTask = new (task);
                NewTask.Text = AllText;
                tasks.Add(NewTask);
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
            }

            int GapSize = (fieldAssignment.Field.DotWidth - TotalWordWidth) / (NWords - 1);
            // round down to whole number of units
            GapSize -= (GapSize % ColumnUnit);
            int Remainder = (fieldAssignment.Field.DotWidth - TotalWordWidth) - (GapSize * (NWords - 1));
            Text = AllText;
            int XPos = task.x;

            // loop through text again, adding a task for each word
            for (int i = 0; i < NWords; i++)
            {
                TextTask NewTask = new(task);
                // Set x position
                NewTask.x = XPos;

                // Text for task is next word
                int NextSpace = Text.IndexOf(' ');
                if (NextSpace == -1)
                    NewTask.Text = Text;
                else
                {
                    NewTask.Text = Text.Substring(0, NextSpace);
                    Text = Text[NextSpace..];
                    Text = Text.TrimStart();
                }

                // Add the new task to the vector
                tasks.Add(NewTask);

                // Calculate position for next word
                var result = GetTaskDimensions(NewTask, out int width, out int height);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return result;
                }

                XPos += width + GapSize;
                if (Remainder > 0)
                {
                    // distribute remainder over first gaps
                    XPos += ColumnUnit;
                    Remainder -= ColumnUnit;
                }
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// This method breaks the passed field assignment string into separate 
        /// fragments according to the width of the assignment box.
        /// A text printing task is added to the passed task vector for each fragment.
        /// The tasks are created to be correct for LEFT TOP justification.
        /// The correct justification must be applied later.
        /// The method breaks the text either between words (wrapping) or anywhere (chopping).
        /// The method sets an output flag to indicate whether the text could
        /// be fitted into the assignment box or not.
        /// </summary>
        /// <param name="chop">Flag whether text should be chopped or wrapped</param>
        /// <param name="fieldAssignment">Field assignment being processed</param>
        /// <param name="form">XFS form object currently used</param>
        /// <param name="maxLines">Max number of lines that can be printed</param>
        /// <param name="overflow">Output, whether an overflow occurred</param>
        /// <param name="requiredLines">Output for required lines</param>
        /// <param name="tasks">Tasks to be processed</param>
        private PrintFormResult BreakTextIntoLines(bool chop, 
                                                   FieldAssignment fieldAssignment,
                                                   Form form,
                                                   int maxLines, 
                                                   out bool overflow, 
                                                   out int requiredLines, 
                                                   List<TextTask> tasks)
        {
            // Initialise no overflow
            overflow = false;
            requiredLines = 0;

            // Strip CRs: we interpret LF on its own (==\n ==0xA) as newline
            // not a CRLF sequence.  So remove any CRs that we are passed so no
            // spurious chars are printed.
            string Text = fieldAssignment.Value;

            int cr;
            while ((cr = Text.IndexOf('\r')) != -1)
            {
                string Left = Text.Substring(0, cr);
                string Right = Text[(cr + 1)..];
                Text = Left + Right;
            }

            // Case where field is empty
            if (string.IsNullOrEmpty(Text))
            {
                if (maxLines > 0)
                {
                    TextTask task = new(fieldAssignment.X,
                                        fieldAssignment.Y,
                                        Text,
                                        fieldAssignment.Field.Style,
                                        fieldAssignment.Field.Font,
                                        fieldAssignment.Field.PointSize,
                                        fieldAssignment.Field.CPI,
                                        fieldAssignment.Field.LPI,
                                        fieldAssignment.Field.Side,
                                        fieldAssignment.Field.Color,
                                        fieldAssignment.Field.Format,
                                        form.Base == Form.BaseEnum.ROWCOLUMN);

                    // Set up x,y for task - these will be correct 
                    // for LEFT and TOP justification, also row/column
                    // Add the task to temp manager
                    tasks.Add(task);
                }

                overflow = false;

                return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
            }


            // Loop until entire text has been split into tasks, one per line
            // we don't worry about vertical/horizontal attributes for now.
            int Y = fieldAssignment.Y;
            int Len = Text.Length;

            while (Len > 0)
            {
                // Set up current task text - this is a guessed task because
                // the text may not fit in the field box.
                TextTask task = new(fieldAssignment.X,
                                    Y,
                                    Text,
                                    fieldAssignment.Field.Style,
                                    fieldAssignment.Field.Font,
                                    fieldAssignment.Field.PointSize,
                                    fieldAssignment.Field.CPI,
                                    fieldAssignment.Field.LPI,
                                    fieldAssignment.Field.Side,
                                    fieldAssignment.Field.Color,
                                    fieldAssignment.Field.Format,
                                    form.Base == Form.BaseEnum.ROWCOLUMN);

                // Now check if the text fits, and break of the longest substring that does fit.
                var result = FindLongestSubstring(task, chop, fieldAssignment, out int width, out int height);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return result;
                }

                // Check for case where nothing could fit
                if (string.IsNullOrEmpty(task.Text))
                {
                    overflow = true;
                    requiredLines += 100;
                    return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
                }

                // Increment required line count
                requiredLines++;

                // Remove the text in the task from the text left to process
                Text = Text[^(Len - task.Text.Length)..];

                // Add the task to task vector if there is still room for it
                // Otherwise discard it: it doesn't fit
                if (tasks.Count < maxLines)
                {
                    tasks.Add(task);
                }
                else
                {
                    overflow = true;
                }

                // Preprocess text remaining
                int Index = 0;

                if (!chop)
                {
                    while (Index < Text.Length && (Text[Index] is ' ' or '\t')) 
                        Index++;
                    // Note: TrimLeft removes newlines which we don't want to do
                }

                if (Index >= Text.Length)
                    break;

                // If text was cut at new line, skip it
                if (Text[Index] == 0x0A)
                {
                    // LF
                    Index++;
                }

                if (!chop)
                {
                    while (Index < Text.Length && (Text[Index] is ' ' or '\t')) 
                        Index++;
                    // Note: TrimLeft removes newlines which we don't want to do
                }

                if (Index >= Text.Length)
                    break;

                Text = Text[^(Text.Length - Index)..];
                Len = Text.Length;

                // Calculate where next line should go
                // If no lines-per-inch was defined for the field, 
                // use text height to figure out bottom of this line
                // Otherwise, use the LPI
                if (fieldAssignment.Field.LPI == -1 || Len == 0)
                    Y += height;
                else
                {
                    int LPIHeight = Device.DotsPerInchTopY / (fieldAssignment.Field.LPI * Device.DotsPerInchBottomY);
                    OrientateY(form, ref LPIHeight);
                    Y += LPIHeight;
                }
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// This method is passed a task containing text that may be too wide
        /// for the field box.The text is adjusted to contain only that
        /// which can fit and the method returns the width and height of the
        /// contructed task when printed.
        /// </summary>
        /// <param name="task">Task being constructed</param>
        /// <param name="chop">flag whether to chop anywhere (otherwise wrap) </param>
        /// <param name="fieldAssignment">FieldAssignment for this task</param>
        /// <param name="width">returned width of printed text</param>
        /// <param name="height">returned height of printed text</param>
        /// <returns></returns>
        private PrintFormResult FindLongestSubstring(TextTask task,
                                                     bool chop,
                                                     FieldAssignment fieldAssignment,
                                                     out int width,
                                                     out int height)
        {
            // If there's a new line in the string, truncate at newline
            int newline = task.Text.IndexOf('\n'); // LF
            if (newline != -1)
            {
                task.Text = task.Text.Substring(0, newline);
                if (string.IsNullOrEmpty(task.Text))
                {
                    task.Text = " ";
                }
                // otherwise \n\n sequences get reduced to single \n
            }

            // Loop until we find the maximum length sub-string that fits in the field box.
            // This is a trial and error process - if the sting doesn't fit, try a short one
            // If that one fits, try a bit longer etc.
            string AllText = task.Text;
            int Current = AllText.Length;
            int ShortestNonFit = Current + 1;
            int LongestFit = 0;
            // flag first time through
            bool FirstTime = true;

            do
            {
                var result = GetTaskDimensions(task, out width, out height);
                if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                {
                    return result;
                }

                if (width > fieldAssignment.Field.DotWidth)
                {
                    // Update shortest-that-doesn't-fit
                    ShortestNonFit = Current;

                    // The width overflows the field box, chop it
                    // Use a different chopping heuristic first time through
                    if (FirstTime)
                    {
                        // If tried whole string & it didn't fit, use error to figure out what length to try
                        Current = (Current * fieldAssignment.Field.DotWidth) / width;
                        if (Current == 0)
                            Current = 1;

                        task.Text = AllText.Substring(0, Current);
                        FirstTime = false;
                    }
                    else
                    {
                        // After first time, just increment/decrement until we get the right length
                        task.Text = AllText.Substring(0, Current);
                        Current--;
                    }
                }
                else
                {
                    LongestFit = Current;

                    // If first time through, the whole string fits, so break
                    if (FirstTime)
                    {
                        break;
                    }

                    // Else try one char more
                    Current++;
                    task.Text = AllText.Substring(0, Current);
                }

            }
            while (ShortestNonFit - LongestFit > 1);

            task.Text = AllText.Substring(0, LongestFit);

            // If we must wrap the text, make sure we break at a word boundary
            if (!chop)
            {
                if (LongestFit == AllText.Length)
                {
                    // If we put all the text in the task, OK
                }
                else if (AllText[LongestFit] == ' ')
                {
                    // If the text is already broken at a word boundary, OK
                }
                else
                {
                    // Otherwise, try to find previous word boundary
                    int i = LongestFit - 1;

                    while (i > 0 && AllText[i] != ' ')
                        i--;

                    if (i > 0)
                    {
                        task.Text = AllText.Substring(0, i);
                    }
                }

                // Trim any trailing spaces
                task.Text = task.Text.TrimEnd();
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Returns as output parameters, the field width and height needed to
        /// print text as a single line.
        /// </summary>
        private PrintFormResult GetSingleLineDimensions(FieldAssignment fieldAssignment, Form form, out int width, out int height)
        {
            // Set task for all text
            TextTask task = new(0, 0,
                                fieldAssignment.Value,
                                fieldAssignment.Field.Style,
                                fieldAssignment.Field.Font,
                                fieldAssignment.Field.PointSize,
                                fieldAssignment.Field.CPI,
                                fieldAssignment.Field.LPI,
                                fieldAssignment.Field.Side,
                                fieldAssignment.Field.Color,
                                fieldAssignment.Field.Format,
                                (form.Base == Form.BaseEnum.ROWCOLUMN));


            // Get dimensions
            return GetTaskDimensions(task, out width, out height);
        }

        /// <summary>
        /// Get width and height required to print passed task.
        /// Check valid results are returned for row column forms.
        /// </summary>
        private PrintFormResult GetTaskDimensions(PrintTask task, out int width, out int height)
        {
            if (!Device.GetTaskDimensions(task, out width, out height))
            {
                return new PrintFormResult(MessagePayload.CompletionCodeEnum.HardwareError,
                                           $"Return failed on GetTaskDimensions.");
            }

            if (width % ColumnUnit != 0 || height % RowUnit != 0)
            {
                Logger.Warning(Constants.Framework, $"Error in printer specific class: GetTaskDimensions returned Width and Height that were not multiples of ROWCOLUMN size for a ROWCOLUMN text task.");
            }

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Assign barcode field
        /// </summary>
        private PrintFormResult ConvertBarcodeFieldAssignment(FieldAssignment fieldAssignment)
        {

            /*
              BarcodeFontName
             */
            BarcodeTask task = new(
                            fieldAssignment.X,
                            fieldAssignment.Y,
                            fieldAssignment.Value,
                            fieldAssignment.Field.Barcode,
                            fieldAssignment.Field.Font,
                            fieldAssignment.Width,
                            fieldAssignment.Height);
           
            // Add the task to the task manager.
            Printer.PrintJob.Tasks.Add(task);

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Assign graphic field
        /// </summary>
        private PrintFormResult ConvertGraphicFieldAssignment(FieldAssignment fieldAssignment)
        {
            GraphicTask task = new (fieldAssignment.X,
                                    fieldAssignment.Y,
                                    fieldAssignment.Width,
                                    fieldAssignment.Height,
                                    fieldAssignment.Field.Format == "BMP" ? GraphicTask.ImageFormatEnum.BMP : GraphicTask.ImageFormatEnum.JPG,
                                    fieldAssignment.Field.Scaling,
                                    Convert.FromBase64String(fieldAssignment.Value).ToList());

            // Add the task to the task manager.
            Printer.PrintJob.Tasks.Add(task);

            return new PrintFormResult(MessagePayload.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// OrientateX, OrientateY
        /// These methods take a number of dots in the x direction or y direction
        /// respectively, apply the 'orientation factor' and return the updated value.
        /// The orientation factor depends on the orientation of the form.
        /// For PORTRAIT forms, the factor is 1 since the forms x direction is
        /// equivalent to the printer's x direction.
        /// For LANDSCAPE forms, x on the form is y on the printer &amp; vice versa.
        /// So a horizontal form measurement in dots needs to be converted to
        /// the equivalent number of dots in the y direction, bearing in mind the
        /// printer resolution in x &amp; y directions can be different.
        /// </summary>
        private void OrientateX(Form form, ref int ndots)
        {
            if (form.Orientation == FormOrientationEnum.PORTRAIT)
            {
                return;
            }
            int xmmtop = Device.DotsPerMMTopX;
            int xmmbottom = Device.DotsPerMMBottomX;
            int ymmtop = Device.DotsPerMMTopY;
            int ymmbottom = Device.DotsPerMMBottomY;
            ndots = (ndots * ymmtop * xmmbottom) / (ymmbottom * xmmtop);
        }
        private void OrientateY(Form form, ref int ndots)
        {
            if (form.Orientation == FormOrientationEnum.PORTRAIT)
            {
                return;
            }
            int xmmtop = Device.DotsPerMMTopX;
            int xmmbottom = Device.DotsPerMMBottomX;
            int ymmtop = Device.DotsPerMMTopY;
            int ymmbottom = Device.DotsPerMMBottomY;
            ndots = (ndots * xmmtop * ymmbottom) / (xmmbottom * ymmtop);
        }

        /// <summary>
        /// Representing field defined in the form
        /// </summary>
        private sealed class FieldAssignment
        {
            public FieldAssignment(FormField Field,
                                   string Value)
            {
                this.Field = Field;
                ElementIndex = -1;
                this.Value = Value;
                X = -1;
                Y = -1;
                Width = -1;
                Height = -1;
            }
            public FieldAssignment(FormField Field,
                                   int ElementIndex,
                                   string Value)
            {
                this.Field = Field;
                this.ElementIndex = ElementIndex;
                this.Value = Value;
                X = -1;
                Y = -1;
                Width = -1;
                Height = -1;
            }

            public FormField Field { get; init; }
            public int ElementIndex { get; set; }
            public string Value { get; init; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        /// <summary>
        /// Minimum charactor spacing required for row by this device
        /// </summary>
        private int RowUnit { get; set; } = 0;
        /// <summary>
        /// Minimum charactor spacing required for column by this device
        /// </summary>
        private int ColumnUnit { get; set; } = 0;
        /// <summary>
        /// Event interface to be used for an internal functions
        /// </summary>
        private IPrintFormEvents PrintFormEvents { get; set; }
    }
}
