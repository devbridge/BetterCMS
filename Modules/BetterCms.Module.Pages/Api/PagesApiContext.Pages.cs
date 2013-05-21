using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Api.Dto;
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using ValidationException = BetterCms.Core.Exceptions.Mvc.ValidationException;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public partial class PagesApiContext : DataApiContext
    {
        /// <summary>
        /// Gets the list of page property entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="loadChilds">Flags, which childs to load.</param>
        /// <param name="includeUnpublished">if set to <c>true</c> include unpublished pages.</param>
        /// <param name="includePrivate">if set to <c>true</c> include private pages.</param>
        /// <returns>
        /// The list of property entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public IList<PageProperties> GetPages(Expression<Func<PageProperties, bool>> filter = null, Expression<Func<PageProperties, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null, PageLoadableChilds loadChilds = PageLoadableChilds.None, bool includeUnpublished = false, bool includePrivate = false)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Title;
                }

                var query = Repository
                    .AsQueryable<PageProperties>()
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage);

                if (!includeUnpublished)
                {
                    query = query.Where(b => b.Status == PageStatus.Published);
                }

                if (!includePrivate)
                {
                    query = query.Where(b => b.IsPublic);
                }

                query = FetchPageChilds(query, loadChilds);

                return query.ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get pages list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Checks if page exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns>
        ///   <c>true</c>if page exists; otherwise <c>false</c>
        /// </returns>
        public bool PageExists(string pageUrl)
        {
            try
            {
                return Repository.Any<PageProperties>(p => p.PageUrl == pageUrl);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to check if page exists by url:{0}", pageUrl);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the page entity.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="loadChilds">Flags, which childs to load.</param>
        /// <returns>
        /// Page entity
        /// </returns>
        public PageProperties GetPage(Guid id, PageLoadableChilds loadChilds = PageLoadableChilds.All)
        {
            try
            {
                var query = Repository
                    .AsQueryable<PageProperties>()
                    .Where(p => p.Id == id);

                return FetchPageChilds(query, loadChilds)
                    .ToList()
                    .FirstOne();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page exists by id:{0}", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <returns></returns>
        public PageProperties CreatePage(CreatePageRequest pageDto)
        {
            ValidateRequest(pageDto);
            
            try
            {
                var pageUrl = CreateOrFixPageUrl(pageDto.PageUrl, pageDto.Title);
                
                var layout = Repository.AsProxy<Layout>(pageDto.LayoutId);
                
                MediaImage image;
                if (pageDto.ImageId != null && !pageDto.ImageId.Value.HasDefaultValue())
                {
                    image = Repository.AsProxy<MediaImage>(pageDto.ImageId.Value);
                }
                else
                {
                    image = null;
                }

                Category category;
                if (pageDto.CategoryId != null && !pageDto.CategoryId.Value.HasDefaultValue())
                {
                    category = Repository.AsProxy<Category>(pageDto.CategoryId.Value);
                }
                else
                {
                    category = null;
                }

                var page = pageDto.ToPageProperties();
                page.Layout = layout;
                page.Image = image;
                page.Category = category;
                page.PageUrl = pageUrl;
                if (pageDto.Status == PageStatus.Published)
                {
                    page.PublishedOn = DateTime.Now;
                }

                Repository.Save(page);
                UnitOfWork.Commit();

                // Notifying, that page is created
                Events.OnPageCreated(page);

                return page;
            }
            catch (ValidationException inner)
            {
                var message = string.Format("Failed to create page. Title: {0}, Url: {1}", pageDto.Title, pageDto.PageUrl);
                Logger.Error(message, inner);
                throw new CmsApiException(inner.Message, inner);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create page.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Fetches the child by given parameters.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="loadChilds">The load child.</param>
        /// <returns>Query with fetched children by given parameters.</returns>
        private static IQueryable<PageProperties> FetchPageChilds(IQueryable<PageProperties> query, PageLoadableChilds loadChilds)
        {
            if (loadChilds.HasFlag(PageLoadableChilds.LayoutRegion))
            {
                query = query
                    .Fetch(p => p.Layout)
                    .ThenFetchMany(l => l.LayoutRegions)
                    .ThenFetch(lr => lr.Region);
            }
            else if (loadChilds.HasFlag(PageLoadableChilds.Layout))
            {
                query = query.Fetch(p => p.Layout);
            }

            if (loadChilds.HasFlag(PageLoadableChilds.Category))
            {
                query = query.Fetch(p => p.Category);
            }

            if (loadChilds.HasFlag(PageLoadableChilds.Image))
            {
                query = query.Fetch(p => p.Image);
            }

            if (loadChilds.HasFlag(PageLoadableChilds.Tags))
            {
                query = query.FetchMany(p => p.PageTags).ThenFetch(pt => pt.Tag);
            }

            return query;
        }

        /// <summary>
        /// Creates the or fix page URL.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        private string CreateOrFixPageUrl(string pageUrl, string title)
        {
            var pageService = Resolve<IPageService>();

            // Create / fix page url
            var createPageUrl = (pageUrl == null);
            if (createPageUrl && !string.IsNullOrWhiteSpace(title))
            {
                pageUrl = pageService.CreatePagePermalink(title, null);
            }
            else
            {
                pageUrl = Resolve<IUrlService>().FixUrl(pageUrl);

                // Validate Url
                pageService.ValidatePageUrl(pageUrl);
            }

            return pageUrl;
        }
    }
}