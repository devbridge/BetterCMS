using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

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
            var creating = request.Id == default(Guid);

            if (creating)
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

            if (creating)
            {
                PopulateDependecies(folder, folder);
            }

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

        private void PopulateDependecies(MediaFolder child, MediaFolder parent)
        {
            var dependecy = new MediaFolderDependency
                                {
                                    Parent = parent,
                                    Child = child
                                };
            Repository.Save(dependecy);

            if (parent != null)
            {
                PopulateDependecies(child, parent.Folder);
            }
        }
    }
}