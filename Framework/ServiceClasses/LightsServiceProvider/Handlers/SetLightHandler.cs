/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Lights.Commands;
using XFS4IoT.Lights.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Lights
{
    public partial class SetLightHandler
    {

        private async Task<CommandResult<SetLightCompletion.PayloadData>> HandleSetLight(ISetLightEvents events, SetLightCommand setLight, CancellationToken cancel)
        {
            if (setLight.Payload.NotesDispenser is null &&
                setLight.Payload.NotesDispenser2 is null &&
                setLight.Payload.BillAcceptor is null &&
                setLight.Payload.BillAcceptor2 is null &&
                setLight.Payload.CardReader is null &&
                setLight.Payload.CardReader2 is null &&
                setLight.Payload.Scanner is null &&
                setLight.Payload.PassbookPrinter is null &&
                setLight.Payload.PinPad is null &&
                setLight.Payload.Contactless is null &&
                setLight.Payload.CoinAcceptor is null &&
                setLight.Payload.CoinDispenser is null &&
                setLight.Payload.ReceiptPrinter is null &&
                setLight.Payload.DocumentPrinter is null &&
                setLight.Payload.CheckUnit is null &&
                setLight.Payload.EnvelopeDepository is null &&
                setLight.Payload.EnvelopeDispenser is null &&
                setLight.Payload.StatusGood is null &&
                setLight.Payload.StatusBad is null &&
                setLight.Payload.StatusWarning is null &&
                setLight.Payload.StatusSupervisor is null &&
                setLight.Payload.StatusInService is null &&
                setLight.Payload.FasciaLight is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No light component specified.");
            }

            Dictionary<LightsCapabilitiesClass.DeviceEnum, List<LightsStatusClass.LightOperation>> stdLights = [];
            if (setLight.Payload.NotesDispenser is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.NotesDispenser}");
                }
                
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.NotesDispenser)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        "default" => LightsStatusClass.LightOperation.PositionEnum.Default,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.NotesDispenser, operations);
            }
            if (setLight.Payload.NotesDispenser2 is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser2))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.NotesDispenser2}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.NotesDispenser2)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.NotesDispenser2, operations);
            }
            if (setLight.Payload.BillAcceptor is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.BillAcceptor}");
                }

                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.BillAcceptor)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.BillAcceptor, operations);
            }
            if (setLight.Payload.BillAcceptor2 is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.BillAcceptor2}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.BillAcceptor2)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2, operations);
            }
            if (setLight.Payload.CardReader is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CardReader}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.CardReader)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);
                    
                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CardReader, operations);
            }
            if (setLight.Payload.CardReader2 is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader2))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CardReader2}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.CardReader2)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CardReader2, operations);
            }
            if (setLight.Payload.Scanner is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Scanner))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.Scanner}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.Scanner)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.Scanner, operations);
            }
            if (setLight.Payload.PassbookPrinter is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.PassbookPrinter))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.PassbookPrinter}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.PassbookPrinter)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.PassbookPrinter, operations);
            }
            if (setLight.Payload.PinPad is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.PinPad))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.PinPad}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.PinPad)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.PinPad, operations);
            }
            if (setLight.Payload.Contactless is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Contactless))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.Contactless}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.Contactless)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.Contactless, operations);
            }
            if (setLight.Payload.CoinAcceptor is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CoinAcceptor}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.CoinAcceptor)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor, operations);
            }
            if (setLight.Payload.CoinDispenser is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinDispenser))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CoinDispenser}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.CoinDispenser)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CoinDispenser, operations);
            }
            if (setLight.Payload.ReceiptPrinter is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.ReceiptPrinter)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter, operations);
            }
            if (setLight.Payload.DocumentPrinter is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.DocumentPrinter}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.DocumentPrinter)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter, operations);
            }
            if (setLight.Payload.EnvelopeDepository is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.EnvelopeDepository)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository, operations);
            }
            if (setLight.Payload.EnvelopeDispenser is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.EnvelopeDispenser)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);
                
                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser, operations);
            }
            if (setLight.Payload.StatusBad is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.StatusBad)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator, operations);
            }
            if (setLight.Payload.StatusGood is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.StatusGood)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator, operations);
            }
            if (setLight.Payload.StatusWarning is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.StatusWarning)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator, operations);
            }
            if (setLight.Payload.StatusSupervisor is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.StatusSupervisor)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator, operations);
            }
            if (setLight.Payload.StatusInService is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.StatusInService)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator, operations);
            }
            if (setLight.Payload.FasciaLight is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.FasciaLight))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.FasciaLight}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.FasciaLight)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.FasciaLight, operations);
            }
            if (setLight.Payload.CheckUnit is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CheckUnit))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CheckUnit}");
                }
                List<LightsStatusClass.LightOperation> operations = [];
                foreach (var position in setLight.Payload.CheckUnit)
                {
                    if (string.IsNullOrEmpty(position.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"An empty position name specified.");
                    }

                    var lightPosition = position.Key switch
                    {
                        "bottom" => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                        "center" => LightsStatusClass.LightOperation.PositionEnum.Center,
                        "front" => LightsStatusClass.LightOperation.PositionEnum.Front,
                        "left" => LightsStatusClass.LightOperation.PositionEnum.Left,
                        "rear" => LightsStatusClass.LightOperation.PositionEnum.Rear,
                        "right" => LightsStatusClass.LightOperation.PositionEnum.Right,
                        "top" => LightsStatusClass.LightOperation.PositionEnum.Top,
                        _ => LightsStatusClass.LightOperation.PositionEnum.Custom,
                    };
                    LightsStatusClass.LightOperation operation = new(
                        Position: lightPosition,
                        Colour: position.Value.Color switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.PositionStatusClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        FlashRate: position.Value.FlashRate switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.PositionStatusClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        Direction: position.Value.Direction switch
                        {
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.PositionStatusClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        },
                        CustomPosition: lightPosition == LightsStatusClass.LightOperation.PositionEnum.Custom ? position.Key : null);

                    operations.Add(operation);
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CheckUnit, operations);
            }

            Dictionary<string, List<LightsStatusClass.LightOperation>> customLights = null;
            // CUSTOM LIGHTS IS NOT SUPPORTED BY CONVERTER NEED A FIX

            Logger.Log(Constants.DeviceClass, "LightsDev.SetLightAsync()");
            var result = await Device.SetLightAsync(new SetLightRequest(stdLights, customLights),
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"LightsDev.SetLightAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
