using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    public class GetLayoutResponse : ResponseBase<LayoutModel>
    {
    }
}