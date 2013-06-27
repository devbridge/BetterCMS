using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetSitemapTree
{
    [DataContract]
    public class GetSitemapTreeResponse : ResponseBase<System.Collections.Generic.List<NodeModel>>
    {
    }
}