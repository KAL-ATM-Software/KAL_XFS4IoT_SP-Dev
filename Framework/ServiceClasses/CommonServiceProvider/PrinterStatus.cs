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
    /// PrinterStatusClass
    /// Store device status for the printer
    /// </summary>
    public sealed class PrinterStatusClass(
        PrinterStatusClass.MediaEnum Media,
        Dictionary<PrinterStatusClass.PaperSourceEnum, PrinterStatusClass.SupplyStatusClass> Paper,
        PrinterStatusClass.TonerEnum Toner,
        PrinterStatusClass.InkEnum Ink,
        PrinterStatusClass.LampEnum Lamp,
        List<PrinterStatusClass.RetractBinsClass> RetractBins,
        int MediaOnStacker,
        PrinterStatusClass.BlackMarkModeEnum BlackMarkMode,
        Dictionary<string, PrinterStatusClass.SupplyStatusClass> CustomPaper = null) : StatusBase
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
            Dual,
            NotSupported,
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

        public sealed class RetractBinsClass : StatusBase
        {
            public RetractBinsClass()
            {
            }
            public RetractBinsClass(StateEnum State,
                                    int Count)
            {
                state = State;
                count = Count;
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
            public StateEnum State 
            { 
                get { return state; }
                set
                {
                    if (state != value)
                    {
                        state = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private StateEnum state = StateEnum.Unknown;
            /// <summary>
            /// The number of media retracted to this bin. This value is persistent; it may be reset to zero by the
            /// Printer.ResetCount command.
            /// </summary>
            public int Count 
            { 
                get { return count; } 
                set
                {
                    if (count != value)
                    {
                        count = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private int count = 0;
        }

        public sealed class SupplyStatusClass : StatusBase
        {
            public SupplyStatusClass()
            {
            }
            public SupplyStatusClass(PaperSupplyEnum PaperSupply,
                                     PaperTypeEnum PaperType)
            {
                paperSupply = PaperSupply;
                paperType = PaperType;
            }

            /// <summary>
            /// Properties Source and CustomSource are set by the framework to generate status changed event
            /// </summary>
            public PaperSourceEnum? Source { get; set; } = null;
            public string CustomSource { get; set; } = null;

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
            public PaperSupplyEnum PaperSupply 
            { 
                get { return paperSupply; } 
                set
                {
                    if (paperSupply != value)
                    {
                        paperSupply = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private PaperSupplyEnum paperSupply = PaperSupplyEnum.NotSupported;

            /// <summary>
            /// Specifies the type of paper loaded as one of the following:
            /// 
            /// * ```Unknown``` - No paper is loaded, reporting of this paper type is not supported or the paper type cannot
            ///   be determined.
            /// * ```Single``` - The paper can be printed on only one side.
            /// * ```Dual``` - The paper can be printed on both sides.
            /// </summary>
            public PaperTypeEnum PaperType 
            {
                get { return paperType; }
                set
                {
                    if (PaperType != value)
                    {
                        paperType = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private PaperTypeEnum paperType = PaperTypeEnum.NotSupported;
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
        public MediaEnum Media 
        {
            get { return media; } 
            set
            {
                if (media != value)
                {
                    media = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MediaEnum media = Media;

        public Dictionary<PaperSourceEnum, SupplyStatusClass> Paper { get; init; } = Paper;

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
        public TonerEnum Toner 
        { 
            get { return toner; }
            set
            {
                if (toner != value)
                {
                    toner = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private TonerEnum toner = Toner;

        /// <summary>
        /// Specifies the status of the stamping ink in the printer as one of the following values:
        /// 
        /// * ```NotSupported``` - Capability not supported by device.
        /// * ```Unknown``` - Status of the stamping ink supply cannot be determined with device in its current state.
        /// * ```Full``` - Ink supply in device is full.
        /// * ```Low``` - Ink supply in device is low.
        /// * ```Out``` - Ink supply in device is empty.
        /// </summary>
        public InkEnum Ink
        {
            get { return ink; }
            set
            {
                if (ink != value)
                {
                    ink = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private InkEnum ink = Ink;

        /// <summary>
        /// Specifies the status of the printer imaging lamp as one of the following values:
        /// 
        /// * ```NotSupported``` - Capability not supported by device.
        /// * ```Unknown``` - Status of the imaging lamp cannot be determined with device in its current state.
        /// * ```Ok``` - The lamp is OK.
        /// * ```Fading``` - The lamp should be changed.
        /// * ```Inop``` - The lamp is inoperative.
        /// </summary>
        public LampEnum Lamp 
        {
            get { return lamp; }
            set
            {
                if (lamp != value)
                {
                    lamp = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private LampEnum lamp = Lamp;

        /// <summary>
        /// An array of bin state objects. If no retain bins are supported, the array will be empty.
        /// </summary>
        public List<RetractBinsClass> RetractBins { get; init; } = RetractBins;

        /// <summary>
        /// The number of media on stacker; applicable only to printers with stacking capability.
        /// -1 if it's unknown.
        /// </summary>
        public int MediaOnStacker 
        {
            get { return mediaOnStacker; }
            set
            {
                if (mediaOnStacker != value)
                {
                    mediaOnStacker = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int mediaOnStacker = MediaOnStacker;

        /// <summary>
        /// Specifies the status of the black mark detection and associated functionality:
        /// 
        /// * ```NotSupported``` - Black mark detection is not supported.
        /// * ```Unknown``` - The status of the black mark detection cannot be determined.
        /// * ```On``` - Black mark detection and associated functionality is switched on.
        /// * ```Off``` - Black mark detection and associated functionality is switched off.
        /// </summary>
        public BlackMarkModeEnum BlackMarkMode 
        {
            get { return blackMarkMode; } 
            set
            {
                if (blackMarkMode != value)
                {
                    blackMarkMode = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private BlackMarkModeEnum blackMarkMode = BlackMarkMode;

        /// <summary>
        /// Paper supply status for vendor specific supply not listed in the prefixed named paper supply
        /// </summary>
        public Dictionary<string, SupplyStatusClass> CustomPaper { get; init; } = CustomPaper;
    }
}
