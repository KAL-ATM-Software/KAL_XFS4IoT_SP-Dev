/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

// KAL specific implementation of barcodereader. 
namespace XFS4IoTFramework.BarcodeReader
{
    public interface IBarcodeReaderDevice : IDevice
    {
        /// <summary>
        /// This command enables the barcode reader.
        /// The barcode reader will scan for barcodes and when it successfully manages to read one or more barcodes the command will complete.
        /// The completion event for this command contains thescanned barcode data.
        /// The device waits for the period of time specified by the property in the ReadRequest.
        /// </summary>
        Task<ReadResult> Read(ReadRequest request,
                              CancellationToken cancellation);

        /// <summary>
        /// Perform device reset command.
        /// </summary>
        Task<DeviceResult> ResetDevice(CancellationToken cancellation);

        /// <summary>
        /// BarcodeReader Status
        /// </summary>
        BarcodeReaderStatusClass BarcodeReaderStatus { get; set; }

        /// <summary>
        /// BarcodeReader Capabilities
        /// </summary>
        BarcodeReaderCapabilitiesClass BarcodeReaderCapabilities { get; set; }

    }
}
