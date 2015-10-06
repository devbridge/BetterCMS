using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Request for author update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostAuthorRequest : RequestBase<SaveAuthorModel>
    {
    }
}