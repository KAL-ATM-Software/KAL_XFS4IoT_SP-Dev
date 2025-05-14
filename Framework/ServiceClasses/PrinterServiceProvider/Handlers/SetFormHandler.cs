/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Printer;
using System.Text.RegularExpressions;
using FieldsClass = XFS4IoT.Printer.Commands.SetFormCommand.PayloadData.FormClass.FieldsClass;
using StyleClass = XFS4IoT.Printer.Commands.SetFormCommand.PayloadData.FormClass.FieldsClass.StyleClass;
using FormClass = XFS4IoT.Printer.Commands.SetFormCommand.PayloadData.FormClass;

namespace XFS4IoTFramework.Printer
{
    public partial class SetFormHandler
    {
        private Task<CommandResult<SetFormCompletion.PayloadData>> HandleSetForm(ISetFormEvents events, SetFormCommand setForm, CancellationToken cancel)
        {
            SetFormCommand.PayloadData.FormClass payloadForm = setForm.Payload.Form;

            if (payloadForm is null)
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No form specified."));
            }
            if (string.IsNullOrEmpty(setForm.Payload.Name))
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No form name specified."));
            }

            // Check required keyword are there
            if (payloadForm.Unit is null)
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Missing required keyword {nameof(payloadForm.Unit)}."));
            }
            else
            {
                if (payloadForm.Unit.X is null ||
                    payloadForm.Unit.Y is null ||
                    payloadForm.Unit.Base is null)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Missing required keyword under {nameof(payloadForm.Unit)} property."));
                }
            }
            if (payloadForm.Size is null)
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Missing required keyword {nameof(payloadForm.Size)}."));
            }
            else
            {
                if (payloadForm.Size.Width is null ||
                    payloadForm.Size.Height is null)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Missing required keyword under {nameof(payloadForm.Size)}property."));
                }
                if (payloadForm.Size.Width < 1 ||
                    payloadForm.Size.Height < 1)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Invalid property value for {nameof(payloadForm.Size.Width)}/{nameof(payloadForm.Size.Height)} property."));
                }
            }
            if (payloadForm.Fields is null ||
                payloadForm.Fields.Count == 0)
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Missing required keyword {nameof(payloadForm.Fields)}."));
            }

            string formVersion = payloadForm.Version?.Version;
            if (formVersion is not null && !Regex.IsMatch(formVersion, "^[1-9][0-9]*(\\.[0-9]+)?$"))
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid version number {formVersion}."));
            }
            string[] version = formVersion?.Split('.');
            (version is null || version.Length == 2).IsTrue($"Invalid version number {formVersion}");

            Form form = new(
                Logger: Logger,
                Device: Device,
                Name: setForm.Payload.Name,
                Base: payloadForm.Unit.Base switch
                {
                    UnitClass.BaseEnum.Inch => Form.BaseEnum.INCH,
                    UnitClass.BaseEnum.Mm => Form.BaseEnum.MM,
                    _ => Form.BaseEnum.ROWCOLUMN,
                },
                UnitX: (payloadForm.Unit.X ?? 0) < 0 ? 0 : (payloadForm.Unit.X ?? 0),
                UnitY: (payloadForm.Unit.Y ?? 0) < 0 ? 0 : (payloadForm.Unit.Y ?? 0),
                Width: (int)payloadForm.Size.Width,
                Height: (int)payloadForm.Size.Height,
                XOffset: payloadForm.Alignment?.X ?? 0,
                YOffset: payloadForm.Alignment?.Y ?? 0,
                VersionMajor: version is null ? null : int.Parse(version[0]),
                VersionMinor: version is null ? null : int.Parse(version[1]),
                Date: payloadForm.Version?.Date,
                Author: payloadForm.Version?.Author,
                Copyright: payloadForm.Copyright,
                Title: payloadForm.Title,
                Comment: payloadForm.Comment,
                Prompt: payloadForm.UserPrompt,
                Skew: payloadForm.Skew ?? 0,
                Alignment: payloadForm.Alignment?.Relative switch
                {
                    FormClass.AlignmentClass.RelativeEnum.TopLeft => Form.AlignmentEnum.TOPLEFT,
                    FormClass.AlignmentClass.RelativeEnum.TopRight => Form.AlignmentEnum.TOPRIGHT,
                    FormClass.AlignmentClass.RelativeEnum.BottomLeft => Form.AlignmentEnum.BOTTOMLEFT,
                    FormClass.AlignmentClass.RelativeEnum.BottomRight => Form.AlignmentEnum.BOTTOMRIGHT,
                    _ => Form.AlignmentEnum.TOPLEFT,
                },
                Orientation: payloadForm.Orientation switch
                {
                    FormClass.OrientationEnum.Landscape => FormOrientationEnum.LANDSCAPE,
                    FormClass.OrientationEnum.Portrait => FormOrientationEnum.PORTRAIT,
                    _ => FormOrientationEnum.PORTRAIT
                });
            
            foreach (var payloadField in payloadForm.Fields)
            {
                if (payloadField.Value is null)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        new(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Missing field value."));
                }

                if (payloadField.Value.Size is null)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        new(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Missing required field {payloadField.Value.Size}."));
                }
                else
                {
                    if (payloadField.Value.Size.Height is null ||
                        payloadField.Value.Size.Width is null)
                    {
                        return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                            new(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Missing required field under ${payloadField.Value.Size} property."));
                    }
                }

                if (payloadField.Value.Position is null)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        new(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Missing required field {payloadField.Value.Position}."));
                }
                else
                {
                    if (payloadField.Value.Position.X is null ||
                        payloadField.Value.Position.Y is null)
                    {
                        return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                            new(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Missing required field under ${payloadField.Value.Position} property."));
                    }
                }

                if (!string.IsNullOrEmpty(payloadField.Value.Color) &&
                    !Regex.IsMatch(payloadField.Value.Color, "^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$"))
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(
                        new(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid color {payloadField.Value.Color}."));
                }

                FieldStyleEnum style = FieldStyleEnum.NORMAL;
                if (payloadField.Value.Style is not null)
                {
                    if (payloadField.Value.Style.Bold ?? false)
                    {
                        style |= FieldStyleEnum.BOLD;
                    }
                    if (payloadField.Value.Style.Italic ?? false)
                    {
                        style |= FieldStyleEnum.ITALIC;
                    }
                    if (payloadField.Value.Style.Underline is not null)
                    {
                        if (payloadField.Value.Style.Underline == StyleClass.UnderlineEnum.Single)
                        {
                            style |= FieldStyleEnum.UNDER;
                        }
                        else if (payloadField.Value.Style.Underline == StyleClass.UnderlineEnum.Double)
                        {
                            style |= FieldStyleEnum.DOUBLEUNDER;
                        }
                    }
                    if (payloadField.Value.Style.Strike is not null)
                    {
                        if (payloadField.Value.Style.Strike == StyleClass.StrikeEnum.Single)
                        {
                            style |= FieldStyleEnum.STRIKETHROUGH;
                        }
                        else if (payloadField.Value.Style.Strike == StyleClass.StrikeEnum.Double)
                        {
                            style |= FieldStyleEnum.DOUBLESTRIKE;
                        }
                    }
                    if (payloadField.Value.Style.Rotate is not null)
                    {
                        if (payloadField.Value.Style.Rotate == StyleClass.RotateEnum.Ninety)
                        {
                            style |= FieldStyleEnum.ROTATE90;
                        }
                        else if (payloadField.Value.Style.Rotate == StyleClass.RotateEnum.TwoSeventy)
                        {
                            style |= FieldStyleEnum.ROTATE270;
                        }
                        else if (payloadField.Value.Style.Rotate == StyleClass.RotateEnum.UpsideDown)
                        {
                            style |= FieldStyleEnum.UPSIDEDOWN;
                        }
                    }
                    if (payloadField.Value.Style.Opaque ?? false)
                    {
                        style |= FieldStyleEnum.OPAQUE;
                    }
                    if (payloadField.Value.Style.Quality is not null)
                    {
                        if (payloadField.Value.Style.Quality == StyleClass.QualityEnum.Letter)
                        {
                            style |= FieldStyleEnum.LETTERQUALITY;
                        }
                        else if (payloadField.Value.Style.Quality == StyleClass.QualityEnum.NearLetter)
                        {
                            style |= FieldStyleEnum.NEARLETTERQUALITY;
                        }
                    }
                    if (payloadField.Value.Style.Script is not null)
                    {
                        if (payloadField.Value.Style.Script == StyleClass.ScriptEnum.Subscript)
                        {
                            style |= FieldStyleEnum.SUBSCRIPT;
                        }
                        else if (payloadField.Value.Style.Script == StyleClass.ScriptEnum.Superscript)
                        {
                            style |= FieldStyleEnum.SUPERSCRIPT;
                        }
                    }
                    if (payloadField.Value.Style.CharacterSpacing is not null)
                    {
                        if (payloadField.Value.Style.CharacterSpacing == StyleClass.CharacterSpacingEnum.Proportional)
                        {
                            style |= FieldStyleEnum.PROPORTIONAL;
                        }
                        else if (payloadField.Value.Style.CharacterSpacing == StyleClass.CharacterSpacingEnum.Condensed)
                        {
                            style |= FieldStyleEnum.CONDENSED;
                        }
                    }
                    if (payloadField.Value.Style.LineSpacing is not null)
                    {
                        if (payloadField.Value.Style.LineSpacing == StyleClass.LineSpacingEnum.Double)
                        {
                            style |= FieldStyleEnum.DOUBLE;
                        }
                        else if (payloadField.Value.Style.LineSpacing == StyleClass.LineSpacingEnum.Triple)
                        {
                            style |= FieldStyleEnum.TRIPLE;
                        }
                        else if (payloadField.Value.Style.LineSpacing == StyleClass.LineSpacingEnum.Quadruple)
                        {
                            style |= FieldStyleEnum.QUADRUPLE;
                        }
                    }
                    if (payloadField.Value.Style.Overscore ?? false)
                    {
                        style |= FieldStyleEnum.OVERSCORE;
                    }
                    if (payloadField.Value.Style.Width is not null)
                    {
                        if (payloadField.Value.Style.Width == StyleClass.WidthEnum.Double)
                        {
                            style |= FieldStyleEnum.DOUBLEHIGH;
                        }
                        else if (payloadField.Value.Style.Width == StyleClass.WidthEnum.Triple)
                        {
                            style |= FieldStyleEnum.TRIPLEHIGH;
                        }
                        else if (payloadField.Value.Style.Width == StyleClass.WidthEnum.Quadruple)
                        {
                            style |= FieldStyleEnum.QUADRUPLEHIGH;
                        }
                    }
                }

                form.AddField(
                    Name: payloadField.Key,
                    X: (int)payloadField.Value.Position.X,
                    Y: (int)payloadField.Value.Position.Y,
                    Width: (int)payloadField.Value.Size.Width,
                    Height: (int)payloadField.Value.Size.Height,
                    Follows: payloadField.Value.Follows,
                    Repeat: payloadField.Value.Index?.RepeatCount ?? 0,
                    XOffset: payloadField.Value.Index?.X ?? 0,
                    YOffset: payloadField.Value.Index?.Y ?? 0,
                    Font: payloadField.Value.Font?.Name,
                    PointSize: payloadField.Value.Font?.PointSize ?? -1,
                    CPI: payloadField.Value.Font?.Cpi ?? -1,
                    LPI: payloadField.Value.Font?.Lpi ?? -1,
                    Format: payloadField.Value.Format,
                    InitialValue: payloadField.Value.InitialValue,
                    Side: payloadField.Value.Side switch
                    {
                        FieldsClass.SideEnum.Front => FieldSideEnum.FRONT,
                        FieldsClass.SideEnum.Back => FieldSideEnum.BACK,
                        _ => FieldSideEnum.FRONT,
                    },
                    Type: payloadField.Value.FieldType switch
                    {
                        FieldsClass.FieldTypeEnum.Text => FieldTypeEnum.TEXT,
                        FieldsClass.FieldTypeEnum.Barcode => FieldTypeEnum.BARCODE,
                        FieldsClass.FieldTypeEnum.Pagemark => FieldTypeEnum.PAGEMARK,
                        FieldsClass.FieldTypeEnum.Ocr => FieldTypeEnum.OCR,
                        FieldsClass.FieldTypeEnum.Msf => FieldTypeEnum.MSF,
                        FieldsClass.FieldTypeEnum.Micr => FieldTypeEnum.MICR,
                        FieldsClass.FieldTypeEnum.Graphic => FieldTypeEnum.GRAPHIC,
                        _ => FieldTypeEnum.TEXT,
                    },
                    Class: payloadField.Value.Class switch
                    {
                        FieldsClass.ClassEnum.Static => FormField.ClassEnum.STATIC,
                        FieldsClass.ClassEnum.Required => FormField.ClassEnum.REQUIRED,
                        _ => FormField.ClassEnum.OPTIONAL,
                    },
                    Access: payloadField.Value.Access switch
                    {
                        FieldsClass.AccessEnum.Read => FieldAccessEnum.READ,
                        FieldsClass.AccessEnum.ReadWrite => FieldAccessEnum.READWRITE,
                        _ => FieldAccessEnum.WRITE,
                    },
                    Overflow: payloadField.Value.Overflow switch
                    {
                        FieldsClass.OverflowEnum.WordWrap => FormField.OverflowEnum.WORDWRAP,
                        FieldsClass.OverflowEnum.Truncate => FormField.OverflowEnum.TRUNCATE,
                        FieldsClass.OverflowEnum.Overwrite => FormField.OverflowEnum.OVERWRITE,
                        FieldsClass.OverflowEnum.BestFit => FormField.OverflowEnum.BESTFIT,
                        _ => FormField.OverflowEnum.TERMINATE,
                    },
                    Style: style,
                    Case: payloadField.Value.Case switch
                    {
                        FieldsClass.CaseEnum.Upper => FormField.CaseEnum.UPPER,
                        FieldsClass.CaseEnum.Lower => FormField.CaseEnum.LOWER,
                        _ => FormField.CaseEnum.NOCHANGE,
                    },
                    Horizontal: payloadField.Value.Horizontal switch
                    {
                        FieldsClass.HorizontalEnum.Right => FormField.HorizontalEnum.RIGHT,
                        FieldsClass.HorizontalEnum.Justify => FormField.HorizontalEnum.JUSTIFY,
                        FieldsClass.HorizontalEnum.Center => FormField.HorizontalEnum.CENTER,
                        _ => FormField.HorizontalEnum.LEFT,
                    },
                    Vertical: payloadField.Value.Vertical switch
                    {
                        FieldsClass.VerticalEnum.Top => FormField.VerticalEnum.TOP,
                        FieldsClass.VerticalEnum.Center => FormField.VerticalEnum.CENTER,
                        _ => FormField.VerticalEnum.BOTTOM,
                    },
                    Color: payloadField.Value.Color switch
                    {
                        "red" => FieldColorEnum.RED,
                        "blue" => FieldColorEnum.BLUE,
                        "gray" => FieldColorEnum.GRAY,
                        "white" => FieldColorEnum.WHITE,
                        "green" => FieldColorEnum.GREEN,
                        "yellow" => FieldColorEnum.YELLOW,
                        _ => FieldColorEnum.BLACK,
                    },
                    Scaling: payloadField.Value.Scaling switch
                    {
                        FieldsClass.ScalingEnum.MaintainAspect => FieldScalingEnum.MAINTAINASPECT,
                        FieldsClass.ScalingEnum.AsIs => FieldScalingEnum.ASIS,
                        _ => FieldScalingEnum.BESTFIT,
                    },
                    Barcode: payloadField.Value.Barcode switch
                    {
                        FieldsClass.BarcodeEnum.Above => FieldBarcodeEnum.ABOVE,
                        FieldsClass.BarcodeEnum.Below => FieldBarcodeEnum.BELOW,
                        FieldsClass.BarcodeEnum.Both => FieldBarcodeEnum.BOTH,
                        _ => FieldBarcodeEnum.NONE,
                    }
                    );
            }

            // Validate form and field agaist the form rule reported by the device class.
            var valid = form.ValidateForm(Device);
            if (valid.Result != ValidationResultClass.ValidateResultEnum.Valid)
            {
                return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(new SetFormCompletion.PayloadData(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid), MessageHeader.CompletionCodeEnum.CommandErrorCode, $"Invalid form: {valid.Reason}"));
            }
            foreach (var field in form.Fields)
            {
                valid = form.ValidateField(field.Key, Device);
                if (valid.Result != ValidationResultClass.ValidateResultEnum.Valid)
                {
                    return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(new SetFormCompletion.PayloadData(SetFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid), MessageHeader.CompletionCodeEnum.CommandErrorCode, $"Invalid field: {field.Key} {valid.Reason}"));
                }
            }

            Printer.SetForm(setForm.Payload.Name, form);

            return Task.FromResult<CommandResult<SetFormCompletion.PayloadData>>(new(MessageHeader.CompletionCodeEnum.Success));
        }
    }
}
