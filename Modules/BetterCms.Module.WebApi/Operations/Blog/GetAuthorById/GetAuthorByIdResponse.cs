using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetAuthorById
{
    [DataContract]
    public class GetAuthorByIdResponse : ResponseBase<AuthorModel>
    {
    }
}