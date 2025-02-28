/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Collections.Generic;
using XFS4IoTFramework.Printer;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface IPrinterService
    {
        /// <summary>
        /// Set form definition 
        /// </summary>
        bool SetForm(string definition, Form from);

        /// <summary>
        /// Set media definition
        /// </summary>
        bool SetMedia(string definition, Media media);

        /// <summary>
        /// Return a list of forms loaded
        /// </summary>
        Dictionary<string, Form> GetForms();

        /// <summary>
        /// Return a list of medias loaded
        /// </summary>
        Dictionary<string, Media> GetMedias();

        /// <summary>
        /// Print jobs asking the device class to print
        /// </summary>
        PrintJobClass PrintJob { get; }

        /// <summary>
        /// The method can convert print job to a single image that can be sent to the printer.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="bitCount">Bits per pixel in returned data</param>
        /// <param name="UpsideDown"></param>
        /// <param name="imageInfo">Information image created</param>
        bool PrintToBitmap(PrintJobClass job, int bitCount, bool UpsideDown, out ImageInfo imageInfo);

        /// <summary>
        /// This method can be called in the device class to obtain the dimensions of a task object when printed using PrintToImage
        /// </summary>
        /// <param name="task">Task to print data</param>
        /// <param name="width">Width of rectangle needed to contain the task</param>
        /// <param name="height">Height of rectangle needed to contain the task</param>
        bool GetBitmapPrintDimensions(PrintTask task, out int width, out int height);
    }
    public interface IPrinterServiceClass : IPrinterService, IPrinterUnsolicitedEvents
    {
    }
}
