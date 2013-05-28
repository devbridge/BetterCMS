using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Blog.Helpers.Extensions
{
    public static class RenderPageViewModelExtensions
    {
        /// <summary>
        /// Extends rendering page view model with the blog post data.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <param name="page">The page.</param>
        public static void ExtendWithBlogData(this RenderPageViewModel viewModel, IPage page)
        {
            var blogPost = page as BlogPost;
            if (blogPost != null)
            {
                if (viewModel.Bag.BlogPostData == null)
                {
                    viewModel.Bag.BlogPostData = new DynamicDictionary();
                }

                viewModel.Bag.BlogPostData.ActivationDate = blogPost.ActivationDate;
                viewModel.Bag.BlogPostData.ExpirationDate = blogPost.ExpirationDate;

                if (blogPost.Author != null)
                {
                    viewModel.Bag.BlogPostData.AuthorId = blogPost.Author.Id;
                    viewModel.Bag.BlogPostData.AuthorName = blogPost.Author.Name;
                }
            }
        }

        /// <summary>
        /// Determines whether rendering page is blog post.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        ///   <c>true</c> if rendering page is blog post; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBlogPost(this RenderPageViewModel viewModel)
        {
            return viewModel.Bag.BlogPostData != null;
        }

        public static bool IsBlogPostActive(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null)
            {
                if (!(DateTime.Now < viewModel.Bag.BlogPostData.ActivationDate
                    || (((DateTime?)viewModel.Bag.BlogPostData.ExpirationDate).HasValue && ((DateTime?)viewModel.Bag.BlogPostData.ExpirationDate).Value < DateTime.Now)))
                {
                    return true;
                }
                return false;
            }
            throw new CmsException("The page is not a blog post.");
        }

        /// <summary>
        /// Gets the name of the blog post author.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public static string GetBlogPostAuthorName(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null)
            {
                return viewModel.Bag.BlogPostData.AuthorName as string;
            }
            return null;
        }
    }
}