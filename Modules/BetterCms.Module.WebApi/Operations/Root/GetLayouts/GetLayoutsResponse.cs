using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.GetLayouts
{
    [DataContract]
    public class GetLayoutsResponse : ListResponseBase<LayoutModel>
    {
    }
}