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
using System.Linq;

using Autofac;

using BetterCms.Core.Exceptions;

using BetterCms.Module.GoogleAnalytics.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Dependencies;
using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Web;

namespace BetterCms.Module.GoogleAnalytics.Command.Sitemap
{
    public class GetSitemapCommand : CommandBase, ICommand<GetSitemapModel, GoogleSitemapUrlSet>
    {
        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ISitemapService sitemapService;

        private readonly ILanguageService languageService;

        private readonly IPageService pageService;
        
        public GetSitemapCommand(ICmsConfiguration cmsConfiguration, ISitemapService sitemapService, ILanguageService languageService, IPageService pageService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.sitemapService = sitemapService;
            this.languageService = languageService;
            this.pageService = pageService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitemapModel"></param>
        /// <returns></returns>
        public GoogleSitemapUrlSet Execute(GetSitemapModel sitemapModel)
        {
            Pages.Models.Sitemap sitemap;

            if (sitemapModel.SitemapId.HasDefaultValue())
            {
                sitemap = sitemapService.GetByTitle(GoogleAnalyticsModuleHelper.GetSitemapTitle(cmsConfiguration)) ?? sitemapService.GetFirst();
                if (sitemap == null)
                {
                    throw new CmsException("There aren't any sitemaps created.");
                }
            }
            else
            {
                sitemap = sitemapService.Get(sitemapModel.SitemapId);
                if (sitemap == null)
                {
                    throw new EntityNotFoundException(typeof(Pages.Models.Sitemap), sitemapModel.SitemapId);
                }
            }

            var urlset = new GoogleSitemapUrlSet();
            var languages = languageService.GetLanguages().ToList();

            IHttpContextAccessor httpContextAccessor;
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            }

            foreach (var node in sitemap.Nodes.Distinct())
            {
                if (node.Page != null)
                {
                    var url = new GoogleSitemapUrl(GoogleAnalyticsModuleHelper.GetDateTimeFormat(cmsConfiguration))
                    {
                        Location = httpContextAccessor.MapPublicPath(node.Page.PageUrl),
                        LastModifiedDateTime = node.ModifiedOn,
                        ChangeFrequency = GoogleAnalyticsModuleHelper.GetChangeFrequency(cmsConfiguration),
                        Priority = GoogleAnalyticsModuleHelper.GetPriority(cmsConfiguration)
                    };

                    if (node.Page.LanguageGroupIdentifier != null)
                    {
                        var pageTranslations = pageService.GetPageTranslations((Guid)node.Page.LanguageGroupIdentifier);
                        foreach (var pageTranslation in pageTranslations)
                        {
                            if (pageTranslation.LanguageId != null && pageTranslation.PageUrl != node.Page.PageUrl)
                            {
                                url.Links.Add(
                                    new GoogleSitemapLink
                                    {
                                        LinkType = GoogleAnalyticsModuleHelper.GetLinkType(cmsConfiguration),
                                        LanguageCode = languages.First(l => l.Id == pageTranslation.LanguageId).Code,
                                        Url = httpContextAccessor.MapPublicPath(pageTranslation.PageUrl)
                                    });
                            }
                        }
                    }

                    urlset.Urls.Add(url);
                }
                else
                {
                    if (!string.IsNullOrEmpty(node.Url))
                    {
                        var url = new GoogleSitemapUrl(GoogleAnalyticsModuleHelper.GetDateTimeFormat(cmsConfiguration))
                        {
                            Location = node.Url,
                            LastModifiedDateTime = node.ModifiedOn,
                            ChangeFrequency = GoogleAnalyticsModuleHelper.GetChangeFrequency(cmsConfiguration),
                            Priority = GoogleAnalyticsModuleHelper.GetPriority(cmsConfiguration)
                        };

                        urlset.Urls.Add(url);
                    }
                }
            }

            return urlset;
        }
    }
}