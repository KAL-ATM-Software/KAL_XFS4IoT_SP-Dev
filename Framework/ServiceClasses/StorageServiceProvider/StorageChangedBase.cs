/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

namespace XFS4IoTFramework.Storage
{
    /// <summary>
    /// Indicate that a class can be used for event type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EventAttribute : Attribute
    {
        public enum EventTypeEnum
        {
            CountChanged,
            StorageChanged,
        }
        public EventTypeEnum Type { get; set; }
    }

    public abstract record StorageChangedBaseRecord : INotifyPropertyChanged
    {
        /// <summary>
        /// Handle common storage changed event. the device class can set to null if the device class wants to disable it.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = null;

        /// <summary>
        /// Constructor
        /// The storage changed event is enabled by default and possible to disable from device class if it's needed.
        /// </summary>
        public StorageChangedBaseRecord()
        {
        }

        /// <summary>
        /// This method must be called in the derived object where the status property value is being changed.
        /// </summary>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string StorageId { get; set; } = null;

        public string ParentPropertyName { get; set; } = null;
    }

    public abstract class StorageChangedBaseClass : INotifyPropertyChanged
    {
        /// <summary>
        /// Handle common storage changed event. the device class can set to null if the device class wants to disable it.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = null;

        /// <summary>
        /// Constructor
        /// The storage changed event is enabled by default and possible to disable from device class if it's needed.
        /// </summary>
        public StorageChangedBaseClass()
        {
        }

        /// <summary>
        /// This method must be called in the derived object where the status property value is being changed.
        /// </summary>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string StorageId { get; set; } = null;
    }
}