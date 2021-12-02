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
    /// <summary>
    /// PrinterStatusClass
    /// Store device status for the printer
    /// </summary>
    public sealed class PrinterStatusClass
    {
        public enum MediaEnum
        {
            NotSupported,
            Unknown,
            Present,
            NotPresent,
            Jammed,
            Entering,
            Retracted
        }

        public enum PaperSourceEnum
        {
            Upper,
            Lower,
            External,
            AUX,
            AUX2,
            Park,
        }

        public enum PaperSupplyEnum
        {
            NotSupported,
            Unknown,
            Full,
            Low,
            Out,
            Jammed
        }

        public enum PaperTypeEnum
        {
            Unknown,
            Single,
            Dual
        }

        public enum BlackMarkModeEnum
        {
            NotSupported,
            Unknown,
            On,
            Off
        }

        public enum TonerEnum
        {
            NotSupported,
            Unknown,
            Full,
            Low,
            Out
        }

        public enum InkEnum
        {
            NotSupported,
            Unknown,
            Full,
            Low,
            Out

        }

        public enum LampEnum
        {
            NotSupported,
            Unknown,
            Ok,
            Fading,
            Inop
        }

        public sealed class RetractBinsClass
        {
            public RetractBinsClass()
            {
                State = StateEnum.Unknown;
                Count = 0;
            }
            public RetractBinsClass(StateEnum State,
                                    int Count)
            {
                this.State = State;
                this.Count = Count;
            }

            public enum StateEnum
            {
                Unknown,
                Ok,
                Full,
                High,
                Missing
            }

            /// <summary>
            /// Specifies the state of the printer retract bin as one of the following:
            /// 
            /// * ```Ok``` - The retract bin of the printer is in a healthy state.
            /// * ```Full``` - The retract bin of the printer is full.
            /// * ```Unknown``` - Status cannot be determined with device in its current state.
            /// * ```High``` - The retract bin of the printer is nearly full.
            /// * ```Missing``` - The retract bin is missing.
            /// </summary>
            public StateEnum State { get; set; }

            /// <summary>
            /// The number of media retracted to this bin. This value is persistent; it may be reset to zero by the
            /// Printer.ResetCount command.
            /// </summary>
            public int Count { get; set; }
        }

        public sealed class SupplyStatusClass
        {
            public SupplyStatusClass()
            {
                PaperSupply = PaperSupplyEnum.Unknown;
                PaperType = PaperTypeEnum.Unknown;
            }
            public SupplyStatusClass(PaperSupplyEnum PaperSupply,
                                     PaperTypeEnum PaperType)
            {
                this.PaperSupply = PaperSupply;
                this.PaperType = PaperType;
            }

            /// <summary>
            /// Specifies the state of paper supplies as one of the following values:
            /// 
            /// * ```NotSupported``` - Capability not supported by the device.
            /// * ```Unknown``` - Status cannot be determined with device in its current state.
            /// * ```Full``` - The paper supply is full.
            /// * ```Low``` - The paper supply is low.
            /// * ```Out``` - The paper supply is empty.
            /// * ```Jammed``` - The paper supply is jammed.
            /// </summary>
            public PaperSupplyEnum PaperSupply { get; set; }
            /// <summary>
            /// Specifies the type of paper loaded as one of the following:
            /// 
            /// * ```Unknown``` - No paper is loaded, reporting of this paper type is not supported or the paper type cannot
            ///   be determined.
            /// * ```Single``` - The paper can be printed on only one side.
            /// * ```Dual``` - The paper can be printed on both sides.
            /// </summary>
            public PaperTypeEnum PaperType { get; set; }
        }

        public PrinterStatusClass(MediaEnum Media,
                                  Dictionary<PaperSourceEnum, SupplyStatusClass> Paper,
                                  TonerEnum Toner,
                                  InkEnum Ink,
                                  LampEnum Lamp,
                                  List<RetractBinsClass> RetractBins,
                                  int MediaOnStacker,
                                  BlackMarkModeEnum BlackMarkMode,
                                  Dictionary<string, SupplyStatusClass> CustomPaper = null)
        {
            this.Media = Media;
            this.Paper = Paper;
            this.Toner = Toner;
            this.Ink = Ink;
            this.Lamp = Lamp;
            this.RetractBins = RetractBins;
            this.MediaOnStacker = MediaOnStacker;
            this.BlackMarkMode = BlackMarkMode;
            this.CustomPaper = CustomPaper;
        }

        /// <summary>
        /// Specifies the state of the print media (i.e. receipt, statement, passbook, etc.) as one of the following
        /// values. This field does not apply to journal printers:
        /// 
        /// * ```NotSupported``` - The capability to report the state of the print media is not supported by the device.
        /// * ```Unknown``` - The state of the print media cannot be determined with the device in its current state.
        /// * ```Present``` - Media is in the print position, on the stacker or on the transport (i.e. a passbook in the
        ///   parking station is not considered to be present). On devices with continuous paper supplies, this value is
        ///   set when paper is under the print head. On devices with no supply or individual sheet supplies, this value
        ///   is set when paper/media is successfully inserted/loaded.
        /// * ```NotPresent``` - Media is not in the print position or on the stacker.
        /// * ```Jammed``` - Media is jammed in the device.
        /// * ```Entering``` - Media is at the entry/exit slot of the device.
        /// * ```Retracted``` - Media was retracted during the last command which controlled media.
        /// </summary>
        public MediaEnum Media { get; set; }

        
        public Dictionary<PaperSourceEnum, SupplyStatusClass> Paper { get; set; }

        /// <summary>
        /// Specifies the state of the toner or ink supply or the state of the ribbon as one of the following:
        /// 
        /// * ```NotSupported``` - Capability not supported by device.
        /// * ```Unknown``` - Status of toner or ink supply or the ribbon cannot be determined with device in its 
        ///   current state.
        /// * ```Full``` - The toner or ink supply is full or the ribbon is OK.
        /// * ```Low``` - The toner or ink supply is low or the print contrast with a ribbon is weak.
        /// * ```Out``` - The toner or ink supply is empty or the print contrast with a ribbon is not sufficient any
        ///   more.
        /// </summary>
        public TonerEnum Toner { get; set; }

        /// <summary>
        /// Specifies the status of the stamping ink in the printer as one of the following values:
        /// 
        /// * ```NotSupported``` - Capability not supported by device.
        /// * ```Unknown``` - Status of the stamping ink supply cannot be determined with device in its current state.
        /// * ```Full``` - Ink supply in device is full.
        /// * ```Low``` - Ink supply in device is low.
        /// * ```Out``` - Ink supply in device is empty.
        /// </summary>
        public InkEnum Ink { get; set; }

        /// <summary>
        /// Specifies the status of the printer imaging lamp as one of the following values:
        /// 
        /// * ```NotSupported``` - Capability not supported by device.
        /// * ```Unknown``` - Status of the imaging lamp cannot be determined with device in its current state.
        /// * ```Ok``` - The lamp is OK.
        /// * ```Fading``` - The lamp should be changed.
        /// * ```Inop``` - The lamp is inoperative.
        /// </summary>
        public LampEnum Lamp { get; set; }

        /// <summary>
        /// An array of bin state objects. If no retain bins are supported, the array will be empty.
        /// </summary>
        public List<RetractBinsClass> RetractBins { get; set; }

        /// <summary>
        /// The number of media on stacker; applicable only to printers with stacking capability.
        /// </summary>
        public int MediaOnStacker { get; set; }

        /// <summary>
        /// Specifies the status of the black mark detection and associated functionality:
        /// 
        /// * ```NotSupported``` - Black mark detection is not supported.
        /// * ```Unknown``` - The status of the black mark detection cannot be determined.
        /// * ```On``` - Black mark detection and associated functionality is switched on.
        /// * ```Off``` - Black mark detection and associated functionality is switched off.
        /// </summary>
        public BlackMarkModeEnum BlackMarkMode { get; set; }

        /// <summary>
        /// Paper supply status for vendor specific supply not listed in the prefixed named paper supply
        /// </summary>
        public Dictionary<string, SupplyStatusClass> CustomPaper { get; set; }
    }
}
