// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSitemapCommand.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemap
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapCommand : CommandBase, ICommand<Guid, SitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the tag service.
        /// </summary>
        /// <value>
        /// The tag service.
        /// </value>
        public ITagService TagService { get; set; }

        /// <summary>
        /// Gets or sets the language service.
        /// </summary>
        /// <value>
        /// The language service.
        /// </value>
        public ILanguageService LanguageService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>
        /// Sitemap view model.
        /// </returns>
        public SitemapViewModel Execute(Guid sitemapId)
        {
            if (sitemapId.HasDefaultValue())
            {
                var langs = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguagesLookupValues().ToList() : new List<LookupKeyValue>();
                return new SitemapViewModel()
                    {
                        AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                        UserAccessList = AccessControlService.GetDefaultAccessList(SecurityService.GetCurrentPrincipal()).Select(f => new UserAccessViewModel(f)).ToList(),
                        ShowLanguages = CmsConfiguration.EnableMultilanguage && langs.Any(),
                        Languages = langs,
                        ShowMacros = CmsConfiguration.EnableMacros
                    };
            }

            IEnumerable<AccessRule> userAccessFuture;
            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                userAccessFuture = Repository
                    .AsQueryable<Models.Sitemap>()
                    .Where(x => x.Id == sitemapId && !x.IsDeleted)
                    .SelectMany(x => x.AccessRules)
                    .OrderBy(x => x.Identity)
                    .ToFuture();
            }
            else
            {
                userAccessFuture = null;
            }

            var tagsFuture = TagService.GetSitemapTagNames(sitemapId);
            var languagesFuture = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguagesLookupValues() : null;
            var pagesToFuture = SitemapHelper.GetPagesToFuture(CmsConfiguration.EnableMultilanguage, Repository);

            IQueryable<Models.Sitemap> sitemapQuery = Repository.AsQueryable<Models.Sitemap>()
                .Where(map => map.Id == sitemapId)
                .FetchMany(map => map.Nodes)
                .ThenFetch(node => node.Page);

            if (CmsConfiguration.EnableMultilanguage)
            {
                sitemapQuery = sitemapQuery
                    .FetchMany(map => map.Nodes)
                    .ThenFetchMany(node => node.Translations);
            }

            var sitemap = sitemapQuery.Distinct().ToFuture().ToList().First();
            var languages = CmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : new List<LookupKeyValue>();
            var model = new SitemapViewModel
                {
                    Id = sitemap.Id,
                    Version = sitemap.Version,
                    Title = sitemap.Title,
                    RootNodes =
                        SitemapHelper.GetSitemapNodesInHierarchy(
                            CmsConfiguration.EnableMultilanguage,
                            sitemap.Nodes.Distinct().Where(f => f.ParentNode == null).ToList(),
                            sitemap.Nodes.Distinct().ToList(),
                            languages.Select(l => l.Key.ToGuidOrDefault()).ToList(),
                            (pagesToFuture ?? new List<SitemapHelper.PageData>()).ToList()),
                    Tags = tagsFuture.ToList(),
                    AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                    ShowLanguages = CmsConfiguration.EnableMultilanguage && languages.Any(),
                    Languages = languages,
                    ShowMacros = CmsConfiguration.EnableMacros
                };

            if (userAccessFuture != null)
            {
                model.UserAccessList = userAccessFuture.Select(x => new UserAccessViewModel(x)).ToList();

                var rules = model.UserAccessList.Cast<IAccessRule>().ToList();

                SetIsReadOnly(model, rules);
            }

            return model;
        }
    }
}