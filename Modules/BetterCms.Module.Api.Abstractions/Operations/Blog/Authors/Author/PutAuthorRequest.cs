using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutAuthorRequest : PutRequestBase<SaveAuthorModel>
    {
    }
}