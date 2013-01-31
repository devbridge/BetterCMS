using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.MediaManager.ViewModels.Upload;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Upload.ConfirmUpload
{
    public class ConfirmUploadCommand : CommandBase, ICommand<MultiFileUploadViewModel, ConfirmUploadResponse>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        public ConfirmUploadResponse Execute(MultiFileUploadViewModel request)
        {            
            ConfirmUploadResponse response = new ConfirmUploadResponse();

            if (request.UploadedFiles != null && request.UploadedFiles.Count > 0)
            {
                MediaFolder folder = null;

                if (request.SelectedFolderId != null && request.SelectedFolderId.Value != Guid.Empty)
                {
                    folder = Repository.AsProxy<MediaFolder>(request.SelectedFolderId.Value);
                }

                List<MediaFile> files = new List<MediaFile>();
                foreach (var fileId in request.UploadedFiles)
                {
                    var file = Repository.FirstOrDefault<MediaFile>(fileId);
                    if (folder != null && (file.Folder == null || file.Folder.Id != folder.Id))
                    {
                        file.Folder = folder;
                    }
                    file.IsTemporary = false;
                    Repository.Save(file);                    
                    files.Add(file);
                }

                UnitOfWork.Commit();

                response.Medias = files.Select(Convert).ToList();
            }

            return response;
        }

        private MediaFileViewModel Convert(MediaFile file)
        {
            MediaFileViewModel model;

            if (file.Type == MediaType.File)
            {
                model = new MediaFileViewModel();
            }
            else if (file.Type == MediaType.Audio)
            {
                model = new MediaAudioViewModel();
            }
            else if (file.Type == MediaType.Video)
            {
                model = new MediaVideoViewModel();
            }
            else if (file.Type == MediaType.Image)
            {
                var imageFile = (MediaImage)file;
                model = new MediaImageViewModel {
                                                    ThumbnailUrl = imageFile.PublicThumbnailUrl,
                                                    Tooltip = imageFile.Title
                                                };
            }
            else
            {
                throw new CmsException(string.Format("A file type {0} is not supported.", file.Type));
            }

            model.Id = file.Id;
            model.Name = file.Title;
            model.Type = file.Type;
            model.Version = file.Version;
            model.ContentType = MediaContentType.File;
            model.PublicUrl = file.PublicUrl;
            model.FileExtension = file.OriginalFileExtension;

            return model;
        }
    }
}
