/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net.WebSockets;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace TestClientForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxServiceURI.Text = "ws://localhost";
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private async void AcceptCard_Click(object sender, EventArgs e)
        {
            var cardReader = new XFS4IoTClient.ClientConnection(new Uri($"{textBoxCardReader.Text}"));

            try
            {
                await cardReader.ConnectAsync();
            }
            catch (Exception)
            {
                return;
            }

            var readRawDataCmd = new ReadRawDataCommand(
                Guid.NewGuid().ToString(),
                new ReadRawDataCommand.PayloadData(
                    Timeout: CommandTimeout,
                    Track1: true,
                    Track2: true,
                    Track3: true,
                    Chip: false,
                    Security: false,
                    FluxInactive: false,
                    Watermark: false,
                    MemoryChip: false,
                    Track1Front: false,
                    FrontImage: false,
                    BackImage: false,
                    Track1JIS: false,
                    Track3JIS: false,
                    Ddi: false));

            textBoxCommand.Text = readRawDataCmd.Serialise();

            await cardReader.SendCommandAsync(readRawDataCmd);

            textBoxResponse.Text = string.Empty;
            textBoxEvent.Text = string.Empty;

            for (; ; )
            {
                object cmdResponse = await cardReader.ReceiveMessageAsync();
                if (cmdResponse is ReadRawDataCompletion response)
                {
                    textBoxResponse.Text = response.Serialise();
                    break;
                }
                else if (cmdResponse is XFS4IoT.CardReader.Events.MediaInsertedEvent cardInsertedEv)
                {
                    textBoxEvent.Text = cardInsertedEv.Serialise();
                }
                else if (cmdResponse is XFS4IoT.CardReader.Events.InsertCardEvent insertCardEv)
                {
                    textBoxEvent.Text = insertCardEv.Serialise();
                }
                else
                {
                    textBoxEvent.Text += "<Unknown Event>";
                }
            }
        }

        private async void EjectCard_Click(object sender, EventArgs e)
        {
            var cardReader = new XFS4IoTClient.ClientConnection(new Uri($"{textBoxCardReader.Text}"));

            try
            {
                await cardReader.ConnectAsync();
            }
            catch (Exception)
            {
                return;
            }

            var ejectCmd = new EjectCardCommand(
                Guid.NewGuid().ToString(), new EjectCardCommand.PayloadData(
                    Timeout: CommandTimeout,
                    EjectPosition: EjectCardCommand.PayloadData.EjectPositionEnum.ExitPosition));

            textBoxCommand.Text = ejectCmd.Serialise();

            await cardReader.SendCommandAsync(ejectCmd);

            textBoxResponse.Text = string.Empty;
            textBoxEvent.Text = string.Empty;

            while (true)
            {
                switch (await cardReader.ReceiveMessageAsync())
                {
                    case EjectCardCompletion response:
                        textBoxResponse.Text = response.Serialise();
                        if (response.Payload.CompletionCode != XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success)
                            return;
                        break;

                    case XFS4IoT.CardReader.Events.MediaRemovedEvent removedEv:
                        textBoxEvent.Text += removedEv.Serialise();
                        return;

                    default:
                        textBoxEvent.Text += "<Unknown Event>";
                        break;
                }
            }
        }

        private async void buttonStatus_Click(object sender, EventArgs e)
        {
            var cardReader = new XFS4IoTClient.ClientConnection(new Uri($"{textBoxCardReader.Text}"));

            try
            {
                await cardReader.ConnectAsync();
            }
            catch (Exception)
            {
                return;
            }

            var statusCmd = new StatusCommand(Guid.NewGuid().ToString(), new StatusCommand.PayloadData(CommandTimeout));
            textBoxCommand.Text = statusCmd.Serialise();

            await cardReader.SendCommandAsync(statusCmd);

            textBoxResponse.Text = string.Empty;
            textBoxEvent.Text = string.Empty;

            object cmdResponse = await cardReader.ReceiveMessageAsync();
            if (cmdResponse is StatusCompletion response)
            {
                textBoxResponse.Text = response.Serialise();
                textBoxStDevice.Text = response.Payload?.Common?.Device?.ToString();
                textBoxStMedia.Text = response.Payload?.CardReader?.Media?.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var cardReader = new XFS4IoTClient.ClientConnection(new Uri($"{textBoxCardReader.Text}"));

            try
            {
                await cardReader.ConnectAsync();
            }
            catch (Exception)
            {
                return;
            }

            var capabilitiesCmd = new CapabilitiesCommand(Guid.NewGuid().ToString(), new CapabilitiesCommand.PayloadData(CommandTimeout));
            textBoxCommand.Text = capabilitiesCmd.Serialise();

            await cardReader.SendCommandAsync(capabilitiesCmd);

            textBoxResponse.Text = string.Empty;
            textBoxEvent.Text = string.Empty;

            object cmdResponse = await cardReader.ReceiveMessageAsync();
            if (cmdResponse is CapabilitiesCompletion response)
            {
                textBoxResponse.Text = response.Serialise();
                textBoxDeviceType.Text = response.Payload.CardReader.Type.ToString();
            }
        }

        private async void ServiceDiscovery_Click(object sender, EventArgs e)
        {
            int[] PortRanges = new int[]
            {
                80,  // Only for HTTP
                443, // Only for HTTPS
                5846,
                5847,
                5848,
                5849,
                5850,
                5851,
                5852,
                5853,
                5854,
                5855,
                5856
            };

            string commandString = string.Empty;
            string responseString = string.Empty;
            string cardServiceURI = string.Empty;

            textBoxCommand.Text = commandString;
            textBoxResponse.Text = responseString;
            textBoxCardReader.Text = cardServiceURI;
            textBoxEvent.Text = string.Empty;

            ServicePort = null;


            foreach (int port in PortRanges)
            {
                try
                {
                    WebSocketState state;
                    using (var socket = new ClientWebSocket())
                    {
                        var cancels = new CancellationTokenSource();
                        cancels.CancelAfter(40_000);
                        await socket.ConnectAsync(new Uri($"{textBoxServiceURI.Text}:{port}/xfs4iot/v1.0"), cancels.Token);
                        state = socket.State;
                    }

                    if (state == WebSocketState.Open)
                    {
                        ServicePort = port;
                        var Discovery = new XFS4IoTClient.ClientConnection(new Uri($"{textBoxServiceURI.Text}:{ServicePort}/xfs4iot/v1.0"));

                        try
                        {
                            await Discovery.ConnectAsync();
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        var getServiceCommand = new GetServiceCommand(Guid.NewGuid().ToString(), new GetServiceCommand.PayloadData(CommandTimeout));
                        commandString = getServiceCommand.Serialise();
                        await Discovery.SendCommandAsync(getServiceCommand);

                        object cmdResponse = await Discovery.ReceiveMessageAsync();
                        if (cmdResponse is GetServiceCompletion response)
                        {
                            responseString = response.Serialise();
                            var service =
                                (from ep in response.Payload.Services
                                 where ep.ServiceUri.Contains("CardReader")
                                 select ep
                                ).FirstOrDefault()
                                ?.ServiceUri;

                            if (!string.IsNullOrEmpty(service))
                                cardServiceURI = service;
                        }
                        break;
                    }
                }
                catch (WebSocketException)
                { }
                catch (System.Net.HttpListenerException)
                { }
                catch (TaskCanceledException)
                { }
            }

            if (ServicePort is null)
            {
                textBoxPort.Text = "";
                MessageBox.Show("Failed on finding services.");
            }
            else
                textBoxPort.Text = ServicePort.ToString();

            textBoxCommand.Text = commandString;
            textBoxResponse.Text = responseString;
            textBoxCardReader.Text = cardServiceURI;
        }

        int? ServicePort = null;
        readonly int CommandTimeout = 60000;
    }
}
