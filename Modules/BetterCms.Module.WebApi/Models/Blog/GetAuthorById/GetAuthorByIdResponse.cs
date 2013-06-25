using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Blog.GetAuthorById
{
    [DataContract]
    public class GetAuthorByIdResponse : ResponseBase<AuthorModel>
    {
    }
}