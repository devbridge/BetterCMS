using System;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.History.RestoreContentVersion
{
    /// <summary>
    /// Command for restoring page content version
    /// </summary>
    public class RestorePageContentVersionCommand : CommandBase, ICommand<Guid, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The id of the archived page content version.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Execute(Guid pageContentId)
        {
            var pageContent = Repository.First<PageContent>(pageContentId);
            
            if (pageContent.Content.Status == ContentStatus.Archived)
            {

            }
            else
            {
                throw new CmsException(string.Format("A page content can be restored only from the archived version."));
            }

            return true;
        }

        private bool IsValidHistoricalContent(IHistorical f)
        {
            return !f.IsDeleted && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived);
        }

        private PageContentHistoryItem Convert(IHistorical content)
        {
            return new PageContentHistoryItem
                       {
                           Id = content.Id,
                           Version = content.Version,                           
                           Status = content.Status,
                           ArchivedByUser = content.Status == ContentStatus.Archived ? content.CreatedByUser : null,
                           ArchivedOn = content.Status == ContentStatus.Archived ? content.CreatedOn : (DateTime?)null,
                           DisplayedFor = content.Status == ContentStatus.Archived && content.PublishedOn != null
                                   ? content.CreatedOn - content.PublishedOn.Value
                                   : (TimeSpan?)null,
                           PublishedByUser = content.Status == ContentStatus.Published ? content.PublishedByUser : null,
                           PublishedOn = content.Status == ContentStatus.Published ? content.PublishedOn : null                           
                       };
        }        
    }
}