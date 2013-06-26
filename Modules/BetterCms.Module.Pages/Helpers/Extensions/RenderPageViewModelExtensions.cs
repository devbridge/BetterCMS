using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
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

                viewModel.Bag.PageData.Image = pageData.Image;
                viewModel.Bag.PageData.SecondaryImage = pageData.SecondaryImage;
                viewModel.Bag.PageData.FeaturedImage = pageData.FeaturedImage;
                viewModel.Bag.PageData.PageTags = pageData.PageTags;
                viewModel.Bag.PageData.Category = pageData.Category;
            }
        }

        public static MediaImage GetPageImage(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                return viewModel.Bag.PageData.Image as MediaImage;
            }
            return null;
        }

        public static MediaImage GetPageSecondaryImage(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                return viewModel.Bag.PageData.SecondaryImage as MediaImage;
            }
            return null;
        }

        public static MediaImage GetPageFeaturedImage(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                return viewModel.Bag.PageData.FeaturedImage as MediaImage;
            }
            return null;
        }

        public static IList<PageTag> GetPageTags(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                return viewModel.Bag.PageData.PageTags as IList<PageTag>;
            }
            return null;
        }

        public static Category GetPageCategory(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.PageData != null)
            {
                return viewModel.Bag.PageData.Category as Category;
            }
            return null;
        }
    }
}