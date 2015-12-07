// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveBlogPostCommand.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    /// <summary>
    /// Command saves existing or creates new blog post
    /// </summary>
    public class SaveBlogPostCommand : CommandBase, ICommand<SaveBlogPostCommandRequest, SaveBlogPostCommandResponse>
    {
        private readonly IBlogService blogService;

        private readonly IWidgetService widgetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveBlogPostCommand" /> class.
        /// </summary>
        /// <param name="blogService">The blog service.</param>
        /// <param name="widgetService">The widget service.</param>
        public SaveBlogPostCommand(IBlogService blogService, IWidgetService widgetService)
        {
            this.blogService = blogService;
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public SaveBlogPostCommandResponse Execute(SaveBlogPostCommandRequest request)
        {
            if (request.Content.ContentTextMode == ContentTextMode.Markdown)
            {
                request.Content.OriginalText = request.Content.Content;
                request.Content.Content = null;
            }

            string[] error;
            var blogPost = blogService.SaveBlogPost(request.Content, request.ChildContentOptionValues, Context.Principal, out error, false);
            if (blogPost == null)
            {
                Context.Messages.AddError(error);
                return null;
            }

            var response = new SaveBlogPostCommandResponse
                       {
                           Id = blogPost.Id,
                           Version = blogPost.Version,
                           Title = blogPost.Title,
                           PageUrl = blogPost.PageUrl,
                           ModifiedByUser = blogPost.ModifiedByUser,
                           ModifiedOn = blogPost.ModifiedOn.ToFormattedDateString(),
                           CreatedOn = blogPost.CreatedOn.ToFormattedDateString(),
                           Status = blogPost.Status,
                           DesirableStatus = request.Content.DesirableStatus,
                           PageContentId = blogPost.PageContents[0].Id,
                           ContentId = blogPost.PageContents[0].Content.Id,
                           ContentVersion = blogPost.PageContents[0].Content.Version,
                       };

            if (request.Content.IncludeChildRegions)
            {
                var content = blogPost.PageContents[0].Content;
                var contentData = (content.History != null
                    ? content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) ?? content
                    : content);

                response.Regions = widgetService.GetWidgetChildRegionViewModels(contentData);
            }

            return response;
        }
    }
}