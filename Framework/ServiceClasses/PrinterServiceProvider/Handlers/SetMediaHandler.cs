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
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Printer;
using System.Text.RegularExpressions;
using MediaClass = XFS4IoT.Printer.Commands.SetMediaCommand.PayloadData.MediaClass;


namespace XFS4IoTFramework.Printer
{
    public partial class SetMediaHandler
    {

        private Task<CommandResult<SetMediaCompletion.PayloadData>> HandleSetMedia(ISetMediaEvents events, SetMediaCommand setMedia, CancellationToken cancel)
        {
            SetMediaCommand.PayloadData.MediaClass payloadMedia = setMedia.Payload.Media;

            if (payloadMedia is null)
            {
                return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No media specified."));
            }
            if (string.IsNullOrEmpty(setMedia.Payload.Name))
            {
                return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No media name specified."));
            }

            // Check required keyword are there
            if (payloadMedia.Unit is null)
            {
                return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Missing required keyword {nameof(payloadMedia.Unit)}."));
            }
            else
            {
                if (payloadMedia.Unit.X is null ||
                    payloadMedia.Unit.Y is null ||
                    payloadMedia.Unit.Base is null)
                {
                    return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Missing required keyword under {nameof(payloadMedia.Unit)} property."));
                }
            }
            if (payloadMedia.Size is null)
            {
                return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Missing required keyword {nameof(payloadMedia.Size)}."));
            }
            else
            {
                if (payloadMedia.Size.Width is null ||
                    payloadMedia.Size.Height is null)
                {
                    return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Missing required keyword under {nameof(payloadMedia.Size)} property."));
                }

                if (payloadMedia.Size.Width  < 1 ||
                    payloadMedia.Size.Height < 1)
                {
                    return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Invalid property value for {nameof(payloadMedia.Size.Width)}/{nameof(payloadMedia.Size.Height)} property."));
                }
            }

            Media.SourceEnum mediaSource = Media.SourceEnum.ANY;
            string customSource = null;
            if (payloadMedia.Source is not null)
            {
                if (!Regex.IsMatch(payloadMedia.Source, "^any$|^upper$|^lower$|^external$|^aux$|^aux2$|^park$|^[a-zA-Z]([a-zA-Z0-9]*)$"))
                {
                    return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid value specified to the {nameof(payloadMedia.Source)}."));
                }
                mediaSource = payloadMedia.Source switch
                {
                    "any" => Media.SourceEnum.ANY,
                    "aux" => Media.SourceEnum.AUX,
                    "park" => Media.SourceEnum.PARK,
                    "aux2" => Media.SourceEnum.AUX2,
                    "external" => Media.SourceEnum.EXTERNAL,
                    "lower" => Media.SourceEnum.LOWER,
                    "upper" => Media.SourceEnum.UPPER,
                    _ => Media.SourceEnum.CUSTOM,
                };

                if (mediaSource == Media.SourceEnum.CUSTOM)
                {
                    customSource = payloadMedia.Source;
                }
            }

            // Convert media object accordingly
            Media media = new(
                Logger,
                Device,
                setMedia.Payload.Name,
                payloadMedia.MediaType switch
                {
                    MediaClass.MediaTypeEnum.Generic => Media.TypeEnum.GENERIC,
                    MediaClass.MediaTypeEnum.Passbook => Media.TypeEnum.PASSBOOK,
                    MediaClass.MediaTypeEnum.Multipart => Media.TypeEnum.MULTIPART,
                    _ => Media.TypeEnum.GENERIC,
                },
                mediaSource,
                payloadMedia.Unit.Base switch
                {
                    UnitClass.BaseEnum.Inch => Media.BaseEnum.INCH,
                    UnitClass.BaseEnum.Mm => Media.BaseEnum.MM,
                    UnitClass.BaseEnum.RowColumn => Media.BaseEnum.ROWCOLUMN,
                    _ => Media.BaseEnum.ROWCOLUMN,
                },
                (payloadMedia.Unit.X ?? 0) < 0 ? 0 : (payloadMedia.Unit.X ?? 0),
                (payloadMedia.Unit.Y ?? 0) < 0 ? 0 : (payloadMedia.Unit.Y ?? 0),
                (int)payloadMedia.Size.Width,
                (int)payloadMedia.Size.Height,
                payloadMedia.PrintArea?.X ?? 0,
                payloadMedia.PrintArea?.Y ?? 0,
                payloadMedia.PrintArea?.Width ?? -1,
                payloadMedia.PrintArea?.Height ?? -1,
                payloadMedia.Restricted?.X ?? 0,
                payloadMedia.Restricted?.Y ?? 0,
                payloadMedia.Restricted?.Width ?? -1,
                payloadMedia.Restricted?.Height ?? -1,
                payloadMedia.Fold is null ? Media.FoldEnum.NONE : payloadMedia.Fold switch
                {
                    MediaClass.FoldEnum.Horizontal => Media.FoldEnum.HORIZONTAL,
                    MediaClass.FoldEnum.Vertical => Media.FoldEnum.VERTICAL,
                    _ => Media.FoldEnum.NONE,
                },
                payloadMedia.Staggering ?? 0,
                payloadMedia.Page ?? 0,
                payloadMedia.Lines ?? 0,
                customSource);

            // Validate media agaist the media rule reported by the device class.
            var valid = media.ValidateMedia(Device);
            if (valid.Result != ValidationResultClass.ValidateResultEnum.Valid)
            {
                return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(new SetMediaCompletion.PayloadData(SetMediaCompletion.PayloadData.ErrorCodeEnum.MediaInvalid), MessageHeader.CompletionCodeEnum.CommandErrorCode, $"Invalid media: {valid.Reason}"));
            }

            Printer.SetMedia(setMedia.Payload.Name, media);

            return Task.FromResult<CommandResult<SetMediaCompletion.PayloadData>>(new(MessageHeader.CompletionCodeEnum.Success));
        }
    }
}
