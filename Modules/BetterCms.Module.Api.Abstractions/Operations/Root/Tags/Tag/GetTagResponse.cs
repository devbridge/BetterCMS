using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [DataContract]
    public class GetTagResponse : ResponseBase<TagModel>
    {
    }
}