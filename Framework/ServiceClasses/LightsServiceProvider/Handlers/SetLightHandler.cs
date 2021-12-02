/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
                 setLight.Payload.ExtendedProperties.Count > 0) &&
                setLight.Payload.BillAcceptor is null &&
                setLight.Payload.BillAcceptor2 is null &&
                setLight.Payload.CardReader is null &&
                setLight.Payload.CardUnit2 is null &&
                setLight.Payload.ChequeUnit is null &&
                setLight.Payload.CoinAcceptor is null &&
                setLight.Payload.CoinDispenser is null &&
                setLight.Payload.Contactless is null &&
                setLight.Payload.DocumentPrinter is null &&
                setLight.Payload.EnvelopeDepository is null &&
                setLight.Payload.EnvelopeDispenser is null &&
                setLight.Payload.FasciaLight is null)
            {
                return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"No light component specified.");
            }

            Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> stdLights = new();
            
            if (setLight.Payload.BillAcceptor is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.BillAcceptor}");
                }

                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.BillAcceptor,
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
                            setLight.Payload.BillAcceptor.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.BillAcceptor2}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2,
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
                            setLight.Payload.BillAcceptor2.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CardReader}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CardReader,
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
                            setLight.Payload.CardReader.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
            if (setLight.Payload.CardUnit2 is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardUnit2))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CardUnit2}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CardUnit2,
                        new LightsStatusClass.LightOperation
                        (
                            setLight.Payload.CardUnit2.Position switch
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
                            setLight.Payload.CardUnit2.FlashRate switch
                            {
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                                _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                            },
                            setLight.Payload.CardUnit2.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                                _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                            },
                            setLight.Payload.CardUnit2.Direction switch
                            {
                                XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                                XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                                _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                            }
                        ));
            }
            if (setLight.Payload.ChequeUnit is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.ChequeUnit))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.ChequeUnit}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.BillAcceptor,
                        new LightsStatusClass.LightOperation
                        (
                            setLight.Payload.ChequeUnit.Position switch
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
                            setLight.Payload.ChequeUnit.FlashRate switch
                            {
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
                                XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
                                _ => LightsStatusClass.LightOperation.FlashRateEnum.Off,
                            },
                            setLight.Payload.ChequeUnit.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
                                _ => LightsStatusClass.LightOperation.ColourEnum.Default,
                            },
                            setLight.Payload.ChequeUnit.Direction switch
                            {
                                XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry => LightsStatusClass.LightOperation.DirectionEnum.Entry,
                                XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit => LightsStatusClass.LightOperation.DirectionEnum.Exit,
                                _ => LightsStatusClass.LightOperation.DirectionEnum.None,
                            }
                        ));
            }
            if (setLight.Payload.CoinAcceptor is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CoinAcceptor}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor,
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
                            setLight.Payload.CoinAcceptor.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinDispenser))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.CoinDispenser}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.CoinDispenser,
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
                            setLight.Payload.CoinDispenser.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
            if (setLight.Payload.Contactless is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Contactless))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.Contactless}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.Contactless,
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
                            setLight.Payload.Contactless.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
            if (setLight.Payload.DocumentPrinter is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.DocumentPrinter}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter,
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
                            setLight.Payload.DocumentPrinter.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository,
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
                            setLight.Payload.EnvelopeDepository.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser,
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
                            setLight.Payload.EnvelopeDispenser.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
            if (setLight.Payload.FasciaLight is not null)
            {
                if (!Lights.LightsCapabilities.Lights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.FasciaLight))
                {
                    return new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Unsupported light specified. {LightsCapabilitiesClass.DeviceEnum.FasciaLight}");
                }
                stdLights.Add(LightsCapabilitiesClass.DeviceEnum.FasciaLight,
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
                            setLight.Payload.FasciaLight.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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

            Dictionary<string, LightsStatusClass.LightOperation> customLights = null;
            if (setLight.Payload.ExtendedProperties?.Count > 0)
            {
                customLights = new();
                foreach (var custom in setLight.Payload.ExtendedProperties)
                {

                    if (!Lights.LightsCapabilities.CustomLights.ContainsKey(custom.Key))
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
                            custom.Value.Colour switch
                            {
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Blue => LightsStatusClass.LightOperation.ColourEnum.Blue,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan => LightsStatusClass.LightOperation.ColourEnum.Cyan,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Green => LightsStatusClass.LightOperation.ColourEnum.Green,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta => LightsStatusClass.LightOperation.ColourEnum.Magenta,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Red => LightsStatusClass.LightOperation.ColourEnum.Red,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.White => LightsStatusClass.LightOperation.ColourEnum.White,
                                XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow => LightsStatusClass.LightOperation.ColourEnum.Yellow,
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
