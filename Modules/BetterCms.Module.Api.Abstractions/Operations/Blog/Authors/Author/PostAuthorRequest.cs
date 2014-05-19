using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Request for author update or creation.
    /// </summary>
    [Route("/authors", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostAuthorRequest : RequestBase<SaveAuthorModel>, IReturn<PostAuthorResponse>
    {
    }
}