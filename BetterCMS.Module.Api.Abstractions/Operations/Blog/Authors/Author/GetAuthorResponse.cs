using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    [DataContract]
    public class GetAuthorResponse : ResponseBase<AuthorModel>
    {
    }
}