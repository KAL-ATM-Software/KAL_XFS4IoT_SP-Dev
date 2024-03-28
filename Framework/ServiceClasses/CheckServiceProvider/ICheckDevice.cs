/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Check
{
    public interface ICheckDevice : IDevice
    {
        /// <summary>
        /// This method accepts media into the device from the input position
        /// and could be called multiple times.
        /// A media-in transaction is initiated by the first method and remains active until the
        /// transaction is either confirmed through the MediaInEnd, or cancelled by the
        /// MediaInRollBack, the RetractMedia or Reset. Multiple calls to the
        /// this methodcan be made while a transaction is active to obtain additional items 
        /// from the customer. If a media-in transaction is active.
        /// When the command is executed, if there is no media in the input slot
        /// then the device is enabled for media entry and the
        /// Check.NoMediaEvent event is generated when the device is ready to
        /// accept media. When the customer inserts the media a Check.MediaInsertedEvent
        /// event is generated and media processing begins.
        /// If media is already present at the input slot then a Check.MediaInsertedEvent
        /// event is generated and media processing begins immediately.
        /// The Check.MediaDataEvent  event delivers the code line and all requested image data 
        /// during execution of this command.One event is generated for each media item scanned 
        /// by this command. The Check.MediaDataEvent event is not generated for refused media items.
        /// A failure during processing a single media item does not mean that the operation
        /// has failed even if some or all of the media are refused by the media reader.
        /// In this case the operation will return Success and one or more Check.MediaRefusedEvent
        /// events will be sent to report the why the items have been refused.
        /// Refused items are not presented back to the customer with this command.
        /// The Check.MediaRefusedEvent  event indicates whether or not media
        /// must be returned to the customer before further media movement operations can be executed.
        /// If the [Check.MediaRefusedEvent](#check.mediarefusedevent) event indicates
        /// that the media must be returned then the application must use the Check.PresentMedia 
        /// command to return the refused items. If the event does not indicate that the application
        /// must return the media items then the application can still elect to return the media items
        /// using the Check.PresentMedia command or instead allow the refused items to be returned 
        /// during the Check.MediaInEnd or Check.MediaInRollBack operations.
        /// 
        /// If there is no stacker on the device or ApplicationRefuse property is true
        /// then just one of the media items inserted are processed by this operation,
        /// and therefore the command completes as soon as the last image for the
        /// first item is produced or when the first item is automatically refused.
        /// If there is a stacker on the device then the command completes when the
        /// last image for the last item is produced or when the last item is refused.
        /// </summary>
        Task<MediaInResult> MediaInAsync(MediaInCommandEvents events, 
                                         MediaInRequest request,
                                         CancellationToken cancellation);

        /// <summary>
        /// This method ends a media-in transaction. 
        /// If media items are on the stacker as a result of a Check.MediaIn operation,
        /// they are moved to the destination specified by SetMediaParameters method. 
        /// Any additional actions specified for the items by SetMediaParameters such as printing, 
        /// stamping and rescanning are also executed.
        /// If the destination has not been set for a media item then the Service will decide which storage unit to put the item into. 
        /// If no items are in the device, the operation will complete with the NoMediaPresent error and
        /// The way in which media is returned to the customer as a result of this command is defined by
        /// PresentControl capability. If false, the application must call Check.PresentMedia
        /// to present the media items to be returned as a result of this operation.
        /// If true the Service presents any returned items implicitly and the application does not 
        /// need to call Check.PresentMedia.
        /// If items have been refused and the Check.MediaRefusedEvent event
        /// has indicated that the items must be returned then these items must be returned using the
        /// Check.PresentMedia command before the MediaInEnd method is issued, otherwise a RefusedItems
        /// error will be returned. If items have been refused and the Check.MediaRefusedEvent event 
        /// has indicated that the items do not need to be returned then the MediaInEnd
        /// operation causes any refused items which have not yet been returned to the customer 
        /// (via the PresentMedia operation) to be returned along with any items that the application 
        /// has selected to return to the customer(via the SetMediaParameters method). 
        /// Even if all items are being deposited, previously refused items will be returned to the
        /// customer by this command.The Check.MediaPresentedEvent event(s)
        /// inform the application of the position where the media has been presented to.
        /// This operation completes when all the media items have been put into their
        /// specified storage units and in the case where media is returned to the customer
        /// as a result of this command, after the last bunch of media items to be
        /// returned to the customer has been presented, but before the last bunch is taken.
        /// 
        /// The media-in transaction is ended even if this command does not complete successfully.
        /// </summary>
        Task<MediaInEndResult> MediaInEndAsync(MediaInEndCommandEvents events,
                                               MediaInEndRequest request,
                                               CancellationToken cancellation);

        /// <summary>
        /// This method ends a media-in transaction. All media that
        /// is in the device as a result of MediaIn operation is 
        /// returned to the customer. Nothing is printed on the media.If no items
        /// are in the device the command will complete with the NoMediaPresent error.
        /// The way in which media is returned to the customer as a result of this command is defined by
        /// PresentControl capability. If false, the application must perform PresentMedia operation
        /// to present the media items to be returned as a result of this operation.
        /// If true the Service presents any returned items implicitly and the application does not need
        /// to perform PresentMedia operation.
        /// If items have been refused and the Check.MediaRefusedEvent event has indicated that the 
        /// items must be returned then these items must be returned using the PresentMedia operation
        /// before the MediaInRollBack operation is performed, otherwise a RefusedItems error will be 
        /// returned.If items have been refused and the Check.MediaRefusedEvent has indicated that
        /// the items do not need to be returned then the MediaInRollBack operation causes any refused
        /// items which have not yet been returned to the customer (via the PresentMedia operation)
        /// to be returned along with any items that are returned as a result of the rollback.
        /// The Check.MediaPresentedEvent event(s) inform the application of the
        /// position where the media has been presented to.
        /// 
        /// In the case where media is returned to the customer as a result of this operation,
        /// this method completes when the last bunch of media items to be returned to the customer 
        /// has been presented, but before the last bunch is taken.
        /// 
        /// The media-in transaction is ended even if this command does not complete successfully.
        /// </summary>
        Task<MediaInRollbackResult> MediaInRollbackAsync(MediaInRollbackCommandEvents events,
                                                         MediaInRollbackRequest request,
                                                         CancellationToken cancellation);

        /// <summary>
        /// On devices where items can be physically rescanned or
        /// all the supported image formats can be generated during this method
        /// (regardless of the images requested during the Check.MediaIn command), 
        /// i.e. where rescan capability is true, then this method is
        /// used to obtain additional images and/or reread the code line for media
        /// already in the device.
        /// If rescan capability is false, this method is used to
        /// retrieve an image or code line that was initially obtained when the
        /// media was initially processed (e.g.during the Check.MediaIn or Check.GetNextItem command).
        /// In this case, all images required must have been previously been requested during the
        /// Check.MediaIn command.
        /// </summary>
        Task<ReadImageResult> ReadImageAsync(ReadImageRequest request, 
                                             CancellationToken cancellation);

        /// <summary>
        /// This method is used to present media items to the customer.
        /// Applications can use this command to return refused items without
        /// terminating the media-in transaction. This allows customers to correct
        /// the problem with the media item and reinsert during execution of a
        /// subsequent Check.MediaIn command.
        /// This method is also used to return items after media-in transaction ended with
        /// Check.MediaInEnd or Check.MediaInRollBack command
        /// A Check.MediaPresentedEvent event is generated when media is
        /// presented and a Check.MediaTakenEvent event is generated when the
        /// This method completes when the last bunch of media items to be returned to the customer has been presented, 
        /// but before the last bunch is taken.
        /// </summary>
        Task<PresentMediaResult> PresentMediaAsync(PresentMediaCommandEvents events,
                                                   PresentMediaRequest request,
                                                   CancellationToken cancellation);

        /// <summary>
        /// The media is removed from its present position (media present in device, media entering, 
        /// unknown position) and stored in the area specified in the input parameters.
        /// if a high or full condition is reached as a result of this method.
        /// If the storage unit is already full and the operation cannot be executed, 
        /// an error is returned and the media remains in its present position.
        /// If media items are to be endorsed/stamped during this operation, then
        /// SetMediaParameters is called from application prior to the this method. Where endorsing is specified, the
        /// same text will be printed on all media items that are detected.
        ///  This method ends the current media-in transaction.
        ///If no items are in the device the command will complete with the NoMediaPresent error.
        /// </summary>
        Task<RetractMediaResult> RetractMediaAsync(RetractMediaCommandEvents events,
                                                   RetractMediaRequest request, 
                                                   CancellationToken cancellation);

        /// <summary>
        /// This method is used by the application to perform a hardware reset which will attempt to return 
        /// the device to a known good state.
        /// This method does not override a lock obtained on another application or service handle.
        /// The device will attempt to retract or eject any items found anywhere within the device.
        /// This may not always be possible because of hardware problems.
        /// One or more Check.MediaDetectedEvent command event will inform the application 
        /// where items were actually moved to.
        /// </summary>
        Task<ResetDeviceResult> ResetAsync(ResetCommandEvents events, 
                                           ResetDeviceRequest request, 
                                           CancellationToken cancellation);

        /// <summary>
        /// This method is used to get the next item from the
        /// multi-item feed unit and capture the item data. The data and the format
        /// of the data that is generated by this method are defined by the input
        /// parameters of this method. The media data is
        /// reported via the Check.MediaDataEvent event.
        /// 
        /// This method must be supported by all Services where the
        /// hardware does not have a stacker or where the Service supports
        /// the application making the accept/refuse decision. On single item feed
        /// devices this method simply returns the error code NoMediaPresent. This allows a single application flow to
        /// be used on all devices without a stacker.
        /// </summary>
        Task<GetNextItemResult> GetNextItemAsync(GetNextItemCommandEvents events,
                                                 GetNextItemRequest request,
                                                 CancellationToken cancellation);

        /// <summary>
        /// This command is used to cause the predefined actions (move item to destination, 
        /// stamping, endorsing, re-imaging) to beexecuted on the current media item. 
        /// This method only applies to devices without stackers.
        /// </summary>
        Task<ActionItemResult> ActionItemAsync(ActionItemCommandEvents events,
                                               ActionItemRequest request,
                                               CancellationToken cancellation);

        /// <summary>
        /// The media that has been presented to the customer will be expelled out of the device.
        /// This method completes after the bunch has been expelled from the device.
        /// This method does not end the current media-in transaction.
        /// </summary>
        Task<ExpelMediaResult> ExpelMediaAsync(CancellationToken cancellation);

        /// <summary>
        /// This command is used by applications to indicate if the current media item should 
        /// be accepted or refused. Applications only usethis command when the Check.MediaIn command 
        /// is used in the mode where the application can decide if each physically acceptablemedia 
        /// item should be accepted or refused, capability ApplicationRefuse is true.
        /// </summary>
        Task<AcceptItemResult> AcceptItemAsync(AcceptItemRequest request, 
                                               CancellationToken cancellation);

        /// <summary>
        /// After the supplies have been replenished, this method is used to indicate that one or 
        /// more supplies have been replenished and are expected to be in a healthy state.
        /// Hardware that cannot detect the level of a supply and reports on the supply's status using metrics 
        /// (or some other means), must assume the supply has been fully replenished after this method is issued.
        /// </summary>
        Task<DeviceResult> SupplyReplenishAsync(SupplyReplenishRequest request,
                                                CancellationToken cancellation);

        /// <summary>
        /// This method is used to predefine parameters for the specified media item or all items. 
        /// The method can be called multiple times to specify individual parameters for each required media item.
        /// Any parameter specified replaces any parameters specified for the same media item (or items) on previous commands.
        /// The parameters which can be specified include:
        /// - Destination
        /// - Endorsements, i.e., text to be printed on the media or whether the media is to be stamped
        /// - Images of the media after it has been printed on or stamped
        /// 
        /// The media is not moved immediately by this command.The requested actions are performed during subsequent methods which
        /// move the media:
        /// On devices with stackers, MediaInEnd method
        /// On devices without stackers, ActionItem method
        /// 
        /// If the bunch is returned with MediaInRollback, none of the requested actions will be performed.
        /// If the media is to be returned to the customer using MediaInEnd or ActionItem, the media can still be
        /// endorsed if the device is capable of that.
        /// The Service will determine which storage unit to use for any items that have not had a destination set by the
        /// application.
        /// </summary>
        Task<SetMediaParametersResult> SetMediaParametersAsync(SetMediaParametersRequest request, 
                                                               CancellationToken cancellation);


        /// <summary>
        /// Check scanner Status
        /// </summary>
        CheckScannerStatusClass CheckScannerStatus { get; set; }

        /// <summary>
        /// Check scanner Capabilities
        /// </summary>
        CheckScannerCapabilitiesClass CheckScannerCapabilities { get; set; }
    }
}
