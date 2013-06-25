using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetModuleByName
{
    [DataContract]
    public class GetModuleByNameRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        [DataMember(Order = 10, Name = "moduleName")]
        public string ModuleName { get; set; }
    }
}