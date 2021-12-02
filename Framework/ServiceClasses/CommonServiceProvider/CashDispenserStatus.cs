/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public sealed class CashDispenserStatusClass
    {
        public sealed class PositionStatusClass
        {
            public enum PositionStatusEnum
            {
                Empty,
                NotEmpty,
                Unknown,
                NotSupported
            }

            public enum TransportEnum
            {
                Ok,
                Inoperative,
                Unknown,
                NotSupported
            }

            public enum JammedShutterPositionEnum
            {
                NotSupported,
                NotJammed,
                Open,
                PartiallyOpen,
                Closed,
                Unknown
            }

            public enum TransportStatusEnum
            {
                Empty,
                NotEmpty,
                NotEmptyCustomer,
                NotEmptyUnknown,
                NotSupported
            }

            public PositionStatusClass(CashManagementStatusClass.ShutterEnum Shutter, 
                                       PositionStatusEnum PositionStatus, 
                                       TransportEnum Transport, 
                                       TransportStatusEnum TransportStatus, 
                                       JammedShutterPositionEnum JammedShutterPosition)
            {
                this.Shutter = Shutter;
                this.PositionStatus = PositionStatus;
                this.Transport = Transport;
                this.TransportStatus = TransportStatus;
                this.JammedShutterPosition = JammedShutterPosition;
            }

            public PositionStatusClass()
            {
                Shutter = CashManagementStatusClass.ShutterEnum.Unknown;
                PositionStatus = PositionStatusEnum.Unknown;
                Transport = TransportEnum.Unknown;
                TransportStatus = TransportStatusEnum.NotSupported;
                JammedShutterPosition = JammedShutterPositionEnum.Unknown;
            }

            /// <summary>
            /// Supplies the state of the shutter. Following values are possible:
            /// 
            /// * ```closed``` - The shutter is operational and is closed.
            /// * ```open``` - The shutter is operational and is open.
            /// * ```jammed``` - The shutter is jammed and is not operational. The field jammedShutterPosition provides the positional state of the shutter.
            /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
            /// * ```notSupported``` - The physical device has no shutter or shutter state reporting is not supported.
            /// </summary>
            public CashManagementStatusClass.ShutterEnum Shutter { get; set; }

            /// <summary>
            /// Returns information regarding items which may be at the output position. 
            /// If the device is a recycler it is possible that the output position will not be empty due to a previous cash-in operation.
            /// Following values are possible:
            /// 
            /// * ```empty``` - The output position is empty.
            /// * ```notEmpty``` - The output position is not empty.
            /// * ```unknown``` - Due to a hardware error or other condition, the state of the output position cannot be determined.
            /// * ```notSupported``` - The device is not capable of reporting whether or not items are at the output position.
            /// </summary>
            public PositionStatusEnum PositionStatus { get; set; }

            /// <summary>
            /// Supplies the state of the transport mechanism. The transport is defined as any area leading to or from the position.
            /// Following values are possible:
            /// 
            /// * ```ok``` - The transport is in a good state.
            /// * ```inoperative``` - The transport is inoperative due to a hardware failure or media jam.
            /// * ```unknown``` -Due to a hardware error or other condition the state of the transport cannot be determined.
            /// * ```notSupported``` - The physical device has no transport or transport state reporting is not supported.
            /// </summary>
            public TransportEnum Transport { get; set; }

            /// <summary>
            /// Returns information regarding items which may be on the transport. If the device is a recycler 
            /// device it is possible that the transport will not be empty due to a previous cash-in operation. 
            /// Following values are possible:
            /// 
            /// * ```empty``` - The transport is empty.
            /// * ```notEmpty``` - The transport is not empty.
            /// * ```notEmptyCustomer``` - Items which a customer has had access to are on the transport.
            /// * ```notEmptyUnknown``` - Due to a hardware error or other condition it is not known whether there are items on the transport.
            /// * ```notSupported``` - The device is not capable of reporting whether items are on the transport.
            /// </summary>
            public TransportStatusEnum TransportStatus { get; set; }

            /// <summary>
            /// Returns information regarding the position of the jammed shutter.
            /// Following values are possible:
            /// 
            /// * ```notSupported``` - The physical device has no shutter or the reporting of the position of a jammed shutter is not supported.
            /// * ```notJammed``` - The shutter is not jammed.
            /// * ```open``` - The shutter is jammed, but fully open.
            /// * ```partiallyOpen``` - The shutter is jammed, but partially open.
            /// * ```closed``` - The shutter is jammed, but fully closed.
            /// * ```unknown``` - The position of the shutter is unknown.
            /// </summary>
            public JammedShutterPositionEnum JammedShutterPosition { get; set; }

        }

        public enum IntermediateStackerEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            NotEmptyUnknown,
            Unknown,
            NotSupported
        }

        public CashDispenserStatusClass(IntermediateStackerEnum IntermediateStacker,
                                        Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PositionStatusClass> Positions = null)
        {
            this.IntermediateStacker = IntermediateStacker;
            this.Positions = Positions;
        }

        /// <summary>
        /// Supplies the state of the intermediate stacker. These bills are typically present on the intermediate 
        /// stacker as a result of a retract operation or because a dispense has been performed without a subsequent present.
        /// Following values are possible:
        /// 
        /// * ```empty``` - The intermediate stacker is empty.
        /// * ```notEmpty``` - The intermediate stacker is not empty. The items have not been in customer access.
        /// * ```notEmptyCustomer``` - The intermediate stacker is not empty. The items have been in customer access. If the device is 
        /// a recycler then the items on the intermediate stacker may be there as a result of a previous cash-in operation.
        /// * ```notEmptyUnknown``` - The intermediate stacker is not empty. It is not known if the items have been in customer access.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the intermediate stacker cannot be determined.
        /// * ```notSupported``` - The physical device has no intermediate stacker.
        /// </summary>
        public IntermediateStackerEnum IntermediateStacker { get; set; }

        /// <summary>
        /// Array of structures for each position to which items can be dispensed or presented.
        /// </summary>
        public Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PositionStatusClass> Positions { get; init; }
    }
}
