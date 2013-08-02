using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [DataContract]
    public class GetAuthorsResponse : ListResponseBase<AuthorModel>
    {
    }
}