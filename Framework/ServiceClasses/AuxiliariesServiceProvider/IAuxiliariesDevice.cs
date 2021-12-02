/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * IAuxiliariesDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of auxiliaries. 
namespace XFS4IoTFramework.Auxiliaries
{
    public interface IAuxiliariesDevice : IDevice
    {

        /// <summary>
        /// This command is used to retrieve the availability of the auto start-up time function as well as the currentconfiguration of the auto start-up time.
        /// </summary>
        Task<XFS4IoT.Auxiliaries.Completions.GetAutoStartupTimeCompletion.PayloadData> GetAutoStartupTime(IGetAutoStartupTimeEvents events, 
                                                                                                          XFS4IoT.Auxiliaries.Commands.GetAutoStartupTimeCommand.PayloadData payload, 
                                                                                                          CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to clear the time at which the machine will automatically start.
        /// </summary>
        Task<XFS4IoT.Auxiliaries.Completions.ClearAutoStartupTimeCompletion.PayloadData> ClearAutoStartupTime(IClearAutoStartupTimeEvents events, 
                                                                                                              XFS4IoT.Auxiliaries.Commands.ClearAutoStartupTimeCommand.PayloadData payload, 
                                                                                                              CancellationToken cancellation);

        /// <summary>
        /// This command is used to register for, or deregister events from the Auxiliaries Unit. Thedefault condition is that all events are deregistered. The events are only registered or deregistered for the session which sends the command, all other sessions are unaffected.No action has been taken if this command returns an error. If a hardware error occurs while executing the command,the command will return OK, but events will be generated which indicates the auxiliaries which have failed.
        /// </summary>
        Task<XFS4IoT.Auxiliaries.Completions.RegisterCompletion.PayloadData> Register(IRegisterEvents events, 
                                                                                      XFS4IoT.Auxiliaries.Commands.RegisterCommand.PayloadData payload, 
                                                                                      CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to set or clear one or more device auxiliaries.
        /// </summary>
        Task<XFS4IoT.Auxiliaries.Completions.SetAuxiliariesCompletion.PayloadData> SetAuxiliaries(ISetAuxiliariesEvents events, 
                                                                                                  XFS4IoT.Auxiliaries.Commands.SetAuxiliariesCommand.PayloadData payload, 
                                                                                                  CancellationToken cancellation);

        /// <summary>
        /// This command is used to set the time at which the machine will automatically start. It is also used to disable automatic start-up.If a new start-up time is set by this command it will replace any previously set start-up time.Before the auto start-up can take place the operating system must be shut down.
        /// </summary>
        Task<XFS4IoT.Auxiliaries.Completions.SetAutostartupTimeCompletion.PayloadData> SetAutostartupTime(ISetAutostartupTimeEvents events, 
                                                                                                          XFS4IoT.Auxiliaries.Commands.SetAutostartupTimeCommand.PayloadData payload, 
                                                                                                          CancellationToken cancellation);

    }
}
