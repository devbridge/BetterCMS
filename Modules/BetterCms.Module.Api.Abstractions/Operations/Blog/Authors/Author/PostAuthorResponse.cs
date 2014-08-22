using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Author creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostAuthorResponse : SaveResponseBase
    {
    }
}