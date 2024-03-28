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
    public sealed class CashDispenserStatusClass(
        CashDispenserStatusClass.IntermediateStackerEnum IntermediateStacker,
        Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashManagementStatusClass.PositionStatusClass> Positions = null) : StatusBase
    {
        public enum IntermediateStackerEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            NotEmptyUnknown,
            Unknown,
            NotSupported,
        }

        /// <summary>
        /// Supplies the state of the intermediate stacker. These bills are typically present on the intermediate 
        /// stacker as a result of a retract operation or because a dispense has been performed without a subsequent present.
        /// Following values are possible:
        /// 
        /// * ```Empty``` - The intermediate stacker is empty.
        /// * ```NotEmpty``` - The intermediate stacker is not empty. The items have not been in customer access.
        /// * ```NotEmptyCustomer``` - The intermediate stacker is not empty. The items have been in customer access. If the device is 
        /// a recycler then the items on the intermediate stacker may be there as a result of a previous cash-in operation.
        /// * ```NotEmptyUnknown``` - The intermediate stacker is not empty. It is not known if the items have been in customer access.
        /// * ```Unknown``` - Due to a hardware error or other condition, the state of the intermediate stacker cannot be determined.
        /// * ```NotSupported``` - The physical device has no intermediate stacker.
        /// </summary>
        public IntermediateStackerEnum IntermediateStacker 
        {
            get { return intermediateStacker; }
            set
            {
                if (intermediateStacker != value)
                {
                    intermediateStacker = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private IntermediateStackerEnum intermediateStacker = IntermediateStacker;

        /// <summary>
        /// Array of structures for each position to which items can be dispensed or presented.
        /// </summary>
        public Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashManagementStatusClass.PositionStatusClass> Positions { get; init; } = Positions;
    }
}
