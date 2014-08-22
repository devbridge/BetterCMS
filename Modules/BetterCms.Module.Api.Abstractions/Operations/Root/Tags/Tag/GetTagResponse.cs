using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Response with tag data.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetTagResponse : ResponseBase<TagModel>
    {
    }
}