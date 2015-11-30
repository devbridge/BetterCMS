// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveSitemapNodeCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.SaveSitemapNode
{
    /// <summary>
    /// Saves sitemap node data.
    /// </summary>
    public class SaveSitemapNodeCommand : CommandBase, ICommand<SitemapNodeViewModel, SitemapNodeViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Execution result.</returns>
        public SitemapNodeViewModel Execute(SitemapNodeViewModel request)
        {
            var sitemap =
                Repository.AsQueryable<Models.Sitemap>()
                          .Where(map => map.Id == request.SitemapId)
                          .FetchMany(f => f.AccessRules)
                          .FetchMany(map => map.Nodes)
                          .ThenFetch(mapNode => mapNode.Page)
                          .FetchMany(map => map.Nodes)
                          .ThenFetch(mapNode => mapNode.Translations)
                          .Distinct()
                          .ToList()
                          .First();

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite);
            }

            UnitOfWork.BeginTransaction();

            if (!request.SitemapId.HasDefaultValue())
            {
                SitemapService.ArchiveSitemap(sitemap);
            }

            bool updatedInDB;
            var node = SitemapService.SaveNode(out updatedInDB, sitemap, request.Id, request.Version, request.Url, request.Title, request.Macro, request.PageId, request.UsePageTitleAsNodeTitle, request.DisplayOrder, request.ParentId);

            UnitOfWork.Commit();

            if (updatedInDB)
            {
                if (request.Id.HasDefaultValue())
                {
                    Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
                }
                else
                {
                    Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
                }

                Events.SitemapEvents.Instance.OnSitemapUpdated(node.Sitemap);
            }

            return new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version
                };
        }
    }
}