using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [DataContract]
    public class GetLayoutRegionsResponse : ListResponseBase<RegionModel>
    {
    }
}