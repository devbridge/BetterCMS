using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    public class AuthorsService : Service, IAuthorsService
    {
        private readonly IRepository repository;

        public AuthorsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetAuthorsResponse Get(GetAuthorsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Blog.Models.Author>()
                .Select(author => new AuthorModel
                    {
                        Id = author.Id,
                        Version = author.Version,
                        CreatedBy = author.CreatedByUser,
                        CreatedOn = author.CreatedOn,
                        LastModifiedBy = author.ModifiedByUser,
                        LastModifiedOn = author.ModifiedOn,

                        Name = author.Name,
                        ImageId = author.Image != null ? author.Image.Id : (System.Guid?)null,
                        ImageUrl = author.Image != null ? author.Image.PublicUrl : null,
                        ImageThumbnailUrl = author.Image != null ? author.Image.PublicThumbnailUrl : null,
                        ImageCaption= author.Image != null ? author.Image.Caption : null
                    })
                .ToDataListResponse(request);

            return new GetAuthorsResponse
                       {
                           Data = listResponse
                       };
        }
    }
}