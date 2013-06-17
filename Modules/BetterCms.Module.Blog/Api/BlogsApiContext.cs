using System;
using System.Linq;

using Autofac;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Blog.Api.Events;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public class BlogsApiContext : DataApiContext
    {
        private static readonly BlogsApiEvents events;

        private readonly ITagService tagService;

        private readonly IBlogService blogService;

        private readonly IAuthorService authorService;

        /// <summary>
        /// Initializes the <see cref="BlogsApiContext" /> class.
        /// </summary>
        static BlogsApiContext()
        {
            events = new BlogsApiEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogsApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="blogService">The blog service.</param>
        /// <param name="authorService">The author service.</param>
        /// <param name="repository">The repository.</param>
        public BlogsApiContext(ILifetimeScope lifetimeScope, ITagService tagService, IBlogService blogService, IAuthorService authorService, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
            this.tagService = tagService;
            this.blogService = blogService;
            this.authorService = authorService;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public new static BlogsApiEvents Events
        {
            get
            {
                return events;
            }
        }

        /// <summary>
        /// Gets the list of blog entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of blog entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        /*[Obsolete]
        public DataListResponse<BlogPostModel> GetBlogPosts_OLD(GetBlogPostsRequest request)
        {
            try
            {
                var query = Repository
                    .AsQueryable<BlogPost>()
                    .ApplyFilters(request);

                if (!request.IncludeUnpublished)
                {
                    query = query.Where(b => b.Status == PageStatus.Published);
                }

                if (!request.IncludeNotActive)
                {
                    query = query.Where(b => b.ActivationDate < DateTime.Now && (!b.ExpirationDate.HasValue || DateTime.Now < b.ExpirationDate.Value));
                }

                var totalCount = query.ToRowCountFutureValue(request);

                query = query
                    .AddOrderAndPaging(request)
                    .Fetch(b => b.Author);

                return query.ToDataListResponse(totalCount);
            }
            catch (Exception inner)
            {
                const string message = "Failed to get blog posts list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }*/

        public DataListResponse<BlogPostModel> GetBlogPosts(GetBlogPostsRequest request)
        {
            return blogService
                .GetBlogPostsAsQueryable(request)
                .ToDataListResponse(request);
        }

        /// <summary>
        /// Gets the list of author entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        [Obsolete("This method is obsolete; use method GetAuthorsAsQueryable() instead.")]
        public DataListResponse<Author> GetAuthors(GetAuthorsRequest request = null)
        {
            try
            {
                var query = Repository
                    .AsQueryable<Author>()
                    .ApplyFilters(request);

                var totalCount = query.ToRowCountFutureValue(request);

                query = query
                    .AddOrderAndPaging(request)
                    .Fetch(a => a.Image);

                return query.ToDataListResponse(totalCount);

            }
            catch (Exception inner)
            {
                const string message = "Failed to get authors list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Updates the blog post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public BlogPost UpdateBlogPost(UpdateBlogPostRequest request)
        {
            ValidateRequest(request);
            
            if (request.LiveToDate.HasValue && request.LiveToDate < request.LiveFromDate)
            {
                var message = string.Format("Expiration date must be greater that activation date.");
                Logger.Error(message);
                throw new CmsApiValidationException(message);
            }

            try
            {
                UnitOfWork.BeginTransaction(); 
                var blog = Repository
                    .AsQueryable<BlogPost>(b => b.Id == request.Id)
                    .FirstOne();

                blog.Version = request.Version;
                blog.Title = request.Title;
                blog.Description = request.IntroText;

                // TODO: update only is content is published.
                // blog.ActivationDate = request.LiveFromDate;
                // blog.ExpirationDate = request.LiveToDate;

                blog.Image = request.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.ImageId.Value) : null;
                blog.Author = request.AuthorId.HasValue ? Repository.AsProxy<Author>(request.AuthorId.Value) : null;
                blog.Category = request.CategoryId.HasValue ? Repository.AsProxy<Category>(request.CategoryId.Value) : null;

                // TODO: should it be allowed to change blog content, url and status?

                Repository.Save(blog);

                /*IList<Tag> newTags = null;
                tagService.SavePageTags(blog, request.Tags, out newTags);*/

                UnitOfWork.Commit();

                Events.OnBlogUpdated(blog);

                // Notify about new created tags.
                /*PagesApiContext.Events.OnTagCreated(newTags);*/

                return blog;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to update blog post {0}.", request.Id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        public IQueryable<AuthorModel> GetAuthorsAsQueryable()
        {
            var guid = Guid.NewGuid();
            var models = authorService.GetAuthorsAsQueryable().Where(a => a.Image != null && a.Image.Id == guid);
            return models;
        }

        public AuthorCreateResponce CreateAuthor(AuthorCreateRequest request)
        {
            return authorService.CreateAuthor(request);
        }

        public AuthorUpdateResponce UpdateAuthor(AuthorUpdateRequest request)
        {
            return authorService.UpdateAuthor(request);
        }

        public AuthorDeleteResponce DeleteAuthor(AuthorDeleteRequest request)
        {
            return authorService.DeleteAuthor(request);
        }

        public IQueryable<BlogPostModel> GetBlogPostsAsQueryable(GetBlogPostsRequest request = null)
        {
            var models = blogService.GetBlogPostsAsQueryable(request);

            return models;
        }
    }
}