using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    public class AuthorsService : Service, IAuthorsService
    {
        public GetAuthorsResponse Get(GetAuthorsRequest request)
        {
            // TODO: need implementation
            return new GetAuthorsResponse
                       {
                           Data = new DataListResponse<AuthorModel>
                                      {
                                          TotalCount = 158,
                                          Items = new List<AuthorModel>
                                                      {
                                                          new AuthorModel(),
                                                          new AuthorModel(),
                                                          new AuthorModel()
                                                      }
                                      }
                       };
        }
    }
}