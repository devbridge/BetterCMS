using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    public class AuthorService : Service, IAuthorService
    {
        public GetAuthorResponse Get(GetAuthorRequest request)
        {
            // TODO: need implementation
            return new GetAuthorResponse
                       {
                           Data = new AuthorModel
                                      {
                                          Id = request.AuthorId
                                      }
                       };
        }
    }
}