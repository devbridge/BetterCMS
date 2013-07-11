using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Extensions
{
    public static class CommandBaseExtensions
    {
        public static MediaPathViewModel LoadPath(this CommandBase command, MediaManagerViewModel request, MediaType mediaType)
        {
            var emptyFolderViewModel = new MediaFolderViewModel { Id = Guid.Empty, Type = mediaType };
            var model = new MediaPathViewModel
            {
                CurrentFolder = emptyFolderViewModel
            };
            var folders = new List<MediaFolderViewModel> { emptyFolderViewModel };

            if (!request.CurrentFolderId.HasDefaultValue())
            {
                var mediaFolder = command.Repository.FirstOrDefault<MediaFolder>(e => e.Id == request.CurrentFolderId && e.Original == null);
                model.CurrentFolder = mediaFolder != null
                    ? new MediaFolderViewModel
                    {
                        Id = mediaFolder.Id,
                        Name = mediaFolder.Title,
                        Version = mediaFolder.Version,
                        Type = mediaFolder.Type,
                        ParentFolderId = mediaFolder.Folder != null ? mediaFolder.Folder.Id : Guid.Empty
                    }
                    : new MediaFolderViewModel();
                while (mediaFolder != null)
                {
                    folders.Insert(
                        1,
                        new MediaFolderViewModel
                        {
                            Id = mediaFolder.Id,
                            Name = mediaFolder.Title,
                            Type = mediaFolder.Type,
                            ParentFolderId = mediaFolder.Folder != null ? mediaFolder.Folder.Id : Guid.Empty
                        });
                    mediaFolder = mediaFolder.Folder;
                }
            }

            model.Folders = folders;
            return model;
        }
    }
}