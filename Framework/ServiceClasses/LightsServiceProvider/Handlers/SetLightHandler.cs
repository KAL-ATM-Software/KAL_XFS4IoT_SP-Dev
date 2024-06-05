/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Lights
{
    public partial class SetLightHandler
    {

        private async Task<SetLightCompletion.PayloadData> HandleSetLight(ISetLightEvents events, SetLightCommand setLight, CancellationToken cancel)
        {
            if ((setLight.Payload.ExtendedProperties is null ||
                 setLight.Payload.ExtendedProperties.Count == 0) &&
                setLight.Payload.NotesDispenser is null &&
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
                return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"No light component specified.");
            }

            Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> stdLights = new();
            if (setLight.Payload.NotesDispenser is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.NotesDispenser}");
                }

                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.NotesDispenser,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.NotesDispenser.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.NotesDispenser.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.NotesDispenser.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.NotesDispenser.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.NotesDispenser2 is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser2))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.NotesDispenser2}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.BillAcceptor2,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.NotesDispenser2.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.NotesDispenser2.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.NotesDispenser2.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.NotesDispenser2.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.BillAcceptor is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.BillAcceptor}");
                }

                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.BillAcceptor,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.BillAcceptor.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.BillAcceptor.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.BillAcceptor.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.BillAcceptor.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.BillAcceptor2 is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.BillAcceptor2}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.BillAcceptor2,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.BillAcceptor2.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.BillAcceptor2.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.BillAcceptor2.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.BillAcceptor2.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.CardReader is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CardReader}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.CardReader,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.CardReader.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.CardReader.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.CardReader.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.CardReader.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.CardReader2 is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader2))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CardReader2}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.CardReader2,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.CardReader2.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.CardReader2.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.CardReader2.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.CardReader2.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.Scanner is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Scanner))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.Scanner}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.Scanner,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.Scanner.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.Scanner.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.Scanner.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.Scanner.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.PassbookPrinter is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.PassbookPrinter))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.PassbookPrinter}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.PassbookPrinter,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.PassbookPrinter.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.PassbookPrinter.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.PassbookPrinter.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.PassbookPrinter.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.PinPad is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.PinPad))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.PinPad}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.PinPad,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.PinPad.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.PinPad.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.PinPad.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.PinPad.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.Contactless is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Contactless))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.Contactless}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.Contactless,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.Contactless.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.Contactless.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.Contactless.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.Contactless.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.CoinAcceptor is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CoinAcceptor}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.CoinAcceptor,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.CoinAcceptor.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.CoinAcceptor.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.CoinAcceptor.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.CoinAcceptor.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.CoinDispenser is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinDispenser))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CoinDispenser}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.CoinDispenser,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.CoinDispenser.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.CoinDispenser.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.CoinDispenser.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.CoinDispenser.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.ReceiptPrinter is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.ReceiptPrinter.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.ReceiptPrinter.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.ReceiptPrinter.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.ReceiptPrinter.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.DocumentPrinter is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.DocumentPrinter}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.DocumentPrinter,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.DocumentPrinter.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.DocumentPrinter.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.DocumentPrinter.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.DocumentPrinter.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.EnvelopeDepository is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.EnvelopeDepository.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.EnvelopeDepository.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.EnvelopeDepository.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.EnvelopeDepository.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.EnvelopeDispenser is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.EnvelopeDispenser.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.EnvelopeDispenser.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.EnvelopeDispenser.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.EnvelopeDispenser.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.StatusBad is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.StatusBad.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.StatusBad.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.StatusBad.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.StatusBad.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.StatusGood is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.StatusGood.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.StatusGood.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.StatusGood.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.StatusGood.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.StatusWarning is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.StatusWarning.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.StatusWarning.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.StatusWarning.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.StatusWarning.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.StatusSupervisor is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.StatusSupervisor.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.StatusSupervisor.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.StatusSupervisor.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.StatusSupervisor.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.StatusInService is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.StatusInService.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.StatusInService.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.StatusInService.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.StatusInService.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.FasciaLight is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.FasciaLight))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.FasciaLight}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.FasciaLight,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.FasciaLight.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.FasciaLight.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.FasciaLight.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.FasciaLight.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }
            if (setLight.Payload.CheckUnit is not null)
            {
                if (!Common.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CheckUnit))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CheckUnit}");
                }
                stdLights.Add(
                    LightsCapabilitiesClass.DeviceEnum.CheckUnit,
                    new LightsStatusClass.LightOperation
                    (
                        setLight.Payload.CheckUnit.Position switch
                        {
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                            XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                            _ => LightsStatusClass.LightOperation.PositionEnum.Default
                        },
                        setLight.Payload.CheckUnit.FlashRate switch
                        {
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                            XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                            _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                        },
                        setLight.Payload.CheckUnit.Color switch
                        {
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                            XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                            _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                        },
                        setLight.Payload.CheckUnit.Direction switch
                        {
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                            XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                            _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                        }
                    ));
            }

            Dictionary<string, LightsStatusClass.LightOperation> customLights = null;
            if (setLight.Payload.ExtendedProperties?.Count > 0)
            {
                customLights = [];
                foreach (var custom in setLight.Payload.ExtendedProperties)
                {

                    if (!Common.LightsCapabilities.CustomLights.ContainsKey(custom.Key))
                    {
                        return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported custom light specified. {custom.Key}");
                    }

                    customLights.Add(custom.Key,
                        new LightsStatusClass.LightOperation
                        (
                            custom.Value.Position switch
                            {
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom => LightsStatusClass.LightOperation.PositionEnum.Bottom,
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Center => LightsStatusClass.LightOperation.PositionEnum.Center,
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Front => LightsStatusClass.LightOperation.PositionEnum.Front,
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Left => LightsStatusClass.LightOperation.PositionEnum.Left,
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Rear => LightsStatusClass.LightOperation.PositionEnum.Rear,
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Right => LightsStatusClass.LightOperation.PositionEnum.Right,
                                XFS4IoT.Lights.LightStateClass.PositionEnum.Top => LightsStatusClass.LightOperation.PositionEnum.Top,
                                _ => LightsStatusClass.LightOperation.PositionEnum.Default
                            },
                            custom.Value.FlashRate switch
                            {
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                                _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                            },
                            custom.Value.Color switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColorEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColorEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColorEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColorEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                                _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                            },
                            custom.Value.Direction switch
                            {
                                XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                                XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                                _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                            }
                        ));
                }
            }
            Logger.Log(Constants.DeviceClass, "LightsDev.SetLightAsync()");
            var result = await Device.SetLightAsync(new SetLightRequest(stdLights, customLights),
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"LightsDev.SetLightAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new SetLightCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode);
        }
    }
}
