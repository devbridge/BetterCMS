using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [DataContract]
    public class GetLayoutsResponse : ListResponseBase<LayoutModel>
    {
    }
}