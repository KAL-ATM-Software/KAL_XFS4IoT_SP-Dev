/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public sealed class PositionCapabilitiesClass
    {
        /// <summary>
        /// Retract - The items may be retracted to a retract cash unit.
        /// Transport - The items may be retracted to the transport.
        /// Stacker - The items may be retracted to the intermediate stacker.
        /// Reject - The items may be retracted to a reject cash unit.
        /// ItemCassette - The items may be retracted to the item cassettes, i.e. cassettes that can be dispensed from.
        /// Default - The item may be retracted to the default position.
        /// </summary>
        [Flags]
        public enum UsageEnum
        {
            In = 1 << 0,
            Refuse = 1 << 1,
            Rollback = 1 << 2,
        }

        /// <summary>
        /// Retract - The items may be retracted to a retract storage unit.
        /// Transport - The items may be retracted to the transport.
        /// Stacker - The items may be retracted to the intermediate stacker.
        /// Reject - The items may be retracted to a reject storage unit.
        /// BillCassettes - Items may be retracted to item cassettes, i.e. cash-in and recycle storage units.
        /// CashIn - Items may be retracted to a cash-in storage unit.
        /// </summary>
        [Flags]
        public enum RetractAreaEnum
        {
            Retract = 1 << 0,
            Reject = 1 << 1,
            Transport = 1 << 2,
            Stacker = 1 << 3,
            BillCassettes = 1 << 4 ,
            CashIn = 1 << 5,
        }

        public PositionCapabilitiesClass(UsageEnum Usage,
                                         bool ShutterControl,
                                         bool ItemsTakenSensor,
                                         bool ItemsInsertedSensor,
                                         RetractAreaEnum RetractAreas,
                                         bool PresentControl,
                                         bool PreparePresent)
        {
            this.Usage = Usage;
            this.ShutterControl = ShutterControl;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.ItemsInsertedSensor = ItemsInsertedSensor;
            this.RetractAreas = RetractAreas;
            this.PresentControl = PresentControl;
            this.PreparePresent = PreparePresent;
        }

        /// <summary>
        /// Indicates if a position is used to input, reject or rollback.
        /// </summary>
        public UsageEnum Usage { get; init; }

        /// <summary>
        /// If true the shutter is controlled implicitly by the Service.
        /// 
        /// If false the shutter must be controlled explicitly by the application using the 
        /// CashManagement.OpenShutter and CashManagement.CloseShutter commands.
        /// 
        /// In either case the [CashAcceptor.PresentMedia](#cashacceptor.presentmedia) command may be used if
        /// PresentControl is false. The ShutterControl field is always true if the described position has no 
        /// shutter.
        /// </summary>
        public bool ShutterControl { get; init; }

        /// <summary>
        /// Specifies whether or not the described position can detect when items at the exit position are taken by the user.
        /// 
        /// If true the service generates an accompanying
        /// CashManagement.ItemsTakenEvent. If false this event is not generated. 
        /// 
        /// This field relates to output and refused positions.
        /// </summary>
        public bool ItemsTakenSensor { get; init; }

        /// <summary>
        /// Specifies whether the described position has the ability to detect when items have been inserted by the user. 
        /// 
        /// If true the service generates an accompanying CashManagement.ItemsInsertedEvent. 
        /// If false this event is not generated. 
        /// 
        /// This field relates to all input positions.
        /// </summary>
        public bool ItemsInsertedSensor { get; init; }

        /// <summary>
        /// Specifies the areas to which items may be retracted from this position. This is not reported if the device 
        /// cannot retract.
        /// </summary>
        public RetractAreaEnum RetractAreas { get; init; }

        /// <summary>
        /// Specifies how the presenting of media items is controlled. 
        /// 
        /// If true then the CashAcceptor.PresentMedia command is not supported and items 
        /// are moved to the output position for removal as part of the relevant command, e.g. CashAcceptor.CashIn or 
        /// CashAcceptor.CashInRollback where there is implicit shutter control. 
        /// 
        /// If false then items returned or rejected can be moved to the output position using the 
        /// CashAcceptor.PresentMedia command, this includes items returned or rejected as part of a 
        /// CashAcceptor.CashIn or CashAcceptor.CashInRollback operation. The CashAcceptor.PresentMedia
        /// command will open and close the shutter implicitly.
        /// </summary>
        public bool PresentControl { get; init; }

        /// <summary>
        /// Specifies how the presenting of items is controlled. 
        /// 
        /// If false then items to be removed are moved to the output position as part of the relevant command. e.g.
        /// CashManagement.OpenShutter, CashAcceptor.PresentMedia or CashAcceptor.CashInRollback.
        /// 
        /// If true then items are moved to the output position using the CashAcceptor.PreparePresent command.
        /// </summary>
        public bool PreparePresent { get; init; }
    }
}
