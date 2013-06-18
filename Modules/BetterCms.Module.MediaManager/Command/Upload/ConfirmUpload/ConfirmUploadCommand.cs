using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
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
            ConfirmUploadResponse response = new ConfirmUploadResponse { SelectedFolderId = request.SelectedFolderId ?? Guid.Empty, ReuploadMediaId = request.ReuploadMediaId };

            if (request.UploadedFiles != null && request.UploadedFiles.Count > 0)
            {
                MediaFolder folder = null;

                if (request.SelectedFolderId != null && request.SelectedFolderId.Value != Guid.Empty)
                {
                    folder = Repository.AsProxy<MediaFolder>(request.SelectedFolderId.Value);
                    if (folder.IsDeleted)
                    {
                        response.FolderIsDeleted = true;
                        return response;
                    }
                }

                UnitOfWork.BeginTransaction();

                List<MediaFile> files = new List<MediaFile>();
                if (request.ReuploadMediaId.HasDefaultValue())
                {
                    foreach (var fileId in request.UploadedFiles)
                    {
                        if (!fileId.HasDefaultValue())
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
                    }
                }
                else
                {
                    // Re-upload performed.
                    var fileId = request.UploadedFiles.FirstOrDefault();
                    if (!fileId.HasDefaultValue())
                    {
                        var originalMedia = Repository.First<MediaFile>(request.ReuploadMediaId);
                        Repository.Save(originalMedia.CreateHistoryItem());

                        var file = Repository.FirstOrDefault<MediaFile>(fileId);
                        file.CopyDataTo(originalMedia);
                        originalMedia.IsTemporary = false;
                        files.Add(originalMedia);
                    }
                }

                UnitOfWork.Commit();

                response.Medias = files.Select(Convert).ToList();

                // Notify.
                foreach (var mediaFile in files)
                {
                    MediaManagerApiContext.Events.OnMediaFileUpdated(mediaFile);
                }
            }

            return response;
        }

        private MediaFileViewModel Convert(MediaFile file)
        {
            MediaFileViewModel model;
            bool isProcessing = !file.IsUploaded.HasValue;
            bool isFailed = file.IsUploaded.HasValue && !file.IsUploaded.Value;

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
                isProcessing = isProcessing || !imageFile.IsOriginalUploaded.HasValue || !imageFile.IsThumbnailUploaded.HasValue;
                isFailed = isFailed 
                    || (imageFile.IsOriginalUploaded.HasValue && !imageFile.IsOriginalUploaded.Value)
                    || (imageFile.IsThumbnailUploaded.HasValue && !imageFile.IsThumbnailUploaded.Value);
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
            model.IsProcessing = isProcessing;
            model.IsFailed = isFailed;

            return model;
        }
    }
}
