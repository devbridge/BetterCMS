using System;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.DestroyContentDraft
{
    public class DestroyContentDraftCommand : CommandBase, ICommand<Guid, DestroyContentDraftCommandResponse>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>
        public DestroyContentDraftCommandResponse Execute(Guid pageContentId)
        {
            var content = Repository
                .AsQueryable<Root.Models.Content>(p => p.Id == pageContentId)
                .Fetch(f => f.Original)
                .First();

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
                    throw new CmsException(string.Format("Cannot find draft version for content with id = {0}.", pageContentId));
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
                DemandAccess(RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                DemandAccess(RootModuleConstants.UserRoles.PublishContent);
            }

            Repository.Delete(content);
            UnitOfWork.Commit();

            var response = new DestroyContentDraftCommandResponse
                       {
                           PublishedId = content.Original.Id,
                           Id = content.Original.Id,
                           Version = content.Original.Version,
                           WidgetName = content.Original.Name,
                           IsPublished = true,
                           HasDraft = false,
                           DesirableStatus = ContentStatus.Published
                       };

            // Try to cast to widget
            var widget = content.Original as HtmlContentWidget;
            if (widget != null && widget.Category != null)
            {
                response.CategoryName = widget.Category.Name;
            }

            return response;
        }
    }
}