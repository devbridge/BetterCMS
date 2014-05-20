using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Response for author delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteAuthorResponse : DeleteResponseBase
    {
    }
}