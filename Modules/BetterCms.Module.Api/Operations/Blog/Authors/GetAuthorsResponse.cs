using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [DataContract]
    public class GetAuthorsResponse : ListResponseBase<AuthorModel>
    {
    }
}