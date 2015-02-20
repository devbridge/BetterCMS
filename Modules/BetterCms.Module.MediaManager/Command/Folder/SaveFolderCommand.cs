using System;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    public class SaveFolderCommand : CommandBase, ICommand<MediaFolderViewModel, MediaFolderViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        public MediaFolderViewModel Execute(MediaFolderViewModel request)
        {
            MediaFolder folder;

            if (request.Id == default(Guid))
            {
                folder = new MediaFolder();
            }
            else
            {
                folder = Repository.AsProxy<MediaFolder>(request.Id);
            }

            folder.Version = request.Version;
            folder.Title = request.Name;
            folder.Folder = request.ParentFolderId.HasValue && request.ParentFolderId.Value != default(Guid)
                                ? Repository.AsProxy<MediaFolder>(request.ParentFolderId.Value)
                                : null;
            folder.Type = request.Type;

            Repository.Save(folder);
            UnitOfWork.Commit();

            if (request.Id == default(Guid))
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderCreated(folder);
            }
            else
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderUpdated(folder);
            }

            return new MediaFolderViewModel
                       {
                           Id = folder.Id, 
                           Version = folder.Version, 
                           Name = folder.Title
                       };
        }
    }
}