using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTServer
{
    /// <summary>
    /// Interface for using JsonSchema validation library for testing in comming
    /// command message
    /// </summary>
    public interface IJsonSchemaValidator
    {
        /// <summary>
        /// SP framework call once the ServicePublisher object gets created to load 
        /// any of JSON schema library to validate XFS4 command message.
        /// </summary>
        Task LoadSchemaAsync();

        /// <summary>
        /// This method will be called when the SP framework receives in-comming command messages.
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="FailedReason"></param>
        /// <returns></returns>
        bool Validate(string Command, out string FailedReason);

        /// <summary>
        /// This property must set if the XFS4 JSON schema is loaded successfully.
        /// If this property is set to false, Validate method won't be called from the SP framework.
        /// </summary>
        bool SchemaLoaded { get; set; }
    }
}
