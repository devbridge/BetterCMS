using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [DataContract]
    [Serializable]
    public class GetTagsResponse : ListResponseBase<TagModel>
    {
    }
}