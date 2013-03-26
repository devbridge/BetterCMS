using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.RestoreContentVersion
{
    /// <summary>
    /// Command for restoring page content version
    /// </summary>
    public class RestoreContentVersionCommand : CommandBase, ICommand<Guid, bool>
    {
        /// <summary>
        /// Gets or sets the content service.
        /// </summary>
        /// <value>
        /// The content service.
        /// </value>
        public IContentService contentService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The id of the archived page content version.</param>
        /// <returns>True, if restore is successfull</returns>
        public bool Execute(Guid pageContentId)
        {
            var content = Repository
                .AsQueryable<Root.Models.Content>(p => p.Id == pageContentId)
                .Fetch(f => f.Original)
                .First();

            var contentType = content.GetType();
            if (contentType == typeof(HtmlContentWidget) || contentType == typeof(ServerControlWidget))
            {
                DemandAccess(RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                DemandAccess(RootModuleConstants.UserRoles.PublishContent);
            }

            contentService.RestoreContentFromArchive(content);

            UnitOfWork.Commit();

            return true;
        }
    }
}