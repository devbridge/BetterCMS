using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [DataContract]
    public class GetLayoutsResponse : ListResponseBase<LayoutModel>
    {
    }
}