// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreSitemapVersionCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.RestoreSitemapVersion
{
    /// <summary>
    /// Command for restoring sitemap version.
    /// </summary>
    public class RestoreSitemapVersionCommand : CommandBase, ICommand<SitemapRestoreViewModel, SiteSettingSitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the content service.
        /// </summary>
        /// <value>
        /// The content service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c>, if successfully restored.
        /// </returns>
        public SiteSettingSitemapViewModel Execute(SitemapRestoreViewModel request)
        {
            var archive =
                Repository.AsQueryable<SitemapArchive>()
                          .Where(sitemapArchive => sitemapArchive.Id == request.SitemapVersionId)
                          .Fetch(sitemapArchive => sitemapArchive.Sitemap)
                          .ThenFetchMany(f => f.AccessRules)
                          .Fetch(sitemapArchive => sitemapArchive.Sitemap)
                          .ThenFetchMany(map => map.Nodes)
                          .ThenFetch(node => node.Page)
                          .Fetch(sitemapArchive => sitemapArchive.Sitemap)
                          .ThenFetchMany(map => map.Nodes)
                          .ThenFetchMany(node => node.Translations)
                          .Distinct()
                          .ToList()
                          .First();

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(archive.Sitemap, Context.Principal, AccessLevel.ReadWrite);
            }

// TODO: do we need a such confirmation?
//            // Check if user has confirmed restoration if exist nodes linked to deleted pages.
//            if (!request.IsUserConfirmed)
//            {
//                var hasDeletedPages = SitemapService.CheckIfArchiveHasDeletedPages(archive);
//                if (hasDeletedPages)
//                {
//                    var message = NavigationGlobalization.RestoreSitemap_ArchiveHasDeletedPages_RestoreConfirmationMessage;
//                    var logMessage = string.Format("User is trying to restore sitemap which has deleted pages. Confirmation is required. archiveId: {0}, sitemapId: {1}", archive.Id, archive.Sitemap.Id);
//                    throw new ConfirmationRequestException(() => message, logMessage);
//                }
//            }

            UnitOfWork.BeginTransaction();

            SitemapService.ArchiveSitemap(archive.Sitemap);
            var restoredSitemap = SitemapService.RestoreSitemapFromArchive(archive);

            UnitOfWork.Commit();

            Events.SitemapEvents.Instance.OnSitemapRestored(restoredSitemap);

            return new SiteSettingSitemapViewModel
                {
                    Id = restoredSitemap.Id,
                    Title = restoredSitemap.Title,
                    Version = restoredSitemap.Version
                };
        }
    }
}