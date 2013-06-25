using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetModules
{
    [DataContract]
    public class GetModulesResponse : ListResponseBase<ModuleModel>
    {
    }
}