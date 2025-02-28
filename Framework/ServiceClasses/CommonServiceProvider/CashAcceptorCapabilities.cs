/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CashAcceptorCapabilitiesClass
    /// Store device capabilites for the cash acceptor device
    /// </summary>
    public sealed class CashAcceptorCapabilitiesClass(
        CashManagementCapabilitiesClass.TypeEnum Type,
        int MaxCashInItems,
        bool Shutter,
        bool ShutterControl,
        int IntermediateStacker,
        bool ItemsTakenSensor,
        Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashAcceptorCapabilitiesClass.PositionClass> Positions,
        CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas,
        CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions,
        CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions,
        CashAcceptorCapabilitiesClass.CashInLimitEnum CashInLimit,
        CashAcceptorCapabilitiesClass.CountActionEnum CountActions,
        CashAcceptorCapabilitiesClass.RetainCounterfeitActionEnum RetainCounterfeitAction)
    {
        [Flags]
        public enum CashInLimitEnum
        {
            NotSupported = 0,
            ByTotalItems = 1 << 0,
            ByAmount = 1 << 1,
        }

        [Flags]
        public enum CountActionEnum
        {
            NotSupported = 0,
            Individual = 1 << 0,
            All = 1 << 1,
        }

        // Specifies whether such items are retained by the device if detected during a cash-in transaction.
        [Flags]
        public enum RetainCounterfeitActionEnum
        {
            NotSupported = 0,
            Level2 = 1 << 0,
            Level3 = 1 << 1,
            Inked = 1 << 2,
        }

        /// <summary>
        /// Supplies the type of CashAcceptor. The following values are possible:
        /// 
        /// * ```TellerBill``` - The CashAcceptor is a Teller Bill Acceptor.
        /// * ```SelfServiceBill``` - The CashAcceptor is a Self-Service Bill Acceptor.
        /// * ```TellerCoin``` - The CashAcceptor is a Teller Coin Acceptor.
        /// * ```SelfServiceCoin``` - The CashAcceptor is a Self-Service Coin Acceptor.
        /// </summary>
        public CashManagementCapabilitiesClass.TypeEnum Type { get; init; } = Type;

        /// <summary>
        /// Supplies the maximum number of items that can be accepted in a single 
        /// CashAcceptor.CashIn command. This value reflects the hardware limitations of the 
        /// device and therefore it does not change as part of the 
        /// CashAcceptor.CashInStart command.
        /// </summary>
        public int MaxCashInItems { get; init; } = MaxCashInItems;

        /// <summary>
        /// If true then the device has a shutter and explicit shutter control through the commands 
        /// CashManagement.OpenShutter and CashManagement.CloseShutter is supported. The definition of a shutter will depend on the h/w 
        /// implementation. On some devices where items are automatically detected and accepted then a shutter is simply
        /// a latch that is opened and closed, usually under implicit control by the Service. On other devices, the term
        /// shutter refers to a door, which is opened and closed to allow the customer to place the items onto a tray.
        /// If a Service cannot detect when items are inserted and there is a shutter on the device, then it must 
        /// provide explicit application control of the shutter.
        /// </summary>
        public bool Shutter { get; init; } = Shutter;

        /// <summary>
        /// If true the shutter is controlled implicitly by the service.
        /// 
        /// If false the shutter must be controlled explicitly by the application using the
        /// CashManagement.OpenShutter and
        /// CashManagement.CloseShutter commands.
        /// 
        /// In either case the CashAcceptor.PresentMedia command may be used if the 
        ///*presentControl property is false.
        /// 
        /// This property is always true if the device has no shutter.
        /// This field applies to all shutters and all positions.
        /// </summary>
        public bool ShutterControl { get; init; } = ShutterControl;

        /// <summary>
        /// Specifies the number of items the intermediate stacker for cash-in can hold.
        /// </summary>
        public int IntermediateStacker { get; init; } = IntermediateStacker;

        /// <summary>
        /// Specifies whether the CashAcceptor can detect when items at the exit position are taken by the user. 
        /// If true the Service generates an accompanying CashManagement.ItemsTakenEvent. 
        /// If false this event is not generated. 
        /// This property relates to all output positions.
        /// </summary>
        public bool ItemsTakenSensor { get; init; } = ItemsTakenSensor;

        public sealed class PositionClass
        {
            [Flags]
            public enum UsageEnum
            {
                NotSupported = 0,
                In = 1 << 0,
                Refuse = 1 << 1,
                Rollback = 1 << 2,
            }

            [Flags]
            public enum RetractAreaEnum
            {
                NotSupported = 0,
                Retract = 1 << 0,
                Reject = 1 << 1,
                Transport = 1 << 2,
                Stacker = 1 << 3,
                BillCassettes = 1 << 4,
                CashIn = 1 << 5,
            }

            /// <summary>
            /// This class representing position capabilities.
            /// </summary>
            public PositionClass(UsageEnum Usage,
                                 bool ShutterControl,
                                 bool ItemsTakenSensor,
                                 bool ItemsInsertedSensor,
                                 bool PresentControl,
                                 bool PreparePresent,
                                 RetractAreaEnum RetractArea)
            {
                this.Usage = Usage;
                this.ShutterControl = ShutterControl;
                this.ItemsTakenSensor = ItemsTakenSensor;
                this.ItemsInsertedSensor = ItemsInsertedSensor;
                this.PresentControl = PresentControl;
                this.PreparePresent = PreparePresent;
                this.RetractArea = RetractArea;
            }

            /// <summary>
            /// Specifies supported input or output position
            /// </summary>
            public CashManagementCapabilitiesClass.PositionEnum Position { get; init; }

            /// <summary>
            /// Specifies supported usage for input, reject or rollback.
            /// </summary>
            public UsageEnum Usage { get; init; }

            /// <summary>
            /// Specifies true if the shutter is controlled implicitly, otherwise false.
            /// </summary>
            public bool ShutterControl { get; init; }

            /// <summary>
            /// Specifies whether or not the described position can detect when items at the exit position are taken by the user.
            /// </summary>
            public bool ItemsTakenSensor { get; init; }

            /// <summary>
            /// Specifies whether the described position has the ability to detect when items have been inserted by the user. 
            /// </summary>
            public bool ItemsInsertedSensor { get; init; }

            /// <summary>
            /// Specifies how the presenting of media items is controlled. 
            /// </summary>
            public bool PresentControl { get; init; }

            /// <summary>
            /// Specifies how the presenting of items is controlled. 
            /// </summary>
            public bool PreparePresent { get; init; }

            /// <summary>
            /// Specifies the areas to which items can be retracted from this position.
            /// </summary>
            public RetractAreaEnum RetractArea { get; init; }
        }

        /// <summary>
        /// Specifies the CashAcceptor input and output positions which are available.
        /// </summary>
        public Dictionary<CashManagementCapabilitiesClass.PositionEnum, PositionClass> Positions { get; init; } = Positions;

        /// <summary>
        /// Retract areas support of this device
        /// </summary>
        public CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas { get; init; } = RetractAreas;

        /// <summary>
        /// Action support on retracting cash to the transport
        /// </summary>
        public CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions { get; init; } = RetractTransportActions;

        /// <summary>
        /// Action support on retracting cash to the stacker
        /// </summary>
        public CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions { get; init; } = RetractStackerActions;

        /// <summary>
        /// Specifies which cash-in limitations are supported for the
        /// CashAcceptor.CashInStart command. If the device does not have the capability to
        /// limit the amount.
        /// </summary>
        public CashInLimitEnum CashInLimit { get; init; } = CashInLimit;

        /// <summary>
        /// Specifies the count action supported by the CashAcceptor.CashUnitCount
        /// command.
        /// </summary>
        public CountActionEnum CountActions { get; init; } = CountActions;

        /// <summary>
        /// If [counterfeit, inked or suspect](#cashmanagement.generalinformation.noteclassification) items
        /// are supported by the Service(see
        /// [classifications](#common.capabilities.completion.description.cashmanagement.classifications)), this
        /// specifies whether such items are retained by the device if detected during a cash-in
        /// transaction.See[acceptor](#common.status.completion.description.cashmanagement.acceptor) for details of
        /// the impact on offering cash-in transactions if unable to retain items due to storage unit status.
        /// 
        /// This applies regardless of whether their specific note type is configured to not
        /// be accepted by [CashAcceptor.ConfigureNoteTypes] (#cashacceptor.configurenotetypes).
        /// </summary>
        public RetainCounterfeitActionEnum RetainCounterfeitAction { get; init; } = RetainCounterfeitAction;
    }
}
