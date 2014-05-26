using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [DataContract]
    [Serializable]
    public class GetAuthorsResponse : ListResponseBase<AuthorModel>
    {
    }
}