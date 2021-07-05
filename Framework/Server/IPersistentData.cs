using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    public interface IPersistentData
    {
        /// <summary>
        /// Store
        /// This method stores object persistently on the file system. i.e. hard disk, NVRAM
        /// </summary>
        /// <typeparam name="TValue">Type of object</typeparam>
        /// <param name="name">Namespace for the storage</param>
        /// <param name="obj">The object to serialize data and store it on the file system.</param>
        /// <returns></returns>
        public bool Store<TValue>(string name, TValue obj) where TValue : class;

        /// <summary>
        /// Load
        /// This method load persistent data from the file system and return deserialized data.
        /// </summary>
        /// <typeparam name="TValue">Type of object</typeparam>
        /// <param name="name">Namespace for the storage</param>
        /// <returns>Return deserialized object if the persistent data is read successfully. otherwise null.</returns>
        public TValue Load<TValue>(string name) where TValue : class;
    }
}
