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

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CashAcceptorCapabilitiesClass
    /// Store device capabilites for the cash acceptor device
    /// </summary>
    public sealed class CashAcceptorCapabilitiesClass
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

        public enum CounterfeitActionEnum
        {
            None,
            Level2,
            Level23
        }

        public CashAcceptorCapabilitiesClass(CashManagementCapabilitiesClass.TypeEnum Type,
                                             int MaxCashInItems,
                                             bool Shutter,
                                             bool ShutterControl,
                                             int IntermediateStacker,
                                             bool ItemsTakenSensor,
                                             CashManagementCapabilitiesClass.PositionEnum Positions,
                                             CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas,
                                             CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions,
                                             CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions,
                                             CashInLimitEnum CashInLimit,
                                             CountActionEnum CountActions,
                                             CounterfeitActionEnum CounterfeitAction)
        {
            this.Type = Type;
            this.MaxCashInItems = MaxCashInItems;
            this.Shutter = Shutter;
            this.ShutterControl = ShutterControl;
            this.IntermediateStacker = IntermediateStacker;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.Positions = Positions;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.CashInLimit = CashInLimit;
            this.CountActions = CountActions;
            this.CounterfeitAction = CounterfeitAction;
        }

        /// <summary>
        /// Supplies the type of CashAcceptor. The following values are possible:
        /// 
        /// * ```TellerBill``` - The CashAcceptor is a Teller Bill Acceptor.
        /// * ```SelfServiceBill``` - The CashAcceptor is a Self-Service Bill Acceptor.
        /// * ```TellerCoin``` - The CashAcceptor is a Teller Coin Acceptor.
        /// * ```SelfServiceCoin``` - The CashAcceptor is a Self-Service Coin Acceptor.
        /// </summary>
        public CashManagementCapabilitiesClass.TypeEnum Type { get; init; }

        /// <summary>
        /// Supplies the maximum number of items that can be accepted in a single 
        /// CashAcceptor.CashIn command. This value reflects the hardware limitations of the 
        /// device and therefore it does not change as part of the 
        /// CashAcceptor.CashInStart command.
        /// </summary>
        public int MaxCashInItems { get; init; } = 1;

        /// <summary>
        /// If true then the device has a shutter and explicit shutter control through the commands 
        /// CashManagement.OpenShutter and CashManagement.CloseShutter is supported. The definition of a shutter will depend on the h/w 
        /// implementation. On some devices where items are automatically detected and accepted then a shutter is simply
        /// a latch that is opened and closed, usually under implicit control by the Service. On other devices, the term
        /// shutter refers to a door, which is opened and closed to allow the customer to place the items onto a tray.
        /// If a Service cannot detect when items are inserted and there is a shutter on the device, then it must 
        /// provide explicit application control of the shutter.
        /// </summary>
        public bool Shutter { get; init; }

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
        public bool ShutterControl { get; init; }

        /// <summary>
        /// Specifies the number of items the intermediate stacker for cash-in can hold.
        /// </summary>
        public int IntermediateStacker { get; init; }

        /// <summary>
        /// Specifies whether the CashAcceptor can detect when items at the exit position are taken by the user. 
        /// If true the Service generates an accompanying CashManagement.ItemsTakenEvent. 
        /// If false this event is not generated. 
        /// This property relates to all output positions.
        /// </summary>
        public bool ItemsTakenSensor { get; init; }

        /// <summary>
        /// Specifies the CashAcceptor input and output positions which are available.
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum Positions { get; init; }

        /// <summary>
        /// Retract areas support of this device
        /// </summary>
        public CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas { get; init; }

        /// <summary>
        /// Action support on retracting cash to the transport
        /// </summary>
        public CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions { get; init; }

        /// <summary>
        /// Action support on retracting cash to the stacker
        /// </summary>
        public CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions { get; init; }

        /// <summary>
        /// Specifies which cash-in limitations are supported for the
        /// CashAcceptor.CashInStart command. If the device does not have the capability to
        /// limit the amount.
        /// </summary>
        public CashInLimitEnum CashInLimit { get; init; }

        /// <summary>
        /// Specifies the count action supported by the CashAcceptor.CashUnitCount
        /// command.
        /// </summary>
        public CountActionEnum CountActions { get; init; }

        /// <summary>
        /// Specifies whether counterfeit or suspect items (see 
        /// [Note Classification](#cashmanagement.generalinformation.noteclassification)) are allowed to be returned
        /// to the customer during a cash-in transaction. If items are not to be returned to the customer by 
        /// these rules, they will not be returned regardless of whether their specific note type is configured to not 
        /// be accepted by [CashAcceptor.ConfigureNoteTypes](#cashacceptor.configurenotetypes). The following rules are 
        /// possible:
        /// 
        /// * ```None``` - The device is not able to classify items as counterfeit or suspect.
        /// * ```Level2``` - Items are classified including counterfeit or suspect. Counterfeit items will not be returned to 
        /// the customer in a cash-in transaction.
        /// * ```Level23``` - Items are classified including counterfeit or suspect. Counterfeit and suspect items will not be 
        /// returned to the customer in a cash-in transaction.
        /// </summary>
        public CounterfeitActionEnum CounterfeitAction { get; init; }
    }
}
