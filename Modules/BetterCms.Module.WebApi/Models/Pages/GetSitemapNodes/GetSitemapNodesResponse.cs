using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetSitemapNodes
{
    [DataContract]
    public class GetSitemapNodesResponse : ListResponseBase<NodeModel>
    {
    }
}