using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [DataContract]
    public class GetTagsResponse : ListResponseBase<TagModel>
    {
    }
}