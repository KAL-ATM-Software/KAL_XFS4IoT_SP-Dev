/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
        /// Load form or media definition 
        /// </summary>
        bool LoadDefinition(string definition, bool overwrite = false);

        /// <summary>
        /// Load a single form or media definition 
        /// </summary>
        bool LoadSingleDefinition(string definition, bool overwrite, out XFS4IoT.Printer.Events.DefinitionLoadedEvent.PayloadData.TypeEnum? type, out string name, out string errorMsg);

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
    }
    public interface IPrinterServiceClass : IPrinterService, IPrinterUnsolicitedEvents
    {
    }
}
