/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using CardReader;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

// KAL specific implementation of cardreader. 
namespace XFS4IoTFramework.CardReader
{
    public interface ICardReaderDevice : ICommonDevice
    {

        /// <summary>
        /// This command is used to retrieve the list of forms available on the device.
        /// </summary>
        Task<FormListCompletion.PayloadData> FormList(ICardReaderConnection connection, FormListCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve details of the definition of a specified form.
        /// </summary>
        Task<QueryFormCompletion.PayloadData> QueryForm(ICardReaderConnection connection, QueryFormCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the complete list of registration authority Interface Module (IFM) identifiers. The primary registration  authority is EMVCo but other organizations are also supported for historical or local country requirements.New registration authorities may be added in the future so applications should be able to handle the return of new (as yet undefined) IFM  identifiers.
        /// </summary>
        Task<QueryIFMIdentifierCompletion.PayloadData> QueryIFMIdentifier(ICardReaderConnection connection, QueryIFMIdentifierCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the supported payment system applications available within an intelligent contactless card unit. The payment  system application can either be identified by an AID or by the AID in combination with a Kernel Identifier. The Kernel Identifier has been  introduced by the EMVCo specifications; see Reference [3].
        /// </summary>
        Task<EMVClessQueryApplicationsCompletion.PayloadData> EMVClessQueryApplications(ICardReaderConnection connection, EMVClessQueryApplicationsCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// For motor driven card readers, the ID card unit checks whether a card has been inserted. If so, the tracks are read immediately as described in  the form specified by the *formName* parameter. If no card has been inserted, and for all other categories of devices, the ID card unit waits for the application specified timeout for a card  to be either inserted or pulled through. Again the next step is reading the tracks specified in the form (see Section XXX, Form Definition, for a more detailed description of the forms mechanism). When the SECURE tag is specified in the associated form, the results of a security check  via a security module (i.e. MM, CIM86) are specified and added to the track data.A [CardReader.InsertCardEvent](#message-CardReader.InsertCardEvent) will be generated when there is no card in the card reader and the device is ready to accept a card. If the security check fails however this should not stop valid data being returned. The error WFS_ERR_IDC_SECURITYFAIL  will be returned if the form specifies only security data to be read and the security check could not be executed, in all other cases  WFS_SUCCESS will be returned with the security field of the output parameter set to the relevant value including WFS_IDC_SEC_HWERROR.
        /// </summary>
        Task<ReadTrackCompletion.PayloadData> ReadTrack(ICardReaderConnection connection, ReadTrackCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// For motor-driven card readers, the ID card unit checks whether a card has been inserted. If so, the data is written to the track as described  in the form specified by the *formName* parameter, and the other parameters.If no card has been inserted, and for all other categories of devices, the ID card unit waits for the application specified timeout for a card  to be either inserted or pulled through. The next step is writing the data defined by the form and the parameters to the respective track (see  Section XXX, Form Definition, for a more detailed description of the forms mechanism).This procedure is followed by data verification.A [CardReader.InsertCardEvent](#message-CardReader.InsertCardEvent)  will be generated when there is no card in the card reader and the device  is ready to accept a card.If power fails during a write the outcome of the operation will be vendor specific, there is no guarantee that the write will have succeeded.
        /// </summary>
        Task<WriteTrackCompletion.PayloadData> WriteTrack(ICardReaderConnection connection, WriteTrackCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is only applicable to motor driven card readers and latched dip card readers.For motorized card readers the default operation is that the card is driven to the exit slot from where the user can remove it. The card remains in position for withdrawal until either it is taken or another command is issued that moves the card.For latched dip readers, this command causes the card to be unlatched (if not already unlatched), enabling removal.After successful completion of this command, a [CardReader.MediaRemovedEvent](#message-CardReader.MediaRemovedEvent) is generated to inform the application when the card is taken.
        /// </summary>
        Task<EjectCardCompletion.PayloadData> EjectCard(ICardReaderConnection connection, EjectCardCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// The card is removed from its present position (card inserted into device, card entering, unknown position) and stored in the retain bin;  applicable to motor-driven card readers only. The ID card unit sends a [CardReader.RetainBinThresholdEvent](#message-CardReader.RetainBinThresholdEvent) if the  storage capacity of the retain bin is reached. If the storage capacity has already been reached, and the command cannot be executed, an error is returned and the card remains in its present  position.
        /// </summary>
        Task<RetainCardCompletion.PayloadData> RetainCard(ICardReaderConnection connection, RetainCardCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This function resets the present value for number of cards retained to zero.  The function is possible for motor-driven card readers only.The number of cards retained is controlled by the service and can be requested  before resetting via [CardReader.Status](#command-CardReader.Status).
        /// </summary>
        Task<ResetCountCompletion.PayloadData> ResetCount(ICardReaderConnection connection, ResetCountCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used for setting the DES key that is necessary for operating a CIM86 module. The command must be executed before the first readcommand is issued to the card reader.
        /// </summary>
        Task<SetKeyCompletion.PayloadData> SetKey(ICardReaderConnection connection, SetKeyCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// For motor driven card readers, the card unit checks whether a card has been inserted. If so, all specified tracks are read immediately. If reading the chip is requested, the chip will be contacted and reset and the ATR (Answer To Reset) data will be read. When this command  completes the chip will be in contacted position. This command can also be used for an explicit cold reset of a previously contacted chip.This command should only be used for user cards and should not be used for permanently connected chips.If no card has been inserted, and for all other categories of card readers, the card unit waits for the period of time specified in the call  for a card to be either inserted or pulled through. The next step is trying to read all tracks specified.The [CardReader.InsertCardEvent](#message-CardReader.InsertCardEvent) will be generated when there is no card in the card reader and the device is ready to accept a card. In addition to that, a security check via a security module (i.e. MM, CIM86) can be requested. If the security check  fails however this should not stop valid data being returned. The response _securityFail_ will be returned if the command specifies only  security data to be read and the security check could not be executed, in all other cases _ok_ will be returned with the data field of the  output parameter set to the relevant value including _hardwareError_.For non-motorized Card Readers which read track data on card exit, the **invalidData** error code is returned when a call to is made to read both track data and chip data.If the card unit is a latched dip unit then the device will latch the card when the chip card will be read, i.e. **chip** is specified (see  below). The card will remain latched until a call to **ejectCard** is made.For contactless chip card readers a collision of two or more card signals may happen.  In this case, if the device is not able to pick the  strongest signal, **errorCardCollision** will be returned.
        /// </summary>
        Task<ReadRawDataCompletion.PayloadData> ReadRawData(ICardReaderConnection connection, ReadRawDataCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// For motor-driven card readers, the ID card unit checks whether a card has been inserted. If so, the data is written to the tracks.If no card has been inserted, and for all other categories of devices, the ID card unit waits for the application specified timeout for a card  to be either inserted or pulled through. The next step is writing the data to the respective tracks.The [CardReader.InsertCardEvent](#message-CardReader.InsertCardEvent) event will be generated when there is no card in the card reader and the  device is ready to accept a card.The application must pass the magnetic stripe data in ASCII without any sentinels. The data will be converted by the Service Provider (ref [CardReader.ReadRawData](#command-CardReader.ReadRawData)). If the data passed in is too long the invalidError error code will be returned.This procedure is followed by data verification.If power fails during a write the outcome of the operation will be vendor specific, there is no guarantee that the write will have succeeded.
        /// </summary>
        Task<WriteRawDataCompletion.PayloadData> WriteRawData(ICardReaderConnection connection, WriteRawDataCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to communicate with the chip. Transparent data is sent from the application to the chip and the response of  the chip is returned transparently to the application.The identification information e.g. ATR of the chip must be obtained before issuing this command. The identification information  for a user card or the Memory Card Identification (when available) must initially be obtained using  [CardReader.ReadRawData](#command-CardReader.ReadRawData). The identification information for subsequent resets of a user card  can be obtained using either CardReader.ReadRawData [CardReader.ChipPower](#command-CardReader.ChipPower). The ATR for permanent  connected chips is always obtained through CardReader.ChipPower.For contactless chip card readers, applications need to specify which chip to contact with, as part of *chipData*, if more than  one chip has been detected and multiple identification data has been returned by the CardReader.ReadRawData command.For contactless chip card readers a collision of two or more card signals may happen. In this case, if the device is not able to  pick the strongest signal, the errCardCollision error code will be returned.
        /// </summary>
        Task<ChipIOCompletion.PayloadData> ChipIO(ICardReaderConnection connection, ChipIOCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used by the application to perform a hardware reset which will attempt to return the IDC device to a known good state. This  command does not over-ride a lock obtained by another application or service handle.If the device is a user ID card unit, the device will attempt to either retain, eject or will perform no action on any user cards found in the  IDC as specified in the input parameter. It may not always be possible to retain or eject the items as specified because of hardware problems.  If a user card is found inside the device the [CardReader.MediaInsertedEvent](#message-CardReader.MediaInsertedEvent) will inform the  application where card was actually moved to. If no action is specified the user card will not be moved even if this means that the IDC cannot  be recovered.If the device is a permanent chip card unit, this command will power-off the chip.For devices with parking station capability there will be one *MediaInsertedEvent* for each card found.
        /// </summary>
        Task<ResetCompletion.PayloadData> Reset(ICardReaderConnection connection, ResetCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command handles the power actions that can be done on the chip.For user chips, this command is only used after the chip has been contacted for the first time using the [CardReader.ReadRawData](#command-CardReader.ReadRawData) command. For contactless user chips, this command may be used to deactivate the contactless card communication.For permanently connected chip cards, this command is the only way to control the chip power.
        /// </summary>
        Task<ChipPowerCompletion.PayloadData> ChipPower(ICardReaderConnection connection, ChipPowerCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command takes the form name and output of a successful [CardReader.ReadRawData](#command-CardReader.ReadRawData) and returns the parsed string.
        /// </summary>
        Task<ParseDataCompletion.PayloadData> ParseData(ICardReaderConnection connection, ParseDataCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to move a card that is present in the reader to a parking station. A parking station is defined as an area in the IDC,  which can be used to temporarily store the card while the device performs operations on another card. This command is also used to move a  card from the parking station to the read/write, chip I/O or transport position. When a card is moved from the parking station to the  read/write, chip I/O or transport position (*parkOut*), the read/write, chip I/O or transport position must not be occupied with another card,  otherwise the error *cardPresent* will be returned.After moving a card to a parking station, another card can be inserted and read by calling, e.g., [CardReader.ReadRawData](#command-CardReader.ReadRawData) or [CardReader.ReadTrack](#command-CardReader.ReadTrack).Cards in parking stations will not be affected by any IDC commands until they are removed from the parking station using this command, except  for the [CardReader.Reset](#command-CardReader.Reset) command, which will move the cards in the parking stations as specified in its input as  part of the reset action if possible.
        /// </summary>
        Task<ParkCardCompletion.PayloadData> ParkCard(ICardReaderConnection connection, ParkCardCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to configure an intelligent contactless card reader before performing a  contactless transaction. This command sets terminal related data elements, the list of terminal  acceptable applications with associated application specific data and any encryption key data required  for offline data authentication.This command should be used prior to [CardReader.EMVClessPerformTransaction](#command-CardReader.EMVClessPerformTransaction) if the  command. It may be called once on application start up or when any of the configuration parameters  require to be changed. The configuration set by this command is persistent.This command should be called with a complete list of acceptable payment system applications as  any previous configurations will be replaced.
        /// </summary>
        Task<EMVClessConfigureCompletion.PayloadData> EMVClessConfigure(ICardReaderConnection connection, EMVClessConfigureCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command is used to enable an intelligent contactless card reader. The transaction will start as soon as the card tap is detected.Based on the configuration of the contactless chip card and the reader device, this command could return data  formatted either as magnetic stripe information or as a set of BER-TLV encoded EMV tags.This command supports magnetic stripe emulation cards and EMV-like contactless cards but cannot be used on  storage contactless cards. The latter must be managed using the [CardReader.ReadRawData](#command-CardReader.ReadRawData) and [CardReader.ChipIO](#command-CardReader.ChipIO) commands.For specific payment system's card profiles an intelligent card reader could return a set of EMV tags along with magnetic stripe formatted data. In this case, two contactless card data structures will be returned, one  containing the magnetic stripe like data and one containing BER-TLV encoded tags.If no card has been tapped, the contactless chip card reader waits for the period of time specified in the  command call for a card to be tapped.For intelligent contactless card readers, any in-built audio/visual feedback such as Beep/LEDs, need to be controlled directly by the reader. These indications should be implemented based on the EMVCo and payment  system's specifications.
        /// </summary>
        Task<EMVClessPerformTransactionCompletion.PayloadData> EMVClessPerformTransaction(ICardReaderConnection connection, EMVClessPerformTransactionCommand.PayloadData payload, CancellationToken cancellation);

        /// <summary>
        /// This command performs the post authorization processing on payment systems contactless cards.Before an online authorized transaction is considered complete, further chip processing may be requested by the  issuer. This is only required when the authorization response includes issuer update data; either issuer scripts  or issuer authentication data.The command enables the contactless card reader and waits for the customer to re-tap their card. The contactless chip card reader waits for the period of time specified in the WFSExecute call for a card to be tapped.
        /// </summary>
        Task<EMVClessIssuerUpdateCompletion.PayloadData> EMVClessIssuerUpdate(ICardReaderConnection connection, EMVClessIssuerUpdateCommand.PayloadData payload, CancellationToken cancellation);

       
        Task WaitForCardTaken(ICardReaderConnection connection, CancellationToken cancellation);
    }
}
