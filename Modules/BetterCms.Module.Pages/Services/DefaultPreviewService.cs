using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Services;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext.Fetching;

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
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The security service
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// The child content service
        /// </summary>
        private readonly IChildContentService childContentService;

        /// <summary>
        /// The content projections service
        /// </summary>
        private readonly IContentProjectionService contentProjectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPreviewService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="contentProjectionService">The content projection service.</param>
        /// <param name="childContentService">The child content service.</param>
        public DefaultPreviewService(IRepository repository, ISecurityService securityService, 
            IContentProjectionService contentProjectionService, IChildContentService childContentService)
        {
            this.repository = repository;
            this.securityService = securityService;
            this.childContentService = childContentService;
            this.contentProjectionService = contentProjectionService;
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
                .FetchMany(f => f.ChildContents)
                .ThenFetch(f => f.Child)
                .FetchMany(f => f.ChildContents)
                .ThenFetchMany(f => f.Options)
                .ToList()
                .FirstOrDefault();

            if (pageContent.Content != null)
            {
                if (pageContent.Content is Models.HtmlContentWidget || pageContent.Content is Models.ServerControlWidget)
                {
                    DemandAccess(user, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration);
                }
                else
                {
                    DemandAccess(user, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent);
                }
            }

            childContentService.RetrieveChildrenContentsRecursively(true, new[] { pageContent.Content });

            var contentProjection = contentProjectionService.CreatePageContentProjection(true, pageContent, new List<PageContent> { pageContent }, retrieveCorrectVersion: false);

            var pageViewModel = new RenderPageViewModel
                                    {
                                        Contents = new List<PageContentProjection> { contentProjection },
                                        Stylesheets = new List<IStylesheetAccessor> { contentProjection },
                                        Regions = new List<PageRegionViewModel> { regionViewModel },
                                        AreRegionsEditable = true,
                                        IsPreviewing = true
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