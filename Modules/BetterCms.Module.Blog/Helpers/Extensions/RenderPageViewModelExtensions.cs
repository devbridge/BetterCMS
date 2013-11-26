using System;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Blog.ViewModels.Blog;
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

                var blogPostProjection = viewModel.Contents.FirstOrDefault(projection => projection.Content.GetType() == typeof(BlogPostContent));
                if (blogPostProjection != null)
                {
                    var content = blogPostProjection.Content as BlogPostContent;
                    if (content != null)
                    {
                        viewModel.Bag.BlogPostData.BlogPostContent = content;
                    }
                }

                viewModel.Bag.BlogPostData.BlogPost = blogPost;
            }
        }

        /// <summary>
        /// Gets the blog post view model.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>Blog post view model</returns>
        public static RenderBlogPostViewModel GetBlogPostModel(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null)
            {
                if (viewModel.Bag.BlogPostData.BlogPostViewModel == null)
                {
                    var blogPost = viewModel.Bag.BlogPostData.BlogPost as BlogPost;
                    if (blogPost != null)
                    {
                        var content = viewModel.Bag.BlogPostData.BlogPostContent as BlogPostContent;
                        viewModel.Bag.BlogPostData.BlogPostViewModel = new RenderBlogPostViewModel(blogPost, content);
                    }
                }

                return viewModel.Bag.BlogPostData.BlogPostViewModel;
            }

            return null;
        }

        /// <summary>
        /// Gets the blog post author view model.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>Blog post author view model</returns>
        public static RenderBlogPostAuthorViewModel GetBlogPostAuthorModel(this RenderPageViewModel viewModel)
        {
            var blogPostModel = GetBlogPostModel(viewModel);
            if (blogPostModel != null)
            {
                return blogPostModel.Author;
            }

            return null;
        }

        /// <summary>
        /// Determines whether rendering page is blog post.
        /// </summary>
        /// <param name="viewModel">The rendering page view model.</param>
        /// <returns>
        ///   <c>true</c> if rendering page is blog post; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBlogPost(this RenderPageViewModel viewModel)
        {
            return viewModel.Bag.BlogPostData != null;
        }

        /// <summary>
        /// Determines whether blog post is active.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        ///   <c>true</c> if blog post is active; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="CmsException">The page is not a blog post.</exception>
        public static bool IsBlogPostActive(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null && viewModel.Bag.BlogPostData.BlogPostContent != null)
            {
                if (!(DateTime.Now < viewModel.Bag.BlogPostData.BlogPostContent.ActivationDate
                    || (((DateTime?)viewModel.Bag.BlogPostData.BlogPostContent.ExpirationDate).HasValue && ((DateTime?)viewModel.Bag.BlogPostData.BlogPostContent.ExpirationDate).Value < DateTime.Now)))
                {
                    return viewModel.Bag.BlogPostData.BlogPostContent.Status == ContentStatus.Published;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the name of the blog post author.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.BlogPostData or GetBlogPostModel() method")]
        public static string GetBlogPostAuthorName(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null)
            {
                var blogPost = viewModel.Bag.BlogPostData.BlogPost as BlogPost;
                if (blogPost != null)
                {
                    var author = viewModel.Bag.BlogPostData.BlogPost.Author as Author;
                    if (author != null)
                    {
                        return author.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the blog post live from date.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.BlogPostData or GetBlogPostModel() method")]
        public static DateTime? GetBlogPostLiveFromDate(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null)
            {
                var content = viewModel.Bag.BlogPostData.BlogPostContent as BlogPostContent;
                if (content != null)
                {
                    return content.ActivationDate;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the blog post live to date.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        [Obsolete("Use Bag.BlogPostData or GetBlogPostModel() method")]
        public static DateTime? GetBlogPostLiveToDate(this RenderPageViewModel viewModel)
        {
            if (viewModel.Bag.BlogPostData != null)
            {
                var content = viewModel.Bag.BlogPostData.BlogPostContent as BlogPostContent;
                if (content != null)
                {
                    return content.ExpirationDate;
                }
            }

            return null;
        }
    }
}