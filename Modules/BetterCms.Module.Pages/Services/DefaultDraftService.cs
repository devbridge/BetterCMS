using System;
using System.Linq;
using System.Security.Principal;


using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Exceptions;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Exceptions.DataTier;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultDraftService : IDraftService
    {
        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        private readonly IRepository repository;

        private readonly IUnitOfWork unitOfWork;

        public DefaultDraftService(ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService,
            IRepository repository, IUnitOfWork unitOfWork)
        {
            this.accessControlService = accessControlService;
            this.cmsConfiguration = cmsConfiguration;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public Root.Models.Content DestroyDraftContent(Guid contentId, int version, IPrincipal principal)
        {
            var contentQuery = repository
                .AsQueryable<Root.Models.Content>(p => p.Id == contentId)
                .Fetch(f => f.Original).AsQueryable();

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                contentQuery = contentQuery.FetchMany(f => f.PageContents).ThenFetch(f => f.Page).ThenFetchMany(f => f.AccessRules).AsQueryable();
            }

            var content = contentQuery.ToList().FirstOne();

            // Throw concurrent data exception (user needs to reload page):
            // - content may be null, if looking for already deleted draft
            // - content may be changed, if looking for 
            if (version > 0 && version != content.Version)
            {
                throw new ConcurrentDataException(content);
            }

            var pageContents = content.PageContents;

            // If content is published, try to get it's active draft
            if (content.Status == ContentStatus.Published)
            {
                content = repository
                    .AsQueryable<Root.Models.Content>(p => p.Original == content)
                    .Where(c => c.Status == ContentStatus.Draft && !c.IsDeleted)
                    .Fetch(f => f.Original)
                    .FirstOrDefault();

                if (content == null)
                {
                    // Throw concurrent data exception (user needs to reload page):
                    // - content may be null, if looking for already deleted draft
                    throw new DraftNotFoundException(new Root.Models.Content());
                }
            }

            if (content.Status != ContentStatus.Draft)
            {
                throw new CmsException(string.Format("Only draft version can be destroyed. Id: {0}, Status: {1}", content.Id, content.Status));
            }

            if (content.Original == null)
            {
                throw new CmsException(string.Format("Draft version cannot be destroyed - it has no published original version. Id: {0}, Status: {1}", content.Id, content.Status));
            }

            var contentType = content.GetType();
            if (contentType == typeof(HtmlContentWidget) || contentType == typeof(ServerControlWidget))
            {
                accessControlService.DemandAccess(principal, RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                bool checkedAccess = false;
                if (content is HtmlContent)
                {
                    var pageContent = pageContents.FirstOrDefault();
                    if (pageContent != null && pageContent.Page != null)
                    {
                        checkedAccess = true;
                        accessControlService.DemandAccess(pageContent.Page, principal, AccessLevel.ReadWrite, RootModuleConstants.UserRoles.EditContent);
                    }
                }

                if (!checkedAccess)
                {
                    accessControlService.DemandAccess(principal, RootModuleConstants.UserRoles.EditContent);
                }
            }

            repository.Delete(content);
            unitOfWork.Commit();

            Events.PageEvents.Instance.OnContentDraftDestroyed(content.Original);

            return content;
        }
    }
}