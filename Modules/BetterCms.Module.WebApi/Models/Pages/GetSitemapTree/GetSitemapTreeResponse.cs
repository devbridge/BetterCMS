using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetSitemapTree
{
    [DataContract]
    public class GetSitemapTreeResponse : ListResponseBase<NodeModel>
    {
    }
}