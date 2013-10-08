using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    public class AuthorsService : Service, IAuthorsService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public AuthorsService(IRepository repository, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
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

                        ImageId = author.Image != null && !author.Image.IsDeleted ? author.Image.Id : (Guid?)null,
                        ImageUrl = author.Image != null && !author.Image.IsDeleted ? fileUrlResolver.EnsureFullPathUrl(author.Image.PublicUrl) : (string)null,
                        ImageThumbnailUrl = author.Image != null && !author.Image.IsDeleted ? fileUrlResolver.EnsureFullPathUrl(author.Image.PublicThumbnailUrl) : (string)null,
                        ImageCaption = author.Image != null && !author.Image.IsDeleted ? author.Image.Caption : (string)null
                    })
                .ToDataListResponse(request);

            return new GetAuthorsResponse
                       {
                           Data = listResponse
                       };
        }
    }
}