using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [DataContract]
    public class PostTagsResponse : ResponseBase<Guid?>
    {
    }
}