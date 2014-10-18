using System;
using System.Linq;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Core.Web;

using BetterCms.Module.GoogleAnalytics.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.GoogleAnalytics.Command.Sitemap
{
    public class GetSitemapCommand : CommandBase, ICommand<GetSitemapModel, GoogleSitemapUrlSet>
    {
        private readonly ICmsConfiguration cmsConfiguration;

        // todo: WHY????
        //private readonly ICmsConfigurationService cmsConfigurationService;

        private readonly ISitemapService sitemapService;

        private readonly ILanguageService languageService;

        private readonly IPageService pageService;
        
        public GetSitemapCommand(ICmsConfiguration cmsConfiguration, ISitemapService sitemapService, 
            ILanguageService languageService, IPageService pageService)//, ICmsConfigurationService cmsConfigurationService)
        {
            this.cmsConfiguration = cmsConfiguration;
            //this.cmsConfigurationService = cmsConfigurationService;
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
                var timeoutValue = cmsConfiguration.Storage.ProcessTimeout;

                sitemap =
                    sitemapService.GetByTitle(GoogleAnalyticsModuleHelper.GetSitemapTitle(cmsConfiguration)) ?? sitemapService.GetFirst();
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