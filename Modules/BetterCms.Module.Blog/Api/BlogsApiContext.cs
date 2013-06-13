using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Blog.Api.Events;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;

using NHibernate.Criterion;
using NHibernate.OData;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public class BlogsApiContext : DataApiContext
    {
        private readonly ITagService tagService;

        private readonly IAuthorService authorService;

        private static readonly BlogsApiEvents events;

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
        /// <param name="repository">The repository.</param>
        public BlogsApiContext(ILifetimeScope lifetimeScope, ITagService tagService, IAuthorService authorService, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
            this.tagService = tagService;
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

        public IQueryable<BlogPost> GetBlogPostsQueryable()
        {
            return Repository.AsQueryable<BlogPost>();

            /*var criteria = UnitOfWork
                .Session
                .ODataQuery<BlogPost>();*/

            /*if (tags != null && tags.Length > 0)
            {
                PageTag pageTagAlias = null;
                BlogPost blogAlias = null;

                DetachedCriteria tagCriteria = DetachedCriteria.For<PageTag>("p");
                //tagCriteria.createAlias("state", "st");
                //tagCriteria.Add(Restrictions.Eq("st.abbreviation", abbreviation));
                tagCriteria.Add(Restrictions.EqProperty("p.Id", "Id"));

                criteria.Add(Subqueries.Exists(tagCriteria.SetProjection(Projections.Property("p.Id"))));

//                DetachedCriteria dCriteria = DetachedCriteria.For<PageTag>()
//                    .SetProjection(Projections.Property(() => pageTagAlias.Id))
//                    .Add(Restrictions.EqProperty(Projections.Property(() => pageTagAlias.Id), Projections.Property(() => blogAlias.Id)));
//                    //.Add(Restrictions.Eq(Projections.Property(() => pageTagAlias.Tag.Name), tags[0]));
//
//                criteria = criteria.Add(Subqueries.Exists(dCriteria));
            }

            return criteria.List<BlogPost>();*/
        }

        /// <summary>
        /// Gets the list of blog entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of blog entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<BlogPost> GetBlogPosts(GetBlogPostsRequest request)
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
        }

        /// <summary>
        /// Gets the list of author entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        [Obsolete("This method is obsolete; use method insteadGetAuthorsAsQueryable() instead.")]
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
                blog.ActivationDate = request.LiveFromDate;
                blog.ExpirationDate = request.LiveToDate;
                blog.Image = request.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.ImageId.Value) : null;
                blog.Author = request.AuthorId.HasValue ? Repository.AsProxy<Author>(request.AuthorId.Value) : null;
                blog.Category = request.CategoryId.HasValue ? Repository.AsProxy<Category>(request.CategoryId.Value) : null;

                // TODO: should it be allowed to change blog content, url and status?

                Repository.Save(blog);

                IList<Tag> newTags = null;
                tagService.SavePageTags(blog, request.Tags, out newTags);

                UnitOfWork.Commit();

                Events.OnBlogUpdated(blog);

                // Notify about new created tags.
                PagesApiContext.Events.OnTagCreated(newTags);

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
            return authorService.GetAuthorsAsQueryable();
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
    }
}