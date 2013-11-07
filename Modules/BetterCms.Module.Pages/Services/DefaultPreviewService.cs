using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Services;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Cms;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultPreviewService : IPreviewService
    {
        /// <summary>
        /// The region identifier.
        /// </summary>
        private const string regionIdentifier = "VersionContent";

        /// <summary>
        /// The region id.
        /// </summary>
        private const string regionId = "41195FE2-DB5D-412E-A648-ED02B279C8F3";

        /// <summary>
        /// The page content projection factory
        /// </summary>
        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        /// <summary>
        /// The option service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The security service
        /// </summary>
        private readonly ISecurityService securityService;

        public DefaultPreviewService(PageContentProjectionFactory pageContentProjectionFactory, IOptionService optionService, IRepository repository, ISecurityService securityService)
        {
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.repository = repository;
            this.optionService = optionService;
            this.securityService = securityService;
        }

        /// <summary>
        /// Gets the view model for rendering widget preview.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="user">The user.</param>
        /// <returns>
        /// View model for rendering widget preview
        /// </returns>
        public RenderPageViewModel GetContentPreviewViewModel(Guid contentId, IPrincipal user, bool allowJavaScript)
        {
            // Creating fake region.
            var regionGuid = new Guid(regionId);
            var region = new Region { Id = regionGuid, RegionIdentifier = regionIdentifier };
            var regionViewModel = new PageRegionViewModel { RegionId = regionGuid, RegionIdentifier = regionIdentifier };

            // Creating fake page content and loading it's children.
            var pageContent = new PageContent
            {
                Options = new List<PageContentOption>(),
                Region = region
            };

            pageContent.Content = repository
                .AsQueryable<ContentEntity>(c => c.Id == contentId)
                .FetchMany(f => f.ContentOptions)
                .ToList()
                .FirstOrDefault();

            if (pageContent.Content != null)
            {
                DemandAccess(user, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent);
            }

            var options = optionService.GetMergedOptionValues(pageContent.Content.ContentOptions, pageContent.Options);

            var contentProjection = pageContentProjectionFactory.Create(pageContent, pageContent.Content, options);

            var pageViewModel = new RenderPageViewModel
                                    {
                                        Contents = new List<PageContentProjection> { contentProjection },
                                        Stylesheets = new List<IStylesheetAccessor> { contentProjection },
                                        Regions = new List<PageRegionViewModel> { regionViewModel },
                                        AreRegionsEditable = true
                                    };
            if (allowJavaScript)
            {
                pageViewModel.JavaScripts = new List<IJavaScriptAccessor> { contentProjection };
            }

            return pageViewModel;
        }

        /// <summary>
        /// Demands the access.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <exception cref="SecurityException">Forbidden: Access is denied.</exception>
        private void DemandAccess(IPrincipal user, params string[] roles)
        {
            if (!securityService.IsAuthorized(user, string.Join(",", roles)))
            {
                throw new SecurityException("Forbidden: Access is denied.");
            }
        }
    }
}