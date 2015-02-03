using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.Helpers.Extensions
{
    public static class RenderPageViewModelExtensions
    {
        /// <summary>
        /// Extends rendering page view model with the page data.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <param name="page">The page.</param>
        public static void ExtendWithPageData(this RenderPageViewModel viewModel, IPage page)
        {
            var pageData = page as PageProperties;
            if (pageData != null)
            {
                if (viewModel.Bag.PageData == null)
                {
                    viewModel.Bag.PageData = new DynamicDictionary();
                }

                viewModel.Bag.PageData.PageProperties = page;
            }
        }

        /// <summary>
        /// Gets the page view model.
        /// </summary>
        /// <param name="viewModel">The page view model.</param>
        /// <returns>Page view model</returns>
        public static RenderPagePropertiesViewModel GetPageModel(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                if (viewModel.Bag.PageData.PagePropertiesViewModel == null)
                {
                    var page = viewModel.Bag.PageData.PageProperties as PageProperties;
                    if (page != null)
                    {
                        page.IsReadOnly = viewModel.IsReadOnly;
                        viewModel.Bag.PageData.PagePropertiesViewModel = new RenderPagePropertiesViewModel(page);
                    }
                }

                return viewModel.Bag.PageData.PagePropertiesViewModel;
            }

            return null;
        }

        /// <summary>
        /// Gets the page main image view model.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>Page main image view model</returns>
        public static RenderPageImageViewModel GetPageMainImageModel(this RenderPageViewModel viewModel)
        {
            var pageModel = GetPageModel(viewModel);
            if (pageModel != null)
            {
                return pageModel.MainImage;
            }

            return null;
        }
        
        /// <summary>
        /// Gets the page secondary image view model.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>Page secondary image view model</returns>
        public static RenderPageImageViewModel GetPageSecondaryImageModel(this RenderPageViewModel viewModel)
        {
            var pageModel = GetPageModel(viewModel);
            if (pageModel != null)
            {
                return pageModel.SecondaryImage;
            }

            return null;
        }
        
        /// <summary>
        /// Gets the page featured image view model.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>Page featured image view model</returns>
        public static RenderPageImageViewModel GetPageFeaturedImageModel(this RenderPageViewModel viewModel)
        {
            var pageModel = GetPageModel(viewModel);
            if (pageModel != null)
            {
                return pageModel.FeaturedImage;
            }

            return null;
        }

        /// <summary>
        /// Gets the page category view model.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>Page category view model</returns>
        public static RenderPageCategoryViewModel GetPageCategoryModel(this RenderPageViewModel viewModel)
        {
            var pageModel = GetPageModel(viewModel);
            if (pageModel != null && pageModel.Categories != null)
            {          
                return pageModel.Categories.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Gets the page tags list.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>The list with page tags.</returns>
        public static IList<string> GetPageTagsList(this RenderPageViewModel viewModel)
        {
            var pageModel = GetPageModel(viewModel);
            if (pageModel != null)
            {
                return pageModel.Tags;
            }

            return null;
        }

        /// <summary>
        /// Gets the page image.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.PageData or GetPageModel() method")]
        public static MediaImage GetPageImage(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                var page = viewModel.Bag.PageData.PageProperties as PageProperties;
                if (page != null)
                {
                    return page.Image;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the page secondary image.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.PageData or GetPageModel() method")]
        public static MediaImage GetPageSecondaryImage(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                var page = viewModel.Bag.PageData.PageProperties as PageProperties;
                if (page != null)
                {
                    return page.SecondaryImage;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the page featured image.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.PageData or GetPageModel() method")]
        public static MediaImage GetPageFeaturedImage(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                var page = viewModel.Bag.PageData.PageProperties as PageProperties;
                if (page != null)
                {
                    return page.FeaturedImage;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the page tags.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.PageData or GetPageModel() method")]
        public static IList<PageTag> GetPageTags(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                var page = viewModel.Bag.PageData.PageProperties as PageProperties;
                if (page != null)
                {
                    return page.PageTags;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the page category.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.PageData or GetPageModel() method")]
        public static IEnumerable<PageCategory> GetPageCategories(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                var page = viewModel.Bag.PageData.PageProperties as PageProperties;
                if (page != null && page.Categories != null)
                {
                    return page.Categories;
                }
            }

            return Enumerable.Empty<PageCategory>();
        }
    }
}