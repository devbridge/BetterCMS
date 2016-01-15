// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAuthorListCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.GetAuthorList
{
    public class GetAuthorListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<AuthorViewModel>>
    {
        private IAuthorService authorService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public GetAuthorListCommand(IAuthorService authorService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.authorService = authorService;
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<AuthorViewModel> Execute(SearchableGridOptions request)
        {
                var query = Repository.AsQueryable<Author>();
                var authors = query.Select(
                        author =>
                        new AuthorViewModel
                        {
                            Id = author.Id,
                            Version = author.Version,
                            Name = author.Name,
                            Description = author.Description,
                            Image = author.Image != null && !author.Image.IsDeleted
                                    ?
                                    new ImageSelectorViewModel
                                    {
                                        ImageId = author.Image.Id,
                                        ImageVersion = author.Image.Version,
                                        ImageUrl = fileUrlResolver.EnsureFullPathUrl(author.Image.PublicUrl),
                                        ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(author.Image.PublicThumbnailUrl),
                                        ImageTooltip = author.Image.Caption,
                                        FolderId = author.Image.Folder != null ? author.Image.Folder.Id : (System.Guid?)null
                                    }
                                    : null
                        });

                if (!string.IsNullOrWhiteSpace(request.SearchQuery))
                {
                    authors = authors.Where(a => a.Name.Contains(request.SearchQuery));
                }

                request.SetDefaultSortingOptions("Name");
                var count = authors.ToRowCountFutureValue();
                authors = authors.AddSortingAndPaging(request);

                return new SearchableGridViewModel<AuthorViewModel>(authors.ToList(), request, count.Value);
        }
    }
}