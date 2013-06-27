using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.GetLayoutRegions
{
    [DataContract]
    public class GetLayoutRegionsResponse : ListResponseBase<RegionModel>
    {
    }
}