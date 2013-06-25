using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Blog.GetAuthors
{
    [DataContract]
    public class GetAuthorsResponse : ListResponseBase<AuthorModel>
    {
    }
}