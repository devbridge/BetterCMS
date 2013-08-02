using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [DataContract]
    public class GetLayoutRegionsResponse : ListResponseBase<RegionModel>
    {
    }
}