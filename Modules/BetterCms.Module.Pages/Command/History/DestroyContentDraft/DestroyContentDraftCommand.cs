using System;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.DestroyContentDraft
{
    public class DestroyContentDraftCommand : CommandBase, ICommand<DestroyContentDraftCommandRequest, DestroyContentDraftCommandResponse>
    {
        private ICmsConfiguration cmsConfiguration;

        public DestroyContentDraftCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="ConcurrentDataException"></exception>
        /// <exception cref="CmsException"></exception>
        public DestroyContentDraftCommandResponse Execute(DestroyContentDraftCommandRequest request)
        {
            var contentQuery = Repository
                .AsQueryable<Root.Models.Content>(p => p.Id == request.Id)
                .Fetch(f => f.Original).AsQueryable();

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                contentQuery = contentQuery.FetchMany(f => f.PageContents).ThenFetch(f => f.Page).ThenFetchMany(f => f.AccessRules).AsQueryable();
            }

            var content = contentQuery.ToList().FirstOrDefault();

            // Throw concurrent data exception (user needs to reload page):
            // - content may be null, if looking for already deleted draft
            // - content may be changed, if looking for 
            if (content == null || request.Version != content.Version)
            {
                throw new ConcurrentDataException(content ?? new Root.Models.Content());
            }

            var pageContents = content.PageContents;

            // If content is published, try to get it's active draft
            if (content.Status == ContentStatus.Published)
            {
                content = Repository
                    .AsQueryable<Root.Models.Content>(p => p.Original == content)
                    .Where(c => c.Status == ContentStatus.Draft && !c.IsDeleted)
                    .Fetch(f => f.Original)
                    .FirstOrDefault();

                if (content == null)
                {
                    // Throw concurrent data exception (user needs to reload page):
                    // - content may be null, if looking for already deleted draft
                    throw new ConcurrentDataException(new Root.Models.Content());
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
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.Administration);
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
                        AccessControlService.DemandAccess(pageContent.Page, Context.Principal, AccessLevel.ReadWrite, RootModuleConstants.UserRoles.EditContent);
                    }
                }

                if (!checkedAccess)
                {
                    AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
                }
            }

            Repository.Delete(content);
            UnitOfWork.Commit();

            var response = new DestroyContentDraftCommandResponse
                       {
                           PublishedId = content.Original.Id,
                           Id = content.Original.Id,
                           OriginalId = content.Original.Id,
                           Version = content.Original.Version,
                           OriginalVersion = content.Original.Version,
                           WidgetName = content.Original.Name,
                           IsPublished = true,
                           HasDraft = false,
                           DesirableStatus = ContentStatus.Published
                       };

            // Try to cast to widget
            var widget = content.Original as HtmlContentWidget;
            if (widget != null && widget.Category != null && !widget.Category.IsDeleted)
            {
                response.CategoryName = widget.Category.Name;
            }

            return response;
        }
    }
}