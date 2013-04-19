using BetterCms.Core.DataContracts;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Blog.Helpers.Extensions
{
    public static class RenderPageViewModelExtensions
    {
        /// <summary>
        /// Extends renderign page view model with the blog post data.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <param name="page">The page.</param>
        public static void ExtendWithBlogData(this RenderPageViewModel viewModel, IPage page)
        {
            var blogPost = page as BlogPost;
            if (blogPost != null && blogPost.Author != null)
            {
                if (viewModel.Bag.BlogPostData == null)
                {
                    viewModel.Bag.BlogPostData = new DynamicDictionary();
                }
                viewModel.Bag.BlogPostData.AuthorId = blogPost.Author.Id;
                viewModel.Bag.BlogPostData.AuthorName = blogPost.Author.Name;
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