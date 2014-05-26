using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Response after author saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutAuthorResponse : SaveResponseBase
    {
    }
}