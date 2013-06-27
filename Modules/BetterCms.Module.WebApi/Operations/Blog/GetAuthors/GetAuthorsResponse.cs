using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetAuthors
{
    [DataContract]
    public class GetAuthorsResponse : ListResponseBase<AuthorModel>
    {
    }
}