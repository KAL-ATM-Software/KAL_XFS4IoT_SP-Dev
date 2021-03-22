/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardReader;
using XFS4IoT;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace KAL.XFS4IoTSP.CardReader.Simulator
{
    public class SimulatedCardReader : ICardReaderDevice
    {
        public SimulatedCardReader(ILogger Logger)
        {
            Logger.IsNotNull($"Invalid parameter received in the {nameof(SimulatedCardReader)} constructor. {nameof(Logger)}");
            this.Logger = Logger;
        }


        public async Task<ReadRawDataCompletion.PayloadData> ReadRawData(ICardReaderConnection connection, 
                                                                                       ReadRawDataCommand.PayloadData payload,
                                                                                       CancellationToken cancellation)
        {
            await Task.Delay(2000, cancellation);
            connection.MediaInsertedEvent();

            await Task.Delay(1000, cancellation);

            MediaStatus = StatusCompletion.PayloadData.CardReaderClass.MediaEnum.Present;

            return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, "ok", null,
                new ReadRawDataCompletion.PayloadData.Track1Class(ReadRawDataCompletion.PayloadData.Track1Class.StatusEnum.Ok, "B1234567890123456^SMITH/JOHN.MR^020945852301200589800568000000"),
                new ReadRawDataCompletion.PayloadData.Track2Class(ReadRawDataCompletion.PayloadData.Track2Class.StatusEnum.Ok, "1234567890123456=0209458523012005898"),
                new ReadRawDataCompletion.PayloadData.Track3Class(ReadRawDataCompletion.PayloadData.Track3Class.StatusEnum.Ok, "011234567890123456==000667788903609640040000006200013010000020000098120209105123==00568000999999"));
        }


        public void Enable() { }

        public Task<FormListCompletion.PayloadData> FormList(ICardReaderConnection connection,
                                                                           FormListCommand.PayloadData payload, 
                                                                           CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<QueryFormCompletion.PayloadData> QueryForm(ICardReaderConnection connection, 
                                                                             QueryFormCommand.PayloadData payload, 
                                                                             CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<QueryIFMIdentifierCompletion.PayloadData> QueryIFMIdentifier(ICardReaderConnection connection, 
                                                                                 QueryIFMIdentifierCommand.PayloadData payload,
                                                                                 CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<EMVClessQueryApplicationsCompletion.PayloadData> EMVClessQueryApplications(ICardReaderConnection connection, 
                                                                                               EMVClessQueryApplicationsCommand.PayloadData payload,
                                                                                               CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ReadTrackCompletion.PayloadData> ReadTrack(ICardReaderConnection connection, 
                                                               ReadTrackCommand.PayloadData payload,
                                                               CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<WriteTrackCompletion.PayloadData> WriteTrack(ICardReaderConnection connection, 
                                                                 WriteTrackCommand.PayloadData payload,
                                                                 CancellationToken cancellation) => throw new System.NotImplementedException();
        public async Task<EjectCardCompletion.PayloadData> EjectCard(ICardReaderConnection connection,
                                                                     EjectCardCommand.PayloadData payload,
                                                                     CancellationToken cancellation)
        {
            await Task.Delay(1000, cancellation);

            MediaStatus = StatusCompletion.PayloadData.CardReaderClass.MediaEnum.Entering;

            return new EjectCardCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                       "ok");
        }

        public Task<RetainCardCompletion.PayloadData> RetainCard(ICardReaderConnection connection,
                                                                 RetainCardCommand.PayloadData payload,
                                                                 CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ResetCountCompletion.PayloadData> ResetCount(ICardReaderConnection connection, 
                                                                 ResetCountCommand.PayloadData payload,
                                                                 CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<SetKeyCompletion.PayloadData> SetKey(ICardReaderConnection connection,
                                                         SetKeyCommand.PayloadData payload,
                                                         CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<WriteRawDataCompletion.PayloadData> WriteRawData(ICardReaderConnection connection,
                                                                     WriteRawDataCommand.PayloadData payload,
                                                                     CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ChipIOCompletion.PayloadData> ChipIO(ICardReaderConnection connection,
                                                         ChipIOCommand.PayloadData payload,
                                                         CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ResetCompletion.PayloadData> Reset(ICardReaderConnection connection,
                                                       ResetCommand.PayloadData payload,
                                                       CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ChipPowerCompletion.PayloadData> ChipPower(ICardReaderConnection connection,
                                                               ChipPowerCommand.PayloadData payload,
                                                               CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ParseDataCompletion.PayloadData> ParseData(ICardReaderConnection connection,
                                                               ParseDataCommand.PayloadData payload,
                                                               CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<ParkCardCompletion.PayloadData> ParkCard(ICardReaderConnection connection,
                                                             ParkCardCommand.PayloadData payload,
                                                             CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<EMVClessConfigureCompletion.PayloadData> EMVClessConfigure(ICardReaderConnection connection,
                                                                               EMVClessConfigureCommand.PayloadData payload,
                                                                               CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<EMVClessPerformTransactionCompletion.PayloadData> EMVClessPerformTransaction(ICardReaderConnection connection,
                                                                                                 EMVClessPerformTransactionCommand.PayloadData payload,
                                                                                                  CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<EMVClessIssuerUpdateCompletion.PayloadData> EMVClessIssuerUpdate(ICardReaderConnection connection,
                                                                                     EMVClessIssuerUpdateCommand.PayloadData payload,
                                                                                     CancellationToken cancellation) => throw new System.NotImplementedException();
        

        
        public Task<StatusCompletion.PayloadData> Status(ICommonConnection connection,
                                                                   StatusCommand.PayloadData payload,
                                                                   CancellationToken cancellation)
        {
            StatusCompletion.PayloadData.CommonClass common = new StatusCompletion.PayloadData.CommonClass(
                StatusCompletion.PayloadData.CommonClass.DeviceEnum.Online,
                new List<string>(), 
                new StatusCompletion.PayloadData.CommonClass.GuideLightsClass(
                    StatusCompletion.PayloadData.CommonClass.GuideLightsClass.FlashRateEnum.Off,
                    StatusCompletion.PayloadData.CommonClass.GuideLightsClass.ColorEnum.Green,
                    StatusCompletion.PayloadData.CommonClass.GuideLightsClass.DirectionEnum.Off),
                StatusCompletion.PayloadData.CommonClass.DevicePositionEnum.Inposition,
                0,
                StatusCompletion.PayloadData.CommonClass.AntiFraudModuleEnum.Ok);

            StatusCompletion.PayloadData.CardReaderClass cardReader = new StatusCompletion.PayloadData.CardReaderClass(
                MediaStatus,
                StatusCompletion.PayloadData.CardReaderClass.RetainBinEnum.Ok,
                StatusCompletion.PayloadData.CardReaderClass.SecurityEnum.NotSupported,
                0,
                StatusCompletion.PayloadData.CardReaderClass.ChipPowerEnum.PoweredOff,
                StatusCompletion.PayloadData.CardReaderClass.ChipModuleEnum.Ok,
                StatusCompletion.PayloadData.CardReaderClass.MagWriteModuleEnum.Ok,
                StatusCompletion.PayloadData.CardReaderClass.FrontImageModuleEnum.Ok,
                StatusCompletion.PayloadData.CardReaderClass.BackImageModuleEnum.Ok,
                new List<string>());

            return Task.FromResult(new StatusCompletion.PayloadData(StatusCompletion.PayloadData.CompletionCodeEnum.Success,
                                                                    "ok",
                                                                    common,
                                                                    cardReader));
        }

        public Task<CapabilitiesCompletion.PayloadData> Capabilities(ICommonConnection connection, 
                                                                     CapabilitiesCommand.PayloadData payload,
                                                                     CancellationToken cancellation)
        {
            CapabilitiesCompletion.PayloadData.CommonClass.GuideLightsClass guideLights = new CapabilitiesCompletion.PayloadData.CommonClass.GuideLightsClass(
                new CapabilitiesCompletion.PayloadData.CommonClass.GuideLightsClass.FlashRateClass(true, true, true, true),
                new CapabilitiesCompletion.PayloadData.CommonClass.GuideLightsClass.ColorClass(true, true, true, true, true, true, true),
                new CapabilitiesCompletion.PayloadData.CommonClass.GuideLightsClass.DirectionClass(false, false));

            CapabilitiesCompletion.PayloadData.CommonClass common = new CapabilitiesCompletion.PayloadData.CommonClass(
                "1.0",
                new CapabilitiesCompletion.PayloadData.CommonClass.DeviceInformationClass(
                    "Simulator",
                    "123456-78900001",
                    "1.0",
                    "KAL simualtor",
                    new CapabilitiesCompletion.PayloadData.CommonClass.DeviceInformationClass.FirmwareClass(
                    "XFS4 SP",
                    "1.0",
                    "1.0"),
                    new CapabilitiesCompletion.PayloadData.CommonClass.DeviceInformationClass.SoftwareClass(
                    "XFS4 SP",
                    "1.0",
                    "1.0")),
                new CapabilitiesCompletion.PayloadData.CommonClass.VendorModeIformationClass(
                    true,
                    new List<string>() { "ReadRawData", "EjectCard" }),
                new List<string>() { "MediaInsertedEvent", "MediaRemovedEvent" },
                guideLights,
                false,
                false,
                new List<string>(),
                false,
                false,
                false);

            CapabilitiesCompletion.PayloadData.CardReaderClass cardReader = new CapabilitiesCompletion.PayloadData.CardReaderClass(
                CapabilitiesCompletion.PayloadData.CardReaderClass.TypeEnum.Motor,
                new CapabilitiesCompletion.PayloadData.CardReaderClass.ReadTracksClass(true, true, true, false, false, false, false, false, false, false),
                new CapabilitiesCompletion.PayloadData.CardReaderClass.WriteTracksClass(true, true, true, false, false, false),
                new CapabilitiesCompletion.PayloadData.CardReaderClass.ChipProtocolsClass(true, true, false, false, false, false, false),
                100,
                CapabilitiesCompletion.PayloadData.CardReaderClass.SecurityTypeEnum.NotSupported,
                CapabilitiesCompletion.PayloadData.CardReaderClass.PowerOnOptionEnum.NoAction,
                CapabilitiesCompletion.PayloadData.CardReaderClass.PowerOffOptionEnum.NoAction,
                false, false, 
                new CapabilitiesCompletion.PayloadData.CardReaderClass.WriteModeClass(false, true, true, true),
                new CapabilitiesCompletion.PayloadData.CardReaderClass.ChipPowerClass(false, true, true, true),
                new CapabilitiesCompletion.PayloadData.CardReaderClass.MemoryChipProtocolsClass(true, true),
                new CapabilitiesCompletion.PayloadData.CardReaderClass.EjectPositionClass(true, false),
                0);


            List<CapabilitiesCompletion.PayloadData.InterfacesClass> interfaces = new List<CapabilitiesCompletion.PayloadData.InterfacesClass>
            {
                new CapabilitiesCompletion.PayloadData.InterfacesClass(
                    CapabilitiesCompletion.PayloadData.InterfacesClass.NameEnum.Common,
                    new List<string>(){ "Status", "Capabilities" },
                    new List<string>(),
                    1000,
                    new List<string>()),
                new CapabilitiesCompletion.PayloadData.InterfacesClass(
                    CapabilitiesCompletion.PayloadData.InterfacesClass.NameEnum.CardReader,
                    new List<string>{ "ReadRawData", "EjectCard", "Reset" },
                    new List<string>{ "MediaDetectedEvent", "MediaInsertedEvent", "MediaRemovedEvent", "MediaRetainedEvent", "InvalidMediaEvent" },
                    1000,
                    new List<string>())
            };

            return Task.FromResult(new CapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                          "ok",
                                                                          interfaces,
                                                                          common,
                                                                          cardReader));
        }

        public Task<SetGuidanceLightCompletion.PayloadData> SetGuidanceLight(ICommonConnection connection,
                                                                             SetGuidanceLightCommand.PayloadData payload,
                                                                             CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<PowerSaveControlCompletion.PayloadData> PowerSaveControl(ICommonConnection connection,
                                                                             PowerSaveControlCommand.PayloadData payload,
                                                                             CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<SynchronizeCommandCompletion.PayloadData> SynchronizeCommand(ICommonConnection connection,
                                                                                 SynchronizeCommandCommand.PayloadData payload,
                                                                                 CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<SetTransactionStateCompletion.PayloadData> SetTransactionState(ICommonConnection connection,
                                                                                   SetTransactionStateCommand.PayloadData payload,
                                                                                   CancellationToken cancellation) => throw new System.NotImplementedException();
        public Task<GetTransactionStateCompletion.PayloadData> GetTransactionState(ICommonConnection connection,
                                                                                   GetTransactionStateCommand.PayloadData payload,
                                                                                   CancellationToken cancellation) => throw new System.NotImplementedException();

        public async Task WaitForCardTaken(ICardReaderConnection connection, CancellationToken cancellation)
        {
            await Task.Delay(1000, cancellation);

            MediaStatus = StatusCompletion.PayloadData.CardReaderClass.MediaEnum.NotPresent;

            connection.MediaRemovedEvent();
        }

        public ILogger Logger { get; }


        private StatusCompletion.PayloadData.CardReaderClass.MediaEnum MediaStatus = StatusCompletion.PayloadData.CardReaderClass.MediaEnum.NotPresent;
    }
}