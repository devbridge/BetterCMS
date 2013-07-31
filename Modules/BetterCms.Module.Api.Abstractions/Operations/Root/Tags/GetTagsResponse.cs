using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [DataContract]
    public class GetTagsResponse : ListResponseBase<TagModel>
    {
    }
}