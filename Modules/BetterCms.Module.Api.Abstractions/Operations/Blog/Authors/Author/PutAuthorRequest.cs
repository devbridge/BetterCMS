using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/authors/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutAuthorRequest : PutRequestBase<SaveAuthorModel>, IReturn<PutAuthorResponse>
    {
    }
}