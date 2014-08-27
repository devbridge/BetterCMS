using System;
using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.GoogleAnalytics.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.GoogleAnalytics.Command.Sitemap
{
    public class GetSitemapCommand : CommandBase, ICommand<GetSitemapModel, GoogleSitemapUrlSet>
    {
        private readonly ICmsConfiguration _cmsConfiguration;

        private readonly ISitemapService _sitemapService;

        private readonly ILanguageService _languageService;

        private readonly IPageService _pageService;
        
        public GetSitemapCommand(ICmsConfiguration cmsConfiguration, ISitemapService sitemapService, ILanguageService languageService, IPageService pageService)
        {
            _cmsConfiguration = cmsConfiguration;
            _sitemapService = sitemapService;
            _languageService = languageService;
            _pageService = pageService;
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
                sitemap = _sitemapService.GetByTitle(GoogleAnalyticsModuleHelper.GetSitemapTitle(_cmsConfiguration)) ?? _sitemapService.GetFirst();
                if (sitemap == null)
                    throw new CmsException("There aren't any sitemaps created.");
            }
            else
            {
                sitemap = _sitemapService.Get(sitemapModel.SitemapId);
                if (sitemap == null)
                    throw new EntityNotFoundException(typeof(Pages.Models.Sitemap), sitemapModel.SitemapId);
            }

            var urlset = new GoogleSitemapUrlSet();
            var languages = _languageService.GetLanguages().ToList();

            foreach (var node in sitemap.Nodes.Distinct())
            {
                if (node.Page != null)
                {
                    var url = new GoogleSitemapUrl(GoogleAnalyticsModuleHelper.GetDateTimeFormat(_cmsConfiguration))
                    {
                        Location = node.Page.PageUrl,
                        LastModifiedDateTime = node.ModifiedOn,
                        ChangeFrequency = GoogleAnalyticsModuleHelper.GetChangeFrequency(_cmsConfiguration),
                        Priority = GoogleAnalyticsModuleHelper.GetPriority(_cmsConfiguration)
                    };

                    if (node.Page.LanguageGroupIdentifier != null)
                    {
                        var pageTranslations = _pageService.GetPageTranslations((Guid)node.Page.LanguageGroupIdentifier);
                        foreach (var pageTranslation in pageTranslations)
                        {
                            if (pageTranslation.LanguageId != null && pageTranslation.PageUrl != node.Page.PageUrl)
                            {
                                url.Links.Add(new GoogleSitemapLink
                                {
                                    LinkType = GoogleAnalyticsModuleHelper.GetLinkType(_cmsConfiguration),
                                    LanguageCode = languages.First(l => l.Id == pageTranslation.LanguageId).Code,
                                    Url = pageTranslation.PageUrl
                                });
                            }
                        }
                    }  

                    urlset.Urls.Add(url);
                }
            }

            return urlset;
        }
    }
}