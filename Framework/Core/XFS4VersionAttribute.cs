using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoT
{
    /// <summary>
    /// Indicate the XFS4 version for the Command/Completion/Event
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class XFS4VersionAttribute : Attribute
    {
        public string Version { get; set; }
    }
}
