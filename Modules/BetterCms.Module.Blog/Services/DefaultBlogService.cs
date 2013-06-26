using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Blog.Api.Mappers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Helpers;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogService : IBlogService
    {
        /// <summary>
        /// The cms configuration
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlogService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="repository">The repository.</param>
        public DefaultBlogService(ICmsConfiguration configuration, IUrlService urlService, IRepository repository)
        {
            this.configuration = configuration;
            this.urlService = urlService;
            this.repository = repository;
        }

        /// <summary>
        /// Creates the blog URL from the given blog title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>Created blog URL</returns>
        public string CreateBlogPermalink(string title)
        {
            var url = title.Transliterate();
            url = urlService.AddPageUrlPostfix(url, configuration.ArticleUrlPattern);

            return url;
        }

        /// <summary>
        /// Gets the list of blog service models.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The list of blog service models
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public IQueryable<BlogPostModel> GetBlogPostsAsQueryable(GetBlogPostsRequest filter = null)
        {
            var models = repository.AsQueryable<Models.BlogPost>();

            if (filter == null || !filter.IncludeUnpublished)
            {
                models = models.Where(b => b.Status == PageStatus.Published);
            }

            if (filter == null || !filter.IncludeNotActive)
            {
                models = models.Where(b => b.ActivationDate < DateTime.Now && (!b.ExpirationDate.HasValue || DateTime.Now < b.ExpirationDate.Value));
            }
            
            if (filter == null || !filter.IncludeArchivedItems)
            {
                models = models.Where(b => !b.IsArchived);
            }

            return models.Select(EntityToServiceModelMapper.ToBlogPostModel);
                /*blog =>
                    new BlogPostModel
                    {
                        Id = blog.Id,
                        Version = blog.Version,
                        PageUrl = blog.PageUrl,
                        Title = blog.Title,
                        IntroText = blog.Description,
                        CreatedOn = blog.CreatedOn,
                        ModifiedOn = blog.ModifiedOn,
                        CreatedByUser = blog.CreatedByUser,
                        ModifiedByUser = blog.ModifiedByUser,
                        Status = blog.Status,
                        ActivationDate = blog.ActivationDate,
                        ExpirationDate = blog.ExpirationDate,
                        CategoryId = blog.Category.Id,
                        CategoryName = blog.Category.Name,
                        AuthorId = blog.Author.Id,
                        AuthorName = blog.Author.Name,
                        MainImageId = blog.Image.Id,
                        MainImagePublicUrl = blog.Image.PublicUrl,
                        FeaturedImageId = blog.FeaturedImage.Id,
                        FeaturedImagePublicUrl = blog.FeaturedImage.PublicUrl,
                        SecondaryImageId = blog.SecondaryImage.Id,
                        SecondaryImagePublicUrl = blog.SecondaryImage.PublicUrl
                    });*/
        }
    }
}