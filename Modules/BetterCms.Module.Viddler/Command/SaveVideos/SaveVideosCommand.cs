using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Models;

namespace BetterCms.Module.Viddler.Command.Videos.SaveVideos
{
    internal class SaveVideosCommand : CommandBase, ICommand<SaveVideosRequest, SaveVideosResponse>
    {
        public SaveVideosResponse Execute(SaveVideosRequest request)
        {
            var response = new SaveVideosResponse { SelectedFolderId = request.SelectedFolderId ?? Guid.Empty, ReuploadMediaId = request.ReuploadMediaId };

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

                var files = new List<Video>();
                if (request.ReuploadMediaId.HasDefaultValue())
                {
                    foreach (var fileId in request.UploadedFiles)
                    {
                        if (!fileId.HasDefaultValue())
                        {
                            var file = Repository.FirstOrDefault<Video>(fileId);
                            if (folder != null && (file.Folder == null || file.Folder.Id != folder.Id))
                            {
                                file.Folder = folder;
                            }
                            file.IsTemporary = false;
                            file.PublishedOn = DateTime.Now;
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
                        var originalMedia = Repository.First<Video>(request.ReuploadMediaId);
                        Repository.Save(originalMedia.CreateHistoryItem());

                        var file = Repository.FirstOrDefault<Video>(fileId);
                        file.CopyDataTo(originalMedia);
                        originalMedia.IsTemporary = false;
                        originalMedia.PublishedOn = DateTime.Now;
                        files.Add(originalMedia);
                    }
                }

                UnitOfWork.Commit();

                // Notify.
                foreach (var mediaFile in files)
                {
                    Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaFile);
                }

                response.Medias =
                    files.Select(
                        file =>
                        (MediaFileViewModel)
                        new MediaVideoViewModel
                            {
                                Id = file.Id,
                                Name = file.Title,
                                Type = file.Type,
                                Version = file.Version,
                                ContentType = MediaContentType.File,
                                PublicUrl = file.PublicUrl,
                                FileExtension = file.OriginalFileExtension,
                                IsProcessing = !file.IsUploaded.HasValue,
                                IsFailed = file.IsUploaded.HasValue && !file.IsUploaded.Value
                            }).ToList();
            }

            return response;
        }
    }
}