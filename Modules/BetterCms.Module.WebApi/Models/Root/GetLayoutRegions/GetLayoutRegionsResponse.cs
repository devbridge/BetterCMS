using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetLayoutRegions
{
    [DataContract]
    public class GetLayoutRegionsResponse : ListResponseBase<RegionModel>
    {
    }
}