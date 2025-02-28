﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    public sealed class BarcodeReaderStatusClass(
        BarcodeReaderStatusClass.ScannerStatusEnum ScannerStatus = BarcodeReaderStatusClass.ScannerStatusEnum.Unknown) : StatusBase
    {
        public enum ScannerStatusEnum
        {
            On,
            Off,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Specifies the scanner status (laser, camera or other technology) as one of the following:
        /// 
        /// * ```on``` - Scanner is enabled for reading.
        /// * ```off``` - Scanner is disabled.
        /// * ```inoperative``` - Scanner is inoperative due to a hardware error.
        /// * ```unknown``` - Scanner status cannot be determined.
        /// </summary>
        public ScannerStatusEnum ScannerStatus 
        {
            get { return scannerStatus; } 
            set
            {
                if (scannerStatus != value)
                {
                    scannerStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ScannerStatusEnum scannerStatus = ScannerStatus;
    }
}
