/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * ICardReaderDevice.cs uses automatically generated parts. 
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoT.Completions;

// KAL specific implementation of cardreader. 
namespace XFS4IoTFramework.CardReader
{
    /// <summary>
    /// Interface defnition for the device specific class
    /// </summary>
    public interface ICardReaderDevice : IDevice
    {
        /// <summary>
        /// For motor driven card readers, the card unit checks whether a card has been inserted. 
        /// All specified tracks are read immediately if the device can read with the low level accept command and store read data in the device specific class and set data on the ReadCardData method call.
        /// If reading the chip is requested, the chip will be contacted and reset and the ATR (AnswerTo Reset) data will be read. 
        /// When this command completes the chip will be in contacted position. This command can also be used for an explicit cold reset of a previously contacted chip.
        /// This command should only be used for user cards and should not be used for permanently connected chips.
        /// If no card has been inserted, and for all other categories of card readers, the card unit waits for the period of time specified in the call for a card to be either inserted or pulled through.
        /// The InsertCardEvent will be generated when there is no card in the cardreader and the device is ready to accept a card.
        /// </summary>
        Task<AcceptCardResult> AcceptCardAsync(IAcceptCardEvents events,
                                               AcceptCardRequest acceptCardInfo,
                                               CancellationToken cancellation);

        /// <summary>
        /// Read alltracks specified.
        /// All specified tracks are read immediately or 
        /// A security check via a security module(i.e. MM, CIM86) can be requested. If the security check fails however this should not stop valid data beingreturned.
        /// The response securityFail will be returned if the command specifies only security data to be read andthe security check could not be executed, in all other cases ok will be returned with the data field of theoutput parameter set to the relevant value including *hardwareError*.
        /// For non-motorized Card Readers which read track data on card exit, the invalidData error code is returned whena call to is made to read both track data and chip data.
        /// If the card unit is a latched dip unit then the device will latch the card when the chip card will be read, 
        /// The card will remain latched until a call to EjectCard is made.
        /// For contactless chip card readers a collision of two or more card signals may happen. 
        /// In this case, if the deviceis not able to pick the strongest signal, errorCardCollision will be returned.
        /// </summary>
        Task<ReadCardResult> ReadCardAsync(IReadRawDataEvents events,
                                           ReadCardRequest dataToRead,
                                           CancellationToken cancellation);

        /// <summary>
        /// The device is ready to accept a card.
        /// The application must pass the magnetic stripe data in ASCII without any sentinels. 
        /// If the data passed in is too long. The invalidError error code will be returned.
        /// This procedure is followed by data verification.
        /// If power fails during a write the outcome of the operation will be vendor specific, there is no guarantee that thewrite will have succeeded.
        /// </summary>
        Task<WriteCardResult> WriteCardAsync(IWriteRawDataEvents events,
                                             WriteCardRequest dataToWrite,
                                             CancellationToken cancellation);

        /// <summary>
        /// This command is only applicable to motor driven card readers and latched dip card readers.
        /// For motorized card readers the default operation is that the card is driven to the exit slot from where the usercan remove it.
        /// The card remains in position for withdrawal until either it is taken or another command is issuedthat moves the card.
        /// For latched dip readers, this command causes the card to be unlatched (if not already unlatched), enablingremoval.
        /// After successful completion of this command, a CardReader.MediaRemovedEvent is generated to inform the application when the card is taken.
        /// </summary>
        Task<EjectCardResult> EjectCardAsync(EjectCardRequest ejectCardInfo, 
                                             CancellationToken cancellation);

        /// <summary>
        /// The card is removed from its present position (card inserted into device, card entering, unknown position) and stored in the retain bin;
        /// applicable to motor-driven card readers only.
        /// The ID card unit sends a CardReader.RetainBinThresholdEvent if the storage capacity of the retainbin is reached.
        /// If the storage capacity has already been reached, and the command cannot be executed, an error isreturned and the card remains in its present position.
        /// </summary>
        Task<CaptureCardResult> CaptureCardAsync(IRetainCardEvents events, 
                                                 CancellationToken cancellation);

        /// <summary>
        /// This command is used to communicate with the chip.
        /// Transparent data is sent from the application to the chip andthe response of the chip is returned transparently to the application.
        /// The identification information e.g. ATR of the chip must be obtained before issuing this command. 
        /// The identification information for a user card or the Memory Card Identification (when available) must initially be obtained using CardReader.ReadRawData.
        /// The identification information for subsequentresets of a user card can be obtained using either CardReader.ReadRawDat or CardReader.ChipPower. 
        /// The ATR for permanent connected chips is always obtained through CardReader.ChipPower.
        /// For contactless chip card readers, applications need to specify which chip to contact with, as part of chipData, if more than one chip has been detected and multiple identification data has been returned by the CardReader.ReadRawData command.
        /// For contactless chip card readers a collision of two or more card signals may happen. 
        /// In this case, if the deviceis not able to pick the strongest signal, the cardCollision error code will be returned.
        /// </summary>
        Task<ChipIOResult> ChipIOAsync(ChipIORequest dataToSend,
                                       CancellationToken cancellation);

        /// <summary>
        /// This command is used by the application to perform a hardware reset which will attempt to return the card readerdevice to a known good state.
        /// This command does not over-ride a lock obtained by another application or service handle.
        /// If the device is a user ID card unit, the device will attempt to either retain, eject or will perform no action onany user cards found in the device as specified in the input parameter. 
        /// It may not always be possible to retain oreject the items as specified because of hardware problems. 
        /// If a user card is found inside the device the CardReader.MediaInsertedEvent will inform the application where card wasactually moved to.
        /// If no action is specified the user card will not be moved even if this means that the devicecannot be recovered.
        /// If the device is a permanent chip card unit, this command will power-off the chip.For devices with parking station capability there will be one MediaInsertedEvent for each card found.
        /// </summary>
        Task<ResetDeviceResult> ResetDeviceAsync(IResetEvents events,
                                                 ResetDeviceRequest cardAction,
                                                 CancellationToken cancellation);

        /// <summary>
        /// This function resets the present value for number of cards retained to zero.
        /// The function is possible formotor-driven card readers only.
        /// The number of cards retained is controlled by the service.
        /// </summary>
        Task<ResetCountResult> ResetBinCountAsync(CancellationToken cancellation);

        /// <summary>
        /// This command is used for setting the DES key that is necessary for operating a CIM86 module.
        /// The command must beexecuted before the first read command is issued to the card reader.
        /// </summary>
        Task<SetCIM86KeyResult> SetCIM86KeyAsync(SetCIM86KeyRequest keyInfo,
                                                 CancellationToken cancellation);

        /// <summary>
        /// This command handles the power actions that can be done on the chip.For user chips, this command is only used after the chip has been contacted for the first time using the[CardReader.ReadRawData](#cardreader.readrawdata) command. For contactless user chips, this command may be used todeactivate the contactless card communication.For permanently connected chip cards, this command is the only way to control the chip power.
        /// </summary>
        Task<ChipPowerResult> ChipPowerAsync(IChipPowerEvents events,
                                             ChipPowerRequest action,
                                             CancellationToken cancellation);

        /// <summary>
        /// This command is used to move a card that is present in the reader to a parking station.
        /// A parking station isdefined as an area in the ID card unit, which can be used to temporarily store the card while the device performs operations on another card. 
        /// This command is also used to move a card from the parking station to the read/write,chip I/O or transport position. 
        /// When a card is moved from the parking station to the read/write, chip I/O ortransport position parkOut, the read/write, chip I/O or transport position must not be occupied with anothercard, otherwise the error cardPresent will be returned.
        /// After moving a card to a parking station, another card can be inserted and read by calling, e.g.,CardReader.ReadRawData.
        /// Cards in parking stations will not be affected by any CardReader commands until they are removed from the parkingstation using this command, except for the CardReader.Reset command, which will move thecards in the parking stations as specified in its input as part of the reset action if possible.
        /// </summary>
        Task<ParkCardResult> ParkCardAsync(ParkCardRequest parkCardInfo,
                                           CancellationToken cancellation);

        /// <summary>
        /// This command is used to configure an intelligent contactless card reader before performing a contactlesstransaction.
        /// This command sets terminal related data elements, the list of terminal acceptable applications with associated application specific data and any encryption key data required for offline data authentication.
        /// This command should be used prior to CardReader.EMVClessPerformTransaction command. 
        /// It may be calledonce on application start up or when any of the configuration parameters require to be changed. 
        /// The configurationset by this command is persistent.This command should be called with a complete list of acceptable payment system applications as any previous configurations will be replaced.
        /// </summary>
        Task<EMVContactlessConfigureResult> EMVContactlessConfigureAsync(EMVContactlessConfigureRequest terminalConfig,
                                                                         CancellationToken cancellation);

        /// <summary>
        /// This command is used to enable an intelligent contactless card reader.
        /// The transaction will start as soon as thecard tap is detected.
        /// Based on the configuration of the contactless chip card and the reader device, this command could return dataformatted either as magnetic stripe information or as a set of BER-TLV encoded EMV tags.
        /// This command supports magnetic stripe emulation cards and EMV-like contactless cards but cannot be used on storagecontactless cards. 
        /// The latter must be managed using the CardReader.ReadRawData and CardReader.ChipIO commands.
        /// For specific payment system's card profiles an intelligent card reader could return a set of EMV tags along withmagnetic stripe formatted data. 
        /// In this case, two contactless card data structures will be returned, onecontaining the magnetic stripe like data and one containing BER-TLV encoded tags.
        /// If no card has been tapped, the contactless chip card reader waits for the period of time specified in the commandcall for a card to be tapped.
        /// For intelligent contactless card readers, any in-built audio/visual feedback such as Beep/LEDs, need to becontrolled directly by the reader. 
        /// These indications should be implemented based on the EMVCo and payment system'sspecifications.
        /// </summary>
        Task<EMVContactlessPerformTransactionResult> EMVContactlessPerformTransactionAsync(IEMVClessPerformTransactionEvents events,
                                                                                           EMVContactlessPerformTransactionRequest transactionData,
                                                                                           CancellationToken cancellation);

        /// <summary>
        /// This command performs the post authorization processing on payment systems contactless cards.
        /// Before an online authorized transaction is considered complete, further chip processing may be requested by theissuer. 
        /// This is only required when the authorization response includes issuer update data either issuer scriptsor issuer authentication data.
        /// The command enables the contactless card reader and waits for the customer to re-tap their card.
        /// The contactless chip card reader waits for the period of time specified in the command all for a card to be tapped.
        /// </summary>
        Task<EMVContactlessIssuerUpdateResult> EMVContactlessIssuerUpdateAsync(IEMVClessIssuerUpdateEvents events,
                                                                               EMVContactlessIssuerUpdateRequest transactionData,
                                                                               CancellationToken cancellation);


        /// <summary>
        /// This command is used to retrieve the complete list of registration authority Interface Module (IFM) identifiers.
        /// The primary registration authority is EMVCo but other organizations are also supported for historical or localcountry requirements.
        /// New registration authorities may be added in the future so applications should be able to handle the return of new(as yet undefined) IFM identifiers.
        /// </summary>
        QueryIFMIdentifierResult QueryIFMIdentifier();

        /// <summary>
        /// This command is used to retrieve the supported payment system applications available within an intelligentcontactless card unit. 
        /// The payment system application can either be identified by an AID or by the AID incombination with a Kernel Identifier. 
        /// The Kernel Identifier has been introduced by the EMVCo specifications; seeReference [3].
        /// </summary>
        QueryEMVApplicationResult EMVContactlessQueryApplications();


        /// <summary>
        /// Specify the type of cardreader
        /// </summary>
        DeviceTypeEnum DeviceType { get; }

        /// <summary>
        /// Specify the current status of media
        /// </summary>
        MediaStatusEnum MediaStatus { get; }

    }
}
