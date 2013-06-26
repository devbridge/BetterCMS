using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetSitemapTree
{
    [DataContract]
    public class GetSitemapTreeResponse : ResponseBase<System.Collections.Generic.List<NodeModel>>
    {
    }
}