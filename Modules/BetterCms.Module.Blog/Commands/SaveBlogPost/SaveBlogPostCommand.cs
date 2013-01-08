using System;
using System.Collections.Generic;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    /// <summary>
    /// Command saves existing or creates new blog post
    /// </summary>
    public class SaveBlogPostCommand : CommandBase, ICommand<BlogPostViewModel, BlogPostViewModel>
    {
        private const string regionIdentifier = BlogModuleConstants.BlogPostMainContentRegionIdentifier;

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public BlogPostViewModel Execute(BlogPostViewModel request)
        {
            // TODO: pass layout from UI
            // TODO: validate layout
            // TODO: load region
            var layout = Repository.FirstOrDefault<Layout>(l => !l.IsDeleted && l.Name == "Default Two Columns");

            // Loading region
            var region = Repository.FirstOrDefault<Region>(r => !r.IsDeleted && r.RegionIdentifier == regionIdentifier);
            if (region == null)
            {
                var message = string.Format(BlogGlobalization.SaveBlogPost_RegionNotFound_Message, regionIdentifier);
                var logMessage = string.Format("Region {0} for rendering blog post content not found.", regionIdentifier);
                throw new ValidationException(e => message, logMessage);
            }

            var isNew = request.Id.HasDefaultValue();

            BlogPost blogPost;
            HtmlContent content = null;
            PageContent pageContent = null;

            // Loading blog post and it's content, or creating new, if such not exists
            if (!isNew)
            {
                blogPost = Repository.First<BlogPost>(request.Id);
                pageContent = Repository.FirstOrDefault<PageContent>(c => c.Page == blogPost && c.Region == region && !c.IsDeleted);
                if (pageContent != null && pageContent.Content != null)
                {
                    content = Repository.FirstOrDefault<HtmlContent>(c => c.Id == pageContent.Content.Id);
                }
            }
            else
            {
                blogPost = new BlogPost();
            }

            if (content == null)
            {
                content = new HtmlContent { Name = request.Title };
            }

            if (pageContent == null)
            {
                pageContent = new PageContent { Region = region, Content = content, Page = blogPost };
            }

            blogPost.Title = request.Title;
            blogPost.Description = request.IntroText;
            blogPost.Version = request.Version;
            blogPost.Layout = layout;

            // TODO: generate?
            blogPost.PageUrl = "/" + Guid.NewGuid().ToString() + "/";

            // TODO
            if (isNew)
            {
                blogPost.IsPublished = true;
                blogPost.PublishedOn = DateTime.Now;
                blogPost.IsPublic = true;
            }

            if (request.AuthorId.HasValue)
            {
                blogPost.Author = Repository.AsProxy<Author>(request.AuthorId.Value);
            }
            else
            {
                blogPost.Author = null;
            }

            if (request.CategoryId.HasValue)
            {
                blogPost.Category = Repository.AsProxy<Category>(request.CategoryId.Value);
            }
            else
            {
                blogPost.Category = null;
            }

            Repository.Save(blogPost);

            // Saving content
            if (string.IsNullOrWhiteSpace(content.Name))
            {
                content.Name = request.Title;
            }
            content.Html = request.Content;
            content.ActivationDate = request.LiveFromDate;
            content.ExpirationDate = request.LiveToDate;

            Repository.Save(content);
            Repository.Save(pageContent);

            // Commit
            UnitOfWork.Commit();

            return new BlogPostViewModel
                       {
                           Id = blogPost.Id,
                           Version = blogPost.Version,
                           Title = blogPost.Title,
                           PageUrl = blogPost.PageUrl
                       };
        }
    }
}