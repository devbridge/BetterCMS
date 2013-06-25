using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetLayouts
{
    [DataContract]
    public class GetLayoutsResponse : ListResponseBase<LayoutModel>
    {
    }
}